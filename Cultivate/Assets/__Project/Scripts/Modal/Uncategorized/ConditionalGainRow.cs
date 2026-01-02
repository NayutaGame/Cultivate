
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class ConditionalGainRow : GainRow
{
    [SerializeField] private string _condDescription;
    private Dirty<int> _currentValue;
    private Dirty<bool> _cond;
    [SerializeField] private int _extraCredit;

    public ConditionalGainRow(
        string condDescription,
        Func<int> currentValueGetter,
        Func<bool> condGenerator,
        int extraCredit)
    {
        _condDescription = condDescription;
        _currentValue = new Dirty<int>(currentValueGetter ?? (() => 0));
        _cond = new Dirty<bool>(condGenerator ?? (() => false));
        _extraCredit = extraCredit;
    }

    public ConditionalGainRow() : this(default, default, default, default)
    {
        
    }

    public override void InvalidateCache()
    {
        _currentValue.SetDirty();
        _cond.SetDirty();
    }

    public override int CalculateGain()
    {
        return _extraCredit;
        // return _cond.Value ? _extraCredit : 0;
    }

    public override GainStyle GetGainStyle()
    {
        return _cond.Value
            ? (_extraCredit >= 0 ? GainStyle.Positive : GainStyle.Negative)
            : GainStyle.Inactive;
    }

    public override Description GetDescriptionText()
    {
        return _condDescription;
    }

    public override string GetScoreText()
    {
        int gain = CalculateGain();
        if (gain > 0)
        {
            return $"+{gain}";
        }
        else if (gain < 0)
        {
            return $"{gain}";
        }
        else
        {
            return "0";
        }
    }
}
