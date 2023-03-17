using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeEntry : Entry
{
    private string _description;
    public string Description;

    public NodeEntry(string name, string description) : base(name)
    {
        _description = description;
    }
}
