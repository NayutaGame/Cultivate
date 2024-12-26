
using System;

[Serializable]
public class DifficultyEntry : Entry
{
    public string GetName() => GetId();

    [NonSerialized] public int _order;
    
    [NonSerialized] public string Description;
    [NonSerialized] public RunClosure[] _runClosures;
    [NonSerialized] public StageClosure[] _stageClosures;
    [NonSerialized] private string[] InheritedDifficultyNames;
    [NonSerialized] public DifficultyEntry[] InheritedDifficulties;

    public DifficultyEntry(string id, int order,
        string description = null, string[] inheritedDifficultyNames = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null) : base(id)
    {
        _order = order;
        
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
    public static implicit operator DifficultyEntry(int index) => Encyclopedia.DifficultyCategory[index];
}
