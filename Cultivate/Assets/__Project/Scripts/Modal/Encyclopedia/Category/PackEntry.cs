
using System;
using System.Linq;
using CLLibrary;
using UnityEngine;

[Serializable]
public class PackEntry : Entry, IPack
{
    [NonSerialized] public string Name;
    [NonSerialized] public WuXing? WuXing;
    [NonSerialized] public string Description;
    [NonSerialized] public string Trivia;
    [NonSerialized] public SkillEntry[] Cards;
    [NonSerialized] public SkillEntry[] StartCards;

    [NonSerialized] private SpriteEntry _spriteEntry;
    
    public PackEntry(string id,
        string name,
        WuXing? wuXing,
        string description = null,
        string trivia = null,
        string[] cardNames = null,
        string[] startCardNames = null
    ) : base(id)
    {
        Name = name;
        WuXing = wuXing;
        Description = description ?? "没有描述";
        Trivia = trivia ?? "没有趣闻";
        Cards = cardNames?.Map(SkillEntry.FromNameOrId).ToArray() ?? Array.Empty<SkillEntry>();
        StartCards = startCardNames?.Map(SkillEntry.FromNameOrId).ToArray() ?? Array.Empty<SkillEntry>();
        
        _spriteEntry = $"Pack{GetName()}";
    }
    
    public string GetName() => Name;
    public WuXing? GetWuXing() => WuXing;
    public string GetDescription() => Description;
    public string GetTrivia() => Trivia;
    
    public Sprite GetSprite() => _spriteEntry?.Sprite;
    public bool Equipped() => false;
    public bool IsUnlocked() => false;
    public string GetUnlockCondition() => null;

    public static implicit operator PackEntry(string id) => Encyclopedia.PackCategory[id];

    public static PackEntry FromName(string name)
        => Encyclopedia.PackCategory.FirstObj(e => e.GetName() == name);
}
