using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderModel : WidgetModel
{
    public float Value;
    public float MinValue;
    public float MaxValue;
    public bool WholeNumbers;

    public SliderModel(string name, int minValue, int maxValue, bool wholeNumbers) : base(name)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        WholeNumbers = wholeNumbers;
    }

    public static SliderModel Default
        => new("默认Slider", 0, 100, true);
}
