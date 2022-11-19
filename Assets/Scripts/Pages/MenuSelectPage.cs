using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelectPage : Page
{
    public GameObject specialButton;
    protected override void Awake()
    {
        base.Awake();
        onPreShow.AddListener(Initialize);
    }

    void Initialize() {
        specialButton.SetActive(SelectedArObjectManager.instance.selectedArea == "Jazyky");
    }
}
