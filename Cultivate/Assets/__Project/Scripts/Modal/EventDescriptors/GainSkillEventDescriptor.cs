using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainSkillEventDescriptor : EventDescriptor
{
    public Func<GainSkillDetails, RunTech, bool> _cond;

    public override void Register(RunTech runTech)
    {
        RunManager.Instance.GainSkillEvent += runTech.GainSkill;
    }

    public override void Unregister(RunTech runTech)
    {
        RunManager.Instance.GainSkillEvent -= runTech.GainSkill;
    }

    public GainSkillEventDescriptor(string description, Func<GainSkillDetails, RunTech, bool> cond) : base(description)
    {
        _cond = cond;
    }
}
