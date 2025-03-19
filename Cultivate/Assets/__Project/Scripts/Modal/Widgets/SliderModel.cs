
using System;

public class SliderModel : WidgetModel
{
    public int MinValue;
    public int MaxValue;
    
    public Action<int> _setFunc;
    public int DefaultValue;
    
    public int Value
    {
        get => Settings.GetData()[Name];
        set
        {
            Settings.GetData()[Name] = value;
            _setFunc?.Invoke(value);
        }
    }

    public SliderModel(
        string name,
        Settings settings,
        int minValue,
        int maxValue,
        Action<int> setFunc,
        int defaultValue) : base(name, settings)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        _setFunc = setFunc;
        DefaultValue = defaultValue;
    }

    public static SliderModel CreateWithDefaultRange(
        string name,
        Settings settings,
        Action<int> setFunc,
        int defaultValue)
        => new(name, settings, 0, 100, setFunc, defaultValue);
}
