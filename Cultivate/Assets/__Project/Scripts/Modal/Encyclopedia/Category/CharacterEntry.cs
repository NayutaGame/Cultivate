
using System;
using UnityEngine;

public class CharacterEntry : Entry
{
    public string GetName() => GetId();
    
    public string Description;
    public string AbilityDescription;

    public RunClosure[] _runClosures;
    public StageClosure[] _stageClosures;

    private PrefabEntry _stageModel;

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
