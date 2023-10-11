
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class Map : Addressable
{
    private static readonly int HEIGHT = 3;
    private static readonly int WIDTH = 10;

    public event Action<JingJie> JingJieChangedEvent;
    public void JingJieChanged(JingJie jingJie) => JingJieChangedEvent?.Invoke(jingJie);

    private RunNode[] _list;

    private RunNode this[int x, int y]
    {
        get => _list[x * HEIGHT + y];
        set => _list[x * HEIGHT + y] = value;
    }

    private bool InRange(int x, int y)
    {
        int i = x * HEIGHT + y;
        return 0 <= i && i < _list.Length;
    }

    private RunNode this[Vector2Int pos]
    {
        get => this[pos.x, pos.y];
        set => this[pos.x, pos.y] = value;
    }

    public EntityPool EntityPool { get; }
    private AutoPool<NodeEntry> _b;
    private AutoPool<NodeEntry> _r;
    private AutoPool<NodeEntry> _a;
    private Dictionary<JingJie, AutoPool<NodeEntry>[]> _poolConfiguration;

    public bool Selecting { get; private set; }
    private Vector2Int _heroPosition;

    public RunNode CurrentNode
    {
        get
        {
            if (_heroPosition.x < 0)
                return null;
            return this[_heroPosition];
        }
    }

    public PanelDescriptor SelectedNode(RunNode selected)
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
        return CurrentNode.CurrentPanel;
    }

    public PanelDescriptor ReceiveSignal(Signal signal)
    {
        PanelDescriptor panelDescriptor = CurrentNode.CurrentPanel.ReceiveSignal(signal);
        if (panelDescriptor != null)
        {
            CurrentNode.ChangePanel(panelDescriptor);
        }
        else
        {
            TryFinishNode();
        }
        return panelDescriptor;
    }

    private void TryFinishNode()
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

        CurrentNode.ChangePanel(null);

        if (isEnd)
            JingJie += 1;
    }

    private readonly Dictionary<JingJie, AudioEntry> JingJieToAudio = new()
    {
        { JingJie.LianQi, "练气BGM" },
        { JingJie.ZhuJi, "筑基BGM" },
        { JingJie.JinDan, "金丹BGM" },
        { JingJie.YuanYing, "元婴BGM" },
        { JingJie.HuaShen, "化神BGM" },
    };

    private JingJie _jingJie;
    public JingJie JingJie
    {
        get => _jingJie;
        set
        {
            _jingJie = value;
            JingJieChanged(_jingJie);
            RefreshPools();
            AudioManager.Instance.Play(JingJieToAudio[_jingJie]);
        }
    }

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Map()
    {
        _accessors = new()
        {
            { "Nodes", () => _list },
            { "CurrentNode", () => CurrentNode },
        };

        _list = new RunNode[HEIGHT * WIDTH];

        EntityPool = new();
        EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal());
        _b = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is BattleNodeEntry).ToList());
        _r = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is RewardNodeEntry).ToList());
        _a = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n is AdventureNodeEntry).ToList());
        _poolConfiguration = new Dictionary<JingJie, AutoPool<NodeEntry>[]>()
        {
            { JingJie.LianQi   , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _b, _r } },
            { JingJie.ZhuJi    , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _b, _r } },
            { JingJie.JinDan   , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _b, _r } },
            { JingJie.YuanYing , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _b, _r } },
            { JingJie.HuaShen  , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _r, _b } },
            { JingJie.FanXu    , new[] { _b, _r, _b, _a, _b, _r, _b, _a, _r, _b } },
        };

        // _poolConfiguration = new Dictionary<JingJie, AutoPool<NodeEntry>[]>()
        // {
        //     { JingJie.LianQi   , new[] { _a, _b, _b, _r, _b, _r, _b, _a, _r, _b } },
        //     { JingJie.ZhuJi    , new[] { _a, _b, _b, _r, _b, _r, _b, _a, _r, _b } },
        //     { JingJie.JinDan   , new[] { _a, _b, _b, _r, _b, _r, _b, _a, _r, _b } },
        //     { JingJie.YuanYing , new[] { _a, _b, _b, _r, _b, _r, _b, _a, _r, _b } },
        //     { JingJie.HuaShen  , new[] { _a, _b, _b, _r, _b, _r, _b, _a, _r, _b } },
        //     { JingJie.FanXu    , new[] { _a, _b, _b, _r, _b, _r, _b, _a, _r, _b } },
        // };

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
                NodeEntry nodeEntry = pool.ForcePopItem(pred: e => e.CanCreate(this, currX));
                if (nodeEntry is BattleNodeEntry battleNodeEntry)
                {
                    DrawEntityDetails d = new DrawEntityDetails(_jingJie, x <= 3, 3 < x && x <= 6, 6 < x);
                    this[x, y] = new BattleRunNode(this, new Vector2Int(x, y), _jingJie, battleNodeEntry, d);
                }
                else
                {
                    this[x, y] = new RunNode(this, new Vector2Int(x, y), _jingJie, nodeEntry);
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

    public bool HasAdventureAfterwards(int currX)
    {
        for (int x = currX + 1; x < _poolConfiguration[_jingJie].Length; x++)
        {
            if (_poolConfiguration[_jingJie][x] == _a)
                return true;
        }

        return false;
    }

    public bool RedrawNextAdventure()
    {
        int currX = _heroPosition.x;
        for (int x = currX + 1; x < _poolConfiguration[_jingJie].Length; x++)
        for (int y = 0; y < 3; y++)
        {
            RunNode node = this[x, y];
            if (node is { Entry: AdventureNodeEntry adventureNodeEntry })
            {
                NodeEntry newNodeEntry = _a.ForcePopItem(pred: n => n != adventureNodeEntry);
                this[x, y] = new RunNode(this, new Vector2Int(x, y), _jingJie, newNodeEntry);
                return true;
            }
        }
        return false;
    }
}
