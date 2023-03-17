
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class Map : IInventory
{
    private static readonly int HEIGHT = 3;
    private static readonly int WIDTH = 10;

    private RunNode[] _list;
    public int GetCount() => _list.Length;

    public string GetIndexPathString() => "TryGetRunNode";

    public RunNode this[int x, int y]
    {
        get => _list[x * HEIGHT + y];
        private set => _list[x * HEIGHT + y] = value;
    }

    private AutoPool<NodeEntry> _b;
    private AutoPool<NodeEntry> _a;
    private AutoPool<NodeEntry> _boss;
    private AutoPool<NodeEntry> _m;
    private Dictionary<JingJie, AutoPool<NodeEntry>[]> _poolConfiguration;

    private int _heroPosition;

    private int HeroPosition
    {
        get => _heroPosition;
        set
        {
            _heroPosition = value;
        }
    }

    public Map()
    {
        _list = new RunNode[HEIGHT * WIDTH];

        _b = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is BattleNodeEntry).ToList());
        _a = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is AdventureNodeEntry).ToList());
        _boss = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is BossNodeEntry).ToList());
        _m = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is MarketNodeEntry).ToList());
        _poolConfiguration = new Dictionary<JingJie, AutoPool<NodeEntry>[]>()
        {
            { JingJie.LianQi   , new[] { _b, _a, _b, _a, _boss, _m } },
            { JingJie.ZhuJi    , new[] { _b, _b, _a, _b, _a, _boss, _m } },
            { JingJie.JinDan   , new[] { _b, _b, _a, _b, _b, _a, _boss, _m } },
            { JingJie.YuanYing , new[] { _b, _b, _a, _b, _b, _a, _b, _boss, _m } },
            { JingJie.HuaShen  , new[] { _b, _b, _a, _b, _b, _a, _b, _b, _m, _boss } },
            { JingJie.FanXu    , new[] { _b, _b, _a, _b, _b, _a, _b, _b, _m, _boss } },
        };
    }

    public void SetJingJie(JingJie jingJie)
    {
        AutoPool<NodeEntry>[] pools = _poolConfiguration[jingJie];

        for (int x = 0; x < WIDTH; x++)
        {
            AutoPool<NodeEntry> pool = x < pools.Length ? pools[x] : null;
            for (int y = 0; y < HEIGHT; y++)
            {
                if (pool == null)
                {
                    this[x, y] = null;
                    continue;
                }

                if ((pool == _boss || pool == _m) && y != 0)
                {
                    this[x, y] = null;
                    continue;
                }

                this[x, y] = new RunNode(pool.ForcePopItem());
            }
        }

        HeroPosition = 0;
    }
}
