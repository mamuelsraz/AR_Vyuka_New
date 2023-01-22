using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Unity.RemoteConfig;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesStreamingManager : MonoBehaviour
{
    public static AddressablesStreamingManager Instance;
    public ARAssetCatalog catalog;
    public Dictionary<LanguageARAsset, GameObject> cachedARAssets;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);

        catalog = null;
        cachedARAssets = new Dictionary<LanguageARAsset, GameObject>();
    }

    public struct userAttributes { }
    public struct appAttributes { }

    public ARAssetStreamingHandle<GameObject> LoadArObj(LanguageARAsset asset, string path)
    {
        if (cachedARAssets.ContainsKey(asset))
        {
            var loadedObj = cachedARAssets[asset];
            return null;
        }

        var handle = new ARAssetStreamingHandle<GameObject>();

        StartCoroutine(DownloadARAsset(asset, handle));
        return handle;
    }

    IEnumerator DownloadARAsset(LanguageARAsset asset, ARAssetStreamingHandle<GameObject> response)
    {
        var asyncHandle = Addressables.LoadAssetAsync<GameObject>(asset.asset);
        while (!asyncHandle.IsDone)
        {
            response.progress = asyncHandle.PercentComplete;
            response.Tick?.Invoke();
            yield return null;
        }
        if (asyncHandle.Status != AsyncOperationStatus.Succeeded)
        {
            response.OnFail?.Invoke();
        }
        else
        {

            GameObject loadedObj = Instantiate(asyncHandle.Result);
            loadedObj.SetActive(false);

            cachedARAssets.Add(asset, loadedObj);
            response.OnComplete?.Invoke(loadedObj);
        }
    }

    public ARAssetStreamingHandle<ARAssetCatalog> GetAssetCatalog()
    {
        if (catalog != null) return null;

        var handle = new ARAssetStreamingHandle<ARAssetCatalog>();

        Action<ConfigResponse> action = null;
        action = (args) =>
        {
            OnFetchCompleted(args, handle);
            ConfigManager.FetchCompleted -= action;
        };

        ConfigManager.FetchCompleted += action;
        ConfigManager.SetEnvironmentID("ea43718d-0a8a-4e5e-96d6-c86ab2d851a9");
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(
            new userAttributes(), new appAttributes());

        return handle;
    }

    void OnFetchCompleted(ConfigResponse response, ARAssetStreamingHandle<ARAssetCatalog> handle)
    {
        if (response.status != ConfigRequestStatus.Success)
        {
            Debug.LogError("Fetching remote config failed!");
            handle.OnFail?.Invoke();
            return;
        }

        string json = ConfigManager.appConfig.GetJson("catalog");
        catalog = JsonUtility.FromJson<ARAssetCatalog>(json);
        handle.OnComplete?.Invoke(catalog);
    }
}

public class ARAssetStreamingHandle<T>
{
    public Action<T> OnComplete;
    public Action Tick;
    public float progress = 0;
    public Action OnFail;
}
