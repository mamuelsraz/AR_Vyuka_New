using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ArListSection : MonoBehaviour
{
    bool canClick = true;
    public RectTransform rt;
    public ContentSizeFitter fitter;
    public Transform arrow;
    float height;

    private void Start()
    {
        height = rt.sizeDelta.y;
        /*fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;*/
    }

    bool toggled = true;
    public void Toggle() {
        if (!canClick) return;

        if (toggled) Open();
        else Close();
        toggled = !toggled;
    }

    void Open() {
        fitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
        Canvas.ForceUpdateCanvases();
        float newHeight = rt.sizeDelta.y;
        fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
        canClick = false;

        arrow.DORotate(new Vector3(0, 0, 180), 0.5f);

        rt.DOSizeDelta(new Vector2(rt.sizeDelta.x, newHeight), 0.5f).onComplete += () =>
        {
            fitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
            canClick = true;
        };
    }

    void Close() {
        fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        canClick = false;
        arrow.DORotate(new Vector3(0, 0, 0), 0.5f);

        rt.DOSizeDelta(new Vector2(rt.sizeDelta.x, height), 0.5f).onComplete += () =>
        {
            canClick = true;
        };
    }
}
