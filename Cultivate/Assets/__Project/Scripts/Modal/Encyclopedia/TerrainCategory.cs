using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCategory : Category<TerrainEntry>
{
    public TerrainCategory()
    {
        List = new()
        {
            new TerrainEntry("空", ""),
            new TerrainEntry("修", "修为+1", xiuWei: 1),
            new TerrainEntry("产", "产能+1", chanNeng: 1),
        };
    }
}
