
using System;
using System.Collections.Generic;

public class SettingsTab : Addressable
{
    public string Name;
    public WidgetListModel Widgets;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public SettingsTab(string name, WidgetListModel widgets)
    {
        _accessors = new()
        {
            { "Widgets",                        () => Widgets },
        };
        
        Name = name;
        Widgets = widgets;
    }
}
