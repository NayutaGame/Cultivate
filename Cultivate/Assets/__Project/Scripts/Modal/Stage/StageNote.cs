
using System;
using UnityEngine;

public class StageNote : ISkill
{
    public int EntityIndex;
    public int TemporalIndex;
    public StageSkill Skill;

    [NonSerialized] public CostDescription ActualCostDescription;
    [NonSerialized] public string ActualDescription;

    public StageNote(int entityIndex, int temporalIndex, StageSkill skill, int currCounter = 0, int maxCounter = 0)
    {
        EntityIndex = entityIndex;
        TemporalIndex = temporalIndex;
        Skill = skill.Clone();

        _currCounter = currCounter;
        _maxCounter = maxCounter;
    }

    public bool IsHome
        => EntityIndex == 0;
    
    
    

    private int _currCounter;
    public int GetCurrCounter() => _currCounter;

    private int _maxCounter;
    public int GetMaxCounter() => _maxCounter;

    public Sprite GetSprite()
        => Skill.Entry.GetSprite();

    public WuXing? GetWuXing()
        => Skill.Entry.GetWuXing();

    public string GetName()
        => Skill.Entry.GetName();

    public TagComposite GetTagComposite()
        => Skill.Entry.GetTagComposite();

    public string GetCascadeAnnotated()
        => Skill.Entry.GetCascadeAnnotated();

    public string GetTrivia()
        => Skill.Entry.GetTrivia();

    public JingJie GetJingJie()
        => Skill.GetJingJie();

    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
        => GetJingJie() == showingJingJie && ActualCostDescription != null
            ? ActualCostDescription
            : Skill.Entry.GetLiteralCostDescription(showingJingJie);

    public string GetHighlight(JingJie showingJingJie)
        => GetJingJie() == showingJingJie && ActualDescription != null
            ? ActualDescription
            : Skill.Entry.GetHighlight(showingJingJie);

    public Sprite GetJingJieSprite(JingJie showingJingJie)
        => Skill.Entry.GetJingJieSprite(showingJingJie);

    public JingJie NextJingJie(JingJie showingJingJie)
        => Skill.Entry.NextJingJie(showingJingJie);
}
