
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
}
