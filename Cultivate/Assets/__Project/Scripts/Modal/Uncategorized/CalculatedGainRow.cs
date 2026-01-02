
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class CalculatedGainRow : GainRow
{
    [SerializeField] private string _pointDescription;
    private Dirty<int> _currentCount;
    [SerializeField] private int _extraCreditPerPoint;

    public CalculatedGainRow(
        string pointDescription,
        Func<int> currentCountGetter,
        int extraCreditPerPoint)
    {
        _pointDescription = pointDescription;
        _currentCount = new Dirty<int>(currentCountGetter ?? (() => 0));
        _extraCreditPerPoint = extraCreditPerPoint;
    }

    public CalculatedGainRow() : this(default, default, default)
    {
        
    }

    public override void InvalidateCache()
    {
        _currentCount.SetDirty();
    }

    public override int CalculateGain()
    {
        return _currentCount.Value * _extraCreditPerPoint;
    }

    public override GainStyle GetGainStyle()
    {
        return CalculateGain() switch
        {
            > 0 => GainStyle.Positive,
            < 0 => GainStyle.Negative,
            _ => GainStyle.Inactive
        };
    }

    public override Description GetDescriptionText()
    {
        return _pointDescription;
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
