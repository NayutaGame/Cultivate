
using System;
using System.Collections.Generic;
using CLLibrary;

public class FormationGroupEntry : Entry, Addressable, IFormationModel
{
    public string GetName() => GetId();
    
    private static readonly int TOLERANCE = 4;

    private int _order;
    public int Order => _order;

    private string _conditionDescription;

    private Func<RunEntity, RunFormationDetails, int> _progressEvaluator;
    public int GetProgress(RunEntity e, RunFormationDetails d) => _progressEvaluator(e, d);

    private ListModel<FormationEntry> _subFormationEntries;
    public ListModel<FormationEntry> SubFormationEntries => _subFormationEntries;

    private int _min;
    private int _max;

    private ListModel<MarkModel> _markListModel;
    public ListModel<MarkModel> GetMarks() => _markListModel;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public FormationGroupEntry(string id, int order, string conditionDescription, Func<RunEntity, RunFormationDetails, int> progressEvaluator, FormationEntry[] formationEntries = null) : base(id)
    {
        _accessors = new Dictionary<string, Func<object>>()
        {
            { "SubFormations", () => _subFormationEntries },
            { "Marks", () => _markListModel },
        };

        _order = order;
        _conditionDescription = conditionDescription;
        _progressEvaluator = progressEvaluator;

        _subFormationEntries = new ListModel<FormationEntry>();

        if (formationEntries != null)
            _subFormationEntries.AddRange(formationEntries);

        _subFormationEntries.Traversal().Do(f => f.SetFormationGroupEntry(this));

        _min = FormationWithLowestJingJie().GetRequirement() - TOLERANCE;
        _max = FormationWithHighestJingJie().GetRequirement();

        _markListModel = new();
        _markListModel.AddRange(_subFormationEntries.Traversal().Map(e =>
            new MarkModel(e.GetRequirement(), e.GetJingJie().ToString())));
    }

    public FormationEntry FirstActivatedFormation(int progress)
        => _subFormationEntries.First(e => progress >= e.GetRequirement());

    public FormationEntry FormationWithLowestJingJie()
        => _subFormationEntries[_subFormationEntries.Count() - 1];

    public FormationEntry FormationWithHighestJingJie()
        => _subFormationEntries[0];

    public FormationEntry FirstFormationWithJingJie(JingJie jingJie)
        => _subFormationEntries.First(e => e.GetJingJie() == jingJie);

    #region IFormationModel

    public JingJie GetLowestJingJie() => FormationWithLowestJingJie().GetJingJie();
    public JingJie? GetActivatedJingJie() => null;
    public string GetConditionDescription() => _conditionDescription;
    public string GetRewardDescriptionFromJingJie(JingJie jingJie) => FirstFormationWithJingJie(jingJie).GetRewardDescription();
    public string GetTriviaFromJingJie(JingJie jingJie) => FirstFormationWithJingJie(jingJie).GetTrivia();
    public JingJie GetIncrementedJingJie(JingJie jingJie)
    {
        int index = _subFormationEntries.Traversal().FirstIdx(e => e.GetJingJie() == jingJie).Value;
        index--;
        if (index < 0)
            index += _subFormationEntries.Count();
        return _subFormationEntries[index].GetJingJie();
    }
    public int GetRequirementFromJingJie(JingJie jingJie) => FirstFormationWithJingJie(jingJie).GetRequirement();

    #endregion

    #region IMarkedSliderModel

    public int GetMin() => _min;
    public int GetMax() => _max;
    public int? GetValue() => null;
    public Address GetMarkListModelAddress(Address address)
        => address.Append(".Marks");

    #endregion
}
