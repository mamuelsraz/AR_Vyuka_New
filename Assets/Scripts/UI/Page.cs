using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class Page : MonoBehaviour
{
    [HideInInspector] RectTransform rect;
    public Transition transition;
    public UnityEvent onPreShow;
    public UnityEvent onPostShow;
    public UnityEvent onPreHide;
    public UnityEvent onPostHide;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void GoTo(Page page) {
        transition.screenActions[0].transform = rect;
        transition.screenActions[1].transform = page.rect;
        onPreHide.Invoke();
        page.gameObject.SetActive(true);
        page.onPreShow.Invoke();

        var handle = transition.StartTransition();
        SubToHandle(handle, this, page);
    }

    public void SubToHandle(TransitionHandle handle, Page from, Page to) {
        //recursive, crash incoming?
        handle.OnFinish += () =>
        {
            from.onPostHide.Invoke();
            to.onPostShow.Invoke();
            from.gameObject.SetActive(false);
        };
        handle.OnCancel += () =>
        {
            to.onPreHide.Invoke();
            from.onPreShow.Invoke();
            SubToHandle(handle.cancelHandle, to, from);
        };
    }
}
