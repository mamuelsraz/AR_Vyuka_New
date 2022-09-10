using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetStreamingManager : MonoBehaviour
{
    public static AssetStreamingManager instance;
    public ArObject[] ArObjectList;
    public Dictionary<ArObject, GameObject> cachedArObjects;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public StreamingHandle LoadList(string path, string list)
    {
        StreamingHandle handle = new StreamingHandle();
        StartCoroutine(DownloadList(handle, path, list));
        return handle;
    }

    public StreamingHandle LoadArObj(ArObject obj, string path)
    {
        var handle = new StreamingHandle();
        if (cachedArObjects.ContainsKey(obj))
        {
            var loadedObj = cachedArObjects[obj];
            handle.Complete.Invoke(new StreamingHandleResponse(StreamingStatus.AlreadyDownloaded, loadedObj));
        }
        StartCoroutine(DownloadArObject(handle, path, obj));
        return handle;
    }

    IEnumerator DownloadList(StreamingHandle handle, string path, string list)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(path + list);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            handle.Complete.Invoke(new StreamingHandleResponse(StreamingStatus.Failed, null));
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            if (bundle != null)
            {
                ArObjectList = bundle.LoadAllAssets<ArObject>();

                handle.Complete.Invoke(new StreamingHandleResponse(StreamingStatus.Success, ArObjectList));
            }
            else
            {
                handle.Complete.Invoke(new StreamingHandleResponse(StreamingStatus.Failed, null));
            }
        }
    }

    IEnumerator DownloadArObject(StreamingHandle handle, string path, ArObject ArObj)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(path + ArObj.bundle);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            handle.Complete.Invoke(new StreamingHandleResponse(StreamingStatus.Failed, null));
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            if (bundle != null)
            {
                GameObject loadedObj = bundle.LoadAllAssets<GameObject>()[0];
                if (loadedObj != null)
                {
                    cachedArObjects.Add(ArObj, loadedObj);
                    handle.Complete.Invoke(new StreamingHandleResponse(StreamingStatus.Success, loadedObj));
                }
                else {
                    handle.Complete.Invoke(new StreamingHandleResponse(StreamingStatus.Failed, null));
                }
            }
            else
            {
                handle.Complete.Invoke(new StreamingHandleResponse(StreamingStatus.Failed, null));
            }
        }
    }
}

public class StreamingHandle
{
    public Action<StreamingHandleResponse> Complete;
}

public class StreamingHandleResponse
{
    public StreamingHandleResponse(StreamingStatus status, object loadedObj)
    {
        this.status = status;
        this.loadedObj = loadedObj;
    }

    public StreamingStatus status;
    public object loadedObj;
}

public enum StreamingStatus
{
    Failed,
    Success,
    AlreadyDownloaded
}
