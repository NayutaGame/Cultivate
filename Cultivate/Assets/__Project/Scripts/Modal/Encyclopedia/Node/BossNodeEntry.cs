using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNodeEntry : NodeEntry
{
    public BossNodeEntry(string name, string description, Action<RunNode> create) : base(name, description, create)
    {
    }
}
