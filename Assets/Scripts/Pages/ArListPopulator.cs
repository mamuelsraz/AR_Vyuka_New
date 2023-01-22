using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArListPopulator : MonoBehaviour
{
    public SelectPage page;
    public Transform parent;
    public GameObject blockPrefab;
    public TMP_Dropdown dropdown;
    List<string> categories;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(OnValueChanged);
        LanguageManager.instance.onChangedLanguage += TryPopulate;
    }

    public void Init()
    {

        categories = new List<string>();
        categories.Add("vše");
        foreach (var item in AddressablesStreamingManager.Instance.catalog.assets)
        {
            if (item.area != SelectedArObjectManager.instance.selectedArea) continue;
            if (!categories.Contains(item.category)) categories.Add(item.category);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(categories);

        Populate();
    }

    void TryPopulate()
    {
        if (SelectedArObjectManager.instance.selectedArea == "Jazyky")
        {
            Populate();
        }
    }

    void OnValueChanged(int i)
    {
        Populate();
    }

    void Populate()
    {
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }

        Dictionary<string, List<ArObject>> sortedCategories = new();

        foreach (var item in AddressablesStreamingManager.Instance.catalog.assets)
        {
            if (item.area != SelectedArObjectManager.instance.selectedArea) continue;
            if (dropdown.value != 0 && categories[dropdown.value] != item.category) continue;
            var block = Instantiate(blockPrefab, parent);
            var language = item.GetBlock(LanguageManager.instance.currentLanguage);
            if (language != null)
                block.GetComponentInChildren<TextMeshProUGUI>().text = language.word;
            else 
                block.GetComponentInChildren<TextMeshProUGUI>().text = item.nickName;
            block.GetComponent<ArListBlock>().arAsset = item;
            block.GetComponent<ArListBlock>().page = page;
        }
    }
}
