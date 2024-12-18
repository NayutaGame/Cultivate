
using System;
using UnityEngine;

[Serializable]
public class CharacterEntry : Entry
{
    public string GetName() => GetId();
    
    [NonSerialized] public string Description;
    [NonSerialized] public string AbilityDescription;

    [NonSerialized] public RunClosure[] _runClosures;
    [NonSerialized] public StageClosure[] _stageClosures;

    [NonSerialized] private PrefabEntry _stageModel;

    public CharacterEntry(string id, string description = null, string abilityDescription = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null) : base(id)
    {
        Description = description ?? "没有描述";
        AbilityDescription = abilityDescription ?? "没有技能描述";

        _runClosures = runClosures ?? Array.Empty<RunClosure>();
        _stageClosures = stageClosures ?? Array.Empty<StageClosure>();

        _stageModel = $"StageModel{GetName()}";
    }

    public static implicit operator CharacterEntry(string id) => Encyclopedia.CharacterCategory[id];
    
    public PrefabEntry GetStagePrefabEntry() => _stageModel ?? Encyclopedia.PrefabCategory.MissingStageModel();
}
