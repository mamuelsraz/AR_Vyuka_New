using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDownload : MonoBehaviour
{
    private void Start()
    {
        AssetStreamingManager.instance.LoadList(AssetStreamingManager.path, "list").Complete += OnListComplete;
    }

    void OnListComplete(StreamingHandleResponse response) {
        if (response.status != StreamingStatus.Failed)
        {
            var obj = AssetStreamingManager.instance.ArObjectList[0];
            AssetStreamingManager.instance.LoadArObj(obj, AssetStreamingManager.path).Complete += OnArObjDownloaded;
        }
        else Debug.LogError("Download error");
    }

    void OnArObjDownloaded(StreamingHandleResponse response) {
        if (response.status != StreamingStatus.Failed) {
            Instantiate((GameObject)response.loadedObj, Vector3.zero, Quaternion.identity);
        }
        else Debug.LogError("Download error");
    }
}
