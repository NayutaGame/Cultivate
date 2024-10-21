
using System;

public class SliderModel : WidgetModel
{
    private float _value;
    
    private Func<float> _getFunc;
    private Action<float> _setFunc;
    public float Value
    {
        get => _getFunc?.Invoke() ?? _value;
        set
        {
            _value = value;
            _setFunc?.Invoke(_value);
        }
    }

    public float MinValue;
    public float MaxValue;
    public bool WholeNumbers;

    public SliderModel(string name, Func<float> getFunc = null, Action<float> setFunc = null, int minValue = 0, int maxValue = 100, bool wholeNumbers = true) : base(name)
    {
        _getFunc = getFunc;
        _setFunc = setFunc;
        MinValue = minValue;
        MaxValue = maxValue;
        WholeNumbers = wholeNumbers;
    }

    public static SliderModel Default
        => new("默认Slider");
}
