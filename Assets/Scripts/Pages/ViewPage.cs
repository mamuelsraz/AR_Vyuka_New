using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TextSpeech;
using UnityEngine.UI;

public class ViewPage : Page
{
    public TouchControls touchControls;
    public Transform prepositionsParent;
    public GameObject languagePanel;
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

    void Deselect() {
        SelectedArObjectManager.instance.selectedObject.SetActive(false);
        languageChangeUI.Hide();
    }

    void Initialize() {
        var arObject = SelectedArObjectManager.instance.selectedArObject;
        if (arObject is LanguageArObject)
        {
            languageChangeUI.Show();
            languagePanel.SetActive(true);
            var languageArObject = (LanguageArObject)arObject;

            var languageBlock = languageArObject.GetBlock(LanguageManager.instance.currentLanguage);
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
                obj.GetComponentInChildren<Button>().onClick.AddListener(() => {
                    Speak(ii);
                });
                i++;
            }
        }
        else languagePanel.SetActive(false);
    }

    private void Update()
    {
        if (active)
        {
            touchControls.RotateAndScaleObj(SelectedArObjectManager.instance.selectedObject.transform);
        }
    }

    public void Speak(int mode)
    {
        LanguageBlock block = LanguageManager.instance.GetCurrentLanguageBlock();
        if (block == null) return;
        TextToSpeech.instance.Setting(block.language, 1, 1);
        switch (mode)
        {
            case 0:
                TextToSpeech.instance.StartSpeak(block.word);
                break;
            case 1:
                TextToSpeech.instance.StartSpeak($"{block.prepositions[0]} {block.word}");
                break;
            case 2:
                TextToSpeech.instance.StartSpeak($"{block.prepositions[1]} {block.word}");
                break;
            default:
                break;
        }
    }
   
}
