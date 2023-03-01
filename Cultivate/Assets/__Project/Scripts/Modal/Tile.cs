
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public readonly int _q;
    public readonly int _r;

    public bool Revealed;
    public RunTileResource Resource;
    public int[] _elements;
    public AcquiredChip Chip;

    public bool Visited;
    public int Cost;

    public Tile(int q, int r)
    {
        _q = q;
        _r = r;

        Revealed = true;

        _elements = new int[WuXing.Length];
        float rand = RandomManager.value;
        if (rand < 0.5f)
        {
            int i = Mathf.FloorToInt(rand * 10);
            _elements[i] = 1;
        }
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

    public WuXing? FirstElement()
    {
        foreach (var e in WuXing.Traversal)
        {
            if (_elements[e] > 0) return e;
        }

        return null;
    }
}
