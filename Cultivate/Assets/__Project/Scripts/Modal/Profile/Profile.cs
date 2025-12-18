
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using System.Linq;

[Serializable]
public class Profile : Addressable, ISerializationCallbackReceiver
{
    private static readonly string RunFilenamePattern = "/Run{0}.json";
    
    [SerializeField] private int _index;
    [SerializeField] private bool _finishedFirstRun;
    [SerializeField] private LevelProfile _levelProfile;
    [SerializeField] private CharacterProfileList _characterProfileList;
    [SerializeField] private DifficultyProfileList _difficultyProfileList;
    [SerializeField] private PackProfileList _packProfileList;
    [SerializeField] private AchievementProfileList _achievementProfileList;
    
    public LevelProfile LevelProfile => _levelProfile;
    public CharacterProfileList CharacterProfileList => _characterProfileList;
    public DifficultyProfileList DifficultyProfileList => _difficultyProfileList;
    public PackProfileList PackProfileList => _packProfileList;
    public AchievementProfileList AchievementProfileList => _achievementProfileList;

    // 战斗记录
    // [SerializeField] private ResultProfileList _resultProfileList;
    // public ResultProfileList ResultProfileList => _resultProfileList;
    
    [NonSerialized] private Dirty<Dictionary<LockIndex, AchievementProfile>> _achievementCache;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "LevelProfile",               thisObject => ((Profile)thisObject)._levelProfile },
        { "CharacterProfileList",       thisObject => ((Profile)thisObject)._characterProfileList },
        { "DifficultyProfileList",      thisObject => ((Profile)thisObject)._difficultyProfileList },
        { "PackProfileList",            thisObject => ((Profile)thisObject)._packProfileList },
        { "AchievementProfileList",     thisObject => ((Profile)thisObject)._achievementProfileList },
    };
    public object Get(string s) => Accessor[s](this);
    public Profile(int index)
    {
        _index = index;
        _levelProfile = LevelProfile.Default();
        _characterProfileList = CharacterProfileList.Default();
        _difficultyProfileList = DifficultyProfileList.Default();
        _packProfileList = PackProfileList.Default();
        _achievementProfileList = AchievementProfileList.Default();
        _finishedFirstRun = false;

        CommonInit();

        _runSaveState = RunSaveState.None;
        _environmentCache = null;
    }

    [NonSerialized] private RunSaveState _runSaveState;
    [NonSerialized] private RunEnvironment _environmentCache;
    
    private void CommonInit()
    {
        _achievementCache = new Dirty<Dictionary<LockIndex, AchievementProfile>>(BuildAchievementCache);
    }

    public RunEnvironment Environment
    {
        get
        {
            if (_environmentCache == null)
                return null;
            return JsonUtility.FromJson<RunEnvironment>(JsonUtility.ToJson(_environmentCache));
        }
        set
        {
            if (value != null)
            {
                value.WriteTime();
                _runSaveState = RunSaveState.Valid;
                _environmentCache = JsonUtility.FromJson<RunEnvironment>(JsonUtility.ToJson(value));
                FileUtility.WritePersistentFile(_environmentCache, GetRunFilename());
            }
            else
            {
                _runSaveState = RunSaveState.None;
                _environmentCache = null;
                FileUtility.DeletePersistentFile(GetRunFilename());
            }
        }
    }

    public string GetRunFilename()
        => string.Format(RunFilenamePattern, _index.ToString());

    public enum RunSaveState
    {
        None,
        Valid,
        Corrupted,
    }

    public bool HasValidSave()
        => _runSaveState == RunSaveState.Valid;

    public bool HasCorruptedSave()
        => _runSaveState == RunSaveState.Corrupted;
    
    public void LoadEnvironment(out RunSaveState runSaveState, out RunEnvironment environment)
    {
        string fileName = GetRunFilename();
        
        if (!FileUtility.IsPersistentFileExists(fileName))
        {
            runSaveState = RunSaveState.None;
            environment = null;
            return;
        }

        try
        {
            environment = FileUtility.ReadPersistentFile<RunEnvironment>(fileName);
            
            if (environment != null && environment.IsCompatible())
            {
                runSaveState = RunSaveState.Valid;
            }
            else
            {
                runSaveState = RunSaveState.Corrupted;
                environment = null;
            }
        }
        catch
        {
            runSaveState = RunSaveState.Corrupted;
            environment = null;
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

    public bool IsFirstRunFinished()
        => _finishedFirstRun;

    public DifficultyProfile GetHighestUnlockedDifficulty()
    {
        return DifficultyProfileList.GetHighestUnlockedDifficulty();
    }

    public bool DifficultyIsUnlocked(string difficultyName)
        => DifficultyIsUnlocked(Encyclopedia.DifficultyCategory.FromName(difficultyName));

    public bool DifficultyIsUnlocked(DifficultyEntry difficultyEntry)
    {
        return DifficultyProfileList.Find(difficultyEntry).IsUnlocked();
    }
    
    private Dictionary<LockIndex, AchievementProfile> BuildAchievementCache()
    {
        var cache = new Dictionary<LockIndex, AchievementProfile>();
        
        foreach (AchievementProfile achievementProfile in _achievementProfileList)
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
        int? slotIndex = character.PackPreset.PackEntries.FirstIdx(packEntry => packEntry == pack);
        if (!slotIndex.HasValue)
            return PackIsUnlocked(pack);
        
        return PackIsGenerallyUnlocked(character, pack, slotIndex.Value);
    }
        
    public bool PackIsGenerallyUnlocked(CharacterEntry character, PackEntry pack, int slotIndex)
    {
        if (PackIsUnlocked(pack))
            return true;
        
        // 卡包未解锁，但是槽位解锁，并且当前角色的初始Preset的这个槽位的卡包是当前卡包
        return SlotIsUnlocked(character, slotIndex) && character.PackPreset.PackEntries[slotIndex] == pack;
    }

    public Description GetPackUnlockCondition(CharacterEntry character, PackEntry pack)
    {
        LockIndex lockIndex = LockIndex.FromPack(pack);
        AchievementProfile achievementProfile = GetAchievementProfileFromLockIndex(lockIndex);
        return achievementProfile.GetEntry().GetConditionDescription();
    }

    public Description GetConstraintUnlockCondition(CharacterEntry character, int slotIndex)
    {
        LockIndex lockIndex = LockIndex.FromSlot(character, slotIndex);
        AchievementProfile achievementProfile = GetAchievementProfileFromLockIndex(lockIndex);
        return achievementProfile.GetEntry().GetConditionDescription();
    }

    public bool CharacterIsUnlocked(CharacterEntry entry)
        => _characterProfileList.Find(entry).IsUnlocked();

    public bool PackIsUnlocked(PackEntry entry)
        => _packProfileList.Find(entry).IsUnlocked();

    public bool SlotIsUnlocked(CharacterEntry entry, int slotIndex)
        => _characterProfileList.Find(entry).SlotIsUnlocked(slotIndex);

    public AchievementProfile GetAchievementProfileFromLockIndex(LockIndex lockIndex)
        => _achievementCache.Value.TryGetValue(lockIndex, out var profile) ? profile : null;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        // 更新成就列表
        UpdateAchievementProfiles();
        
        CommonInit();
        LoadEnvironment(out _runSaveState, out _environmentCache);
    }

    private void UpdateAchievementProfiles()
    {
        // 获取所有已有成就的ID
        var existingIds = new HashSet<string>(_achievementProfileList
            .Select(p => p.GetEntry().GetId()));

        // 遍历Encyclopedia中的所有成就
        foreach (var achievementEntry in Encyclopedia.AchievementCategory)
        {
            // 如果是新成就，添加对应的Profile
            if (!existingIds.Contains(achievementEntry.GetId()))
            {
                Debug.Log($"新添加的成就: {achievementEntry.GetId()}");
                _achievementProfileList.Add(new AchievementProfile(achievementEntry));
            }
        }
    }

    public CharacterProfile FirstCharacterProfile()
    {
        return _characterProfileList[0];
    }

    public void RegisterRunClosures(RunClosureDict runClosureDict)
    {
        _achievementProfileList.Do(achievementProfile =>
            achievementProfile.GetEntry().GetRunClosures().Do(runClosure =>
                runClosureDict.Register(achievementProfile, runClosure)));
    }

    public void UnregisterRunClosures(RunClosureDict runClosureDict)
    {
        _achievementProfileList.Do(achievementProfile =>
            achievementProfile.GetEntry().GetRunClosures().Do(runClosure =>
                runClosureDict.Unregister(achievementProfile, runClosure)));
    }

    public void RegisterStageClosures(StageClosureDict stageClosureDict)
    {
        _achievementProfileList.Do(achievementProfile =>
            achievementProfile.GetEntry().GetStageClosures().Do(stageClosure =>
                stageClosureDict.Register(achievementProfile, stageClosure)));
    }

    public void UnregisterStageClosures(StageClosureDict stageClosureDict)
    {
        _achievementProfileList.Do(achievementProfile =>
            achievementProfile.GetEntry().GetStageClosures().Do(stageClosure =>
                stageClosureDict.Unregister(achievementProfile, stageClosure)));
    }

    public int GetExperience()
        => _levelProfile.Experience;

    public int GetLevel()
        => _levelProfile.Level;

    public (int, int) GainExperienceDryRun(int experienceGain)
        => _levelProfile.GainExperienceDryRun(experienceGain);
    
    
    
    
    
    
    
    
    


    // public void SetCharacterUnlockedQuietly(CharacterEntry entry, bool value)
    //     => _characterProfileList.Find(entry).SetUnlockedQuietly(value);
    //
    // public void SetPackUnlockedQuietly(PackEntry entry, bool value)
    //     => _packProfileList.Find(entry).SetUnlockedQuietly(value);
    //
    // public void SetSlotUnlockedQuietly(CharacterEntry entry, int slotIndex, bool value)
    //     => _characterProfileList.Find(entry).SetSlotUnlockedQuietly(slotIndex, value);

    public void ResetAchievements()
    {
        _achievementProfileList.Do(achievementProfile =>
            achievementProfile.Reset());
        
        AppManager.Instance.ProfileManager.SaveProcedure();
    }

    public void UnlockAchievement(AchievementEntry entry)
    {
        var achievementProfile = AchievementProfileList.First(ap => ap.GetEntry().GetId() == entry.GetId());
        
        if (achievementProfile == null)
        {
            Debug.LogError($"未找到成就: {entry.GetId()}");
            return;
        }
        
        achievementProfile.SetUnlockedQuietly(true);
        
        AppManager.Instance.ProfileManager.SaveProcedure();
    }
    
    public void UnlockEverything()
    {
        _levelProfile.UnlockEverything();
        _characterProfileList.UnlockEverything();
        _difficultyProfileList.UnlockEverything();
        _packProfileList.UnlockEverything();
        _achievementProfileList.UnlockEverything();
        _finishedFirstRun = true;
        
        AppManager.Instance.ProfileManager.SaveProcedure();
    }
    
    public void WriteRunResult(RunEnvironment env, RunResult result, int experienceGain)
    {
        _levelProfile.GainExperience(experienceGain);
        
        if (result.GetOutcome() == RunResult.RunOutcome.Victorious)
        {
            DifficultyEntry curr = env.GetRunConfig().DifficultyProfile.GetEntry();
            DifficultyEntry next = Encyclopedia.DifficultyCategory.GetNext(curr);

            if (next != null)
                _difficultyProfileList.UnlockDifficulty(next);
        }
        
        AppManager.Instance.ProfileManager.SaveProcedure();
        Environment = null;
    }

    public void SetFirstRunFinished(bool value)
        => _finishedFirstRun = value;

    public void TryUnlockNextDifficulty()
    {
        DifficultyEntry curr = GetHighestUnlockedDifficulty().GetEntry();
        DifficultyEntry next = Encyclopedia.DifficultyCategory.GetNext(curr);

        if (next == null)
            return;
        
        _difficultyProfileList.UnlockDifficulty(next);
    }

    public void RepairCorruptedEnvironment()
    {
        _runSaveState = RunSaveState.None;
        Environment = null;
    }
}
