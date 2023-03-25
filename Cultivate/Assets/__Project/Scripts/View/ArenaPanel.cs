using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPanel : Panel
{
    public ArenaWaiGongInventoryView ArenaWaiGongInventoryView;
    public ArenaEditorView ArenaEditorView;

    public override void Configure()
    {
        base.Configure();
        ArenaWaiGongInventoryView.Configure(RunManager.Instance.ArenaWaiGongInventory);
        ArenaEditorView.Configure(RunManager.Instance.ArenaEditor);
    }

    public override void Refresh()
    {
        base.Refresh();
        ArenaWaiGongInventoryView.Refresh();
        ArenaEditorView.Refresh();
    }
}
