
using System;
using System.Collections.Generic;
using UnityEngine;

public class StageNote : AnnotatableSkill
{
    public int EntityIndex;
    public int TemporalIndex;
    public StageSkill Skill;

    [NonSerialized] public CostDescription ActualCostDescription;
    [NonSerialized] public Description ActualDescription;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
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

    public string GetTrivia()
        => Skill.Entry.GetTrivia();

    public JingJie GetJingJie()
        => Skill.GetJingJie();

    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
        => GetJingJie() == showingJingJie && ActualCostDescription != null
            ? ActualCostDescription
            : Skill.Entry.GetLiteralCostDescription(showingJingJie);
    
    public Description GetDescription(JingJie showingJingJie)
        => GetJingJie() == showingJingJie && ActualDescription != null
            ? ActualDescription
            : Skill.Entry.GetDescription(showingJingJie);

    public Sprite GetJingJieSprite(JingJie showingJingJie)
        => Skill.Entry.GetJingJieSprite(showingJingJie);

    public JingJie NextJingJie(JingJie showingJingJie)
        => Skill.Entry.NextJingJie(showingJingJie);

    public bool CanShowAnnotation()
        => true;

    public JingJie GetLowestJingJie()
        => Skill.Entry.GetLowestJingJie();

    public JingJie GetHighestJingJie()
        => Skill.Entry.GetHighestJingJie();
}
