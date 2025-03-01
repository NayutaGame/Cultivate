
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
    
    public PackEntry(string id,
        string name,
        WuXing? wuXing,
        string description = null,
        string trivia = null,
        string[] cardNames = null
    ) : base(id)
    {
        Name = name;
        WuXing = wuXing;
        Description = description ?? "没有描述";
        Trivia = trivia ?? "没有趣闻";
        Cards = cardNames?.Map(SkillEntry.FromNameOrId).ToArray() ?? Array.Empty<SkillEntry>();
    }

    public string GetName() => Name;
    public WuXing? GetWuXing() => WuXing;
    public string GetDescription() => Description;
    public string GetTrivia() => Trivia;

    public static implicit operator PackEntry(string id) => Encyclopedia.PackCategory[id];

    public static PackEntry FromName(string name)
        => Encyclopedia.PackCategory.Traversal.FirstObj(e => e.GetName() == name);
}
