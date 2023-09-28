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
        pool.Populate(RunManager.Instance.Battle.Home.TraversalCurrentSlots()
            .FilterObj(s => s.Skill != null && s.Skill is RunSkill).Map(s => s.Skill as RunSkill));
        pool.Populate(RunManager.Instance.Battle.SkillInventory.Traversal());
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
            RunManager.Instance.Battle.SkillPool.TryDrawSkill(out skills[i],
                pred: skillEntry => !skills.FilterObj(s => s != null).Map(s => s.GetEntry()).Contains(skillEntry) && skillEntry != thisSkill.GetEntry(), jingJie: playerSkills[i].JingJie);
        }

        for (int i = 0; i < playerSkills.Length; i++)
        {
            _inventory.Add(new BarterItem(playerSkills[i], skills[i]));
        }

        RunManager.Instance.Battle.SkillPool.Populate(_inventory.Traversal().Map(b => b.Skill.GetEntry()));
    }

    public bool Exchange(BarterItem barterItem)
    {
        if (!_inventory.Contains(barterItem))
            return false;

        bool inSlot = RunManager.Instance.Battle.Home.TraversalCurrentSlots().Any(s => s.Skill == barterItem.PlayerSkill);
        bool inSkillInventory = RunManager.Instance.Battle.SkillInventory.Contains(barterItem.PlayerSkill);

        if (!inSlot && !inSkillInventory)
            return false;

        if (inSlot)
        {
            SkillSlot slot = RunManager.Instance.Battle.Home.TraversalCurrentSlots().First(s => s.Skill == barterItem.PlayerSkill);
            slot.Skill = null;
        }
        else
        {
            RunManager.Instance.Battle.SkillInventory.Remove(barterItem.PlayerSkill);
        }

        RunManager.Instance.Battle.SkillInventory.Add(barterItem.Skill);

        _inventory.Remove(barterItem);
        return true;
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal) => null;
}
