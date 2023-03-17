using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunNode
{
    private NodeEntry _entry;
    private RunNodeState _state;

    public string GetName() => _entry.Name;

    public RunNode(string entryName) : this(Encyclopedia.NodeCategory[entryName]) { }
    public RunNode(NodeEntry entry)
    {
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
}
