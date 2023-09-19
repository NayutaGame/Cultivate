using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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

        SkillInventoryView.SetAddress(new Address("App.SkillInventory"));
        SkillInventoryView.SetDelegate(InteractDelegate);

        ArenaEditorView.SetAddress(_address);
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
            dragDropTable: new Action<IInteractable, IInteractable>[]
            {
                /*               RunSkill,   SkillSlot */
                /* RunSkill   */ null,       TryWrite,
                /* SkillSlot  */ null,       TryWrite,
            });
        InteractDelegate.SetHandle(InteractDelegate.POINTER_RIGHT_CLICK, 1, (v, d) => TryIncreaseJingJie(v, d));
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillInventoryView.Refresh();
        ArenaEditorView.Refresh();
        ArenaScoreboardView.Refresh();

        ReportView.text = RunManager.Instance.Arena.Report?.ToString();
    }

    private void TryWrite(IInteractable from, IInteractable to)
    {
        Arena arena = _address.Get<Arena>();

        object fromItem = from.Get<object>();
        SkillSlot toSlot = to.Get<SkillSlot>();

        if (fromItem is RunSkill fromSkill)
        {
            arena.TryWrite(fromSkill, toSlot);
        }
        else if (fromItem is SkillSlot fromSlot)
        {
            arena.TryWrite(fromSlot, toSlot);
        }
    }

    private bool TryIncreaseJingJie(IInteractable view, PointerEventData eventData)
    {
        Arena arena = _address.Get<Arena>();
        SkillSlot slot = view.Get<SkillSlot>();
        return arena.TryIncreaseJingJie(slot);
    }
}
