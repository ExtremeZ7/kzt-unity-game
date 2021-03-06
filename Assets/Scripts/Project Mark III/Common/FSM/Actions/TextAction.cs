﻿using UnityEngine;
using Common.FSM;

public class TextAction : FSMAction
{
    string textToShow;
    float duration;
    float cachedDuration;
    string finishEvent;

    public TextAction(FSMState owner)
        : base(owner)
    {
    }

    public void Init(string textToShow, float duration, string finishEvent)
    {
        this.textToShow = textToShow;
        this.duration = duration;
        cachedDuration = duration;
        this.finishEvent = finishEvent;
    }

    public override void OnEnter()
    {
        if (duration <= 0)
        {
            Finish();
            return;
        }
    }

    public override void OnUpdate()
    {
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            Finish();
            return;
        }

        Debug.Log(textToShow);
    }

    public override void OnExit()
    {

    }

    public void Finish()
    {
        if (!string.IsNullOrEmpty(finishEvent))
        {
            GetOwner().SendEvent(finishEvent);
        }
        duration = cachedDuration;
    }
}
