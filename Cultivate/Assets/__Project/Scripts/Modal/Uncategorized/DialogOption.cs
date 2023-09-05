
using System;
using UnityEngine;

public class DialogOption
{
    public string Text;

    public CostDetails Cost;

    public Func<DialogOption, PanelDescriptor> _select;

    public DialogOption(string text, CostDetails cost = null, Func<DialogOption, PanelDescriptor> select = null)
    {
        Text = text;
        Cost = cost;
        _select = select ?? DefaultSelect;
    }

    public bool CanSelect()
    {
        return Cost?.CanCost() ?? true;
    }

    public DialogOption SetSelect(Func<DialogOption, PanelDescriptor> select)
    {
        _select = select;
        return this;
    }

    public PanelDescriptor Select()
    {
        Cost?.Cost();
        return _select(this);
    }

    private PanelDescriptor DefaultSelect(DialogOption dialogOption)
    {
        Cost?.Cost();
        return null;
    }

    public static implicit operator DialogOption(string text) => new(text);
}
