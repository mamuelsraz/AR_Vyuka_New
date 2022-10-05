using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPage : Page
{
    public ArListPopulator populator;
    public Page placePage;
    [HideInInspector] public bool loadChemie;

    private void Awake()
    {
        base.Awake();
        onPostShow.AddListener(Initialize);
    }

    private void Initialize()
    {
        if (AssetStreamingManager.instance.ArObjectList.Length > 0) populator.Populate(loadChemie);
        else AssetStreamingManager.instance.LoadList(AssetStreamingManager.path, "list").Complete += (StreamingHandleResponse response) =>
        {
            if (response.status != StreamingStatus.Failed)
                populator.Populate(loadChemie);
        };
    }

    

    public void ChangePage()
    {
        GoTo(placePage);
    }
}
