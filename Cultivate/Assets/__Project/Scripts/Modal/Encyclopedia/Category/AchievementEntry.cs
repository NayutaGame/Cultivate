using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class AchievementEntry : Entry
{
    private string _name;
    private string _description;
    private string _conditionDescription;
    private RunClosure _runClosure;
    private StageClosure _stageClosure;

    public RunClosure GetRunClosure() => _runClosure;
    public StageClosure GetStageClosure() => _stageClosure;

    public AchievementEntry(
        string id,
        string name,
        string description,
        string conditionDescription,
        RunClosure runClosure = null,
        StageClosure stageClosure = null
        ) : base(id)
    {
        _name = name;
        _description = description;
        _conditionDescription = conditionDescription;
        _runClosure = runClosure;
        _stageClosure = stageClosure;
    }

    public string GetName() => _name;
    public string GetDescription() => _description;
    public string GetConditionDescription() => _conditionDescription;

    public static implicit operator AchievementEntry(string id) 
        => Encyclopedia.AchievementCategory[id];
}