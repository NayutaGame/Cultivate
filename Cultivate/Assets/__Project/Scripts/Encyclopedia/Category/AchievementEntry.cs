using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class AchievementEntry : Entry
{
    [NonSerialized] private string _rawConditionDescription;
    [NonSerialized] private Description _conditionDescription;
    [NonSerialized] private string _rawRewardDescription;
    [NonSerialized] private Description _rewardDescription;
    [NonSerialized] private int _experienceGain;
    [NonSerialized] private RunClosure[] _runClosures;
    [NonSerialized] private StageClosure[] _stageClosures;
    [NonSerialized] private LockIndex? _lockIndex;
    [NonSerialized] private SpriteEntry _spriteEntry;

    public AchievementEntry(
        string id,
        string name,
        string rawConditionDescription,
        string rawRewardDescription,
        int experienceGain = 100,
        LockIndex? lockIndex = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null
        ) : base(id, name)
    {
        _rawConditionDescription = rawConditionDescription;
        _rawRewardDescription = rawRewardDescription;
        _experienceGain = experienceGain;
        _lockIndex = lockIndex;
        _runClosures = runClosures ?? Array.Empty<RunClosure>();
        _stageClosures = stageClosures ?? Array.Empty<StageClosure>();
        _spriteEntry = Encyclopedia.SpriteCategory.FromName($"UnlockIcon{GetName()}");
    }
    
    public override void Init()
    {
        _conditionDescription = new Description(_rawConditionDescription);
        _rewardDescription = new Description(_rawRewardDescription);
    }
    
    public Description GetConditionDescription() => _conditionDescription;
    public Description GetRewardDescription() => _rewardDescription;
    public int GetExperienceGain() => _experienceGain;
    public RunClosure[] GetRunClosures() => _runClosures;
    public StageClosure[] GetStageClosures() => _stageClosures;
    public LockIndex? GetLockIndex() => _lockIndex;
    public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingSkillCardIllustration().Sprite;
}
