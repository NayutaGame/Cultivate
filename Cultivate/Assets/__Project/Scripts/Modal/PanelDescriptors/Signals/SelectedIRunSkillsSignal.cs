using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedIRunSkillsSignal : Signal
{
    public List<object> Selected;

    public SelectedIRunSkillsSignal(List<object> selected)
    {
        Selected = selected;
    }
}
