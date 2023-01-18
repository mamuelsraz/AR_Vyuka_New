using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.VersionControl;
using UnityEngine;

public class AssetBundleCreator
{
    [MenuItem("Tools/Create/AssetBundles for Windows")]
    static void BuildAllAssetBundlesWindows()
    {
        string assetBundleDirectory = "Assets/Bundles/AR_Vyuka_New_Content";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.UncompressedAssetBundle,
                                        BuildTarget.StandaloneWindows);
    }

    [MenuItem("Tools/Create/JSON")]
    static void CreateJSON()
    {
        //ARAssetCatalog ctlg = new(new List<LanguageARAsset>());
        //ctlg.assets.Add(new LanguageARAsset("aaaaaa"));
        //Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(ctlg.assets));

        var addressableGroup = AddressableAssetSettingsDefaultObject.Settings.FindGroup("ARObjects");
        var handle = AdressablesStreamingManager.GetAssetCatalog();
        handle.OnComplete += (ARAssetCatalog catalog) =>
        {
            Debug.Log("Asset Catalog downloaded.");
            foreach (var asset in catalog.assets)
            {
                bool contains = false;
                foreach (var addressable in addressableGroup.entries)
                    contains = asset.asset == addressable.address;
                if (!contains)
                {
                    Debug.Log($"Asset {asset.asset} from catalog is not present in addressables, hiding it");
                    asset.hidden = true;
                }
            }

            foreach (var addressable in addressableGroup.entries)
            {
                bool contains = false;
                foreach (var asset in catalog.assets)
                    contains = asset.asset == addressable.address;
                if (!contains)
                {
                    Debug.Log($"Addressable {addressable.address} is new, adding it to the catalog");
                    catalog.assets.Add(new LanguageARAsset(addressable.address));
                }
            }
            Debug.Log("Asset Catalog updated");
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(catalog);
            Debug.Log(json);
            File.WriteAllText("Assets/catalog.json", json);
            Debug.Log("Asset saved to Assets/catalog.json, please export it to the server");
        };
    }

    [MenuItem("Tools/Create/AssetBundles for Android")]
    static void BuildAllAssetBundlesAndroid()
    {
        string assetBundleDirectory = "Assets/Bundles/AR_Vyuka_New_Content";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.UncompressedAssetBundle,
                                        BuildTarget.Android);
    }

    [MenuItem("Tools/Create/AssetBundles for IOS")]
    static void BuildAllAssetBundlesIOS()
    {
        string assetBundleDirectory = "Assets/Bundles/AR_Vyuka_New_Content";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.UncompressedAssetBundle,
                                        BuildTarget.iOS);
    }
}
