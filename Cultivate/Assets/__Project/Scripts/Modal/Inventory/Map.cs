
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

    public bool InRange(int x, int y)
    {
        int i = x * HEIGHT + y;
        return 0 <= i && i < _list.Length;
    }

    public RunNode this[Vector2Int pos]
    {
        get => this[pos.x, pos.y];
        private set => this[pos.x, pos.y] = value;
    }

    private AutoPool<NodeEntry> _b;
    private AutoPool<NodeEntry> _r;
    private AutoPool<NodeEntry> _a;
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

        RunCanvas.Instance.SetSecondLayerToShow();
    }

    public void TryFinishNode()
    {
        if (Selecting)
            return;

        // if it is the last node, try goto next jingjie / commit run

        bool isEnd = true;
        for (int y = 0; y < HEIGHT; y++)
        {
            if (!InRange(_heroPosition.x + 1, y))
                break;
            RunNode runNode = this[_heroPosition.x + 1, y];
            if (runNode == null)
                continue;
            runNode.State = RunNode.RunNodeState.ToChoose;
            isEnd = false;
        }

        Selecting = true;

        if (isEnd)
            JingJie += 1;

        RunCanvas.Instance.SetSecondLayerToHide();
    }


    private JingJie _jingJie;
    public JingJie JingJie
    {
        get => _jingJie;
        set
        {
            _jingJie = value;
            if (AppManager.Instance != null)
                if (AppManager.Instance.StageManager != null)
                {
                    RunManager.Instance.Battle.Hero.SetBaseHealth(RunEntity.BaseHP[_jingJie]);
                    RunManager.Instance.Battle.Hero.SetJingJie(_jingJie);
                }
            RefreshPools();
        }
    }

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
    public Map()
    {
        _list = new RunNode[HEIGHT * WIDTH];

        _b = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is BattleNodeEntry).ToList());
        _r = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is RewardNodeEntry).ToList());
        _a = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is AdventureNodeEntry).ToList());

        _accessors = new()
        {
            { "Nodes", () => _list },
        };

        _poolConfiguration = new Dictionary<JingJie, AutoPool<NodeEntry>[]>()
        {
            { JingJie.LianQi   , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _b, _r } },
            { JingJie.ZhuJi    , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _b, _r } },
            { JingJie.JinDan   , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _b, _r } },
            { JingJie.YuanYing , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _b, _r } },
            { JingJie.HuaShen  , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _r, _b } },
            { JingJie.FanXu    , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _r, _b } },
        };
    }

    private void RefreshPools()
    {
        AutoPool<NodeEntry>[] pools = _poolConfiguration[_jingJie];

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

                if (pool != _r && y != 0)
                {
                    this[x, y] = null;
                    continue;
                }

                int currX = x;
                NodeEntry nodeEntry = pool.ForcePopItem(pred: e => e.CanCreate(currX));
                if (nodeEntry is BattleNodeEntry battleNodeEntry)
                {
                    CreateEntityDetails d = new CreateEntityDetails(_jingJie, x <= 2, 4 <= x && x <= 6, x >= 8);
                    this[x, y] = new BattleRunNode(new Vector2Int(x, y), _jingJie, battleNodeEntry, d);
                }
                else
                {
                    this[x, y] = new RunNode(new Vector2Int(x, y), _jingJie, nodeEntry);
                }

                if (x == 0)
                    this[x, y].State = RunNode.RunNodeState.ToChoose;
            }
        }

        Selecting = true;
        _heroPosition = new Vector2Int(-1, 0);
    }

    public AdventureNodeEntry NextAdventure()
    {
        int currX = _heroPosition.x;
        for (int x = currX + 1; x < _poolConfiguration[_jingJie].Length; x++)
        for (int y = 0; y < 3; y++)
        {
            RunNode node = this[x, y];
            if (node is { Entry: AdventureNodeEntry adventureNodeEntry })
                return adventureNodeEntry;
        }
        return null;
    }

    public bool HasAdventrueAfterwards(int currX)
    {
        for (int x = currX + 1; x < _poolConfiguration[_jingJie].Length; x++)
        {
            if (_poolConfiguration[_jingJie][x] == _a)
                return true;
        }

        return false;
    }

    public bool RerollNextAdventure()
    {
        int currX = _heroPosition.x;
        for (int x = currX + 1; x < _poolConfiguration[_jingJie].Length; x++)
        for (int y = 0; y < 3; y++)
        {
            RunNode node = this[x, y];
            if (node is { Entry: AdventureNodeEntry adventureNodeEntry })
            {
                NodeEntry newNodeEntry = _a.ForcePopItem(pred: n => n != adventureNodeEntry);
                this[x, y] = new RunNode(new Vector2Int(x, y), _jingJie, newNodeEntry);
                return true;
            }
        }
        return false;
    }
}
