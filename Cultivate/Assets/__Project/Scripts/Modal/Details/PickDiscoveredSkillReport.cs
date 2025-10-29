
using System;
using UnityEngine;

[Serializable]
public class PickDiscoveredSkillReport : TestReport
{
    [SerializeReference]
    public SkillReference PickedSkill;

    public PickDiscoveredSkillReport()
    {
        
    }

    public static PickDiscoveredSkillReport FromEnvironment(RunEnvironment env)
    {
        return new();
    }

    public override void OnEnter(RunEnvironment env)
    {
        env.PickDiscoveredSkillNeuron.Add(WritePickDiscoveredSkill);
        base.OnEnter(env);
    }

    public override void OnExit(RunEnvironment env)
    {
        env.PickDiscoveredSkillNeuron.Remove(WritePickDiscoveredSkill);
        base.OnExit(env);
    }

    private void WritePickDiscoveredSkill(PickDiscoveredSkillDetails d)
    {
        PickedSkill = d.Skill.Clone();
    }
}
