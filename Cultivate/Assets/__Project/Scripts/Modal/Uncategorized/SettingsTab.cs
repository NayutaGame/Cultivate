
using System;
using System.Collections.Generic;

public class SettingsTab : Addressable
{
    public string Name;
    public WidgetListModel Widgets;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Widgets",                    thisObject => ((SettingsTab)thisObject).Widgets },
    };
    public object Get(string s) => Accessor[s](this);
    public SettingsTab(string name, WidgetListModel widgets)
    {
        Name = name;
        Widgets = widgets;
    }
}
