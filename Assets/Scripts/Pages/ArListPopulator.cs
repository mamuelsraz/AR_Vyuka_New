using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArListPopulator : MonoBehaviour
{
    public SelectPage page;
    public Transform parent;
    public GameObject categoryPrefab;
    public GameObject blockPrefab;

    public void Populate(bool chemie)
    {
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }

        Dictionary<string, List<ArObject>> sortedCategories = new();
        foreach (var item in AssetStreamingManager.instance.ArObjectList)
        {
            if (item.area != SelectedArObjectManager.instance.selectedArea) continue;
            if (!sortedCategories.ContainsKey(item.category))
                sortedCategories.Add(item.category, new List<ArObject>());
            sortedCategories[item.category].Add(item);
        }
        foreach (var category in sortedCategories)
        {
            var obj = Instantiate(categoryPrefab, parent);
            obj.name = category.Key.ToString();
            obj.GetComponentInChildren<TextMeshProUGUI>().text = category.Key.ToString(); // zmenit
            foreach (var arObj in category.Value)
            {
                var block = Instantiate(blockPrefab, obj.transform);
                block.GetComponentInChildren<TextMeshProUGUI>().text = arObj.nickName;
                block.GetComponent<ArListBlock>().arObject = arObj;
                block.GetComponent<ArListBlock>().page = page;
            }
        }
    }
}
