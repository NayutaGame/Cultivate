
using System;

public class DifficultyEntry : Entry
{
    public string GetName() => GetId();
    
    public string Description;

    public RunEventDescriptor[] _runEventDescriptors;
    public StageEventDescriptor[] _stageEventDescriptors;

    private string[] AdditionalDifficultyNames;
    public DifficultyEntry[] AdditionalDifficulties;

    public DifficultyEntry(string id, string description = null, string[] _additionalDifficultyNames = null,
        RunEventDescriptor[] runEventDescriptors = null,
        StageEventDescriptor[] stageEventDescriptors = null) : base(id)
    {
        Description = description ?? "没有描述";
        AdditionalDifficultyNames = _additionalDifficultyNames ?? Array.Empty<string>();

        _runEventDescriptors = runEventDescriptors ?? Array.Empty<RunEventDescriptor>();
        _stageEventDescriptors = stageEventDescriptors ?? Array.Empty<StageEventDescriptor>();
    }

    public void CalcAdditionalDifficulties()
    {
        AdditionalDifficulties = new DifficultyEntry[AdditionalDifficultyNames.Length];
        for (int i = 0; i < AdditionalDifficulties.Length; i++)
            AdditionalDifficulties[i] = AdditionalDifficultyNames[i];
    }

    public static implicit operator DifficultyEntry(string id) => Encyclopedia.DifficultyCategory[id];
}
