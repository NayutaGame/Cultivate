
using System;
using UnityEngine;

public class DialogOption
{
    public string Text;

    private RunCostDetails _runCostDetails;
    public DialogOption SetCost(RunCostDetails runCostDetails)
    {
        _runCostDetails = runCostDetails;
        return this;
    }

    private Func<DialogOption, Cell> _select;
    public DialogOption SetSelect(Func<DialogOption, Cell> select)
    {
        _select = select;
        return this;
    }

    private DialogOption(string text, Func<DialogOption, Cell> select)
    {
        Text = text;
        _runCostDetails = RunCostDetails.Default;
        _select = select ?? DefaultSelect;
    }

    public static DialogOption FromTextAndSelect(string text, Func<DialogOption, Cell> select)
    {
        return new(text, select);
    }

    public static DialogOption FromText(string text)
    {
        return new(text, null);
    }

    public bool CanSelect()
    {
        return _runCostDetails.CanCost();
    }

    public Cell Select()
    {
        _runCostDetails.Cost();
        return _select(this);
    }

    private Cell DefaultSelect(DialogOption dialogOption)
    {
        _runCostDetails.Cost();
        return null;
    }

    public static implicit operator DialogOption(string text) => new(text, null);
}
