using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LibraryPanel : Panel
{
    public ArenaWaiGongInventoryView ArenaWaiGongInventoryView;

    public override void Configure()
    {
        base.Configure();
        ArenaWaiGongInventoryView.Configure(new IndexPath("ArenaWaiGongInventory"));
    }

    public override void Refresh()
    {
        base.Refresh();
        ArenaWaiGongInventoryView.Refresh();
    }
}
