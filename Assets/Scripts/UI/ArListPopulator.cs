using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArListPopulator : MonoBehaviour
{
    public Transform parent;
    public GameObject categoryPrefab;
    public GameObject blockPrefab;

    private void Start()
    {
        if (AssetStreamingManager.instance.ArObjectList.Length > 0) Populate();
        else AssetStreamingManager.instance.LoadList(AssetStreamingManager.path, "list").Complete += (StreamingHandleResponse response) =>
        {
            if (response.status != StreamingStatus.Failed)
                Populate();
        };
    }

    void Populate()
    {
        Dictionary<ArObjCategories, List<ArObject>> sortedCategories = new ();
        foreach (var item in AssetStreamingManager.instance.ArObjectList)
        {
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
            }
        }

        Debug.Log("finished");
        //var contentSizeFitter = parent.GetComponent<ContentSizeFitter>();
        //contentSizeFitter.enabled = false;
        //Canvas.ForceUpdateCanvases();
        //contentSizeFitter.enabled = true;
        //Canvas.ForceUpdateCanvases();
    }
}
