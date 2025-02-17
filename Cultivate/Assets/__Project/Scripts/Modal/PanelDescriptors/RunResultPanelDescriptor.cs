
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

    private RunMilestone[] _milestones;

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
        };

        Debug.Assert(env != null, "RunEnvironment cannot be null");
        Debug.Assert(env.GetResult() != null, "RunResult cannot be null");

        _result = env.GetResult();
        _config = env.GetRunConfig();
        _playTime = env.GetRunfinishedTime();
        _newUnlocks = env.TraversalNewlyUnlockedAchievements().ToArray();
        _profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        
        _milestones = _result.GetCompletedMilestones(env);
        _experienceGain = _milestones.Sum(m => m.ExperienceGain) + 
                        _newUnlocks.Sum(ap => ap.GetEntry().GetExperienceGain());

        _initialExperience = _profile.GetExperience();
        _initialLevel = _profile.GetLevel();

        (_finalExperience, _finalLevel) = _profile.GainExperienceDryRun(_experienceGain);
        
        // 删除存档中的Env
        // AppManager.Instance.ProfileManager.DeleteEnv();
    }

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);

        _profile.GainExperience(_experienceGain);

        AppManager.Instance.ProfileManager.WriteRunResultToCurrent(_result);
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
