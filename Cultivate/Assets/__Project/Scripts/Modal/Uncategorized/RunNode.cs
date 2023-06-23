using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunNode : GDictionary
{
    protected NodeEntry _entry;
    public NodeEntry Entry => _entry;

    protected SpriteEntry _spriteEntry;
    public Sprite Sprite => _spriteEntry.Sprite;

    public Vector2Int Position { get; private set; }
    public JingJie JingJie { get; private set; }

    private RunNodeState _state;
    public RunNodeState State
    {
        get => _state;
        set
        {
            _state = value;

            if (_state == RunNodeState.Current)
                _entry.Create(this);
        }
    }

    public virtual string GetTitle() => _entry.GetTitle();

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
    public RunNode(Vector2Int position, JingJie jingJie, NodeEntry entry)
    {
        _accessors = new()
        {
            { "CurrentPanel",          () => CurrentPanel },
        };

        if (entry is RewardNodeEntry r)
        {
            _spriteEntry = r.SpriteEntry;
        }
        else if (entry is AdventureNodeEntry)
        {
            _spriteEntry = "奇遇";
        }

        Position = position;
        JingJie = jingJie;
        _entry = entry;
        _state = RunNodeState.Locked;
    }

    public enum RunNodeState
    {
        Passed,
        Missed,
        Current,
        ToChoose,
        Locked,
    }

    public PanelDescriptor CurrentPanel { get; private set; }

    public void ChangePanel(PanelDescriptor panel)
    {
        CurrentPanel?.Exit();
        CurrentPanel = panel;
        CurrentPanel?.Enter();
    }
}
