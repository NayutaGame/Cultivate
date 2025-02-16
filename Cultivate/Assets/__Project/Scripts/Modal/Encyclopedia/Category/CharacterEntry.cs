
using System;
using System.Collections.Generic;

[Serializable]
public class CharacterEntry : Entry
{
    public string GetName() => GetId();
    
    [NonSerialized] public string Description;
    [NonSerialized] public string AbilityDescription;

    [NonSerialized] public RunClosure[] _runClosures;
    [NonSerialized] public StageClosure[] _stageClosures;

    [NonSerialized] private PrefabEntry _stageModel;

    [NonSerialized] public PackPreset _packPreset;

    public CharacterEntry(string id, string description = null,
        string abilityDescription = null,
        string unlockConditionDescription = null,
        PackPreset packPreset = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null) : base(id)
    {
        Description = description ?? "没有描述";
        AbilityDescription = abilityDescription ?? "没有技能描述";
        _packPreset = packPreset ?? PackPreset.Default;

        _runClosures = runClosures ?? Array.Empty<RunClosure>();
        _stageClosures = stageClosures ?? Array.Empty<StageClosure>();

        _stageModel = $"StageModel{GetName()}";
    }

    public List<PackEntry> GetDefaultPacks()
    {
        return _packPreset.PackEntries;
    }

    public static implicit operator CharacterEntry(string id) => Encyclopedia.CharacterCategory[id];

    // public static CharacterEntry FromName(string name)
    //     => Encyclopedia.CharacterCategory.Traversal.FirstObj(e => e.GetName() == name);

    public static CharacterEntry FromName(string name)
        => name;
    
    public PrefabEntry GetStagePrefabEntry() => _stageModel ?? Encyclopedia.PrefabCategory.MissingStageModel();
}
