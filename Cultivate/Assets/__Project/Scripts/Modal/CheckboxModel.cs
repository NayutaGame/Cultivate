using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckboxModel
{
    public string Label;
    public bool Value;

    public CheckboxModel(string label)
    {
        Label = label;
    }

    public void Toggle()
    {
        Value = !Value;
    }

    public static CheckboxModel Default
        => new("默认Checkbox");
}
