
using System;
using System.Collections.Generic;
using CLLibrary;

public class RunFormation : IFormationModel, Addressable
{
    private FormationGroupEntry _entry;
    private int _progress;
    private bool _activated;
    private FormationEntry _formationEntry;

    public FormationEntry GetEntry() => _formationEntry;
    public int GetProgress() => _progress;
    public bool IsActivated() => _activated;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private RunFormation(FormationGroupEntry entry, int progress, bool activated, FormationEntry formationEntry)
    {
        _accessors = new Dictionary<string, Func<object>>()
        {
            { "Marks", () => _entry.GetMarks() },
        };

        _entry = entry;
        _progress = progress;
        _activated = activated;
        _formationEntry = formationEntry;
    }

    public static RunFormation From(FormationGroupEntry entry, int progress)
    {
        FormationEntry firstActivated = entry.FirstActivatedFormation(progress);
        if (firstActivated == null || firstActivated.GetJingJie() <= JingJie.LianQi)
            return new(entry, progress, false, entry.FormationWithLowestJingJie());

        return new(entry, progress, true, firstActivated);
    }
    
    public JingJie GetNextActivatingJingJie()
    {
        JingJie? activatedJingJie = GetActivatedJingJie();
        JingJie highestJingJie = GetEntry().GetFormationGroupEntry().SubFormationEntries[0].GetJingJie();
        if (!activatedJingJie.HasValue)
            return GetLowestJingJie();
        
        if (activatedJingJie != highestJingJie)
            return GetIncrementedJingJie(activatedJingJie.Value);
        
        return highestJingJie;
    }

    #region IFormationModel

    public string GetName() => _formationEntry.GetName();
    public JingJie GetLowestJingJie() => _formationEntry.GetLowestJingJie();
    public JingJie? GetActivatedJingJie() => IsActivated() ? _formationEntry.GetActivatedJingJie() : null;
    public string GetConditionDescription() => _formationEntry.GetConditionDescription();
    public string GetRewardDescriptionFromJingJie(JingJie jingJie) => _formationEntry.GetRewardDescriptionFromJingJie(jingJie);

    public string GetHighlightedRewardDescriptionFromJingJie(JingJie jingJie)
        => _formationEntry.GetHighlightedRewardDescriptionFromJingJie(jingJie);

    public string GetRewardDescriptionAnnotationFromJingJie(JingJie jingJie)
        => _formationEntry.GetRewardDescriptionAnnotationFromJingJie(jingJie);

    public string GetTriviaFromJingJie(JingJie jingJie) => _formationEntry.GetTriviaFromJingJie(jingJie);
    public JingJie GetIncrementedJingJie(JingJie jingJie) => _formationEntry.GetIncrementedJingJie(jingJie);
    public int GetRequirementFromJingJie(JingJie jingJie) => _formationEntry.GetRequirementFromJingJie(jingJie);
    public Predicate<ISkill> GetContributorPred() => _formationEntry.GetContributorPred();
    public SpriteEntry GetSprite() => _formationEntry.GetSprite();

    #endregion

    #region IMarkedSliderModel

    public int GetMin() => _entry.GetMin();
    public int GetMax() => _entry.GetMax();
    public int? GetValue() => _progress.Clamp(GetMin(), GetMax());
    public Address GetMarkListModelAddress(Address address)
        => address.Append(".Marks");

    #endregion
}
