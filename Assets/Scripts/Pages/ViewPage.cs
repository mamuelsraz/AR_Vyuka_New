using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TextSpeech;
using UnityEngine.UI;
using pingak9;

public class ViewPage : Page
{
    public TouchControls touchControls;
    public Transform prepositionsParent;
    public GameObject languagePanel;
    public TextMeshProUGUI otherText;
    public GameObject prepositionPrefab;
    public TextMeshProUGUI mainText;
    public LanguageChangeUI languageChangeUI;

    protected override void Awake()
    {
        base.Awake();
        onPreShow.AddListener(Initialize);
        onPreHide.AddListener(Deselect);
    }

    private void Start()
    {
        LanguageManager.instance.onChangedLanguage += Initialize;
    }

    void Deselect()
    {
        SelectedArObjectManager.instance.selectedObject.SetActive(false);
        languageChangeUI.Hide();
    }

    void Initialize()
    {
        var arAsset = SelectedArObjectManager.instance.selectedARAsset;
        if (arAsset.area == "Jazyky")
        {
            languageChangeUI.Show();
            languagePanel.SetActive(true);

            var languageBlock = arAsset.GetBlock(LanguageManager.instance.currentLanguage);
            mainText.text = languageBlock.word;

            foreach (Transform child in prepositionsParent)
            {
                GameObject.Destroy(child.gameObject);
            }
            int i = 1;
            foreach (var preposition in languageBlock.prepositions)
            {
                var obj = Instantiate(prepositionPrefab, prepositionsParent);
                var text = obj.GetComponentInChildren<TextMeshProUGUI>();
                text.text = preposition;
                int ii = i;
                obj.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    Speak(ii);
                });
                i++;
            }

            otherText.gameObject.SetActive(false);
        }
        else
        {
            languagePanel.SetActive(false);
            otherText.text = arAsset.nickName;
            otherText.gameObject.SetActive(true);
        }

        ColorChanger.instance.Initialize();
    }

    protected override void Update()
    {
        base.Update();
        if (active)
        {
            touchControls.RotateAndScaleObj(SelectedArObjectManager.instance.selectedObject.transform);
        }
    }

    public void Speak(int mode)
    {
        LanguageBlock block = LanguageManager.instance.GetCurrentLanguageBlock();
        if (block == null) return;
        TextToSpeech.Instance.Setting(block.language, 1, 1);
        string message = "";
        switch (mode)
        {
            case 0:
                message = block.word;
                break;
            case 1:
                string preposition = block.prepositions[0];
                if (preposition[preposition.Length - 1] == '\'')
                    message = $"{preposition}{block.word}";
                else
                    message = $"{preposition} {block.word}";
                break;
            case 2:
                string preposition2 = block.prepositions[1];
                if (preposition2[preposition2.Length - 1] == '\'')
                    message = $"{preposition2}{block.word}";
                else
                    message = $"{preposition2} {block.word}";
                break;
            default:
                break;
        }
        TextToSpeech.Instance.StartSpeak(message);
    }
}
