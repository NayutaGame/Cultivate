using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class BarterPanelDescriptor : PanelDescriptor
{
    private BarterInventory _inventory;

    public BarterPanelDescriptor()
    {
        _accessors = new()
        {
            { "Inventory",                () => _inventory },
        };
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();

        _inventory = new();

        Pool<RunSkill> pool = new Pool<RunSkill>();
        pool.Populate(RunManager.Instance.Environment.Home.TraversalCurrentSlots()
            .FilterObj(s => s.Skill != null && s.Skill is RunSkill).Map(s => s.Skill as RunSkill));
        pool.Populate(RunManager.Instance.Environment.Hand.Traversal());
        pool.Shuffle();

        int count = Mathf.Min(pool.Count(), 6);

        RunSkill[] playerSkills = new RunSkill[count];
        for (int i = 0; i < playerSkills.Length; i++)
        {
            pool.TryPopItem(out playerSkills[i]);
        }

        RunSkill[] skills = new RunSkill[count];
        for (int i = 0; i < skills.Length; i++)
        {
            RunSkill thisSkill = playerSkills[i];
            RunManager.Instance.Environment.SkillPool.TryDrawSkill(out skills[i],
                pred: skillEntry => !skills.FilterObj(s => s != null).Map(s => s.GetEntry()).Contains(skillEntry) && skillEntry != thisSkill.GetEntry(), jingJie: playerSkills[i].JingJie);
        }

        for (int i = 0; i < playerSkills.Length; i++)
        {
            _inventory.Add(new BarterItem(playerSkills[i], skills[i]));
        }

        RunManager.Instance.Environment.SkillPool.Populate(_inventory.Traversal().Map(b => b.Skill.GetEntry()));
    }

    public bool Exchange(BarterItem barterItem)
    {
        if (!_inventory.Contains(barterItem))
            return false;

        SkillSlot slotWithSkill = RunManager.Instance.Environment.Home.FindSlotWithSkill(barterItem.PlayerSkill);
        RunSkill skillInHand = RunManager.Instance.Environment.FindSkillInHand(barterItem.PlayerSkill);
        bool inSlot = slotWithSkill != null;
        bool inSkillInventory = skillInHand != null;

        if (!inSlot && !inSkillInventory)
            return false;

        if (inSlot)
        {
            slotWithSkill.Skill = null;
        }
        else
        {
            RunManager.Instance.Environment.Hand.Remove(barterItem.PlayerSkill);
        }

        RunManager.Instance.Environment.Hand.Add(barterItem.Skill);

        _inventory.Remove(barterItem);
        return true;
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal) => null;
}
