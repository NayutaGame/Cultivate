
using System;
using System.Text;
using UnityEngine;

[Serializable]
public class DifficultyEntry : Entry
{
    public string GetName() => GetId();

    [NonSerialized] public int _order;
    
    [NonSerialized] public string Description;
    [NonSerialized] public string InheritedDescription;
    [NonSerialized] private string[] InheritedDifficultyNames;
    [NonSerialized] public DifficultyEntry[] InheritedDifficulties;
    [NonSerialized] public JingJie FinalJingJie;
    [NonSerialized] public RunClosure[] _runClosures;
    [NonSerialized] public StageClosure[] _stageClosures;

    public DifficultyEntry(string id, int order,
        string description = null, string[] inheritedDifficultyNames = null,
        JingJie? finalJingJie = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null) : base(id)
    {
        _order = order;
        
        Description = description ?? "没有描述";
        InheritedDifficultyNames = inheritedDifficultyNames ?? Array.Empty<string>();
        FinalJingJie = finalJingJie ?? JingJie.HuaShen;

        _runClosures = runClosures ?? Array.Empty<RunClosure>();
        _stageClosures = stageClosures ?? Array.Empty<StageClosure>();
    }

    public void CalcAdditionalDifficulties()
    {
        InheritedDifficulties = new DifficultyEntry[InheritedDifficultyNames.Length];
        for (int i = 0; i < InheritedDifficulties.Length; i++)
            InheritedDifficulties[i] = InheritedDifficultyNames[i];

        StringBuilder sb = new();

        sb.Append($"难度{GetId()}\n\n");
        sb.Append($"{GetId()}. {Description}\n");
        for (int i = 0; i < InheritedDifficulties.Length; i++)
        {
            sb.Append($"{InheritedDifficulties[i].GetId()}. {InheritedDifficulties[i].Description}\n");
        }
        
        InheritedDescription = sb.ToString();
    }

    public static implicit operator DifficultyEntry(string id) => Encyclopedia.DifficultyCategory[id];
    public static implicit operator DifficultyEntry(int index) => Encyclopedia.DifficultyCategory[index];
}
