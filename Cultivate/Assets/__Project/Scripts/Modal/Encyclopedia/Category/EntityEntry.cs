
using System;
using UnityEngine;

[Serializable]
public class EntityEntry : Entry
{
    public string GetName() => GetId();
    
    private string _description;
    public string Description => _description;

    private SpriteEntry _spriteEntry;

    private PrefabEntry _stageModel;
    private PrefabEntry _uiEntityModel;

    public EntityEntry(string id, string description, string modelName = null) : base(id)
    {
        _description = description;

        _spriteEntry = GetName();

        _stageModel = $"StageModel{modelName ?? GetName()}";
        _uiEntityModel = $"UIEntityModel{modelName ?? GetName()}";
    }

    public static implicit operator EntityEntry(string id) => Encyclopedia.EntityCategory[id];
    
    // public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingCharacterPortrait().Sprite;
    public PrefabEntry GetStageModelPrefabEntry() => _stageModel ?? Encyclopedia.PrefabCategory.MissingStageModel();
    public PrefabEntry GetUIEntityModelPrefabEntry() => _uiEntityModel ?? Encyclopedia.PrefabCategory.MissingUIEntityModel();
}
