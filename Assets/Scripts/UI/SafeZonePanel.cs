using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeZonePanel : MonoBehaviour
{
    RectTransform rt;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        Rect rct = Screen.safeArea;
        rt.rect.Set(rct.x, rct.y, rct.width, rct.height);
    }
}
