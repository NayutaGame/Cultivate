
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class FormationGroupEntry : Entry, Addressable, IFormationModel
{
    public string GetName() => GetId();
    
    private static readonly int TOLERANCE = 4;

    private int _order;
    public int Order => _order;

    private Predicate<ISkill> _contributorPred;

    private string _progressDescription;

    private Func<RunEntity, RunFormationDetails, int> _progressEvaluator;
    public int GetProgress(RunEntity e, RunFormationDetails d) => _progressEvaluator(e, d);

    private ListModel<FormationEntry> _subFormationEntries;
    public ListModel<FormationEntry> SubFormationEntries => _subFormationEntries;

    private int _min;
    private int _max;

    private ListModel<MarkModel> _markListModel;
    public ListModel<MarkModel> GetMarks() => _markListModel;

    private SpriteEntry _spriteEntry;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "SubFormations",              thisObject => ((FormationGroupEntry)thisObject)._subFormationEntries },
        { "Marks",                      thisObject => ((FormationGroupEntry)thisObject)._markListModel },
    };
    public object Get(string s) => Accessor[s](this);
    public FormationGroupEntry(string id, int order, Predicate<ISkill> contributorPred, string progressDescription, Func<RunEntity, RunFormationDetails, int> progressEvaluator, FormationEntry[] formationEntries = null) : base(id)
    {
        _order = order;
        _contributorPred = contributorPred;
        _progressDescription = progressDescription;
        _progressEvaluator = progressEvaluator;

        _subFormationEntries = new ListModel<FormationEntry>();

        if (formationEntries != null)
            _subFormationEntries.AddRange(formationEntries);

        _subFormationEntries.Do(f => f.SetFormationGroupEntry(this));

        _min = FormationWithLowestJingJie().GetRequirement() - TOLERANCE;
        _max = FormationWithHighestJingJie().GetRequirement();

        _markListModel = new();
        _markListModel.AddRange(_subFormationEntries.Map(e =>
            new MarkModel(e.GetRequirement(), e.GetJingJie().ToString())));

        _spriteEntry = id;
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
    public string GetConditionDescription() => _progressDescription;

    public Description GetRewardDescription(JingJie jingJie)
        => FirstFormationWithJingJie(jingJie).GetRewardDescription();
    
    public string GetTriviaFromJingJie(JingJie jingJie) => FirstFormationWithJingJie(jingJie).GetTrivia();
    public JingJie GetIncrementedJingJie(JingJie jingJie)
    {
        int index = _subFormationEntries.FirstIdx(e => e.GetJingJie() == jingJie).Value;
        index--;
        if (index < 0)
            index += _subFormationEntries.Count();
        return _subFormationEntries[index].GetJingJie();
    }
    public int GetRequirementFromJingJie(JingJie jingJie) => FirstFormationWithJingJie(jingJie).GetRequirement();
    public Predicate<ISkill> GetContributorPred() => _contributorPred;
    public SpriteEntry GetSprite() => _spriteEntry;

    #endregion

    #region IMarkedSliderModel

    public int GetMin() => _min;
    public int GetMax() => _max;
    public int? GetValue() => null;
    public Address GetMarkListModelAddress(Address address)
        => address.Append(".Marks");

    #endregion
    
    public void GenerateDescription()
    {
        _subFormationEntries.Do(e =>
        {
            e.GenerateDescription();
        });
    }
}
