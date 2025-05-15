
using System;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class RunResultPanelDescriptor : PanelDescriptor
{
    private RunResult _result;                     // 结果状态
    private RunConfig _config;                     // 角色配置
    private TimeSpan _playTime;                    // 游戏时长
    private AchievementProfile[] _newUnlocks;      // 新解锁成就
    private Profile _profile;                      // 玩家存档

    private ListModel<RunMilestone> _milestones;

    private int _initialExperience;
    private int _finalExperience;
    private int _experienceGain;
    private int _initialLevel;
    private int _finalLevel;

    public RunResultPanelDescriptor(RunEnvironment env)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Milestones",               () => _milestones },
        };

        Debug.Assert(env != null, "RunEnvironment cannot be null");
        Debug.Assert(env.GetResult() != null, "RunResult cannot be null");

        _result = env.GetResult();
        _config = env.GetRunConfig();
        _playTime = env.GetRunfinishedTime();
        _newUnlocks = env.TraversalNewlyUnlockedAchievements().ToArray();
        _profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        
        RunMilestone[] milestones = _result.GetCompletedMilestones(env);
        _milestones = new();
        _milestones.AddRange(milestones);
        _experienceGain = milestones.Sum(m => m.ExperienceGain) + 
                          _newUnlocks.Sum(ap => ap.GetEntry().GetExperienceGain());

        _initialExperience = _profile.GetExperience();
        _initialLevel = _profile.GetLevel();

        (_finalLevel, _finalExperience) = _profile.GainExperienceDryRun(_experienceGain);
    }

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);

        AppManager.Instance.ProfileManager.WriteRunResultToCurrent(RunManager.Instance.Environment, _result, _experienceGain);
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
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

    public AchievementProfile[] GetAchievements() => _newUnlocks;
}
