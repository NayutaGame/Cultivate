
using System;

public class RestNodeEntry : NodeEntry
{
    public RestNodeEntry(string id, string description, bool withInPool,
        Action<Map, RunNode, JingJie, int> create,
        Func<Map, JingJie, int, bool> canCreate = null)
        : base(id, description, withInPool, create, canCreate)
    {
    }
}
