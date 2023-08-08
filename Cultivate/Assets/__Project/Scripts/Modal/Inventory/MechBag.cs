using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBag : GDictionary
{
    private Mech[] _meches;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public MechBag()
    {
        _meches = new Mech[MechType.Length];
        for (int i = 0; i < _meches.Length; i++)
            _meches[i] = new(i);

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
