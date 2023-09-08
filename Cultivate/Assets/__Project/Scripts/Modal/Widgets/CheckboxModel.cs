using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckboxModel : WidgetModel
{
    public bool Value;

    public CheckboxModel(string name) : base(name) { }

    public void Toggle()
    {
        Value = !Value;
    }

    public static CheckboxModel Default
        => new("默认Checkbox");
}
