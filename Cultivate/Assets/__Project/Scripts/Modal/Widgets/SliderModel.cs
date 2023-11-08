
using System;

public class SliderModel : WidgetModel
{
    private float _value;

    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            _setFunc?.Invoke(_value);
        }
    }

    public float MinValue;
    public float MaxValue;
    public bool WholeNumbers;

    private Action<float> _setFunc;

    public SliderModel(string name, int minValue, int maxValue, bool wholeNumbers, Action<float> setFunc = null) : base(name)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        WholeNumbers = wholeNumbers;
        _setFunc = setFunc;
    }

    public static SliderModel Default
        => new("默认Slider", 0, 100, true);
}
