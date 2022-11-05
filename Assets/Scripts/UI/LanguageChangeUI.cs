using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;

public class LanguageChangeUI : MonoBehaviour
{
    public Transform mainButton;
    public Transform mask;
    public List<string> languages;
    public List<Sprite> languageGraphics;
    public Image mainGraphic;
    public float openedLenght;
    List<Image> languageRenderers;
    List<string> orderedLanguages;
    public RectTransform languageParent;

    private void Start()
    {
        Hide();

        languageRenderers = new List<Image>();
        foreach (Transform child in languageParent.transform)
        {
            languageRenderers.Add(child.gameObject.GetComponent<Image>());
        }

        LanguageManager.instance.ChangeLanguage(languages[0]);
        orderedLanguages = new List<string>();

        for (int i = 1; i < languages.Count; i++)
        {
            orderedLanguages.Add(languages[i]);
        }

        RenderLanguages();
    }

    public void SetLanguage(int index) {
        string language = orderedLanguages[index];
        string currentLanguage = LanguageManager.instance.currentLanguage;

        orderedLanguages[index] = currentLanguage;
        LanguageManager.instance.ChangeLanguage(language);

        RenderLanguages();
        CloseLanguageBar();
    }

    void RenderLanguages() {
        int mainIndex = languages.IndexOf(LanguageManager.instance.currentLanguage);
        mainGraphic.sprite = languageGraphics[mainIndex];
        for (int i = 0; i < orderedLanguages.Count; i++)
        {
            int index = languages.IndexOf(orderedLanguages[i]);
            languageRenderers[i].sprite = languageGraphics[index];
        }
    }

    bool barOpened = false;
    public void InteractLanguageBar() {
        if (barOpened) CloseLanguageBar();
        else OpenLanguageBar();
    }

    void CloseLanguageBar() {
        languageParent.DOSizeDelta(new Vector2(languageParent.rect.width, 0), 0.25f);
        barOpened = false;
    }

    void OpenLanguageBar() {
        languageParent.DOSizeDelta(new Vector2(languageParent.rect.width, openedLenght), 0.25f);
        barOpened = true;
    }

    public void Show() {
        mainButton.gameObject.SetActive(true);
        mask.gameObject.SetActive(true);
    }

    public void Hide() {
        mainButton.gameObject.SetActive(false);
        mask.gameObject.SetActive(false);
    }
}
