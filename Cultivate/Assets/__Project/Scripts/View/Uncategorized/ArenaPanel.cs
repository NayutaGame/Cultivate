using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ArenaPanel : Panel
{
    public SkillInventoryView SkillInventoryView;
    public ArenaEditorView ArenaEditorView;
    public ArenaScoreboardView ArenaScoreboardView;
    public TMP_Text ReportView;

    private InteractDelegate InteractDelegate;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Arena");

        ConfigureInteractDelegate();

        SkillInventoryView.Configure(new Address("App.SkillInventory"));
        SkillInventoryView.SetDelegate(InteractDelegate);

        ArenaEditorView.Configure(_address);
        ArenaEditorView.SetDelegate(InteractDelegate);

        ArenaScoreboardView.Configure();
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new(2,
            getId: view =>
            {
                object item = view.Get<object>();
                if (item is RunSkill)
                    return 0;
                if (item is SkillSlot)
                    return 1;
                return null;
            },
            dragDropTable: new Func<IInteractable, IInteractable, bool>[]
            {
                /*               RunSkill,   SkillSlot */
                /* RunSkill   */ null,       TryWrite,
                /* SkillSlot  */ null,       TryWrite,
            },
            rMouseTable: new Func<IInteractable, bool>[]
            {
                /* RunSkill   */ null,
                /* SkillSlot  */ TryIncreaseJingJie,
            });
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillInventoryView.Refresh();
        ArenaEditorView.Refresh();
        ArenaScoreboardView.Refresh();

        ReportView.text = RunManager.Instance.Arena.Report?.ToString();
    }

    private bool TryWrite(IInteractable from, IInteractable to)
    {
        Arena arena = _address.Get<Arena>();

        object fromItem = from.Get<object>();
        SkillSlot toSlot = to.Get<SkillSlot>();

        if (fromItem is RunSkill fromSkill)
        {
            return arena.TryWrite(fromSkill, toSlot);
        }
        else if (fromItem is SkillSlot fromSlot)
        {
            return arena.TryWrite(fromSlot, toSlot);
        }

        return false;
    }

    private bool TryIncreaseJingJie(IInteractable view)
    {
        Arena arena = _address.Get<Arena>();
        SkillSlot slot = view.Get<SkillSlot>();
        return arena.TryIncreaseJingJie(slot);
    }
}
