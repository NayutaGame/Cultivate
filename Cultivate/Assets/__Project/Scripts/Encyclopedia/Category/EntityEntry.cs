
using System;
using UnityEngine;

[Serializable]
public class EntityEntry : Entry
{
    [NonSerialized] private string _modelName;
    [NonSerialized] public SpriteEntry CharacterIconSprite;
    [NonSerialized] public SpriteEntry CharacterIconSelectSprite;
    [NonSerialized] public PrefabEntry ConfigModel;
    [NonSerialized] public PrefabEntry StageModel;
    [NonSerialized] public PrefabEntry RunModel;
    [NonSerialized] public PrefabEntry ScribbleModel;

    public EntityEntry(
        string id,
        string name,
        string modelName = null) : base(id, name)
    {
        _modelName = modelName ?? GetName();
        
        CharacterIconSprite = Encyclopedia.SpriteCategory.FromName($"CharacterIcon{_modelName}") ?? Encyclopedia.SpriteCategory.FromName($"CharacterIcon缺失");
        CharacterIconSelectSprite = Encyclopedia.SpriteCategory.FromName($"CharacterIconSelect{_modelName}") ?? Encyclopedia.SpriteCategory.FromName($"CharacterIconSelect缺失");;
        ConfigModel = Encyclopedia.PrefabCategory.FromName($"ConfigModel{_modelName}") ?? Encyclopedia.PrefabCategory.MissingConfigModel();
        StageModel = Encyclopedia.PrefabCategory.FromName($"StageModel{_modelName}") ?? Encyclopedia.PrefabCategory.MissingStageModel();
        RunModel = Encyclopedia.PrefabCategory.FromName($"RunModel{_modelName}") ?? Encyclopedia.PrefabCategory.MissingRunModel();
        ScribbleModel = Encyclopedia.PrefabCategory.FromName($"ScribbleModel{_modelName}") ?? Encyclopedia.PrefabCategory.MissingScribbleModel();
    }
}
