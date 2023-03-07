
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class Tile
{
    public readonly int _q;
    public readonly int _r;

    public bool Revealed;
    public RunTileResource Resource;

    private RunTerrain _terrain;
    public RunTerrain Terrain
    {
        get => _terrain;
        set
        {
            if(_terrain != null)
                Modifier.RemoveLeaf(_terrain.ModifierLeaf);
            _terrain = value;
            if (_terrain != null)
                Modifier.AddLeaf(_terrain.ModifierLeaf);
        }
    }

    public RunBuilding Building;
    public int? SlotIndex;

    public WorkerLock WorkerLock;
    public Worker Worker;





    public Modifier Modifier;
    public int XiuWei => (int)Modifier.Value.ForceGet("turnXiuWeiAdd");
    public int ChanNeng => (int)Modifier.Value.ForceGet("turnChanNengAdd");

    public bool Visited;
    public int Cost;

    public Tile(int q, int r)
    {
        _q = q;
        _r = r;

        Revealed = true;

        Modifier = Modifier.Default;
    }

    public int Distance(Tile other)
    {
        return (Mathf.Abs(_q - other._q)
                + Mathf.Abs(_q + _r - other._q - other._r)
                + Mathf.Abs(_r - other._r)) / 2;
    }

    public int DistanceToOrigin()
    {
        return (Mathf.Abs(_q) + Mathf.Abs(_q + _r) + Mathf.Abs(_r)) / 2;
    }
}
