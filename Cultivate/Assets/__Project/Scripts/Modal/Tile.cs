
using UnityEngine;

public class Tile
{
    public readonly int _q;
    public readonly int _r;

    public AcquiredChip Chip;

    public Tile(int q, int r)
    {
        _q = q;
        _r = r;
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
