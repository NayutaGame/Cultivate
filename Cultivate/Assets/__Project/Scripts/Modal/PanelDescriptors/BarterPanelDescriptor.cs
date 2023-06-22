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
        pool.Populate(RunManager.Instance.Battle.Hero.TraversalCurrentSlots()
            .FilterObj(s => s.Skill != null).Map(s => s.Skill));
        pool.Populate(RunManager.Instance.Battle.SkillInventory);
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
            RunManager.Instance.SkillPool.TryDrawSkill(out skills[i],
                pred: skillEntry => !skills.FilterObj(s => s != null).Map(s => s.Entry).Contains(skillEntry) && skillEntry != thisSkill.Entry, jingJie: playerSkills[i].JingJie);
        }

        for (int i = 0; i < playerSkills.Length; i++)
        {
            _inventory.Add(new BarterItem(playerSkills[i], skills[i]));
        }

        RunManager.Instance.SkillPool.Populate(_inventory.Map(b => b.Skill.Entry));
    }

    public bool Exchange(BarterItem barterItem)
    {
        if (!_inventory.Contains(barterItem))
            return false;

        bool inSlot = RunManager.Instance.Battle.Hero.TraversalCurrentSlots().Any(s => s.Skill == barterItem.PlayerSkill);
        bool inSkillInventory = RunManager.Instance.Battle.SkillInventory.Contains(barterItem.PlayerSkill);

        if (!inSlot && !inSkillInventory)
            return false;

        if (inSlot)
        {
            SkillSlot slot = RunManager.Instance.Battle.Hero.TraversalCurrentSlots().First(s => s.Skill == barterItem.PlayerSkill);
            slot.Skill = null;
        }
        else
        {
            RunManager.Instance.Battle.SkillInventory.RemoveSkill(barterItem.PlayerSkill);
        }

        RunManager.Instance.Battle.SkillInventory.AddSkill(barterItem.Skill);
        return true;
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        RunManager.Instance.Map.TryFinishNode();
        return null;
    }
}
