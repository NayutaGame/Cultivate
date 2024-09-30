
using System;
using System.Collections.Generic;

/// <summary>
/// Formation
/// </summary>
public class Formation : StageClosureOwner, IFormationModel, Addressable
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private RunFormation _runFormation;

    public FormationEntry GetEntry() => _runFormation.GetEntry();

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Formation(StageEntity owner, RunFormation runFormation)
    {
        _accessors = new Dictionary<string, Func<object>>()
        {
            { "Marks", () => GetEntry().GetMarks() },
        };

        _owner = owner;
        _runFormation = runFormation;
    }

    public void Register()
    {
        foreach (StageClosure closure in GetEntry().Closures)
            _owner.Env.ClosureDict.Register(this, closure);
    }

    public void Unregister()
    {
        foreach (StageClosure closure in GetEntry().Closures)
            _owner.Env.ClosureDict.Unregister(this, closure);
    }

    #region IFormationModel

    public string GetName() => _runFormation.GetName();
    public JingJie GetLowestJingJie() => _runFormation.GetLowestJingJie();
    public JingJie? GetActivatedJingJie() => _runFormation.GetActivatedJingJie();
    public string GetConditionDescription() => _runFormation.GetConditionDescription();
    public string GetRewardDescriptionFromJingJie(JingJie jingJie) => _runFormation.GetRewardDescriptionFromJingJie(jingJie);
    public string GetTriviaFromJingJie(JingJie jingJie) => _runFormation.GetTriviaFromJingJie(jingJie);
    public JingJie GetIncrementedJingJie(JingJie jingJie) => _runFormation.GetIncrementedJingJie(jingJie);
    public int GetRequirementFromJingJie(JingJie jingJie) => _runFormation.GetRequirementFromJingJie(jingJie);
    public Predicate<ISkillModel> GetContributorPred() => _runFormation.GetContributorPred();

    #endregion

    #region IMarkedSliderModel

    public int GetMin() => _runFormation.GetMin();
    public int GetMax() => _runFormation.GetMax();
    public int? GetValue() => _runFormation.GetValue();
    public Address GetMarkListModelAddress(Address address)
        => address.Append(".Marks");

    #endregion
}
