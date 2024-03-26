
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

    public void MakeChoice(int choice)
    {
        for (int i = 0; i < _nodes.Count(); i++)
            _nodes[i].SetState(i == choice ? RunNode.RunNodeState.Current : RunNode.RunNodeState.Missed);
    }

    public void ToChoose()
    {
        for (int i = 0; i < _nodes.Count(); i++)
            _nodes[i].SetState(RunNode.RunNodeState.ToChoose);
    }

    public int IndexOf(RunNode runNode)
        => _nodes.IndexOf(runNode);
}
