using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardNodeEntry : NodeEntry
{
    public RewardNodeEntry(string name, string description, Action<RunNode> create, Predicate<int> canCreate = null) : base(name, description, create, canCreate)
    {
    }
}
