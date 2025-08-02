
using System;
using System.Text;
using UnityEngine;

[Serializable]
public class DifficultyEntry : Entry
{
    [NonSerialized] public int _order;
    [NonSerialized] public string Description;
    [NonSerialized] public string InheritedDescription;
    [NonSerialized] private string[] InheritedDifficultyNames;
    [NonSerialized] public DifficultyEntry[] InheritedDifficulties;
    [NonSerialized] public JingJie FinalJingJie;
    [NonSerialized] public bool HomeAllowFormation;
    [NonSerialized] public bool AwayAllowFormation;
    [NonSerialized] public bool AllowRotate;
    [NonSerialized] public RunClosure[] _runClosures;
    [NonSerialized] public StageClosure[] _stageClosures;

    public DifficultyEntry(
        string id,
        string name,
        int order,
        string description = null,
        string[] inheritedDifficultyNames = null,
        JingJie finalJingJie = null,
        bool homeAllowFormation = false,
        bool awayAllowFormation = false,
        bool allowRotate = false,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null) : base(id, name)
    {
        _order = order;
        
        Description = description ?? "没有描述";
        InheritedDifficultyNames = inheritedDifficultyNames ?? Array.Empty<string>();
        FinalJingJie = finalJingJie ?? JingJie.HuaShen;
        HomeAllowFormation = homeAllowFormation;
        AwayAllowFormation = awayAllowFormation;
        AllowRotate = allowRotate;

        _runClosures = runClosures ?? Array.Empty<RunClosure>();
        _stageClosures = stageClosures ?? Array.Empty<StageClosure>();
    }

    public override void Init()
    {
        CalcAdditionalDifficulties();
    }

    public void CalcAdditionalDifficulties()
    {
        InheritedDifficulties = new DifficultyEntry[InheritedDifficultyNames.Length];
        for (int i = 0; i < InheritedDifficulties.Length; i++)
            InheritedDifficulties[i] = Encyclopedia.DifficultyCategory.FromName(InheritedDifficultyNames[i]);

        StringBuilder sb = new();

        sb.Append($"难度{GetId()}\n\n");
        sb.Append($"{GetId()}. {Description}\n");
        for (int i = 0; i < InheritedDifficulties.Length; i++)
        {
            sb.Append($"{InheritedDifficulties[i].GetId()}. {InheritedDifficulties[i].Description}\n");
        }
        
        InheritedDescription = sb.ToString();
    }

    public float GetExperienceMultiplier()
    {
        return 1 + _order * 0.2f;
    }
}
