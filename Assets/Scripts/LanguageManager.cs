using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;
    public string currentLanguage { get; private set; }
    public Action onChangedLanguage;

    private void Awake()
    {
        currentLanguage = "en-EN";
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void ChangeLanguage(string language) {
        currentLanguage = language;
        onChangedLanguage?.Invoke();
    }

    public LanguageBlock GetCurrentLanguageBlock() {
        string language = currentLanguage;
        LanguageBlock block = null;
        var ARObject = SelectedArObjectManager.instance.selectedARAsset;
        if (ARObject.area == "Jazyky") {
            foreach (var item in ARObject.nameInLanguage)
            {
                if (language.Equals(item.language)) {
                    block = item;
                    break;
                }
            }
            if (block != null) {
                return block;
            }
        }
        return null;
    }
}
