
using UnityEngine;

public class StageNote : ISkillModel
{
    public int EntityIndex;
    public int TemporalIndex;
    public StageSkill Skill;

    public StageNote(int entityIndex, int temporalIndex, StageSkill skill, ChannelDetails d = null)
    {
        EntityIndex = entityIndex;
        TemporalIndex = temporalIndex;
        Skill = skill;

        if (d != null)
        {
            _currCounter = d.GetCounter();
            _maxCounter = d.GetChannelTime();
        }
        else
        {
            _currCounter = 0;
            _maxCounter = 0;
        }
    }

    public bool IsHome
        => EntityIndex == 0;

    public Sprite GetSprite()
        // => Skill?.Entry.Sprite ?? Encyclopedia.SkillCategory["聚气术"].Sprite;
        => Skill?.Entry.Sprite;

    public int GetManaCost()
        => Skill?.GetManaCost() ?? 0;

    public string GetName()
        => Skill?.GetName() ?? Encyclopedia.SkillCategory["聚气术"].GetName();

    public string GetAnnotatedDescription(string evaluated = null)
        => Skill?.GetAnnotatedDescription() ?? Encyclopedia.SkillCategory["聚气术"].DescriptionFromLowestJingJie();

    public SkillTypeComposite GetSkillTypeComposite()
        => Skill?.GetSkillTypeCollection() ?? 0;

    public Color GetColor()
        // => Skill?.GetColor() ?? CanvasManager.Instance.JingJieColors[JingJie.LianQi];
        => CanvasManager.Instance.JingJieColors[Skill?.GetJingJie() ?? JingJie.LianQi];

    public Sprite GetCardFace()
        => Skill?.Entry.CardFace;

    public Sprite GetJingJieSprite()
        => CanvasManager.Instance.JingJieSprites[Skill?.GetJingJie() ?? JingJie.LianQi];

    public Sprite GetWuXingSprite()
        => CanvasManager.Instance.GetWuXingSprite(Skill?.Entry.WuXing);

    public string GetAnnotationText()
        => Skill?.GetAnnotationText();

    public string GetTrivia()
        => Skill?.GetTrivia();

    private int _currCounter;
    public int GetCurrCounter() => _currCounter;

    private int _maxCounter;
    public int GetMaxCounter() => _maxCounter;
}
