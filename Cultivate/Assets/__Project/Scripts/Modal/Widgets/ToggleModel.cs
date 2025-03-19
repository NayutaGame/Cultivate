
using System;

public class ToggleModel : WidgetModel
{
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

    public bool IsOn
        => Value > 0;

    public ToggleModel(
        string name,
        Settings settings,
        Action<int> setFunc,
        int defaultValue
        ) : base(name, settings)
    {
        _setFunc = setFunc;
        DefaultValue = defaultValue;
    }

    public void Toggle()
    {
        Value = 1 - Value;
    }
}
