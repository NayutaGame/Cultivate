using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TechTreePanel : Panel
{
    public TechTreeView TechTreeView;

    public override void Configure()
    {
        base.Configure();
        TechTreeView.Configure(new IndexPath("TechInventory.List"));
    }

    public override void Refresh()
    {
        base.Refresh();
        TechTreeView.Refresh();
    }
}
