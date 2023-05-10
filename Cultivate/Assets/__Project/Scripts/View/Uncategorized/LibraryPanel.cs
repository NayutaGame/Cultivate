using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LibraryPanel : Panel
{
    public SkillInventoryView SkillInventoryView;

    public override void Configure()
    {
        base.Configure();
        SkillInventoryView.Configure(new IndexPath("Arena.SkillInventory"));
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillInventoryView.Refresh();
    }
}
