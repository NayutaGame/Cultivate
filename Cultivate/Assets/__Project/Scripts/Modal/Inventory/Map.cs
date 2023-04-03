
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class Map : GDictionary
{
    private static readonly int HEIGHT = 3;
    private static readonly int WIDTH = 10;

    private RunNode[] _list;

    public RunNode this[int x, int y]
    {
        get => _list[x * HEIGHT + y];
        private set => _list[x * HEIGHT + y] = value;
    }

    public RunNode this[Vector2Int pos]
    {
        get => this[pos.x, pos.y];
        private set => this[pos.x, pos.y] = value;
    }

    private AutoPool<NodeEntry> _b;
    private AutoPool<NodeEntry> _a;
    private AutoPool<NodeEntry> _m;
    private Dictionary<JingJie, AutoPool<NodeEntry>[]> _poolConfiguration;

    public bool Selecting { get; private set; }
    private Vector2Int _heroPosition;

    public RunNode TryGetCurrentNode()
    {
        if (_heroPosition.x < 0)
            return null;
        return this[_heroPosition];
    }

    public void SelectedNode(RunNode selected)
    {
        if (_heroPosition.x >= 0)
            this[_heroPosition].State = RunNode.RunNodeState.Passed;

        _heroPosition = new(_heroPosition.x + 1, selected.Position.y);

        for (int y = 0; y < HEIGHT; y++)
        {
            RunNode runNode = this[_heroPosition.x, y];
            if(runNode == null)
                continue;
            runNode.State = y == selected.Position.y ? RunNode.RunNodeState.Current : RunNode.RunNodeState.Missed;
        }

        Selecting = false;

        RunCanvas.Instance.OpenNodePanel();
    }

    public void TryFinishNode()
    {
        if (Selecting)
            return;

        // if it is the last node, try goto next jingjie / commit run

        bool isEnd = true;
        for (int y = 0; y < HEIGHT; y++)
        {
            RunNode runNode = this[_heroPosition.x + 1, y];
            if (runNode == null)
                continue;
            runNode.State = RunNode.RunNodeState.ToChoose;
            isEnd = false;
        }

        Selecting = true;

        if (isEnd)
            RunManager.Instance.JingJie += 1;

        RunCanvas.Instance.OpenMapPanel();
    }

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;

    public Map()
    {
        _list = new RunNode[HEIGHT * WIDTH];

        _b = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is BattleNodeEntry).ToList());
        _a = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is AdventureNodeEntry).ToList());
        _m = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is MarketNodeEntry).ToList());

        _accessors = new()
        {
            { "Nodes", () => _list },
        };

        // _poolConfiguration = new Dictionary<JingJie, AutoPool<NodeEntry>[]>()
        // {
        //     { JingJie.LianQi   , new[] { _b, _a, _b, _a, _b, _m } },
        //     { JingJie.ZhuJi    , new[] { _b, _b, _a, _b, _a, _b, _m } },
        //     { JingJie.JinDan   , new[] { _b, _b, _a, _b, _b, _a, _b, _m } },
        //     { JingJie.YuanYing , new[] { _b, _b, _a, _b, _b, _a, _b, _b, _m } },
        //     { JingJie.HuaShen  , new[] { _b, _b, _a, _b, _b, _a, _b, _b, _m, _b } },
        //     { JingJie.FanXu    , new[] { _b, _b, _a, _b, _b, _a, _b, _b, _m, _b } },
        // };

        _poolConfiguration = new Dictionary<JingJie, AutoPool<NodeEntry>[]>()
        {
            { JingJie.LianQi   , new[] { _b, _a, _b, _a, _b } },
            { JingJie.ZhuJi    , new[] { _b, _b, _a, _b, _a, _b } },
            { JingJie.JinDan   , new[] { _b, _b, _a, _b, _b, _a, _b } },
            { JingJie.YuanYing , new[] { _b, _b, _a, _b, _b, _a, _b, _b } },
            { JingJie.HuaShen  , new[] { _b, _b, _a, _b, _b, _a, _b, _b, _b } },
            { JingJie.FanXu    , new[] { _b, _b, _a, _b, _b, _a, _b, _b, _b } },
        };
    }

    public void SetJingJie(JingJie jingJie)
    {
        AutoPool<NodeEntry>[] pools = _poolConfiguration[jingJie];

        bool isLastBattle = true;
        for (int x = WIDTH - 1; x >= 0; x--)
        {
            AutoPool<NodeEntry> pool = x < pools.Length ? pools[x] : null;
            for (int y = 0; y < HEIGHT; y++)
            {
                if (pool == null)
                {
                    this[x, y] = null;
                    continue;
                }

                if (pool == _m && y != 0)
                {
                    this[x, y] = null;
                    continue;
                }

                // canCreate
                NodeEntry nodeEntry = pool.ForcePopItem();
                if (nodeEntry is BattleNodeEntry battleNodeEntry)
                {
                    CreateEnemyDetails d = new CreateEnemyDetails(jingJie) { AllowNormal = !isLastBattle, AllowElite = !isLastBattle, AllowBoss = isLastBattle};
                    this[x, y] = new BattleRunNode(new Vector2Int(x, y), battleNodeEntry, d);

                    if (isLastBattle)
                    {
                        isLastBattle = false;
                        break;
                    }
                }
                else
                {
                    this[x, y] = new RunNode(new Vector2Int(x, y), nodeEntry);
                }

                if (x == 0)
                    this[x, y].State = RunNode.RunNodeState.ToChoose;
            }
        }

        Selecting = true;
        _heroPosition = new Vector2Int(-1, 0);
    }
}
