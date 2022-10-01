using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
    List<TransitionAction> backwardsActionsInTime;
    Queue<TransitionAction> actionsQueue;
    bool toggle;

    protected float t
    {
        get { return Mathf.InverseLerp(0, duration, timeSinceStart); }
    }

    private void Awake()
    {
        List<TransitionAction> actions = new List<TransitionAction>(moveActions.Count + screenActions.Count);
        actionsInTime = new List<TransitionAction>();
        actions.AddRange(moveActions);
        actions.AddRange(screenActions);

        foreach (var newAction in actions)
        {
            newAction.Initialize();

            int index = 0;
            for (int i = 0; i < actionsInTime.Count; i++)
            {
                if (actionsInTime[i].startTime > newAction.startTime)
                {
                    index = i;
                    break;
                }
                else if (i == actionsInTime.Count - 1) index = actionsInTime.Count;
            }
            index = Mathf.Clamp(index, 0, int.MaxValue);
            actionsInTime.Insert(index, newAction);
        }
        backwardsActionsInTime = new List<TransitionAction>(actionsInTime);
        backwardsActionsInTime.Reverse();

        foreach (var item in actionsInTime)
        {
            Debug.Log(item);
        }
    }

    public virtual void StartTransition(bool back = false)
    {
        OnStart.Invoke();
        started = true;
        timeSinceStart = 0;
        actionsQueue = new Queue<TransitionAction>(back ? backwardsActionsInTime : actionsInTime);

        this.back = back;
        Debug.Log("#####################");
    }

    public void ToggleAndStartTransition()
    {
        StartTransition(toggle);
        toggle = !toggle;
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
        if (actionsQueue.Count < 1) return;
        float currentMinTime = back ? 1 - (actionsQueue.Peek().startTime + actionsQueue.Peek().duration) : actionsQueue.Peek().startTime;
        while (actionsQueue.Count > 0 && currentMinTime < t)
        {
            var action = actionsQueue.Dequeue();
            if (back) action.Back(duration);
            else action.Forward(duration);
            if (actionsQueue.Count > 0)
                currentMinTime = back ? 1 - (actionsQueue.Peek().startTime + actionsQueue.Peek().duration) : actionsQueue.Peek().startTime;
        }
    }
}
