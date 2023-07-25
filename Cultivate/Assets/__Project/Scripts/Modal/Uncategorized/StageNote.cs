
using UnityEngine;

public class StageNote : ISkillModel
{
    public int EntityIndex;
    public int TemporalIndex;
    public StageSkill Skill;

    public StageNote(int entityIndex, int temporalIndex, StageSkill skill)
    {
        EntityIndex = entityIndex;
        TemporalIndex = temporalIndex;
        Skill = skill;
    }

    public bool IsHome
        => EntityIndex == 0;

    public Sprite GetSprite()
        => Skill?.Entry.Sprite;

    public int GetManaCost()
        => Skill?.GetManaCost() ?? 0;

    public string GetName()
        => Skill?.GetName() ?? Encyclopedia.SkillCategory["聚气术"].Name;

    public string GetAnnotatedDescription(string evaluated = null)
        => Skill?.GetAnnotatedDescription() ?? Encyclopedia.SkillCategory["聚气术"].Evaluate(0, 0);

    public SkillTypeCollection GetSkillTypeCollection()
        => Skill?.GetSkillTypeCollection() ?? SkillTypeCollection.None;

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
}
