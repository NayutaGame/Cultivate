
using System;
using UnityEngine;

[Serializable]
public class EntityEntry : Entry
{
    public string GetName() => GetId();
    
    private string _description;
    public string Description => _description;

    private SpriteEntry _spriteEntry;

    public EntityEntry(string id, string description, string model = null) : base(id)
    {
        _description = description;

        _spriteEntry = model ?? GetName();
    }

    public static implicit operator EntityEntry(string id) => Encyclopedia.EntityCategory[id];
    public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingCharacterTachie().Sprite;
}
