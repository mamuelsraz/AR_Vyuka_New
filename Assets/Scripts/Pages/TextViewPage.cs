using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using TMPro;
using UnityEngine;

public class TextViewPage : Page
{
    SimpleHelvetica text;
    public TMP_InputField inputField;
    public TouchControls touchControls;
    public LanguageChangeUI languageChangeUI;
    protected override void Awake()
    {
        base.Awake();

        onPreShow.AddListener(Initialize);
        onPreHide.AddListener(Deselect);
    }

    void Deselect() {
        SelectedArObjectManager.instance.selectedObject.SetActive(false);
        languageChangeUI.Hide();
    }

    protected override void Update()
    {
        base.Update();
        if (active)
        {
            touchControls.RotateAndScaleObj(SelectedArObjectManager.instance.selectedObject.transform);
        }
    }

    void Initialize()
    {
        languageChangeUI.Show();
        text = SelectedArObjectManager.instance.selectedObject.GetComponentInChildren<SimpleHelvetica>();
        if (text == null) return;
        text.Text = inputField.text;
        text.GenerateText();
    }

    public void TextChanged()
    {
        if (text == null) return;
        text.Text = inputField.text;
        text.GenerateText();
    }

    public void Speak()
    {
        TextToSpeech.Instance.Setting(LanguageManager.instance.currentLanguage, 1, 1);

        TextToSpeech.Instance.StartSpeak(inputField.text);
    }
}
