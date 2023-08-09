
using System;
using System.Collections.Generic;

public class MechBag : GDictionary
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
        => _meches[mechType].Count;

    public void AddMech(MechType mechType, int count)
    {
        _meches[mechType].Count += count;
    }

    public bool TryConsumeMech(MechType mechType, int count)
    {
        if (!CanConsumeMech(mechType, count))
            return false;

        _meches[mechType].Count -= count;
        return true;
    }

    public bool CanConsumeMech(MechType mechType, int count)
    {
        return _meches[mechType].Count >= count;
    }
}
