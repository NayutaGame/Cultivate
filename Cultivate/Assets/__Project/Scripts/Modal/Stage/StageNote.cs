
using System.Collections.Generic;
using UnityEngine;

public class StageNote : ISkillModel
{
    public int EntityIndex;
    public int TemporalIndex;
    public StageSkill Skill;

    private Dictionary<string, string> _indicator;

    public StageNote(int entityIndex, int temporalIndex, StageSkill skill, Dictionary<string, string> indicator = null, ChannelDetails channelDetails = null)
    {
        EntityIndex = entityIndex;
        TemporalIndex = temporalIndex;
        Skill = skill;
        _indicator = indicator;

        if (channelDetails != null)
        {
            _currCounter = channelDetails.GetCounter();
            _maxCounter = channelDetails.GetChannelTime();
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
        => Skill.Entry.Sprite;

    public int GetManaCost()
        => Skill.GetManaCost();

    public string GetName()
        => Skill.GetName();

    public string GetHighlight()
        => Skill.GetHighlight(_indicator);

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
