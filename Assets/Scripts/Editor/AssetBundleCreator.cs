using Newtonsoft.Json.Linq;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;
using Unity.RemoteConfig.Editor;
using System;
using Newtonsoft.Json;

public static class AssetBundleCreator
{
    [MenuItem("Tools/Update JSON Catalog")]
    static void JSONTesting()
    {
        if (string.IsNullOrEmpty(CloudProjectSettings.projectId) || string.IsNullOrEmpty(CloudProjectSettings.organizationId))
        {
            Debug.Log("not connected to unity cloud services");
        }
        Debug.Log("connected to unity cloud services");

        RemoteConfigWebApiClient.fetchEnvironmentsFinished += OnEnvironmentsFetched;
        try
        {
            RemoteConfigWebApiClient.FetchEnvironments(Application.cloudProjectId);

        }
        catch (Exception e)
        {
            RemoteConfigWebApiClient.fetchEnvironmentsFinished -= OnEnvironmentsFetched;
            Debug.LogError(e);
        }

        //var RemoteConfigController = new RemoteConfigWindowController();

    }

    static string envID = "";
    static void OnEnvironmentsFetched(JArray a)
    {
        RemoteConfigWebApiClient.fetchEnvironmentsFinished -= OnEnvironmentsFetched;
        JObject env = LoadEnvironment(a, "production");
        envID = env["id"].Value<string>();
        Debug.Log("environment loaded with id: " + envID);
        RemoteConfigWebApiClient.fetchConfigsFinished += OnConfigsLoaded;
        try
        {
            RemoteConfigWebApiClient.FetchConfigs(Application.cloudProjectId, envID);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            RemoteConfigWebApiClient.fetchConfigsFinished -= OnConfigsLoaded;
        }
    }
    public static JObject LoadEnvironment(JArray environments, string currentEnvName)
    {
        if (environments.Count > 0)
        {
            var currentEnvironment = environments[0];
            foreach (var environment in environments)
            {
                if (environment["name"].Value<string>() == currentEnvName)
                {
                    currentEnvironment = environment;
                    break;
                }
            }
            return (JObject)currentEnvironment;
        }
        else
        {
            Debug.LogWarning("No environments loaded");
            return new JObject();
        }
    }

    static void OnConfigsLoaded(JObject a)
    {
        RemoteConfigWebApiClient.fetchConfigsFinished -= OnConfigsLoaded;
        Debug.Log("searching for file named: catalog");
        JArray configContent = a["value"] as JArray;
        JObject catalog = null;
        string id = a["id"].Value<string>();
        foreach (JObject item in configContent)
        {
            if (item["key"].Value<string>() == "catalog")
            {
                catalog = item["value"] as JObject;
                break;
            }
        }

        if (catalog == null)
        {
            Debug.LogError("No file named catalog found");
            return;
        }

        ARAssetCatalog assetCatalog = JsonUtility.FromJson<ARAssetCatalog>(catalog.ToString(Formatting.None));
        CreateJSON(assetCatalog, configContent, id);
    }


    static void CreateJSON(ARAssetCatalog catalog, JArray configContent, string configID)
    {
        Debug.Log("getting assets in addressables");
        var addressableGroup = AddressableAssetSettingsDefaultObject.Settings.FindGroup("ARObjects");
        foreach (var asset in catalog.assets)
        {
            bool contains = false;
            foreach (var addressable in addressableGroup.entries)
            {
                contains = asset.asset == addressable.address;
                if (contains) break;
            }

            if (!contains)
            {
                Debug.Log($"Asset {asset.asset} from catalog is not present in addressables, hiding it");
                asset.hidden = true;
            }
        }

        var iconGroup = AddressableAssetSettingsDefaultObject.Settings.FindGroup("Icons");

        foreach (var addressable in addressableGroup.entries)
        {
            LanguageARAsset languageAsset = null;
            bool contains = false;
            foreach (var asset in catalog.assets)
            {
                contains = asset.asset == addressable.address;
                languageAsset = asset;
                if(contains) break;
            }


            if (!contains)
            {
                Debug.Log($"Addressable {addressable.address} is new, adding it to the catalog");
                languageAsset = new LanguageARAsset(addressable.address);
                catalog.assets.Add(languageAsset);
            }

            foreach (var icon in iconGroup.entries)
            {
                string name = icon.address.Replace("t_", "");
                if (name == addressable.address) {
                    languageAsset.icon = icon.address;
                }
            }
        }
        Debug.Log("Asset Catalog updated");

        foreach (JObject item in configContent)
        {
            if (item["key"].Value<string>() == "catalog")
            {
                string json = JsonConvert.SerializeObject(catalog);
                JObject content = JObject.Parse(json);
                item["value"] = content;
                break;
            }
        }

        Debug.Log(configContent.ToString(Formatting.None));

        RemoteConfigWebApiClient.settingsRequestFinished += OnPutConfigs;
        try
        {
            RemoteConfigWebApiClient.PutConfig(Application.cloudProjectId, envID, configID, configContent);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            RemoteConfigWebApiClient.settingsRequestFinished -= OnPutConfigs;
        }
    }

    static void OnPutConfigs() {
        RemoteConfigWebApiClient.settingsRequestFinished -= OnPutConfigs;
        Debug.Log("Done!");
    }
}
