using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicViewPage : Page
{
    public TextMeshProUGUI text;

    protected override void Awake()
    {
        base.Awake();
        onPreShow.AddListener(Initialize);
    }

    void Initialize() {
        text.text = SelectedArObjectManager.instance.selectedArObject.nickName;
    }
}
