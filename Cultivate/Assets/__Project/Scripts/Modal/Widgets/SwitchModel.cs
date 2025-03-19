
using System;
using System.Collections.Generic;

public class SwitchModel : WidgetModel
{
    public List<string> Options;
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

    public SwitchModel(
        string name,
        Settings settings,
        List<string> options,
        Action<int> setFunc,
        int defaultValue
        ) : base(name, settings)
    {
        Options = options;
        _setFunc = setFunc;
        DefaultValue = defaultValue;
    }

    public string GetContentText()
        => Options[Value];

    public void Prev()
    {
        Value = (Value + Options.Count - 1) % Options.Count;
    }

    public void Next()
    {
        Value = (Value + 1) % Options.Count;
    }
}
