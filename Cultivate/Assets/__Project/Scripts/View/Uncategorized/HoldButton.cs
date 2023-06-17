using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : Selectable
{
    private bool Holding;

    protected override void Awake()
    {
        base.Awake();
        _updateAction = NullAction;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        _updateAction = HoldAction ?? NullAction;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        _updateAction = NullAction;
    }

    private void Update() => _updateAction();
    private Action _updateAction = NullAction;
    public Action HoldAction;

    private static void NullAction() { }
}
