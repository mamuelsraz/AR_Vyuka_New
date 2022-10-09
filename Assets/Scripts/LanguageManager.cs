using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public LanguageManager instance;
    public string currentLanguage { get; private set; }
    public Action onChangedLanguage;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void ChangeLanguage(string language) {
        currentLanguage = language;
        onChangedLanguage?.Invoke();
    }
}
