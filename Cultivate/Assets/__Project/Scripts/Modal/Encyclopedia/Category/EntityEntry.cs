
using System;
using UnityEngine;

[Serializable]
public class EntityEntry : Entry
{
    public string GetName() => GetId();
    
    [NonSerialized] private string _description;
    [NonSerialized] private SpriteEntry _spriteEntry;
    [NonSerialized] private PrefabEntry _stageModel;
    [NonSerialized] private PrefabEntry _runModel;

    public EntityEntry(string id, string description, string modelName = null) : base(id)
    {
        _description = description;

        _spriteEntry = GetName();

        _stageModel = $"StageModel{modelName ?? GetName()}";
        _runModel = $"RunModel{modelName ?? GetName()}";
    }
    
    public string GetDescription() => _description;

    public static implicit operator EntityEntry(string id) => Encyclopedia.EntityCategory[id];
    
    // public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingCharacterPortrait().Sprite;
    public PrefabEntry GetStageModelPrefabEntry() => _stageModel ?? Encyclopedia.PrefabCategory.MissingStageModel();
    public PrefabEntry GetRunModelPrefabEntry() => _runModel ?? Encyclopedia.PrefabCategory.MissingRunModel();
}
