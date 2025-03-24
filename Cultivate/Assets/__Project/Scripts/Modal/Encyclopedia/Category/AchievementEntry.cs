using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class AchievementEntry : Entry
{
    [NonSerialized] private string _name;
    [NonSerialized] private string _conditionDescription;
    // 解锁条件图标
    [NonSerialized] private string _rewardDescription;
    // 奖励图标
    [NonSerialized] private int _experienceGain;
    [NonSerialized] private RunClosure[] _runClosures;
    [NonSerialized] private StageClosure[] _stageClosures;
    [NonSerialized] private LockIndex? _lockIndex;

    public string GetName() => _name;
    public string GetConditionDescription() => _conditionDescription;
    public string GetRewardDescription() => _rewardDescription;
    public int GetExperienceGain() => _experienceGain;
    public RunClosure[] GetRunClosures() => _runClosures;
    public StageClosure[] GetStageClosures() => _stageClosures;
    public LockIndex? GetLockIndex() => _lockIndex;

    public AchievementEntry(
        string id,
        string name,
        string conditionDescription,
        string rewardDescription,
        int experienceGain = 100,
        LockIndex? lockIndex = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null
        ) : base(id)
    {
        _name = name;
        _conditionDescription = conditionDescription;
        _rewardDescription = rewardDescription;
        _experienceGain = experienceGain;
        _lockIndex = lockIndex;
        _runClosures = runClosures ?? Array.Empty<RunClosure>();
        _stageClosures = stageClosures ?? Array.Empty<StageClosure>();
    }

    public static implicit operator AchievementEntry(string id) 
        => Encyclopedia.AchievementCategory[id];
}
