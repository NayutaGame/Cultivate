
using System;

public class AdventureNodeEntry : NodeEntry
{
    public AdventureNodeEntry(string id, string description, bool withInPool,
        Action<Map, RunNode, JingJie, int> create,
        Func<Map, JingJie, int, bool> canCreate = null)
        : base(id, description, withInPool, create, canCreate)
    {
    }
}
