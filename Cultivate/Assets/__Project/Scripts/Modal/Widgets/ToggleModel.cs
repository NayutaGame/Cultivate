using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToggleModel : WidgetModel
{
    public bool Value;

    public ToggleModel(string name) : base(name) { }

    public void Toggle()
    {
        Value = !Value;
    }

    public static ToggleModel Default
        => new("默认Toggle");
}
