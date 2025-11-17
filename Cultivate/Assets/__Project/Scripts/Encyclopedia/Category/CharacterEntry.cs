
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterEntry : Entry, AnnotatableCharacter
{
    [NonSerialized] public string Description;
    [NonSerialized] private string _rawAbilityDescription;
    [NonSerialized] private Description _abilityDescription;
    
    [NonSerialized] public RunClosure[] RunClosures;
    [NonSerialized] public StageClosure[] StageClosures;
    [NonSerialized] public PackPreset PackPreset;
    [NonSerialized] public EntityEntry EntityEntry;

    [NonSerialized] private RoomEntry[] _roomsFromCharacter;
    [NonSerialized] private RoomEntry _roomFromVisitor;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public CharacterEntry(
        string id,
        string name,
        string description = null,
        string rawAbilityDescription = null,
        string unlockConditionDescription = null,
        PackPreset packPreset = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null) : base(id, name)
    {
        Description = description ?? "没有描述";
        _rawAbilityDescription = rawAbilityDescription ?? "没有技能描述";
        PackPreset = packPreset ?? PackPreset.Default;

        RunClosures = runClosures ?? Array.Empty<RunClosure>();
        StageClosures = stageClosures ?? Array.Empty<StageClosure>();

        EntityEntry = Encyclopedia.EntityCategory.FromName(GetName());
    }
    
    public override void Init()
    {
        _abilityDescription = new Description(_rawAbilityDescription);

        _roomsFromCharacter = new RoomEntry[]
        {
            Encyclopedia.RoomCategory.FromName($"Character{GetName()}1"),
        };
        
        _roomFromVisitor = Encyclopedia.RoomCategory.FromName($"Visitor{GetName()}");
    }

    public List<PackEntry> GetDefaultPacks()
    {
        return PackPreset.PackEntries;
    }

    public string GetTitle()
        => GetName();

    public Description GetAbilityDescription()
        => _abilityDescription;

    public RoomEntry[] RoomsFromCharacter => _roomsFromCharacter;
    public RoomEntry RoomFromVisitor => _roomFromVisitor;
    
    public Sprite GetCharacterIconSprite() => EntityEntry.CharacterIconSprite.Sprite;
    public Sprite GetCharacterIconSelectSprite() => EntityEntry.CharacterIconSelectSprite.Sprite;
    public PrefabEntry GetConfigPrefabEntry() => EntityEntry.ConfigModel;
    public PrefabEntry GetStagePrefabEntry() => EntityEntry.StageModel;
    public PrefabEntry GetScribblePrefabEntry() => EntityEntry.ScribbleModel;

    public bool CanShowAnnotation()
        => true;
}
