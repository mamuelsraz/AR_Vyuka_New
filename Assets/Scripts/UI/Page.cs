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

    public void GoBack(Page page) {
        transition.screenActions[0].transform = page.rect;
        transition.screenActions[1].transform = rect;
        onPreHide?.Invoke();
        page.onPreShow?.Invoke();

        var handle = transition.StartTransition(true);
        SubToHandle(handle, this, page);
    }


    public void GoTo(Page page)
    {
        transition.screenActions[0].transform = rect;
        transition.screenActions[1].transform = page.rect;
        onPreHide?.Invoke();
        page.onPreShow?.Invoke();

        var handle = transition.StartTransition();
        SubToHandle(handle, this, page);
    }

    public void SubToHandle(TransitionHandle handle, Page from, Page to) {
        handle.OnFinish += () =>
        {
            onPostHide?.Invoke();
            to.onPostShow?.Invoke();
        };
        handle.OnCancel += (TransitionHandle handle) =>
        {
            to.onPreHide?.Invoke();
            from.onPreShow?.Invoke();
            SubToHandle(handle, to, from);
        };
    }

    void ActivatePanel() {
        active = true;
        gameObject.SetActive(true);
    }

    void DeactivatePanel() {
        active = false;
        //gameObject.SetActive(false);
    }
}
