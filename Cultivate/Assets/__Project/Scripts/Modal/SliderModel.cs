using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderModel
{
    public string Label;
    public float Value;
    public float MinValue;
    public float MaxValue;
    public bool WholeNumbers;

    public SliderModel(string label, int minValue, int maxValue, bool wholeNumbers)
    {
        Label = label;
        MinValue = minValue;
        MaxValue = maxValue;
        WholeNumbers = wholeNumbers;
    }

    public static SliderModel Default
        => new("默认Slider", 0, 100, true);
}
