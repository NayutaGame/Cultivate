using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class AchievementEntry : Entry
{
    [NonSerialized] private string _name;
    [NonSerialized] private string _conditionDescription;
    [NonSerialized] private string _rewardDescription;
    [NonSerialized] private RunClosure _runClosure;
    [NonSerialized] private StageClosure _stageClosure;
    [NonSerialized] private LockIndex? _lockIndex;

    public string GetName() => _name;
    public string GetConditionDescription() => _conditionDescription;
    public string GetRewardDescription() => _rewardDescription;
    public RunClosure GetRunClosure() => _runClosure;
    public StageClosure GetStageClosure() => _stageClosure;
    public LockIndex? GetLockIndex() => _lockIndex;

    public AchievementEntry(
        string id,
        string name,
        string conditionDescription,
        string rewardDescription,
        LockIndex? lockIndex = null,
        RunClosure runClosure = null,
        StageClosure stageClosure = null
        ) : base(id)
    {
        _name = name;
        _conditionDescription = conditionDescription;
        _rewardDescription = rewardDescription;
        _lockIndex = lockIndex;
        _runClosure = runClosure;
        _stageClosure = stageClosure;
    }

    public static implicit operator AchievementEntry(string id) 
        => Encyclopedia.AchievementCategory[id];
}