using pingak9;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SelectPage : Page
{
    public Image hidePanel;
    public ArListPopulator populator;
    public Page placePage;
    [HideInInspector] public bool loadChemie;

    private void Awake()
    {
        base.Awake();
        onPreShow.AddListener(Initialize);
    }

    private void Start()
    {
        hidePanel.color = new Color(hidePanel.color.r, hidePanel.color.g, hidePanel.color.b, 1);
    }

    private void Initialize()
    {
        if (AssetStreamingManager.instance.ArObjectList.Length > 0) populator.Init();
        else AssetStreamingManager.instance.LoadList(AssetStreamingManager.path, "list").Complete += (StreamingHandleResponse response) =>
        {
            if (response.status != StreamingStatus.Failed)
            {
                populator.Init();
                hidePanel.DOFade(0, 0.25f).onComplete += () =>
                {
                    hidePanel.gameObject.SetActive(false);
                };
            }

            else
            {
                NativeDialog.OpenDialog("Nepovedlo se stáhnout seznam!", "Zkontrolujte prosím, zda máte stálé připojení k internetu. Aplikace se po stisknutí [ok] restartuje.", "Ok",
                () =>
                {
                    SceneManager.LoadScene(0);
                });
            }
        };
    }



    public void ChangePage()
    {
        GoTo(placePage);
    }
}
