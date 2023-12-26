
using System;

public class GainSkillTechEventDescriptor : TechEventDescriptor
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

    public GainSkillTechEventDescriptor(string description, Func<GainSkillDetails, RunTech, bool> cond) : base(description)
    {
        _cond = cond;
    }
}
