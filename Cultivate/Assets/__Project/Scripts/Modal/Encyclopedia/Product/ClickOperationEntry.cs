using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOperationEntry : OperationEntry
{
    private Func<bool> _canClick;
    private Action _click;

    public ClickOperationEntry(string name, string description, int cost, Func<bool> canClick, Action click) : base(name, description, cost)
    {
        _canClick = canClick;
        _click = click;
    }

    public override bool IsClick => true;
}
