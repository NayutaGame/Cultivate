using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEditor;
using UnityEngine;

public class BattleRunEnvironment : RunEnvironment
{
    public override void InitInteractDelegate()
    {
        base.InitInteractDelegate();

        _interactDelegate = new(4,
            getID: item =>
            {
                if (item is RunSkill)
                    return 0;
                if (item is SkillInventory)
                    return 1;
                if (item is SkillSlot skillSlot)
                {
                    if (skillSlot.Owner == Hero)
                        return 2;
                    if (skillSlot.Owner == Enemy)
                        return 3;
                }

                return null;
            },
            dragDropTable: new Func<IInteractable, IInteractable, bool>[]
            {
                /*                     RunSkill,   SkillInventory, SkillSlot(Hero), SkillSlot(Enemy) */
                /* RunSkill         */ TryMerge,   null,           TryEquip,        null,
                /* SkillInventory   */ null,       null,           null,            null,
                /* SkillSlot(Hero)  */ TryUnequip, TryUnequip,     TrySwap,         null,
                /* SkillSlot(Enemy) */ null,       null,           null,            null,
            });
    }
}
