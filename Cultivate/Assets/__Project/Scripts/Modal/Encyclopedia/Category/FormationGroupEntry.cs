using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class FormationGroupEntry : Entry, Addressable
{
    private int _order;
    public int Order => _order;

    private ListModel<FormationEntry> _subFormationEntries;
    public ListModel<FormationEntry> SubFormationEntries => _subFormationEntries;

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

        _subFormationEntries = new ListModel<FormationEntry>();

        if (formationEntries != null)
            _subFormationEntries.AddRange(formationEntries);

        _subFormationEntries.Traversal().Do(f =>
        {
            f.SetName(name);
            f.SetOrder(order);
        });
    }

    public FormationEntry FirstActivatedFormation(RunEntity entity, FormationArguments args)
    {
        return _subFormationEntries.Traversal().FirstObj(f => f.CanActivate(entity, args));
    }
}
