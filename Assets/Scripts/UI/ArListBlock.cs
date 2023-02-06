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
    public LanguageARAsset arAsset;
    public Button button;
    public Transform buttonTransform;
    public Image fillImage;
    public Image spriteImage;
    bool downloaded;

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
        if (AddressablesStreamingManager.Instance.cachedARAssets.ContainsKey(arAsset))
        {
            button.enabled = true;
            fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, 0);
            buttonTransform.localEulerAngles = new Vector3(0, 0, 180);
            downloaded = true;
        }

        var handle = AddressablesStreamingManager.Instance.LoadIcon(arAsset);
        if (handle == null)
        {
            spriteImage.sprite = AddressablesStreamingManager.Instance.cachedSprites[arAsset];
        }
        else {
            handle.OnComplete += (sprite) =>
            {
                spriteImage.sprite = sprite;
            };
            handle.OnFail += () => {
                Debug.LogError($"icon ' {arAsset.icon} ' could not be loaded on asset {arAsset.asset}.");
            };
        }
    }

    void Download()
    {
        button.enabled = false;
        var handle = AddressablesStreamingManager.Instance.LoadArObj(arAsset);
        if (handle == null)
        {
            fillImage.DOFade(0, 0.25f);
            buttonTransform.DORotate(new Vector3(0, 0, 180), 0.25f);
            downloaded = true;
        }
        else
        {
            handle.Tick += () =>
            {
                fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, 1);
                fillImage.fillAmount = handle.progress;
            };
            handle.OnFail += () =>
            {
                NativeDialog.OpenDialog("Nepovedlo se stáhnout model!", "Zkontrolujte prosím, zda máte stálé připojení k internetu. Aplikace se po stisknutí [ok] restartuje.", "Ok",
                       () =>
                       {
                           SceneManager.LoadScene(0);
                       });
            };
            handle.OnComplete += (response) =>
            {
                button.enabled = true;
                fillImage.DOFade(0, 0.25f);
                buttonTransform.DORotate(new Vector3(0, 0, 180), 0.25f);
                downloaded = true;
            };
        }
    }

    void Place()
    {
        SelectedArObjectManager.instance.SelectNew(arAsset);
        page.ChangePage();
    }
}
