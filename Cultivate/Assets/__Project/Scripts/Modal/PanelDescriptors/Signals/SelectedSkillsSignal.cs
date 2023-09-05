using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSkillsSignal : Signal
{
    public List<RunSkill> Selected;

    public SelectedSkillsSignal(List<RunSkill> selected)
    {
        Selected = selected;
    }
}
