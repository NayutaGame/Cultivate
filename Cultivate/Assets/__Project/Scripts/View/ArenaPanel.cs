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

        ArenaWaiGongInventoryView.Configure(new IndexPath("ArenaWaiGongInventory"));
        ArenaEditorView.Configure(new IndexPath("Arena"));
    }

    public override void Refresh()
    {
        base.Refresh();
        ArenaWaiGongInventoryView.Refresh();
        ArenaEditorView.Refresh();
    }
}
