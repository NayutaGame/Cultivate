
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using System.Linq;

[Serializable]
public class Profile : Addressable, ISerializationCallbackReceiver
{
    [SerializeField] private bool _finishedFirstRun;

    [SerializeField] private LevelProfile _levelProfile;
    public LevelProfile LevelProfile => _levelProfile;
    [SerializeField] private CharacterProfileList _characterProfileList;
    public CharacterProfileList CharacterProfileList => _characterProfileList;
    [SerializeField] private DifficultyProfileList _difficultyProfileList;
    public DifficultyProfileList DifficultyProfileList => _difficultyProfileList;
    [SerializeField] private PackProfileList _packProfileList;
    public PackProfileList PackProfileList => _packProfileList;
    [SerializeField] private AchievementProfileList _achievementProfileList;
    public AchievementProfileList AchievementProfileList => _achievementProfileList;

    // [SerializeField] private ResultProfileList _resultProfileList;
    // public ResultProfileList ResultProfileList => _resultProfileList;

    [SerializeField] private RunEnvironment _runEnvironment;
    public RunEnvironment RunEnvironment
    {
        get => _runEnvironment;
        set => _runEnvironment = value;
    }

    [NonSerialized] private Dirty<Dictionary<LockIndex, AchievementProfile>> _achievementCache;

    public void WriteRunEnvironment(RunEnvironment env)
    {
        _runEnvironment = JsonUtility.FromJson<RunEnvironment>(JsonUtility.ToJson(env));
    }

    public RunEnvironment ReadRunEnvironment()
    {
        return JsonUtility.FromJson<RunEnvironment>(JsonUtility.ToJson(_runEnvironment));
    }

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private Profile(
        LevelProfile levelProfile = null,
        CharacterProfileList characterProfileList = null,
        DifficultyProfileList difficultyProfileList = null,
        PackProfileList packProfileList = null,
        AchievementProfileList achievementProfileList = null,
        // bool finishedFirstRun = true)
        bool finishedFirstRun = false)
    {
        _accessors = new()
        {
            { "LevelProfile", () => _levelProfile },
            { "CharacterProfileList", () => _characterProfileList },
            { "DifficultyProfileList", () => _difficultyProfileList },
            { "PackProfileList", () => _packProfileList },
            { "AchievementProfileList", () => _achievementProfileList },
        };

        _levelProfile = levelProfile ?? LevelProfile.Default();
        _characterProfileList = characterProfileList ?? CharacterProfileList.Default();
        _difficultyProfileList = difficultyProfileList ?? DifficultyProfileList.Default();
        _packProfileList = packProfileList ?? PackProfileList.Default();
        _achievementProfileList = achievementProfileList ?? AchievementProfileList.Default();
        
        _finishedFirstRun = finishedFirstRun;
        
        _achievementCache = new Dirty<Dictionary<LockIndex, AchievementProfile>>(BuildAchievementCache);
    }

    public static Profile Default()
        => new();

    public static Profile Developer()
        => new(
            LevelProfile.Developer(),
            CharacterProfileList.Developer(),
            DifficultyProfileList.Developer(),
            PackProfileList.Developer(),
            AchievementProfileList.Developer(),
            true);

    public bool IsFirstRunFinished()
        => _finishedFirstRun;

    public void SetFirstRunFinished(bool value)
        => _finishedFirstRun = value;

    public bool HasSave()
        => _runEnvironment != null && _runEnvironment.IsLegit;
    
    private Dictionary<LockIndex, AchievementProfile> BuildAchievementCache()
    {
        var cache = new Dictionary<LockIndex, AchievementProfile>();
        
        foreach (AchievementProfile achievementProfile in _achievementProfileList.Traversal())
        {
            LockIndex? lockIndex = achievementProfile.GetEntry().GetLockIndex();
            if (lockIndex == null)
                continue;

            cache[lockIndex.Value] = achievementProfile;
        }
        return cache;
    }

    public bool PackIsGenerallyUnlocked(CharacterEntry character, PackEntry pack)
    {
        int? slotIndex = character._packPreset.PackEntries.FirstIdx(packEntry => packEntry == pack);
        if (!slotIndex.HasValue)
            return PackIsUnlocked(pack);
        
        return PackIsGenerallyUnlocked(character, pack, slotIndex.Value);
    }
        
    public bool PackIsGenerallyUnlocked(CharacterEntry character, PackEntry pack, int slotIndex)
    {
        if (PackIsUnlocked(pack))
            return true;
        
        // 卡包未解锁，但是槽位解锁，并且当前角色的初始Preset的这个槽位的卡包是当前卡包
        return SlotIsUnlocked(character, slotIndex) && character._packPreset.PackEntries[slotIndex] == pack;
    }

