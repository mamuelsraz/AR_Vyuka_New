using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedArObjectManager : MonoBehaviour
{
    public static SelectedArObjectManager instance;
    public string selectedArea;
    [HideInInspector] public LanguageARAsset selectedARAsset { get; private set; }
    [HideInInspector] public GameObject selectedObject { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
    }

    public void SelectNew(LanguageARAsset arAsset)
    {
        if (selectedObject != null && selectedARAsset.asset == arAsset.asset) return;

        if (selectedObject != null) selectedObject.SetActive(false);
        selectedARAsset = arAsset;
        selectedObject = AddressablesStreamingManager.Instance.cachedARAssets[selectedARAsset];
    }

    public GameObject SpawnCurrent(Transform parent) {
        selectedObject.transform.parent = parent;
        selectedObject.transform.localPosition = Vector3.zero;
        selectedObject.transform.localScale = Vector3.one;
        selectedObject.transform.localRotation = Quaternion.identity;
        selectedObject.SetActive(true);
        return selectedObject;
    }
}
