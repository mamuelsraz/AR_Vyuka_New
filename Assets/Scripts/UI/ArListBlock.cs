using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ArListBlock : MonoBehaviour
{
    [HideInInspector] public ArListPopulator populator;
    public ArObject arObject;
    public Button button;
    public Image fillImage;
    bool downloaded;
    StreamingHandle handle;

    public void Click()
    {
        if (!downloaded)
        {
            Download();
        }
        else Place();
    }

    private void Update()
    {
        if (handle != null)
        {
            fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, 1);
            fillImage.fillAmount = handle.progress;
        }
    }

    void Download() {
        button.enabled = false;
        handle = AssetStreamingManager.instance.LoadArObj(arObject, AssetStreamingManager.path);
        handle.Complete += (StreamingHandleResponse response) =>
        {
            button.enabled = true;
            fillImage.DOFade(0, 0.25f);
            handle = null;
            if (response.status != StreamingStatus.Failed)
            {
                button.transform.DORotate(new Vector3(0, 0, 0), 0.25f);
                downloaded = true;
            }
            else
            {
                Debug.LogError("Something bad happened");
            }
        };
    }

    void Place() {
        SelectedArObjectManager.instance.SelectNew(arObject);
        populator.ChangePage();
    }
}
