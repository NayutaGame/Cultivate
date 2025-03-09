
using System;
using System.Linq;
using CLLibrary;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class RunResult
{
    public enum RunOutcome
    {
        InProgress,
        Defeated,
        Victorious,
    }
    
    [SerializeField] private RunOutcome _outcome;
    public RunOutcome GetOutcome() => _outcome;
    public void SetOutcome(RunOutcome state) => _outcome = state;

    public RunResult()
    {
        _outcome = RunOutcome.InProgress;
    }

    public RunMilestone[] GetCompletedMilestones(RunEnvironment env)
    {
        return RunMilestoneEvaluator.Evaluators.Map(evaluator => evaluator.Eval(env))
            .FilterObj(milestone => milestone.ExperienceGain > 0)
            .ToArray();
    }
}
