using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileResourceCategory : Category<TileResourceEntry>
{
    public TileResourceCategory()
    {
        List = new()
        {
            new TileResourceEntry("金", "金"),
            new TileResourceEntry("水", "水"),
            new TileResourceEntry("木", "木"),
            new TileResourceEntry("火", "火"),
            new TileResourceEntry("土", "土"),
        };
    }
}
