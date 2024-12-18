
using System;

public class DifficultyEntry : Entry
{
    public string GetName() => GetId();
    
    public string Description;

    public RunClosure[] _runClosures;
    public StageClosure[] _stageClosures;

    private string[] InheritedDifficultyNames;
    public DifficultyEntry[] InheritedDifficulties;

    public DifficultyEntry(string id, string description = null, string[] inheritedDifficultyNames = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null) : base(id)
    {
        Description = description ?? "没有描述";
        InheritedDifficultyNames = inheritedDifficultyNames ?? Array.Empty<string>();

        _runClosures = runClosures ?? Array.Empty<RunClosure>();
        _stageClosures = stageClosures ?? Array.Empty<StageClosure>();
    }

    public void CalcAdditionalDifficulties()
    {
        InheritedDifficulties = new DifficultyEntry[InheritedDifficultyNames.Length];
        for (int i = 0; i < InheritedDifficulties.Length; i++)
            InheritedDifficulties[i] = InheritedDifficultyNames[i];
    }

    public static implicit operator DifficultyEntry(string id) => Encyclopedia.DifficultyCategory[id];
}
