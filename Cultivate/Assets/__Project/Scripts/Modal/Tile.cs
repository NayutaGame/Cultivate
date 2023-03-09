
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

public class Tile
{
    public readonly int _q;
    public readonly int _r;

    public bool Revealed;

    public AcquiredRunChip AcquiredRunChip;
    public int[] Powers;

    public string GetPowerString()
    {
        StringBuilder sb = new();
        for (int i = 0; i < Powers.Length; i++)
        {
            if (Powers[i] == 0) continue;
            sb.Append($"{Powers[i]}{(WuXing)i} ");
        }

        return sb.ToString();
    }
    // public RunTileResource Resource;

    // private RunTerrain _terrain;
    // public RunTerrain Terrain
    // {
    //     get => _terrain;
    //     set
    //     {
    //         if(_terrain != null)
    //             Modifier.RemoveLeaf(_terrain.ModifierLeaf);
    //         _terrain = value;
    //         if (_terrain != null)
    //             Modifier.AddLeaf(_terrain.ModifierLeaf);
    //     }
    // }

    // public RunBuilding Building;
    // public int? SlotIndex;
    //
    // public WorkerLock WorkerLock;
    // public Worker Worker;





    // public Modifier Modifier;
    // public int XiuWei => (int)Modifier.Value.ForceGet("turnXiuWeiAdd");
    // public int ChanNeng => (int)Modifier.Value.ForceGet("turnChanNengAdd");

    public bool Visited;
    public int Cost;

    public Tile(int q, int r)
    {
        _q = q;
        _r = r;

        Revealed = true;
        Powers = new int[WuXing.Length];

        // Modifier = Modifier.Default;
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
