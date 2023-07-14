using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class FormationEntry : Entry, GDictionary
{
    private SubFormationEntry[] _subFormationEntries;
    public SubFormationEntry[] SubFormationEntries => _subFormationEntries;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public FormationEntry(string name, SubFormationEntry[] subFormationEntries) : base(name)
    {
        _accessors = new Dictionary<string, Func<object>>()
        {
            { "SubFormations", () => _subFormationEntries },
        };

        _subFormationEntries = subFormationEntries;
        foreach (SubFormationEntry f in _subFormationEntries)
            f.SetName(name);
    }

    public SubFormationEntry FirstActivatedFormation(RunEntity entity, FormationArguments args)
    {
        return _subFormationEntries.FirstObj(f => f.CanActivate(entity, args));
    }
}
