using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOperationEntry : OperationEntry
{
    private Func<OperationProduct, bool> _canClick;
    private Action<OperationProduct> _click;

    public ClickOperationEntry(string name, string description, int cost, Func<OperationProduct, bool> canClick, Action<OperationProduct> click, List<ILock> locks = null) : base(name, description, cost, locks)
    {
        _canClick = canClick;
        _click = click;
    }

    public override bool IsClick => true;

    public bool CanClick(OperationProduct product) => _canClick(product);

    public void Click(OperationProduct product) => _click(product);
}
