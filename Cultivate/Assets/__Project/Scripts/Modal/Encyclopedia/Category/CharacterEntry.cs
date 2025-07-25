
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterEntry : Entry, AnnotatableCharacter
{
    public string GetName() => GetId();
    
    [NonSerialized] public string Description;
    [NonSerialized] private string _rawAbilityDescription;
    [NonSerialized] private Description _abilityDescription;

    [NonSerialized] public RunClosure[] _runClosures;
    [NonSerialized] public StageClosure[] _stageClosures;

    [NonSerialized] private SpriteEntry _characterIconSprite;
    [NonSerialized] private SpriteEntry _characterIconSelectSprite;
    [NonSerialized] private PrefabEntry _configModel;
    [NonSerialized] private PrefabEntry _stageModel;

    [NonSerialized] public PackPreset _packPreset;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public CharacterEntry(string id, string description = null,
        string rawAbilityDescription = null,
        string unlockConditionDescription = null,
        PackPreset packPreset = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null) : base(id)
    {
        Description = description ?? "没有描述";
        _rawAbilityDescription = rawAbilityDescription ?? "没有技能描述";
        _packPreset = packPreset ?? PackPreset.Default;

        _runClosures = runClosures ?? Array.Empty<RunClosure>();
        _stageClosures = stageClosures ?? Array.Empty<StageClosure>();

        _characterIconSprite = $"CharacterIcon{GetName()}";
        _characterIconSelectSprite = $"CharacterIconSelect{GetName()}";
        _configModel = $"ConfigModel{GetName()}";
        _stageModel = $"StageModel{GetName()}";
    }

    public List<PackEntry> GetDefaultPacks()
    {
        return _packPreset.PackEntries;
    }

    public string GetTitle()
        => GetName();

    public Description GetAbilityDescription()
        => _abilityDescription;
    
    public void GenerateDescription()
        => _abilityDescription = new Description(_rawAbilityDescription);

    public static implicit operator CharacterEntry(string id) => Encyclopedia.CharacterCategory[id];

    // public static CharacterEntry FromName(string name)
    //     => Encyclopedia.CharacterCategory.Traversal.FirstObj(e => e.GetName() == name);

    public static CharacterEntry FromName(string name)
        => name;

    public PrefabEntry GetConfigPrefabEntry() => _configModel ?? Encyclopedia.PrefabCategory.MissingConfigModel();
    public PrefabEntry GetStagePrefabEntry() => _stageModel ?? Encyclopedia.PrefabCategory.MissingStageModel();
    public Sprite GetCharacterIconSprite() => _characterIconSprite.Sprite;
    public Sprite GetCharacterIconSelectSprite() => _characterIconSelectSprite.Sprite;

    public bool CanShowAnnotation()
        => true;
}
