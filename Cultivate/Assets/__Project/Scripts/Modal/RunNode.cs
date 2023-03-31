using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunNode
{
    private NodeEntry _entry;
    public Vector2Int Position { get; private set; }

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

    public string GetName() => _entry.Name;

    public RunNode(Vector2Int position, NodeEntry entry)
    {
        Position = position;
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
        CurrentPanel.Enter();
    }
}
