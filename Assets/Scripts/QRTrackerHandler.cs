using QRFoundation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using ZXing.QrCode.Internal;

[System.Serializable]
public class QRCodeMarker
{
    public Transform transform;
    public float width;
    public bool tracked;
    public string content;

    public QRCodeMarker(string content, float width)
    {
        this.content = content;
        this.width = width;
        this.transform = null;
    }

    //!no particular order!
    public Vector3[] GetWorldCorners()
    {
        Vector3[] worldCorners = new Vector3[4] {
                transform.position + transform.up * width/2 - transform.right * width/2,
                transform.position + transform.up * width/2 + transform.right * width/2,
                transform.position - transform.up * width/2 - transform.right * width/2,
                transform.position - transform.up * width/2 + transform.right * width/2,
            };
        return worldCorners;
    }

}

[System.Serializable]
public class CachedCodeEvent : UnityEvent<QRCodeMarker> { };

public class QRTrackerHandler : MonoBehaviour
{
    QRCodeTracker tracker;
    public ARAnchorManager anchorManager;
    [HideInInspector]
    public Dictionary<string, QRCodeMarker> cachedCodes;
    public CachedCodeEvent OnCodeAdded;
    public CachedCodeEvent OnCodeUpdated;
    public CachedCodeEvent OnCodeRemoved;

    private void Start()
    {
        tracker = GetComponent<QRCodeTracker>();
        cachedCodes = new Dictionary<string, QRCodeMarker>();
        tracker.onCodeRegistered.AddListener(OnCodeRegistered);
        tracker.onCodeLost.AddListener(OnCodeLost);
    }

    private void Update()
    {
        if (tracker.trackingState == TrackingState.Registered)
        {
            cachedCodes[tracker.registeredString].width = tracker.lastWidth;
        }
    }

    void OnCodeRegistered(string content, GameObject obj)
    {
        bool newCode = !cachedCodes.ContainsKey(content);
        if (newCode)
        {
            var qrCode = new QRCodeMarker(content, tracker.lastWidth);
            qrCode.transform = new GameObject().transform;
            cachedCodes.Add(content, qrCode);
        }
        var code = cachedCodes[content];

        if (!newCode && !code.tracked) {
            Destroy(code.transform.gameObject.GetComponent<ARAnchor>());
        }
        code.tracked = true;
        code.transform.parent = obj.transform;
        ResetTransform(code.transform);

        if (newCode) OnCodeAdded?.Invoke(code);
        else OnCodeUpdated?.Invoke(code);
    }

    void OnCodeLost()
    {
        QRCodeMarker qrCode = null;
        if (cachedCodes != null && tracker.registeredString != null && cachedCodes.ContainsKey(tracker.registeredString))
            qrCode = cachedCodes[tracker.registeredString];
        else return;

        qrCode.tracked = false;

        var lastParent = qrCode.transform.parent;
        qrCode.transform.parent = null;
        qrCode.transform.position = lastParent.position;
        qrCode.transform.rotation = lastParent.rotation;
        qrCode.transform.localScale = lastParent.localScale;
        qrCode.transform.gameObject.AddComponent<ARAnchor>();

        OnCodeUpdated?.Invoke(qrCode);
    }

    public void RemoveCode(string content)
    {
        if (cachedCodes.ContainsKey(content))
        {
            var code = cachedCodes[content];
            OnCodeRemoved.Invoke(code);
            if (code.tracked)
            {
                tracker.Unregister();
            }
            else Destroy(code.transform.gameObject);
            cachedCodes.Remove(content);
        }
    }

    private void OnDisable()
    {
        foreach (var item in cachedCodes)
        {
            QRCodeMarker code = item.Value;
            OnCodeRemoved.Invoke(code);
            if (!code.tracked) Destroy(code.transform.gameObject);
        }

        cachedCodes = new Dictionary<string, QRCodeMarker>();
    }

    void ResetTransform(Transform t) {
        t.localPosition = Vector3.zero;
        t.localScale = Vector3.one;
        t.localRotation = Quaternion.identity;
    }
}

