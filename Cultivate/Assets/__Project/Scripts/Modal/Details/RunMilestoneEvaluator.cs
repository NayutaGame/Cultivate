
using System;
using UnityEngine;

public class RunMilestoneEvaluator
{
    public static readonly RunMilestoneEvaluator[] Evaluators = new[]
    {
        new RunMilestoneEvaluator(
            evalDescription: env => "通关游戏",
            evalExperienceGain: env => 
            {
                if (env.GetResult().GetOutcome() != RunResult.RunOutcome.Victorious)
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
            evalExperienceGain: env => env.GetResult().GetOutcome() == RunResult.RunOutcome.Victorious
                                       && env.GetRunfinishedTime().TotalMinutes < 45 ? 100 : 0
        ),
    };

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
