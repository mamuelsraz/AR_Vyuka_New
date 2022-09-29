using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[System.Serializable]
public class TransitionAction
{
    [Range(0f, 1f)]
    public float startTime;
    [Range(0f, 1f)]
    public float duration;
    public bool toGoal;
    public virtual TweenCallback Forward()
    {
        throw new NotImplementedException();
    }
    public virtual TweenCallback Back()
    {
        throw new NotImplementedException();
    }
    public virtual void Initialize()
    {

    }
}

[System.Serializable]
public class MoveTransitionAction : TransitionAction
{
    public RectTransform transform;
    public Vector2 pos;
    [HideInInspector] public Vector2 startPos;
    public override void Initialize()
    {
        startPos = transform.anchoredPosition;
    }

    public override TweenCallback Forward()
    {
        transform.anchoredPosition = toGoal ? startPos : pos;
        return transform.DOAnchorPos(toGoal ? pos : startPos, duration).onComplete;
    }

    public override TweenCallback Back()
    {
        transform.anchoredPosition = toGoal ? pos : startPos;
        return transform.DOAnchorPos(toGoal ? startPos : pos, duration).onComplete;
    }
}

[System.Serializable]
public class ScreenTransitionAction : TransitionAction
{
    public RectTransform transform;
    public Vector2 pos;
    [HideInInspector] public Vector2 startPos;
    public override void Initialize()
    {
        startPos = transform.anchoredPosition;
        startPos = new Vector2(startPos.x / Screen.width, startPos.y / Screen.height);
    }

    public override TweenCallback Forward()
    {
        Vector2 pos = new Vector2(this.pos.x * Screen.width, startPos.y / Screen.height);
        transform.anchoredPosition = toGoal ? startPos : pos;
        return transform.DOAnchorPos(toGoal ? pos : startPos, duration).onComplete;
    }

    public override TweenCallback Back()
    {
        Vector2 pos = new Vector2(this.pos.x * Screen.width, startPos.y / Screen.height);
        transform.anchoredPosition = toGoal ? pos : startPos;
        return transform.DOAnchorPos(toGoal ? startPos : pos, duration).onComplete;
    }
}
