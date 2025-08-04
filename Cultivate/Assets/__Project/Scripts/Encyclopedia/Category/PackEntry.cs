
using System;
using System.Linq;
using CLLibrary;
using UnityEngine;

[Serializable]
public class PackEntry : Entry
{
    [NonSerialized] public WuXing WuXing;
    [NonSerialized] public string _rawDescription;
    [NonSerialized] public Description _description;
    [NonSerialized] public string Trivia;
    [NonSerialized] public SkillEntry[] Cards;
    [NonSerialized] public SkillEntry[] StartCards;
    [NonSerialized] private SpriteEntry _spriteEntry;
    
    public PackEntry(
        string id,
        string name,
        WuXing wuXing,
        string rawDescription = null,
        string trivia = null,
        string[] cardNames = null,
        string[] startCardNames = null
    ) : base(id, name)
    {
        WuXing = wuXing;
        _rawDescription = rawDescription ?? "没有描述";
        Trivia = trivia ?? "没有趣闻";
        Cards = ConvertToSkillEntryList(cardNames);
        StartCards = ConvertToSkillEntryList(startCardNames);
        
        _spriteEntry = Encyclopedia.SpriteCategory.FromName($"Pack{GetName()}");
    }

    private static SkillEntry[] ConvertToSkillEntryList(string[] skillNames)
    {
        if (skillNames == null)
            return Array.Empty<SkillEntry>();
        
        return skillNames.Map(skillName =>
        {
            SkillEntry skillEntry = Encyclopedia.SkillCategory.FromName(skillName);
            if (skillEntry == null)
            {
                Debug.Log($"没有找到{skillName}，换成了默认卡牌");
                return Encyclopedia.SkillCategory.Default();
            }

            return skillEntry;
        }).ToArray();
    }
    
    public override void Init()
        => _description = new Description(_rawDescription);
    
    public WuXing GetWuXing() => WuXing;
    public Description GetDescription() => _description;
    public string GetTrivia() => Trivia;
    
    public Sprite GetSprite() => _spriteEntry?.Sprite;
    public bool Equipped() => false;
    public bool IsUnlocked() => false;
    public string GetUnlockCondition() => null;
}
