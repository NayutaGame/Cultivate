using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNeiGong
{
    private RunSkill _runSkill;

    public StageNeiGong(RunSkill runSkill)
    {
        _runSkill = runSkill;

        if (_runSkill != null)
        {
            // register
        }
    }

    public string GetName()
    {
        if (_runSkill == null) return "空";
        return _runSkill.GetName();
    }
}
