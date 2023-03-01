using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTileResource
{
    private TileResourceEntry _entry;

    private bool _sealed;

    public RunTileResource(string entryName) : this(Encyclopedia.TileResourceCategory[entryName]) { }
    public RunTileResource(TileResourceEntry entry)
    {
        _entry = entry;
        _sealed = true;
    }
}
