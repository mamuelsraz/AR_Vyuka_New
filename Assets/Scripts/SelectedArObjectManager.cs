using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedArObjectManager : MonoBehaviour
{
    public static SelectedArObjectManager instance;

    [HideInInspector] public ArObject selectedArObject { get; private set; }
    [HideInInspector] public GameObject selectedObject { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
    }

    public void PlaceNew(ArObject arObject)
    {
        if (selectedObject != null && selectedArObject.bundle == arObject.bundle) return;

        if (selectedObject != null) selectedObject.SetActive(false);
        selectedArObject = arObject;
        selectedObject = AssetStreamingManager.instance.cachedArObjects[selectedArObject];
        selectedObject.SetActive(true);
    }
}
