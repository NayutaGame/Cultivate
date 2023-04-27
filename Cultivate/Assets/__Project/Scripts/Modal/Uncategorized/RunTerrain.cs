using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class RunTerrain
{
    private TerrainEntry _entry;

    public ModifierLeaf ModifierLeaf;

    public int XiuWei
    {
        get => (int)ModifierLeaf.ForceGet("turnXiuWeiAdd");
        set => ModifierLeaf.ForceSet("turnXiuWeiAdd", value);
    }

    public int ChanNeng
    {
        get => (int)ModifierLeaf.ForceGet("turnChanNengAdd");
        set => ModifierLeaf.ForceSet("turnChanNengAdd", value);
    }

    public RunTerrain(string entryName) : this(Encyclopedia.TerrainCategory[entryName]) { }
    public RunTerrain(TerrainEntry entry)
    {
        _entry = entry;

        ModifierLeaf = new();
        XiuWei = _entry.XiuWei;
        ChanNeng = _entry.ChanNeng;
    }

    public string GetName() => _entry.Name;
}
