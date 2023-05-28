using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureNodeEntry : NodeEntry
{
    public AdventureNodeEntry(string name, string description, Action<RunNode> create) : base(name, description, create)
    {
    }

    public override string GetTitle()
    {
        return "奇遇";
    }
}
