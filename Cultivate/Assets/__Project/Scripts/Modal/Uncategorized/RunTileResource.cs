using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTileResource
{
    private TileResourceEntry _entry;

    public RunTileResource(string entryName) : this(Encyclopedia.TileResourceCategory[entryName]) { }
    public RunTileResource(TileResourceEntry entry)
    {
        _entry = entry;
    }

    public string GetName() => _entry.Name;
}
