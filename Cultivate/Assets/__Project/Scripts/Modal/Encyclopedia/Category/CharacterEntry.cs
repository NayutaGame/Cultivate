
using System;

public class CharacterEntry : Entry
{
    public string GetName() => GetId();
    
    public string Description;
    public string AbilityDescription;

    public RunClosure[] _runClosures;
    public StageClosure[] _stageClosures;

    public CharacterEntry(string id, string description = null, string abilityDescription = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null) : base(id)
    {
        Description = description ?? "没有描述";
        AbilityDescription = abilityDescription ?? "没有技能描述";

        _runClosures = runClosures ?? Array.Empty<RunClosure>();
        _stageClosures = stageClosures ?? Array.Empty<StageClosure>();
    }

    public static implicit operator CharacterEntry(string id) => Encyclopedia.CharacterCategory[id];
}
