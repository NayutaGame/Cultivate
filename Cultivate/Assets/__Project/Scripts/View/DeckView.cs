using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckView : MonoBehaviour
{
    public Image PlayerSprite;
    public Button ToggleButton;
    public Button SortButton;
    public RunChipInventoryView PlayerHand;
    public SkillInventoryView PlayerInventory;
    public Button LeftButton;
    public Button RightButton;

    private InteractDelegate InteractDelegate;

    public void Configure()
    {
        ConfigureInteractDelegate();

        PlayerHand.Configure(new IndexPath("Battle.Hero.Slots"));
        PlayerHand.SetDelegate(InteractDelegate);
        PlayerInventory.Configure(new IndexPath("Battle.SkillInventory"));
        PlayerInventory.SetDelegate(InteractDelegate);
    }

    public void Refresh()
    {
        PlayerHand.Refresh();
        PlayerInventory.Refresh();
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new(4,
            getId: view =>
            {
                object item = RunManager.Get<object>(view.GetIndexPath());
                if (item is RunSkill)
                    return 0;
                if (item is SkillInventory)
                    return 1;
                if (item is SkillSlot)
                    return 2;
                return null;
            },
            dragDropTable: new Func<IInteractable, IInteractable, bool>[]
            {
                /*                     RunSkill,   SkillInventory, SkillSlot(Hero) */
                /* RunSkill         */ TryMerge,   null,           TryEquip,
                /* SkillInventory   */ null,       null,           null,
                /* SkillSlot(Hero)  */ TryUnequip, TryUnequip,     TrySwap,
            });
    }

    private bool TryMerge(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(new IndexPath("Battle"));
        RunSkill lhs = RunManager.Get<RunSkill>(from.GetIndexPath());
        RunSkill rhs = RunManager.Get<RunSkill>(to.GetIndexPath());
        return runEnvironment.TryMerge(lhs, rhs);
    }

    private bool TryEquip(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(new IndexPath("Battle"));
        RunSkill toEquip = RunManager.Get<RunSkill>(from.GetIndexPath());
        SkillSlot slot = RunManager.Get<SkillSlot>(to.GetIndexPath());
        return runEnvironment.TryEquip(toEquip, slot);
    }

    private bool TryUnequip(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(new IndexPath("Battle"));
        SkillSlot slot = RunManager.Get<SkillSlot>(from.GetIndexPath());
        return runEnvironment.TryUnequip(slot, null);
    }

    private bool TrySwap(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(new IndexPath("Battle"));
        SkillSlot fromSlot = RunManager.Get<SkillSlot>(from.GetIndexPath());
        SkillSlot toSlot = RunManager.Get<SkillSlot>(to.GetIndexPath());
        return runEnvironment.TrySwap(fromSlot, toSlot);
    }
}
