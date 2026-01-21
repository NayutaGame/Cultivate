
using System;
using UnityEngine;

[Serializable]
public class EntityEntry : Entry
{
    [NonSerialized] private string _description;
    [NonSerialized] private PrefabEntry _stageModel;
    
    [NonSerialized] private SpriteEntry _runIcon;

    public EntityEntry(
        string id,
        string name,
        string description,
        string modelName = null) : base(id, name)
    {
        _description = description;
        
        _runIcon = Encyclopedia.SpriteCategory.FromName($"RunIcon{modelName ?? GetName()}");

        _stageModel = Encyclopedia.PrefabCategory.FromName($"StageModel{modelName ?? GetName()}");
    }
    
    public string GetDescription() => _description;
    
    public PrefabEntry GetStageModelPrefabEntry() => _stageModel ?? Encyclopedia.PrefabCategory.MissingStageModel();

    public Sprite GetRunIcon() => _runIcon?.Sprite ?? Encyclopedia.SpriteCategory.MissingRunIcon().Sprite;
}
