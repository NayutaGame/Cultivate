
using System;
using UnityEngine;

public class DialogOption
{
    public string Text;

    private CostDetails _costDetails;
    public DialogOption SetCost(CostDetails costDetails)
    {
        _costDetails = costDetails;
        return this;
    }

    private Func<DialogOption, PanelDescriptor> _select;
    public DialogOption SetSelect(Func<DialogOption, PanelDescriptor> select)
    {
        _select = select;
        return this;
    }

    private DialogOption(string text, Func<DialogOption, PanelDescriptor> select)
    {
        Text = text;
        _costDetails = CostDetails.Default;
        _select = select ?? DefaultSelect;
    }

    public static DialogOption FromTextAndSelect(string text, Func<DialogOption, PanelDescriptor> select)
    {
        return new(text, select);
    }

    public static DialogOption FromText(string text)
    {
        return new(text, null);
    }

    public bool CanSelect()
    {
        return _costDetails.CanCost();
    }

    public PanelDescriptor Select()
    {
        _costDetails.Cost();
        return _select(this);
    }

    private PanelDescriptor DefaultSelect(DialogOption dialogOption)
    {
        _costDetails.Cost();
        return null;
    }

    public static implicit operator DialogOption(string text) => new(text, null);
}
