using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Transition : MonoBehaviour
{
    public float duration;
    public List<MoveTransitionAction> moveActions;
    public List<ScreenTransitionAction> screenActions;
    public UnityEvent OnStart;
    public UnityEvent OnFinish;
    protected bool started;
    protected bool back;
    float timeSinceStart;
    List<TransitionAction> actionsInTime;
    Queue<TransitionAction> actionsQueue;
    protected float t
    {
        get { return Mathf.InverseLerp(0, duration, timeSinceStart); }
    }

    private void Awake()
    {
        actionsInTime = new List<TransitionAction>();
        foreach (var newAction in moveActions)
        {
            newAction.Initialize();

            int index = 0;
            for (int i = 1; i < actionsInTime.Count; i++)
            {
                if (actionsInTime[i].startTime > newAction.startTime)
                {
                    index = i - 1;
                    break;
                }
            }
            actionsInTime.Insert(index, newAction);
        }
        foreach (var newAction in screenActions)
        {
            newAction.Initialize();

            int index = 0;
            for (int i = 1; i < actionsInTime.Count; i++)
            {
                if (actionsInTime[i].startTime > newAction.startTime)
                {
                    index = i - 1;
                    break;
                }
            }
            actionsInTime.Insert(index, newAction);
        }
    }

    public virtual void StartTransition(bool back = false)
    {
        OnStart.Invoke();
        started = true;
        timeSinceStart = 0;
        actionsQueue = new Queue<TransitionAction>(actionsInTime);
        this.back = back;
    }

    private void Update()
    {
        if (!started) return;

        timeSinceStart += Time.deltaTime;
        UpdateTransition();

        if (timeSinceStart > duration)
            EndTransition();
    }
    protected virtual void EndTransition()
    {
        started = false;
        OnFinish.Invoke();
    }

    protected virtual void UpdateTransition()
    {
        if (actionsQueue == null) return;
        while ((back ? 1f - actionsQueue.Peek().startTime : actionsQueue.Peek().startTime) > t)
        {
            var action = actionsQueue.Dequeue();
            if (back) action.Back();
            else action.Forward();
        }
    }
}
