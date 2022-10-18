using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using pingak9;
using UnityEngine.SceneManagement;

public class ArListBlock : MonoBehaviour
{
    public SelectPage page;
    public ArObject arObject;
    public Button button;
    public Transform buttonTransform;
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

    private void Start()
    {
        if (AssetStreamingManager.instance.cachedArObjects.ContainsKey(arObject))
        {
            button.enabled = true;
            fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, 0);
            buttonTransform.localEulerAngles = new Vector3(0, 0, 180);
            downloaded = true;
        }
    }

    private void Update()
    {
        if (handle != null)
        {
            fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, 1);
            fillImage.fillAmount = handle.progress;
        }
    }

    void Download()
    {
        button.enabled = false;
        handle = AssetStreamingManager.instance.LoadArObj(arObject, AssetStreamingManager.path);
        if (handle == null)
        {
            fillImage.DOFade(0, 0.25f);
            buttonTransform.DORotate(new Vector3(0, 0, 180), 0.25f);
            downloaded = true;
        }
        else handle.Complete += (StreamingHandleResponse response) =>
        {
            button.enabled = true;
            fillImage.DOFade(0, 0.25f);
            handle = null;
            if (response.status != StreamingStatus.Failed)
            {
                buttonTransform.DORotate(new Vector3(0, 0, 180), 0.25f);
                downloaded = true;
            }
            else
                NativeDialog.OpenDialog("Nepovedlo se stáhnout model!", "Zkontrolujte prosím, zda máte stálé připojení k internetu. Aplikace se po stisknutí [ok] restartuje.", "Ok",
                () =>
                {
                    SceneManager.LoadScene(0);
                });
        };
    }

    void Place()
    {
        SelectedArObjectManager.instance.SelectNew(arObject);
        page.ChangePage();
    }
}
