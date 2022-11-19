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
    public virtual TweenCallback Forward(float duration)
    {
        throw new NotImplementedException();
    }
    public virtual TweenCallback Back(float duration)
    {
        throw new NotImplementedException();
    }
    public virtual void Initialize()
    {

    }

    public override string ToString()
    {
        return $"transition: {startTime} {duration}";
    }
}

[System.Serializable]
public class RotateTransitionAction : TransitionAction {
    public Transform transform;
    public float pos;
    public float startPos;

    public override TweenCallback Forward(float duration)
    {
        transform.localEulerAngles  = new Vector3(0, 0, toGoal ? startPos : pos);
        return transform.DOLocalRotate(new Vector3(0, 0, toGoal ? pos : startPos), duration).onComplete;
    }

    public override TweenCallback Back(float duration)
    {
        transform.localEulerAngles = new Vector3(0, 0, toGoal ? pos : startPos);
        return transform.DOLocalRotate(new Vector3(0, 0, toGoal ? startPos : pos), duration).onComplete;
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

    public override TweenCallback Forward(float duration)
    {
        transform.anchoredPosition = toGoal ? startPos : pos;
        return transform.DOAnchorPos(toGoal ? pos : startPos, duration).onComplete;
    }

    public override TweenCallback Back(float duration)
    {
        transform.anchoredPosition = toGoal ? pos : startPos;
        return transform.DOAnchorPos(toGoal ? startPos : pos, duration).onComplete;
    }
}

[System.Serializable]
public class ScreenTransitionAction : TransitionAction
{
    public RectTransform transform;
    public RectTransform canvas;
    public Vector2 pos;
    public Vector2 startPos;
    public override void Initialize()
    {

    }

    public override TweenCallback Forward(float duration)
    {
        Vector2 pos = new Vector2(this.pos.x * canvas.sizeDelta.x, this.pos.y / canvas.sizeDelta.y);
        Vector2 startPos = new Vector2(this.startPos.x * canvas.sizeDelta.x, this.startPos.y / canvas.sizeDelta.y);
        transform.anchoredPosition = toGoal ? startPos : pos;
        return transform.DOAnchorPos(toGoal ? pos : startPos, duration).onComplete;
    }

    public override TweenCallback Back(float duration)
    {
        Vector2 pos = new Vector2(this.pos.x * canvas.sizeDelta.x, this.pos.y / canvas.sizeDelta.y);
        Vector2 startPos = new Vector2(this.startPos.x * canvas.sizeDelta.x, this.startPos.y / canvas.sizeDelta.y);
        transform.anchoredPosition = toGoal ? pos : startPos;
        return transform.DOAnchorPos(toGoal ? startPos : pos, duration).onComplete;
    }
}
