using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureNodeEntry : NodeEntry
{
    public AdventureNodeEntry(string id, string description, bool withInPool, Action<Map, RunNode> create, Func<Map, int, bool> canCreate = null)
        : base(id, description, withInPool, create, canCreate)
    {
    }

    public override string GetTitle()
    {
        return "奇遇";
    }
}
