
using System;

public class DifficultyEntry : Entry
{
    public string GetName() => GetId();
    
    public string Description;

    public RunClosure[] _runClosures;
    public StageClosure[] _stageClosures;

    private string[] AdditionalDifficultyNames;
    public DifficultyEntry[] AdditionalDifficulties;

    public DifficultyEntry(string id, string description = null, string[] _additionalDifficultyNames = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null) : base(id)
    {
        Description = description ?? "没有描述";
        AdditionalDifficultyNames = _additionalDifficultyNames ?? Array.Empty<string>();

        _runClosures = runClosures ?? Array.Empty<RunClosure>();
        _stageClosures = stageClosures ?? Array.Empty<StageClosure>();
    }

    public void CalcAdditionalDifficulties()
    {
        AdditionalDifficulties = new DifficultyEntry[AdditionalDifficultyNames.Length];
        for (int i = 0; i < AdditionalDifficulties.Length; i++)
            AdditionalDifficulties[i] = AdditionalDifficultyNames[i];
    }

    public static implicit operator DifficultyEntry(string id) => Encyclopedia.DifficultyCategory[id];
}
