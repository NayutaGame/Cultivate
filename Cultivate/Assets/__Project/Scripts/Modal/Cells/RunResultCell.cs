
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class RunResultCell : Cell
{
    private RunResult _result;                     // 结果状态
    private RunConfig _config;                     // 角色配置
    private TimeSpan _playTime;                    // 游戏时长
    private Profile _profile;                      // 玩家存档

    private ListModel<RunMilestone> _milestones;
    private ListModel<AchievementProfile> _newUnlocks;

    private int _initialExperience;
    private int _finalExperience;
    private int _experienceGain;
    private int _initialLevel;
    private int _finalLevel;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((RunResultCell)thisObject).GetGuideDescriptor() },
        { "Milestones",                 thisObject => ((RunResultCell)thisObject)._milestones },
        { "Achievements",               thisObject => ((RunResultCell)thisObject)._newUnlocks },
    };
    public override object Get(string s) => Accessor[s](this);
    public RunResultCell(RunEnvironment env)
    {
        Debug.Assert(env != null, "RunEnvironment cannot be null");
        Debug.Assert(env.GetResult() != null, "RunResult cannot be null");

        _result = env.GetResult();
        _config = env.GetRunConfig();
        _playTime = env.GetRunfinishedTime();
        
        AchievementProfile[] newUnlocks = env.TraversalNewlyUnlockedAchievements().ToArray();
        _newUnlocks = new(newUnlocks);
        
        _profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        
        RunMilestone[] milestones = _result.GetCompletedMilestones(env);
        _milestones = new();
        _milestones.AddRange(milestones);
        _experienceGain = milestones.Sum(m => m.ExperienceGain) + 
                          newUnlocks.Sum(ap => ap.GetEntry().GetExperienceGain());

        _initialExperience = _profile.GetExperience();
        _initialLevel = _profile.GetLevel();

        (_finalLevel, _finalExperience) = _profile.GainExperienceDryRun(_experienceGain);
    }

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);

        AppManager.Instance.ProfileManager.GetCurrProfile().WriteRunResult(RunManager.Instance.Environment, _result, _experienceGain);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ReturnToTitleSignal returnToTitleSignal)
        {
            RunManager.Instance.ReturnToTitle();
            return this;
        }

        return this;
    }

    public RunResult.RunOutcome GetRunOutcome() => _result.GetOutcome();
    public string GetCharacterName() => _config.CharacterProfile.GetEntry().GetName();
    public string GetDifficulty() => _config.DifficultyProfile.GetEntry().GetName();
    public string GetPlayTime() => Util.FormatTime(_playTime);

    public int GetInitialExperience() => _initialExperience;
    public int GetFinalExperience() => _finalExperience;
    public int GetExperienceGain() => _experienceGain;
    public int GetInitialLevel() => _initialLevel;
    public int GetFinalLevel() => _finalLevel;
}
