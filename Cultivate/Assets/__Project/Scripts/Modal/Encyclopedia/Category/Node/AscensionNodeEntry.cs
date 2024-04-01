
using System;

public class AscensionNodeEntry : NodeEntry
{
    public AscensionNodeEntry(string id, string description, bool withInPool,
        Action<Map, RunNode, JingJie, int> create,
        Func<Map, JingJie, int, bool> canCreate = null)
        : base(id, description, withInPool, create, canCreate)
    {
    }
}
