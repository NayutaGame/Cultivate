using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ArenaWaiGongInventory: Inventory<RunSkill>
{
    public ArenaWaiGongInventory()
    {
        Clear();
        Encyclopedia.SkillCategory.Traversal.Map(chip => new RunSkill(chip, chip.JingJieRange.Start)).Do(Add);
    }
}
