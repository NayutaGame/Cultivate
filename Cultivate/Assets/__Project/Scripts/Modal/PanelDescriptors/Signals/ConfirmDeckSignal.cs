using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmDeckSignal : Signal
{
    public List<object> Selected;

    public ConfirmDeckSignal(List<object> selected)
    {
        Selected = selected;
    }
}
