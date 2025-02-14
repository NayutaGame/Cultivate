
using System;
using System.Linq;
using CLLibrary;
using UnityEngine;

[Serializable]
public class PackEntry : Entry, IPack
{
    [NonSerialized] public string Name;
    [NonSerialized] public WuXing? WuXing;
    [NonSerialized] public SkillEntry[] Cards;
    [NonSerialized] public string Trivia;
    
    public PackEntry(string id,
        string name,
        WuXing? wuXing,
        string[] cardNames,
        string trivia = null
    ) : base(id)
    {
        Name = name;
        WuXing = wuXing;
        Cards = cardNames.Map(SkillEntry.FromName).ToArray();
        Trivia = trivia;
    }

    public string GetName()
        => Name;
}
