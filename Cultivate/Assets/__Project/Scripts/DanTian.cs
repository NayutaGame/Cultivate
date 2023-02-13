
using System.Collections.Generic;
using UnityEngine;

public class DanTian
{
    public static readonly int RADIUS = 4;
    public static readonly int WIDTH = 2 * RADIUS + 1;

    private Tile[] _tiles;
    private List<Tile> _acquired;

    public Tile this[int q, int r]
    {
        get
        {
            int x = q + 4;
            int y = r + 4;
            int i = WIDTH * y + x;
            return _tiles[i];
        }
        set
        {
            int x = q + 4;
            int y = r + 4;
            int i = WIDTH * y + x;
            _tiles[i] = value;
        }
    }

    public Tile GetTileXY(int x, int y)
    {
        int i = WIDTH * y + x;
        return _tiles[i];
    }

    public void SetTileXY(int x, int y, Tile value)
    {
        int i = WIDTH * y + x;
        _tiles[i] = value;
    }

    public static bool IsInside(int x, int y)
    {
        return DistanceToOrigin(x, y) <= RADIUS;
    }

    public static int DistanceToOrigin(int x, int y)
    {
        int q = x - 4;
        int r = y - 4;
        return (Mathf.Abs(q) + Mathf.Abs(q + r) + Mathf.Abs(r)) / 2;
    }

    public DanTian()
    {
        _tiles = new Tile[WIDTH * WIDTH];
        for (int y = 0; y < WIDTH; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                if (IsInside(x, y))
                {
                    int q = x - 4;
                    int r = y - 4;
                    this[q, r] = new Tile(q, r);
                }
            }
        }

        _acquired = new List<Tile>();
    }

    public void Acquire(RunChip runChip, Tile atTile)
    {
        atTile.RunChip = runChip;
        _acquired.Add(atTile);
    }

    public int GetAcquiredChipCount()
    {
        return _acquired.Count;
    }

    public Tile TryGetAcquiredTile(int i)
    {
        if (i < _acquired.Count)
            return _acquired[i];
        return null;
    }

    public RunChip TryGetAcquiredChip(int i)
    {
        if(i < _acquired.Count)
            return _acquired[i].RunChip;
        return null;
    }

    public void SwapAcquired(int from, int to)
    {
        (_acquired[from], _acquired[to]) = (_acquired[to], _acquired[from]);
    }
}
