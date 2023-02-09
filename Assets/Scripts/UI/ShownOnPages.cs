using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShownOnPages : MonoBehaviour
{
    public List<Page> pages;

    private void Awake()
    {
        foreach (var item in pages)
        {
            item.onPreHide.AddListener(Hide);
            item.onPreShow.AddListener(Show);
        };
    }

    void Show() {
        gameObject.SetActive(true);
    }

    void Hide() {
        gameObject.SetActive(false);
    }
}
