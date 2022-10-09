using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            foreach (var preposition in languageArObject.nameInLanguage[0].prepositions)
            {
                var text = Instantiate(prepositionPrefab, prepositionsParent).GetComponentInChildren<TextMeshProUGUI>();
                text.text = preposition;
                //tts
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

   
}
