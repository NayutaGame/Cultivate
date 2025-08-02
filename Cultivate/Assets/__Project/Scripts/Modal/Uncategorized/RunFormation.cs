
using System;
using System.Collections.Generic;
using CLLibrary;

public class RunFormation : IEmphasizable, AnnotatableFormation 
{
    private FormationGroupEntry _formationGroupEntry;
    private int _progress;
    private bool _activated;
    private FormationEntry _formationEntry;

    public FormationEntry GetEntry() => _formationEntry;
    public int GetProgress() => _progress;
    public void SetProgress(int progress)
    {
        _progress = progress;

        FormationEntry firstActivated = _formationGroupEntry.FirstActivatedFormation(_progress);
        if (firstActivated == null || firstActivated.GetJingJie() <= JingJie.LianQi)
        {
            _activated = false;
            _formationEntry = _formationGroupEntry.FormationWithLowestJingJie();
        }
        else
        {
            _activated = true;
            _formationEntry = firstActivated;
        }
    }

    public bool IsActivated() => _activated;

    private Neuron _emphasisNeuron;
    public Neuron GetEmphasisNeuron()
        => _emphasisNeuron;

    public bool CanShowAnnotation() => true;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Marks",                      thisObject => ((RunFormation)thisObject)._formationGroupEntry.GetMarks() },
    };
    public object Get(string s) => Accessor[s](this);
    private RunFormation(FormationGroupEntry formationGroupEntry, int progress, bool activated, FormationEntry formationEntry)
    {
        _formationGroupEntry = formationGroupEntry;
        _progress = progress;
        _activated = activated;
        _formationEntry = formationEntry;

        _emphasisNeuron = new();
    }

    public void Emphasize()
        => _emphasisNeuron.Invoke();

    public static RunFormation From(FormationGroupEntry entry, int progress)
    {
        FormationEntry firstActivated = entry.FirstActivatedFormation(progress);
        if (firstActivated == null || firstActivated.GetJingJie() <= JingJie.LianQi)
            return new(entry, progress, false, entry.FormationWithLowestJingJie());

        return new(entry, progress, true, firstActivated);
    }

    public JingJie GetActivatedJingJie() => IsActivated() ? _formationEntry.GetActivatedJingJie() : null;
    
    public Predicate<RunSkill> GetContributorPred() => _formationGroupEntry.GetContributorPred();
    
    #region AnnotatableFormation

    public string GetName() => _formationEntry.GetName();
    public string GetConditionDescription() => _formationEntry.GetConditionDescription();

    public int[] GetCriticalProgresses() => _formationGroupEntry.GetCriticalProgresses();

    public Description GetRewardDescription(int progress) => _formationGroupEntry.GetRewardDescription(progress);
    public string GetTrivia(int progress) => _formationGroupEntry.GetTrivia(progress);
    
    public SpriteEntry GetBackgroundSprite()
    {
        JingJie activatedJingJie = GetActivatedJingJie();
        string spriteName = activatedJingJie != null ? $"{activatedJingJie.GetName()}阵法背景" : "未激活阵法背景";
        return Encyclopedia.SpriteCategory.FromName(spriteName);
    }

    public SpriteEntry GetIconSprite() => _formationGroupEntry.GetIconSprite();

    #endregion
}
