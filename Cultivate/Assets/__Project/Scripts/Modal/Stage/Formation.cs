
using System;
using System.Collections.Generic;
using CLLibrary;

/// <summary>
/// Formation
/// </summary>
public class Formation : StageClosureListener, IEmphasizable, AnnotatableFormation
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private RunFormation _runFormation;

    public FormationEntry GetEntry() => _runFormation.GetEntry();

    private Neuron _emphasisNeuron;
    public Neuron GetEmphasisNeuron()
        => _emphasisNeuron;

    public bool CanShowAnnotation() => true;
    
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
    
    public JingJie GetActivatedJingJie()
        => _runFormation.GetActivatedJingJie();

    #region AnnotatableFormation

    public string GetName() => _runFormation.GetName();
    public string GetConditionDescription() => _runFormation.GetConditionDescription();
    public int[] GetCriticalProgresses() => _runFormation.GetCriticalProgresses();
    public int GetProgress() => _runFormation.GetProgress();
    public Description GetRewardDescription(int progress) => _runFormation.GetRewardDescription(progress);
    public string GetTrivia(int progress) => _runFormation.GetTrivia(progress);
    public SpriteEntry GetBackgroundSprite() => _runFormation.GetBackgroundSprite();
    public SpriteEntry GetIconSprite() => _runFormation.GetIconSprite();

    #endregion
}
