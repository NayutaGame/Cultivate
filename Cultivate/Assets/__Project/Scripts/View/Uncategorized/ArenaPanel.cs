using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArenaPanel : Panel
{
    public SkillInventoryView SkillInventoryView;
    public ArenaEditorView ArenaEditorView;
    public ArenaScoreboardView ArenaScoreboardView;
    public TMP_Text ReportView;

    private InteractDelegate InteractDelegate;

    private IndexPath _indexPath;

    public override void Configure()
    {
        base.Configure();

        _indexPath = new IndexPath("Arena");

        ConfigureInteractDelegate();

        SkillInventoryView.Configure(new IndexPath($"{_indexPath}.SkillInventory"));
        SkillInventoryView.SetDelegate(InteractDelegate);

        ArenaEditorView.Configure(_indexPath);
        ArenaEditorView.SetDelegate(InteractDelegate);

        ArenaScoreboardView.Configure();
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new(2,
            getId: view =>
            {
                object item = RunManager.Get<object>(view.GetIndexPath());
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
        Arena arena = RunManager.Get<Arena>(_indexPath);

        object fromItem = RunManager.Get<object>(from.GetIndexPath());
        SkillSlot toSlot = RunManager.Get<SkillSlot>(to.GetIndexPath());

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
        Arena arena = RunManager.Get<Arena>(_indexPath);
        SkillSlot slot = RunManager.Get<SkillSlot>(view.GetIndexPath());
        return arena.TryIncreaseJingJie(slot);
    }
}
