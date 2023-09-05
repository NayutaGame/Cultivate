using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedOptionSignal : Signal
{
    public int Selected;

    public SelectedOptionSignal(int selected)
    {
        Selected = selected;
    }
}
