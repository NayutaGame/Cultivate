using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureNodeEntry : NodeEntry
{
    public AdventureNodeEntry(string name, string description, bool normal, Action<RunNode> create, Func<Map, int, bool> canCreate = null) : base(name, description, normal, create, canCreate)
    {
    }

    public override string GetTitle()
    {
        return "奇遇";
    }
}
