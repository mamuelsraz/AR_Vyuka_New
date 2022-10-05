using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class Page : MonoBehaviour
{
    [HideInInspector] public RectTransform rect;
    public Transition transition;
    public bool active;

    [HideInInspector] public UnityEvent onPreShow;
    [HideInInspector] public UnityEvent onPostShow;
    [HideInInspector] public UnityEvent onPreHide;
    [HideInInspector] public UnityEvent onPostHide;

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        onPreShow.AddListener(ActivatePanel);
        onPostHide.AddListener(DeactivatePanel);
    }

    public void GoTo(Page page) {
        transition.screenActions[0].transform = rect;
        Debug.Log(rect);
        Debug.Log(page.gameObject.name);
        transition.screenActions[1].transform = page.rect;
        onPreHide.Invoke();
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
        };
        handle.OnCancel += () =>
        {
            to.onPreHide.Invoke();
            from.onPreShow.Invoke();
            SubToHandle(handle.cancelHandle, to, from);
        };
    }

    void ActivatePanel() {
        Debug.Log(gameObject.name);
        active = true;
        gameObject.SetActive(true);
    }

    void DeactivatePanel() {
        Debug.LogError(gameObject.name + " " + active);
        active = false;
        gameObject.SetActive(false);
    }
}
