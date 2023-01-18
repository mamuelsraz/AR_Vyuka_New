using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AdressablesStreamingManager : MonoBehaviour
{
    public static ARAssetCatalogHandle GetAssetCatalog() {       
        var handle = new ARAssetCatalogHandle();
#if UNITY_EDITOR
        EditorCoroutineUtility.StartCoroutine(DownloadCatalog(handle), handle);
#else
    Debug.LogError("NOT IMPLEMENTED");
#endif
        return handle;
    }

    static IEnumerator DownloadCatalog(ARAssetCatalogHandle handle) {
        yield return null;
        ARAssetCatalog catalog = new ARAssetCatalog(new List<LanguageARAsset>());
        catalog.assets.Add(new LanguageARAsset("boty"));
        handle.OnComplete.Invoke(catalog);
    }
}

public class ARAssetCatalogHandle
{
    public Action<ARAssetCatalog> OnComplete; 
    public Action OnFail;
}
