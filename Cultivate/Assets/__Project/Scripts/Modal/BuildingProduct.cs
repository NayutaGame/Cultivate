using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProduct : Product
{
    public BuildingEntry _entry;

    public BuildingProduct(string entryName) : this(Encyclopedia.BuildingCategory[entryName]) { }
    public BuildingProduct(BuildingEntry entry)
    {
        _entry = entry;
    }

    public override string GetName() => _entry.Name;

    public override int GetCost() => _entry.Cost;

    public override bool IsClick() => false;

    public override bool IsDrag() => true;

    public override bool CanDrop(Tile tile)
    {
        return tile.Building == null;
        // return _entry.CanDrop(this, tile);
    }

    public override void Drop(Tile tile)
    {
        tile.Building = new RunBuilding(_entry);
        // return _entry.Drop(this, tile);
    }
}
