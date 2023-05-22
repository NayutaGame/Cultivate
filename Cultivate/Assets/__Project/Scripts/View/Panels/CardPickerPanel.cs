using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPickerPanel : Panel
{
    public EntityView HeroView;
    public SkillInventoryView SkillInventoryView;

    public override void Configure()
    {
        base.Configure();
        HeroView.Configure(new IndexPath("Battle.Hero"));
        SkillInventoryView.Configure(new IndexPath("Battle.SkillInventory"));
    }

    public override void Refresh()
    {
        base.Refresh();
    }
}