    public string GetPackUnlockCondition(CharacterEntry character, PackEntry pack)
    {
        LockIndex lockIndex = LockIndex.FromPack(pack.GetId());
        AchievementProfile achievementProfile = GetAchievementProfileFromLockIndex(lockIndex);
        return achievementProfile.GetEntry().GetConditionDescription();
    }

    public string GetConstraintUnlockCondition(CharacterEntry character, int slotIndex)
    {
        LockIndex lockIndex = LockIndex.FromSlot(character.GetId(), slotIndex);
        AchievementProfile achievementProfile = GetAchievementProfileFromLockIndex(lockIndex);
        return achievementProfile.GetEntry().GetConditionDescription();
    }

    public bool CharacterIsUnlocked(CharacterEntry entry)
        => _characterProfileList.Find(entry).IsUnlocked();

    public bool PackIsUnlocked(PackEntry entry)
        => _packProfileList.Find(entry).IsUnlocked();

    public bool SlotIsUnlocked(CharacterEntry entry, int slotIndex)
        => _characterProfileList.Find(entry).SlotIsUnlocked(slotIndex);

    public void SetCharacterUnlockedQuietly(CharacterEntry entry, bool value)
        => _characterProfileList.Find(entry).SetUnlockedQuietly(value);

    public void SetPackUnlockedQuietly(PackEntry entry, bool value)
        => _packProfileList.Find(entry).SetUnlockedQuietly(value);

    public void SetSlotUnlockedQuietly(CharacterEntry entry, int slotIndex, bool value)
        => _characterProfileList.Find(entry).SetSlotUnlockedQuietly(slotIndex, value);

    public AchievementProfile GetAchievementProfileFromLockIndex(LockIndex lockIndex)
        => _achievementCache.Value.TryGetValue(lockIndex, out var profile) ? profile : null;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "LevelProfile", () => _levelProfile },
            { "CharacterProfileList", () => _characterProfileList },
            { "DifficultyProfileList", () => _difficultyProfileList },
            { "PackProfileList", () => _packProfileList },
            { "AchievementProfileList", () => _achievementProfileList },
        };

        // 更新成就列表
        UpdateAchievementProfiles();
        
        _achievementCache = new Dirty<Dictionary<LockIndex, AchievementProfile>>(BuildAchievementCache);
    }

    private void UpdateAchievementProfiles()
    {
        // 获取所有已有成就的ID
        var existingIds = new HashSet<string>(_achievementProfileList.Traversal()
            .Select(p => p.GetEntry().GetId()));

        // 遍历Encyclopedia中的所有成就
        foreach (var achievementEntry in Encyclopedia.AchievementCategory.Traversal)
        {
            // 如果是新成就，添加对应的Profile
            if (!existingIds.Contains(achievementEntry.GetId()))
            {
                Debug.Log($"新添加的成就: {achievementEntry.GetId()}");
                _achievementProfileList.Add(new AchievementProfile(achievementEntry));
            }
        }
    }

    public void WriteRunResult(RunEnvironment env, RunResult result, int experienceGain)
    {
        // 存档经验
        _levelProfile.GainExperience(experienceGain);
        
        // 尝试解锁下一难度
        _difficultyProfileList.TryUnlockNextDifficulty(env, result);

        Debug.Log($"写入RunResult: {result.GetOutcome()}");
    }

    public CharacterProfile FirstCharacterProfile()
    {
        return _characterProfileList[0];
    }

    public void RegisterRunClosures(RunClosureDict runClosureDict)
    {
        _achievementProfileList.Traversal().Do(achievementProfile =>
            achievementProfile.GetEntry().GetRunClosures().Do(runClosure =>
                runClosureDict.Register(achievementProfile, runClosure)));
    }

    public void UnregisterRunClosures(RunClosureDict runClosureDict)
    {
        _achievementProfileList.Traversal().Do(achievementProfile =>
            achievementProfile.GetEntry().GetRunClosures().Do(runClosure =>
                runClosureDict.Unregister(achievementProfile, runClosure)));
    }

    public void RegisterStageClosures(StageClosureDict stageClosureDict)
    {
        _achievementProfileList.Traversal().Do(achievementProfile =>
            achievementProfile.GetEntry().GetStageClosures().Do(stageClosure =>
                stageClosureDict.Register(achievementProfile, stageClosure)));
    }

    public void UnregisterStageClosures(StageClosureDict stageClosureDict)
    {
        _achievementProfileList.Traversal().Do(achievementProfile =>
            achievementProfile.GetEntry().GetStageClosures().Do(stageClosure =>
                stageClosureDict.Unregister(achievementProfile, stageClosure)));
    }

    public int GetExperience()
        => _levelProfile.Experience;

    public int GetLevel()
        => _levelProfile.Level;

    public (int, int) GainExperienceDryRun(int experienceGain)
        => _levelProfile.GainExperienceDryRun(experienceGain);
}
