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
        SkillSlot slotWithSkill = RunManager.Instance.Environment.Home.FindSlotWithSkillEntry(PlayerSkill.GetEntry());
        RunSkill skillInHand = RunManager.Instance.Environment.FindSkillInHandWithEntry(PlayerSkill.GetEntry());
        bool inSlot = slotWithSkill != null;
        bool inSkillInventory = skillInHand != null;

        return inSlot || inSkillInventory;
    }
}
