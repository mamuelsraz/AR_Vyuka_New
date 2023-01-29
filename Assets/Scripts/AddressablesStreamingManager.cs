using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Unity.RemoteConfig;
using UnityEngine.ResourceManagement.AsyncOperations;
using Unity.Services.CCD.Management;

public class AddressablesStreamingManager : MonoBehaviour
{
    public static AddressablesStreamingManager Instance;
    [HideInInspector] public ARAssetCatalog catalog;
    public Dictionary<LanguageARAsset, GameObject> cachedARAssets;
    public Dictionary<LanguageARAsset, Sprite> cachedSprites;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);

        catalog = null;
        cachedARAssets = new Dictionary<LanguageARAsset, GameObject>();
        cachedSprites = new Dictionary<LanguageARAsset, Sprite>();
    }

    public struct userAttributes { }
    public struct appAttributes { }

    public ARAssetStreamingHandle<GameObject> LoadArObj(LanguageARAsset asset)
    {
        if (cachedARAssets.ContainsKey(asset))
        {
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

    public ARAssetStreamingHandle<Sprite> LoadIcon(LanguageARAsset asset)
    {
        if (cachedSprites.ContainsKey(asset))
        {
            return null;
        }

        var handle = new ARAssetStreamingHandle<Sprite>();

        StartCoroutine(DownloadIcon(asset, handle));
        return handle;
    }

    IEnumerator DownloadIcon(LanguageARAsset asset, ARAssetStreamingHandle<Sprite> response) {
        var asyncHandle = Addressables.LoadAssetAsync<Sprite>(asset.icon);
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
            cachedSprites.Add(asset, asyncHandle.Result);
            response.OnComplete?.Invoke(asyncHandle.Result);
        }
    }

    public ARAssetStreamingHandle<ARAssetCatalog> LoadAssetCatalog()
    {
        if (catalog != null && catalog.assets != null && catalog.assets.Count > 0) 
            return null;

        var handle = new ARAssetStreamingHandle<ARAssetCatalog>();

        Action<ConfigResponse> action = null;
        action = (args) =>
        {
            OnFetchCompleted(args, handle);
            ConfigManager.FetchCompleted -= action;
        };

        ConfigManager.FetchCompleted += action;
        ConfigManager.SetEnvironmentID("87ede4f4-07f7-44d1-86c7-2b4385dd729f");
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
        Debug.Log("Catalog loaded:");
        Debug.Log(json);
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
