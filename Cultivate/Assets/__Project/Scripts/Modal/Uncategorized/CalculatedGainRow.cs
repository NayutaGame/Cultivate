
using System;
using CLLibrary;
using UnityEngine;

public class CalculatedGainRow : GainRow
{
    private string _pointDescription;
    private Dirty<int> _currentCount;
    private int _extraCreditPerPoint;

    public CalculatedGainRow(
        string pointDescription,
        Func<int> currentCountGetter,
        int extraCreditPerPoint)
    {
        _pointDescription = pointDescription;
        _currentCount = new Dirty<int>(currentCountGetter ?? (() => 0));
        _extraCreditPerPoint = extraCreditPerPoint;
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

    public override Description GetDescription()
    {
        Description description = new Description();
        description.Join(_pointDescription);
        description.Join($"\t当前{_currentCount.Value} → +{CalculateGain()}");
        return description;
    }
}
