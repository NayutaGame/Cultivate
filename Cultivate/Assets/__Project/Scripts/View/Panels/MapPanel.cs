using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanel : Panel
{
    public MapView MapView;

    public override void Configure()
    {
        base.Configure();
        MapView.Configure(new IndexPath("Map"));
    }

    public override void Refresh()
    {
        base.Refresh();
        MapView.Refresh();
    }
}
