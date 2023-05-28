using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeEntry : Entry
{
    private string _description;
    public string Description;

    private Func<RunNode> _canCreate;
    private Action<RunNode> _create;

    public NodeEntry(string name, string description, Action<RunNode> create) : base(name)
    {
        _description = description;
        _create = create;
    }

    public void Create(RunNode runNode) => _create(runNode);

    public static implicit operator NodeEntry(string name) => Encyclopedia.NodeCategory[name];

    public virtual string GetTitle()
    {
        return Name;
    }
}
