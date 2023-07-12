using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class FormationEntry : Entry
{
    private SubFormationEntry[] _subFormationEntries;

    public FormationEntry(string name, SubFormationEntry[] subFormationEntries) : base(name)
    {
        _subFormationEntries = subFormationEntries;
        foreach (SubFormationEntry f in _subFormationEntries)
            f.SetName(name);
    }

    public SubFormationEntry FirstActivatedFormation(RunEntity entity, FormationArguments args)
    {
        return _subFormationEntries.FirstObj(f => f.CanActivate(entity, args));
    }
}
