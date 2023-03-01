
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DanTian
{
    public static readonly int RADIUS = 4;
    public static readonly int DIAMETER = 2 * RADIUS + 1;

    public static readonly int[] REVEAL_RADIUS = new[] { 2, 2, 3, 3, 4, 4 };

    private Tile[] _tiles;

    public Tile this[int q, int r]
    {
        get
        {
            int x = q + 4;
            int y = r + 4;
            int i = DIAMETER * y + x;
            return _tiles[i];
        }
        set
        {
            int x = q + 4;
            int y = r + 4;
            int i = DIAMETER * y + x;
            _tiles[i] = value;
        }
    }

    public Tile GetTileXY(int x, int y)
    {
        int i = DIAMETER * y + x;
        return _tiles[i];
    }

    public void SetTileXY(int x, int y, Tile value)
    {
        int i = DIAMETER * y + x;
        _tiles[i] = value;
    }

    public static bool IsInsideXY(int x, int y)
    {
        int q = x - 4;
        int r = y - 4;
        return IsInside(q, r);
    }

    public static bool IsInside(int q, int r)
    {
        return DistanceToOrigin(q, r) <= RADIUS;
    }

    public static int DistanceToOrigin(int q, int r)
    {
        return (Mathf.Abs(q) + Mathf.Abs(q + r) + Mathf.Abs(r)) / 2;
    }

    public DanTian()
    {
        _tiles = new Tile[DIAMETER * DIAMETER];
        for (int r = -RADIUS; r < DIAMETER - RADIUS; r++)
        for (int q = -RADIUS; q < DIAMETER - RADIUS; q++)
            if (IsInside(q, r))
                this[q, r] = new Tile(q, r);
    }

    public IEnumerable<Tile> Adjacents(Tile tile, Func<Tile, bool> pred)
    {
        PriorityQueue<Tile, int> frontiers = new();
        _tiles.FilterObj(t => t != null).Do(t =>
        {
            t.Cost = 999;
            t.Visited = false;
        });

        tile.Cost = 0;

        frontiers.Enqueue(tile, 0);
        while (frontiers.Count > 0)
        {
            var toSearch = frontiers.Dequeue();
            toSearch.Visited = true;

            yield return toSearch;

            foreach (var neighbour in Neighbours(toSearch))
            {
                if (neighbour.Visited) continue;
                if (!neighbour.Revealed) continue;
                if (!pred(neighbour)) continue;

                neighbour.Cost = toSearch.Cost + 1;
                frontiers.Enqueue(neighbour, neighbour.Cost);
            }
        }
    }

    private static List<Vector2Int> DirectionVectors = new ()
    {
        new(+1, 0), new(+1, -1), new(0, -1),
        new(-1, 0), new(-1, +1), new(0, +1),
    };

    public IEnumerable<Tile> Neighbours(Tile tile)
    {
        foreach (var dir in DirectionVectors)
        {
            int q = tile._q + dir.x;
            int r = tile._r + dir.y;
            if(!IsInside(q, r)) continue;
            yield return RunManager.Instance.DanTian[q, r];
        }
    }

    public void SetRevealedJingJie(JingJie jingJie)
    {
        for (int r = -RADIUS; r < DIAMETER - RADIUS; r++)
        for (int q = -RADIUS; q < DIAMETER - RADIUS; q++)
            if (IsInside(q, r))
            {
                Tile tile = this[q, r];
                tile.Revealed = tile.DistanceToOrigin() <= REVEAL_RADIUS[jingJie];
            }
    }
}
