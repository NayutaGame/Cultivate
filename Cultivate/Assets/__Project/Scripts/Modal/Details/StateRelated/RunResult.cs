
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
    
    private static readonly RunMilestoneEvaluator[] Evaluators = new[]
    {
        new RunMilestoneEvaluator(
            evalDescription: env => "通关游戏",
            evalExperienceGain: env => 
            {
                if (env.GetResult().GetOutcome() != RunOutcome.Victorious)
                    return 0;
                    
                float baseExp = 500;
                float multiplier = env.GetRunConfig().DifficultyProfile.GetEntry().GetExperienceMultiplier();
                return Mathf.RoundToInt(baseExp * multiplier);
            }
        ),
        
        // new RunMilestoneEvaluator(
        //     evalDescription: env => $"击杀 {env.GetKillCount()} 个敌人",
        //     evalExperienceGain: env => env.GetKillCount() * 10
        // ),
        new RunMilestoneEvaluator(
            evalDescription: env => $"通关时间小于45分钟",
            evalExperienceGain: env => env.GetResult().GetOutcome() == RunOutcome.Victorious
                && env.GetRunfinishedTime().TotalMinutes < 45 ? 100 : 0
        ),
    };

    [SerializeField] private RunOutcome _outcome;
    public RunOutcome GetOutcome() => _outcome;
    public void SetOutcome(RunOutcome state) => _outcome = state;

    public RunResult()
    {
        _outcome = RunOutcome.InProgress;
    }

    public RunMilestone[] GetCompletedMilestones(RunEnvironment env)
    {
        return Evaluators.Map(evaluator => evaluator.Eval(env))
            .FilterObj(milestone => milestone.ExperienceGain > 0)
            .ToArray();
    }
}

public class RunMilestoneEvaluator
{
    private readonly Func<RunEnvironment, string> _evalDescription;
    private readonly Func<RunEnvironment, int> _evalExperienceGain;
    
    public RunMilestoneEvaluator(
        Func<RunEnvironment, string> evalDescription,
        Func<RunEnvironment, int> evalExperienceGain)
    {
        _evalDescription = evalDescription;
        _evalExperienceGain = evalExperienceGain;
    }

    public RunMilestone Eval(RunEnvironment env) => new(
        description: _evalDescription(env),
        experienceGain: _evalExperienceGain(env)
    );
}

public class RunMilestone
{
    public string Description { get; }
    public int ExperienceGain { get; }

    public RunMilestone(string description, int experienceGain)
    {
        Description = description;
        ExperienceGain = experienceGain;
    }
}
