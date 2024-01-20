
using System;
using System.Collections.Generic;
using CLLibrary;

public class FormationGroupEntry : Entry, Addressable
{
    private int _order;
    public int Order => _order;

    private string _conditionDescription;
    public string GetConditionDescription()
        => _conditionDescription;

    private Func<RunEntity, RunFormationDetails, int> _scoreEvaluator;
    public int GetScore(RunEntity e, RunFormationDetails d)
        => _scoreEvaluator(e, d);

    private ListModel<FormationEntry> _subFormationEntries;
    public ListModel<FormationEntry> SubFormationEntries => _subFormationEntries;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public FormationGroupEntry(string name, int order, string conditionDescription, Func<RunEntity, RunFormationDetails, int> scoreEvaluator, FormationEntry[] formationEntries = null) : base(name)
    {
        _accessors = new Dictionary<string, Func<object>>()
        {
            { "SubFormations", () => _subFormationEntries },
        };

        _order = order;
        _conditionDescription = conditionDescription;
        _scoreEvaluator = scoreEvaluator;

        _subFormationEntries = new ListModel<FormationEntry>();

        if (formationEntries != null)
            _subFormationEntries.AddRange(formationEntries);

        _subFormationEntries.Traversal().Do(f =>
        {
            f.SetName(name);
            f.SetOrder(order);
            f.SetConditionDescription(conditionDescription);
        });
    }

    public FormationEntry FirstActivatedFormation(RunEntity e, RunFormationDetails d)
    {
        return _subFormationEntries.Traversal().FirstObj(f => GetScore(e, d) >= f.GetRequirement());
    }
}
