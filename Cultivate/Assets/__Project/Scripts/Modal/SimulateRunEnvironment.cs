using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateRunEnvironment : RunEnvironment
{
    public override void InitDragDropDelegate()
    {
        base.InitDragDropDelegate();

        _dragDropDelegate = new(4, new Func<IDragDrop, IDragDrop, bool>[]
            {
                /*                     RunSkill,   SkillInventory, SkillSlot(Hero), SkillSlot(Enemy) */
                /* RunSkill         */ TryMerge,   null,           TryEquip,        TryWrite,
                /* SkillInventory   */ null,       null,           null,            null,
                /* SkillSlot(Hero)  */ TryUnequip, TryUnequip,     TrySwap,         TryWrite,
                /* SkillSlot(Enemy) */ null,       null,           null,            null,
            },
            item =>
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

                throw new Exception();
            });
    }
}
