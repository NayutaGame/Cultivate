
using UnityEngine;

public class StageNote : ISkillModel
{
    public int EntityIndex;
    public int TemporalIndex;
    public StageSkill Skill;

    public CostResult CostResult;
    public CastResult CastResult;

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

    public Sprite GetWuXingSprite()
        => Skill.Entry.GetWuXingSprite();

    public string GetName()
        => Skill.Entry.GetName();

    public SkillTypeComposite GetSkillTypeComposite()
        => Skill.Entry.GetSkillTypeComposite();

    public string GetExplanation()
        => Skill.Entry.GetExplanation();

    public string GetTrivia()
        => Skill.Entry.GetTrivia();

    public JingJie GetJingJie()
        => Skill.GetJingJie();

    public CostDescription GetCostDescription(JingJie showingJingJie)
        => GetJingJie() == showingJingJie
            ? Skill.Entry.GetCostDescription(showingJingJie, CostResult)
            : Skill.Entry.GetCostDescription(showingJingJie);

    public string GetHighlight(JingJie showingJingJie)
        => GetJingJie() == showingJingJie
            ? Skill.Entry.GetHighlight(showingJingJie, CostResult, CastResult)
            : Skill.Entry.GetHighlight(showingJingJie);

    public Sprite GetJingJieSprite(JingJie showingJingJie)
        => Skill.Entry.GetJingJieSprite(showingJingJie);

    public JingJie NextJingJie(JingJie showingJingJie)
        => Skill.Entry.NextJingJie(showingJingJie);

    public Color GetColor()
        => Skill.Entry.GetColor(GetJingJie());
}
