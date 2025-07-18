
using System;
using System.Collections.Generic;
using CLLibrary;

/// <summary>
/// Formation
/// </summary>
public class Formation : StageClosureListener, IFormationModel, Addressable, IEmphasizable
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private RunFormation _runFormation;

    public FormationEntry GetEntry() => _runFormation.GetEntry();

    private Neuron _emphasisNeuron;
    public Neuron GetEmphasisNeuron()
        => _emphasisNeuron;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Marks",                      thisObject => ((Formation)thisObject).GetEntry().GetMarks() },
    };
    public object Get(string s) => Accessor[s](this);
    public Formation(StageEntity owner, RunFormation runFormation)
    {
        _owner = owner;
        _runFormation = runFormation;

        _emphasisNeuron = new();
    }

    public void Emphasize()
        => _emphasisNeuron.Invoke();
    
    public void Register()
    {
        foreach (StageClosure closure in GetEntry().StageClosures)
            _owner.Env.ClosureDict.Register(this, closure);
    }

    public void Unregister()
    {
        foreach (StageClosure closure in GetEntry().StageClosures)
            _owner.Env.ClosureDict.Unregister(this, closure);
    }

    public bool IsActivated()
        => _runFormation.IsActivated();

    #region IFormationModel

    public string GetName() => _runFormation.GetName();
    public JingJie GetLowestJingJie() => _runFormation.GetLowestJingJie();
    public JingJie? GetActivatedJingJie() => _runFormation.GetActivatedJingJie();
    public string GetConditionDescription() => _runFormation.GetConditionDescription();
    public string GetRewardDescriptionFromJingJie(JingJie jingJie) => _runFormation.GetRewardDescriptionFromJingJie(jingJie);

    public string GetHighlightedRewardDescriptionFromJingJie(JingJie jingJie)
        => _runFormation.GetHighlightedRewardDescriptionFromJingJie(jingJie);

    public string GetRewardDescriptionAnnotationFromJingJie(JingJie jingJie)
        => _runFormation.GetRewardDescriptionAnnotationFromJingJie(jingJie);

    public string GetTriviaFromJingJie(JingJie jingJie) => _runFormation.GetTriviaFromJingJie(jingJie);
    public JingJie GetIncrementedJingJie(JingJie jingJie) => _runFormation.GetIncrementedJingJie(jingJie);
    public int GetRequirementFromJingJie(JingJie jingJie) => _runFormation.GetRequirementFromJingJie(jingJie);
    public Predicate<ISkill> GetContributorPred() => _runFormation.GetContributorPred();
    public SpriteEntry GetSprite() => _runFormation.GetSprite();

    #endregion

    #region IMarkedSliderModel

    public int GetMin() => _runFormation.GetMin();
    public int GetMax() => _runFormation.GetMax();
    public int? GetValue() => _runFormation.GetValue();
    public Address GetMarkListModelAddress(Address address)
        => address.Append(".Marks");

    #endregion
}
