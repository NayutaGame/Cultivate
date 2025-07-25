
using System;
using System.Linq;
using CLLibrary;
using UnityEngine;

[Serializable]
public class PackEntry : Entry
{
    [NonSerialized] public string Name;
    [NonSerialized] public WuXing? WuXing;
    [NonSerialized] public string _rawDescription;
    [NonSerialized] public Description _description;
    [NonSerialized] public string Trivia;
    [NonSerialized] public SkillEntry[] Cards;
    [NonSerialized] public SkillEntry[] StartCards;

    [NonSerialized] private SpriteEntry _spriteEntry;
    
    public PackEntry(string id,
        string name,
        WuXing? wuXing,
        string rawDescription = null,
        string trivia = null,
        string[] cardNames = null,
        string[] startCardNames = null
    ) : base(id)
    {
        Name = name;
        WuXing = wuXing;
        _rawDescription = rawDescription ?? "没有描述";
        Trivia = trivia ?? "没有趣闻";
        Cards = cardNames?.Map(SkillEntry.FromNameOrId).ToArray() ?? Array.Empty<SkillEntry>();
        StartCards = startCardNames?.Map(SkillEntry.FromNameOrId).ToArray() ?? Array.Empty<SkillEntry>();
        
        _spriteEntry = $"Pack{GetName()}";
    }
    
    public void GenerateDescription()
        => _description = new Description(_rawDescription);
    
    public string GetName() => Name;
    public WuXing? GetWuXing() => WuXing;
    public Description GetDescription() => _description;
    public string GetTrivia() => Trivia;
    
    public Sprite GetSprite() => _spriteEntry?.Sprite;
    public bool Equipped() => false;
    public bool IsUnlocked() => false;
    public string GetUnlockCondition() => null;

    public static implicit operator PackEntry(string id) => Encyclopedia.PackCategory[id];

    public static PackEntry FromName(string name)
        => Encyclopedia.PackCategory.FirstObj(e => e.GetName() == name);
}
