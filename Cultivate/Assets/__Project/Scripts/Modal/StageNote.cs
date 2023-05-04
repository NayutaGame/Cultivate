
using UnityEngine;

public class StageNote : ICardModel
{
    public int EntityIndex;
    public int TemporalIndex;
    public StageWaiGong WaiGong;

    public StageNote(int entityIndex, int temporalIndex, StageWaiGong waiGong)
    {
        EntityIndex = entityIndex;
        TemporalIndex = temporalIndex;
        WaiGong = waiGong;
    }

    public bool IsHome
        => EntityIndex == 0;

    public bool GetReveal()
        => true;

    public int GetManaCost()
        => WaiGong?.GetManaCost() ?? 0;

    public Color GetManaCostColor()
        => Color.white;

    public string GetName()
        => WaiGong?.GetName() ?? (Encyclopedia.ChipCategory["聚气术"] as WaiGongEntry).Name;

    public string GetAnnotatedDescription(string evaluated = null)
        => WaiGong?.GetAnnotatedDescription() ?? (Encyclopedia.ChipCategory["聚气术"] as WaiGongEntry).Evaluate(0, 0);

    public SkillTypeCollection GetSkillTypeCollection()
        => WaiGong?.GetSkillTypeCollection() ?? SkillTypeCollection.None;

    public Color GetColor()
        // => WaiGong?.GetColor() ?? CanvasManager.Instance.JingJieColors[JingJie.LianQi];
        => CanvasManager.Instance.JingJieColors[WaiGong?.GetJingJie() ?? JingJie.LianQi];

    public Sprite GetCardFace()
        => WaiGong?.Entry.CardFace;

    public string GetAnnotationText()
        => WaiGong?.GetAnnotationText();
}
