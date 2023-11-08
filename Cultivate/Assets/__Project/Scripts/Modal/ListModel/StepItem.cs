
using System;
using System.Collections.Generic;

public class StepItem : Addressable
{
    public static readonly int Capacity = 3;

    private int _index;
    public NodeListModel _nodes;
    private bool _isChoosing;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public StepItem()
    {
        _accessors = new()
        {
            { "Nodes",         () => _nodes },
        };

        _nodes = new NodeListModel();
    }

    public void SelectedNode(int index)
    {
        for (int y = 0; y < _nodes.Count(); y++)
            _nodes[y].State = y == index ? RunNode.RunNodeState.Current : RunNode.RunNodeState.Missed;
    }

    public void SetToChoose()
    {
        for (int y = 0; y < _nodes.Count(); y++)
            _nodes[y].State = RunNode.RunNodeState.ToChoose;
    }
}
