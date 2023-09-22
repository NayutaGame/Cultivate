using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeEntry : Entry
{
    private string _description;
    public string Description => _description;

    private Predicate<int> _canCreate;
    private Action<RunNode> _create;

    public NodeEntry(string name, string description, Action<RunNode> create, Predicate<int> canCreate = null) : base(name)
    {
        _description = description;
        _create = create;
        _canCreate = canCreate ?? (x => true);
    }

    public bool CanCreate(int x) => _canCreate(x);
    public void Create(RunNode runNode) => _create(runNode);

    public static implicit operator NodeEntry(string name) => Encyclopedia.NodeCategory[name];

    public virtual string GetTitle()
    {
        return Name;
    }
}
