
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
        => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingSkillCardIllustration().Sprite;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public TagEntry(
        string id,
        string name,
        int index,
        int value,
        string rawDescription) : base(id, name)
    {
        _index = index;
        _value = value;
        _spriteEntry = Encyclopedia.SpriteCategory.FromName($"Tag{GetName()}") ??
                       Encyclopedia.SpriteCategory.MissingSkillCardIllustration();
        _rawDescription = rawDescription;
    }
    
    public Description GetDescription() => _description;
    
    public override void Init()
        => _description = new Description(_rawDescription);

    public static TagEntry FromIndex(int index)
        => Encyclopedia.TagCategory.FirstObj(tag => tag._index == index);

    public bool CanShowAnnotation()
        => true;
    
    public static TagComposite operator |(TagEntry left, TagEntry right) => new TagComposite(left.Value | right.Value);
}