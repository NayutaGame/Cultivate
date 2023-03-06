using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunBuilding
{
    private BuildingEntry _entry;

    public RunBuilding(string entryName) : this(Encyclopedia.BuildingCategory[entryName]) { }
    public RunBuilding(BuildingEntry entry)
    {
        _entry = entry;
    }

    public string GetName() => _entry.Name;
}
