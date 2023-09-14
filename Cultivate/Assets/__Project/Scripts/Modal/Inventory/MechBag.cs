
using System;
using System.Collections.Generic;

public class MechBag : Addressable
{
    private Mech[] _meches;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public MechBag()
    {
        _meches = new Mech[MechType.Length];

        int i = 0;
        foreach (MechType t in MechType.Traversal)
        {
            _meches[i] = new(t);
            i++;
        }

        _accessors = new Dictionary<string, Func<object>>()
        {
            { "List", () => _meches },
        };
    }

    public int GetCount(MechType mechType)
        => _meches[mechType._index].Count;

    public void AddMech(MechType mechType, int count = 1)
    {
        _meches[mechType._index].Count += count;
    }

    public bool TryConsumeMech(MechType mechType, int count = 1)
    {
        if (!CanConsumeMech(mechType, count))
            return false;

        _meches[mechType._index].Count -= count;
        return true;
    }

    public bool CanConsumeMech(MechType mechType, int count = 1)
    {
        return _meches[mechType._index].Count >= count;
    }
}
