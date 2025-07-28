
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class TagEntry : Entry, AnnotatableTag
{
    private int _index;
    public int Index => _index;
    
    private long _value;
    public long Value => _value;

    private string _rawDescription;
    private Description _description;

    private SpriteEntry _spriteEntry;
    public Sprite GetIcon()
        => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingSkillIllustration().Sprite;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public TagEntry(string id, int index, int value, string rawDescription) : base(id)
    {
        _index = index;
        _value = value;
        _spriteEntry = ((SpriteEntry)($"Tag{GetName()}")) ?? Encyclopedia.SpriteCategory.MissingSkillIllustration();
        _rawDescription = rawDescription;
    }
    
    public string GetName() => GetId();
    public Description GetDescription() => _description;
    
    public void GenerateDescription()
        => _description = new Description(_rawDescription);

    public static implicit operator long(TagEntry tagEntry) => tagEntry._value;

    public bool CanShowAnnotation()
        => true;
    
    public static TagEntry FromName(string name)
        => Encyclopedia.TagCategory.FirstObj(e => e.GetName() == name) ?? Encyclopedia.TagCategory.DefaultEntry();
}