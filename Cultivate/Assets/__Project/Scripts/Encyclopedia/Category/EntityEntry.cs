
using System;
using UnityEngine;

[Serializable]
public class EntityEntry : Entry
{
    [NonSerialized] private string _description;
    [NonSerialized] private SpriteEntry _spriteEntry;
    [NonSerialized] private PrefabEntry _stageModel;
    [NonSerialized] private PrefabEntry _runModel;

    public EntityEntry(
        string id,
        string name,
        string description,
        string modelName = null) : base(id, name)
    {
        _description = description;

        _spriteEntry = Encyclopedia.SpriteCategory.FromName(GetName());

        _stageModel = Encyclopedia.PrefabCategory.FromName($"StageModel{modelName ?? GetName()}");
        _runModel = Encyclopedia.PrefabCategory.FromName($"RunModel{modelName ?? GetName()}");
    }
    
    public string GetDescription() => _description;
    
    // public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingCharacterPortrait().Sprite;
    public PrefabEntry GetStageModelPrefabEntry() => _stageModel ?? Encyclopedia.PrefabCategory.MissingStageModel();
    public PrefabEntry GetRunModelPrefabEntry() => _runModel ?? Encyclopedia.PrefabCategory.MissingRunModel();
}
