using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsContentModel
{
    public string Name;
    public WidgetModel[] Widgets;

    public SettingsContentModel(string name, WidgetModel[] widgets)
    {
        Name = name;
        Widgets = widgets;
    }
}
