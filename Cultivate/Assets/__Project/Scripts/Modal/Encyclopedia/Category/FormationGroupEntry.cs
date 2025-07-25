
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class FormationGroupEntry : Entry, Addressable
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
    
    private int[] _criticalProgresses;

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
        _subFormationEntries.AddRange(formationEntries);
        _subFormationEntries.Do(f => f.SetFormationGroupEntry(this));

        _min = FormationWithLowestJingJie().GetRequirement() - TOLERANCE;
        _max = FormationWithHighestJingJie().GetRequirement();

        _markListModel = new();
        _markListModel.AddRange(_subFormationEntries.Map(e =>
            new MarkModel(e.GetRequirement(), e.GetJingJie().ToString())));

        _criticalProgresses = _subFormationEntries.Map(e => e.GetRequirement()).ToArray();
        

        _spriteEntry = id;
    }
    
    public int[] GetCriticalProgresses() => _criticalProgresses;

    public FormationEntry FirstActivatedFormation(int progress)
        => _subFormationEntries.First(e => progress >= e.GetRequirement());

    public FormationEntry FormationWithLowestJingJie()
        => _subFormationEntries[_subFormationEntries.Count() - 1];

    public FormationEntry FormationWithHighestJingJie()
        => _subFormationEntries[0];

    public FormationEntry FirstFormationWithJingJie(JingJie jingJie)
        => _subFormationEntries.First(e => e.GetJingJie() == jingJie);

    public FormationEntry FirstFormationWithProgress(int progress)
        => _subFormationEntries.First(e => e.GetRequirement() <= progress);

    public JingJie? GetActivatedJingJie() => null;
    public Predicate<ISkill> GetContributorPred() => _contributorPred;
    
    #region IFormationModel

    public string GetConditionDescription() => _progressDescription;

    public Description GetRewardDescription(int progress)
        => FirstFormationWithProgress(progress).GetRewardDescription();
    
    public string GetTrivia(int progress) => FirstFormationWithProgress(progress).GetTrivia();
    
    public SpriteEntry GetIconSprite() => _spriteEntry;

    #endregion
    
    public void GenerateDescription()
    {
        _subFormationEntries.Do(e =>
        {
            e.GenerateDescription();
        });
    }
}
