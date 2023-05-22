
using System;
using UnityEngine;

public class DialogOption
{
    public string Text;

    public CostDetails Cost;

    public Action<DialogOption> _select;

    public DialogOption(string text, CostDetails cost = null, Action<DialogOption> select = null)
    {
        Text = text;
        Cost = cost;
        _select = select ?? DefaultSelect;
    }

    public bool CanSelect()
    {
        return Cost?.CanCost() ?? true;
    }

    public void Select()
    {
        Cost?.Cost();
        _select(this);
    }

    private void DefaultSelect(DialogOption dialogOption)
    {
        Cost?.Cost();
        RunManager.Instance.Map.TryFinishNode();
    }

    public static implicit operator DialogOption(string text) => new(text);
}
