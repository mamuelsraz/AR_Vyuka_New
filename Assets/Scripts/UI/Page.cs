using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class Page : MonoBehaviour
{
    [HideInInspector] public RectTransform rect;
    [HideInInspector] public Page previousPage = null;
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
        gameObject.SetActive(active);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        if (previousPage == null)
        {
            Application.Quit();
            return;
        }
        transition.screenActions[0].transform = previousPage.rect;
        transition.screenActions[1].transform = rect;
        onPreHide?.Invoke();
        previousPage.onPreShow?.Invoke();

        var handle = transition.StartTransition(true);
        SubToHandle(handle, this, previousPage);
    }


    public void GoTo(Page page)
    {
        transition.screenActions[0].transform = rect;
        transition.screenActions[1].transform = page.rect;
        onPreHide?.Invoke();
        page.onPreShow?.Invoke();
        page.previousPage = this;
        var handle = transition.StartTransition();
        SubToHandle(handle, this, page);
    }

    public void SubToHandle(TransitionHandle handle, Page from, Page to)
    {
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

    void ActivatePanel()
    {
        active = true;
        gameObject.SetActive(true);
    }

    void DeactivatePanel()
    {
        active = false;
        gameObject.SetActive(false);
    }
}
