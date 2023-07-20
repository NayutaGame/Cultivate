using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class FormationGroupEntry : Entry, GDictionary
{
    private int _order;
    public int Order => _order;

    private FormationEntry[] _subFormationEntries;
    public FormationEntry[] SubFormationEntries => _subFormationEntries;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public FormationGroupEntry(string name, int order = 0, FormationEntry[] formationEntries = null) : base(name)
    {
        _accessors = new Dictionary<string, Func<object>>()
        {
            { "SubFormations", () => _subFormationEntries },
        };

        _order = order;

        _subFormationEntries = formationEntries ?? new FormationEntry[] { };
        foreach (FormationEntry f in _subFormationEntries)
        {
            f.SetName(name);
            f.SetOrder(order);
        }
    }

    public FormationEntry FirstActivatedFormation(RunEntity entity, FormationArguments args)
    {
        return _subFormationEntries.FirstObj(f => f.CanActivate(entity, args));
    }
}
