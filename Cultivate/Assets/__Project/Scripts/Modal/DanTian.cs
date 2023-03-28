
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using TMPro;
using UnityEngine;

public class DanTian : GDictionary
{
    public static readonly int RADIUS = 4;
    public static readonly int DIAMETER = 2 * RADIUS + 1;

    public static readonly int[] REVEAL_RADIUS = new[] { 2, 2, 3, 3, 4, 4 };
    public static readonly int[] WORKER_COUNT = new[] { 3, 6, 8, 10, 12, 12 };

    private Tile[] _tiles;

    // private int _workerCount;
    // public int WorkerCount
    // {
    //     get => _workerCount;
    //     set
    //     {
    //         _workerCount = value;
    //         AutoAssignWorkers();
    //     }
    // }
    // private List<WorkerLock> _workerLocks;
    // private List<Worker> _workers;

    private Modifier _modifier;
    public Modifier Modifier => _modifier;

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

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;

    public DanTian()
    {
        _accessors = new()
        {
            { "Tiles",                  () => _tiles },
        };

        InitTiles();
        // _workers = new();
        // _workerLocks = new();
        _modifier = Modifier.Default;
    }

    private void InitTiles()
    {
        _tiles = new Tile[DIAMETER * DIAMETER];
        for (int r = -RADIUS; r < DIAMETER - RADIUS; r++)
        for (int q = -RADIUS; q < DIAMETER - RADIUS; q++)
            if (IsInside(q, r))
                this[q, r] = new Tile(q, r);

        // Pool<Tile> pool = new Pool<Tile>();
        // pool.Populate(_tiles, t => t != null);
        // pool.Shuffle();
        //
        // // distribution
        // // 12 slots, 3 for each ring
        // int slotIndex = 0;
        // for (int distance = 4; distance >= 1; distance--)
        // {
        //     for (int i = 0; i < 3; i++)
        //     {
        //         if (!pool.TryPopFirst(toPop => toPop.DistanceToOrigin() == distance, out Tile tile))
        //         {
        //             throw new Exception("pool is run out of candidates");
        //         }
        //
        //         tile.SlotIndex = slotIndex;
        //         tile.Terrain = new RunTerrain(Encyclopedia.TerrainCategory["空"]);
        //         slotIndex++;
        //     }
        // }
        //
        // // 5 elements, inside ring 2
        // for (int i = 0; i < 5; i++)
        // {
        //     if (!pool.TryPopFirst(tile => tile.DistanceToOrigin() <= 2, out Tile tile))
        //     {
        //         throw new Exception("pool is run out of candidates");
        //     }
        //
        //     tile.Resource = new RunTileResource(Encyclopedia.TileResourceCategory[i]);
        //     tile.Terrain = new RunTerrain(Encyclopedia.TerrainCategory["空"]);
        // }
        //
        // // 1-yield all the rest
        // while (pool.TryPopFirst(t => true, out Tile tile))
        // {
        //     float r = RandomManager.value;
        //     tile.Terrain = new RunTerrain(Encyclopedia.TerrainCategory[r < 0.5f ? "修" : "产"]);
        // }
    }

    public IEnumerable<Tile> Connects(Tile tile, Func<Tile, bool> pred)
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

            foreach (var neighbour in Adjacents(toSearch))
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

    public IEnumerable<Tile> Adjacents(Tile tile)
    {
        foreach (var dir in DirectionVectors)
        {
            int q = tile._q + dir.x;
            int r = tile._r + dir.y;
            if(!IsInside(q, r)) continue;
            yield return this[q, r];
        }
    }

    public IEnumerable<Tile> Ring(int distance)
    {
        foreach (var tile in _tiles)
        {
            if (tile == null) continue;
            if (tile.DistanceToOrigin() != distance) continue;
            yield return tile;
        }
    }

    public IEnumerable<Tile> Revealed()
    {
        foreach (var tile in _tiles)
        {
            if (tile is not { Revealed: true }) continue;
            yield return tile;
        }
    }

    public void SetJingJie(JingJie jingJie)
    {
        // set revealed
        for (int r = -RADIUS; r < DIAMETER - RADIUS; r++)
        for (int q = -RADIUS; q < DIAMETER - RADIUS; q++)
            if (IsInside(q, r))
            {
                Tile tile = this[q, r];
                tile.Revealed = tile.DistanceToOrigin() <= REVEAL_RADIUS[jingJie];
            }

        // set base worker count
        // WorkerCount = WORKER_COUNT[jingJie];
    }

    // public void AddWorkerLock(Tile tile)
    // {
    //     WorkerLock workerLock = new WorkerLock(tile);
    //     tile.WorkerLock = workerLock;
    //     _workerLocks.Add(workerLock);
    // }
    //
    // public void RemoveWorkerLock(Tile tile)
    // {
    //     WorkerLock workerLock = tile.WorkerLock;
    //     tile.WorkerLock = null;
    //     _workerLocks.Remove(workerLock);
    // }
    //
    // public void AddWorker(Tile tile)
    // {
    //     Worker w = new Worker(tile);
    //     _modifier.AddChild(tile.Modifier);
    //     tile.Worker = w;
    //     _workers.Add(w);
    // }
    //
    // public void RemoveAllWorkers()
    // {
    //     _workers.Do(w =>
    //     {
    //         w.Tile.Worker = null;
    //         _modifier.RemoveChild(w.Tile.Modifier);
    //     });
    //     _workers.Clear();
    // }
    //
    // public bool TryToggleWorkerLock(Tile tile)
    // {
    //     if (tile.WorkerLock != null)
    //     {
    //         RemoveWorkerLock(tile);
    //         AutoAssignWorkers();
    //         return true;
    //     }
    //
    //     if (_workerLocks.Count < WorkerCount)
    //     {
    //         AddWorkerLock(tile);
    //         AutoAssignWorkers();
    //         return true;
    //     }
    //
    //     // all workers are locked
    //     return false;
    // }
    //
    // public void AutoAssignWorkers()
    // {
    //     RemoveAllWorkers();
    //
    //     List<Tile> pool = Revealed().ToList();
    //     foreach (var l in _workerLocks)
    //     {
    //         AddWorker(l.Tile);
    //         pool.Remove(l.Tile);
    //     }
    //
    //     int xiuWeiFactor = 100; // 10000 100 1
    //     int chanNengFactor = 100;
    //
    //     PriorityQueue<Tile, int> pq = new();
    //     pool.Do(t => pq.Enqueue(t, -(t.XiuWei * xiuWeiFactor + t.ChanNeng * chanNengFactor)));
    //
    //     int assignCount = Mathf.Min(pq.Count, WorkerCount - _workers.Count);
    //
    //     for (int i = 0; i < assignCount; i++)
    //     {
    //         Tile t = pq.Dequeue();
    //         AddWorker(t);
    //     }
    // }

    public Tile FirstEmptyTile()
        => _tiles.FirstObj(t => t is { Revealed: true, AcquiredRunChip: null });
}
