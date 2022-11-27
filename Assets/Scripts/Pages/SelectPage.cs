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
    public Image progressBar;
    public ArListPopulator populator;
    public PlacePage placePage;
    public ViewPage viewPage;
    public BasicViewPage basicViewPage;
    [HideInInspector] public bool loadChemie;
    public LanguageChangeUI languageChangeUI;
    protected override void Awake()
    {
        base.Awake();
        onPreShow.AddListener(Initialize);
        onPreHide.AddListener(OnPreHide);
    }

    void OnPreHide()
    {
        languageChangeUI.Hide();
    }

    private void Start()
    {
        hidePanel.color = new Color(hidePanel.color.r, hidePanel.color.g, hidePanel.color.b, 1);
    }

    private void Initialize()
    {

        if (SelectedArObjectManager.instance.selectedArea == "Jazyky")
        {
            languageChangeUI.Show();
        }

        var handle = AssetStreamingManager.instance.LoadList(AssetStreamingManager.path, "list");
        if (handle == null)
        {
            populator.Init();
            hidePanel.DOFade(0, 0.25f).onComplete += () =>
            {
                hidePanel.gameObject.SetActive(false);
            };
        }
        else
        {
            handle.Tick += () =>
            {
                progressBar.fillAmount = handle.progress;
            };
            handle.Complete += (StreamingHandleResponse response) =>
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
    }

    public void ChangePage()
    {
        placePage.viewPage = viewPage;
        GoTo(placePage);
    }
}
