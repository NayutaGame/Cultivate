using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCategory : Category<BuildingEntry>
{
    public BuildingCategory()
    {
        // district, wonder
        List = new()
        {
            new BuildingEntry("金建筑", "金建筑", 2),
        };
    }
}
