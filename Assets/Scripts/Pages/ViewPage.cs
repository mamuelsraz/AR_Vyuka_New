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

    protected override void Awake()
    {
        base.Awake();
        onPreShow.AddListener(Initialize);
    }

    void Initialize() {
        var arObject = SelectedArObjectManager.instance.selectedArObject;
        if (arObject is LanguageArObject)
        {
            languagePanel.SetActive(true);
            var languageArObject = (LanguageArObject)arObject;

            mainText.text = languageArObject.nameInLanguage[0].word;

            foreach (Transform child in prepositionsParent)
            {
                GameObject.Destroy(child.gameObject);
            }
            int i = 1;
            foreach (var preposition in languageArObject.nameInLanguage[0].prepositions)
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
        Debug.Log(mode);
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
