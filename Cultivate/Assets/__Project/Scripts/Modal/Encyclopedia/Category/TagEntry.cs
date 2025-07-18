
using UnityEngine;

public class TagEntry : Entry
{
    private int _index;
    public int Index => _index;
    
    private long _value;
    public long Value => _value;

    private SpriteEntry _spriteEntry;
    public Sprite GetIcon()
        => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingSkillIllustration().Sprite;
    
    public TagEntry(string id, int index, int value) : base(id)
    {
        _index = index;
        _value = value;
        _spriteEntry = ((SpriteEntry)($"Tag{GetName()}")) ?? Encyclopedia.SpriteCategory.MissingSkillIllustration();
    }
    
    public string GetName() => GetId();
    
    public static implicit operator long(TagEntry tagEntry) => tagEntry._value;
}