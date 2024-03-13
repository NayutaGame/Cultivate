
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
        Skill = skill;

        _currCounter = currCounter;
        _maxCounter = maxCounter;
    }

    public bool IsHome
        => EntityIndex == 0;

    public Sprite GetSprite()
        => Skill.Entry.Sprite;

    public CostDescription GetCostDescription()
        => Skill.Entry.GetCostDescription(Skill.GetJingJie(), CostResult);

    public string GetName()
        => Skill.GetName();

    public string GetHighlight()
        => Skill.GetHighlight(CastResult);

    public string GetExplanation()
        => Skill.GetExplanation();

    public string GetTrivia()
        => Skill.GetTrivia();

    public SkillTypeComposite GetSkillTypeComposite()
        => Skill.GetSkillTypeCollection();

    public Color GetColor()
        => CanvasManager.Instance.JingJieColors[Skill.GetJingJie()];

    public Sprite GetCardFace()
        => Skill.Entry.CardFace;

    public Sprite GetJingJieSprite()
        => CanvasManager.Instance.JingJieSprites[Skill.GetJingJie()];

    public Sprite GetWuXingSprite()
        => CanvasManager.Instance.GetWuXingSprite(Skill.Entry.WuXing);

    private int _currCounter;
    public int GetCurrCounter() => _currCounter;

    private int _maxCounter;
    public int GetMaxCounter() => _maxCounter;
}
