using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class BarterItem : Addressable
{
    public RunSkill PlayerSkill;
    public RunSkill Skill;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public BarterItem(RunSkill playerSkill, RunSkill skill)
    {
        _accessors = new()
        {
            { "PlayerSkill",         () => PlayerSkill },
            { "Skill",               () => Skill },
        };
        PlayerSkill = playerSkill;
        Skill = skill;
    }

    public bool Affordable()
    {
        bool inSlot = RunManager.Instance.Battle.Home.TraversalCurrentSlots().Any(s => s.Skill == PlayerSkill);
        bool inSkillInventory = RunManager.Instance.Battle.SkillInventory.Contains(PlayerSkill);

        return inSlot || inSkillInventory;
    }
}
