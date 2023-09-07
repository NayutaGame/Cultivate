using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonModel
{
    public string Label;

    public ButtonModel(string label)
    {
        Label = label;
    }

    public void Click()
    {

    }

    public static ButtonModel Default
        => new("默认Button");
}
