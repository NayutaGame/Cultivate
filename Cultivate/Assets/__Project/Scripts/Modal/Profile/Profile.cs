
using System;
using System.Collections.Generic;
using UnityEngine;

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

    // private AchievementProfileList _achievementProfileList;

    // ResultProfile

    [SerializeField] private RunEnvironment _runEnvironment;
    public RunEnvironment RunEnvironment => _runEnvironment;

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
        bool finishedFirstRun = false)
    {
        _accessors = new()
        {
            { "LevelProfile", () => _levelProfile },
            { "CharacterProfileList", () => _characterProfileList },
            { "DifficultyProfileList", () => _difficultyProfileList },
            { "PackProfileList", () => _packProfileList },
        };

        _levelProfile = levelProfile ?? LevelProfile.Default();
        _characterProfileList = characterProfileList ?? CharacterProfileList.Default();
        _difficultyProfileList = difficultyProfileList ?? DifficultyProfileList.Default();
        _packProfileList = packProfileList ?? PackProfileList.Default();

        _finishedFirstRun = finishedFirstRun;
    }

    public static Profile Default()
        => new();

    public static Profile Developer()
        => new(
            LevelProfile.Developer(),
            CharacterProfileList.Developer(),
            DifficultyProfileList.Developer(),
            PackProfileList.Developer(),
            true);

    public bool IsFirstRunFinished()
        => _finishedFirstRun;

    public void SetFirstRunFinished(bool value)
        => _finishedFirstRun = value;

    public bool HasSave()
        => _runEnvironment != null && _runEnvironment.IsLegit;

    public bool PackIsGenerallyUnlocked(CharacterEntry character, PackEntry entry)
    {
        if (_packProfileList.IsUnlocked(entry))
            return true;

        for (int i = 0; i < character._packPreset.PackEntries.Count; i++)
        {
            // 卡包未解锁，但是槽位解锁，并且当前角色的初始Preset的这个槽位的卡包是当前卡包
            if (SlotIsUnlocked(character, i) && character._packPreset.PackEntries[i] == entry)
                return true;
        }

        return false;
    }
        
    public bool PackIsGenerallyUnlocked(CharacterEntry character, PackEntry pack, int slotIndex)
    {
        if (_packProfileList.IsUnlocked(pack))
            return true;
        
        return SlotIsUnlocked(character, slotIndex) && 
               character._packPreset.PackEntries[slotIndex] == pack;
    }

    public bool PackIsUnlocked(PackEntry entry)
        => _packProfileList.IsUnlocked(entry);

    public bool CharacterIsUnlocked(CharacterEntry entry)
        => _characterProfileList.IsUnlocked(entry);

    public bool SlotIsUnlocked(CharacterEntry entry, int slotIndex)
        => _characterProfileList.SlotIsUnlocked(entry, slotIndex);

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
        };
        
        // when new entry is added, order will be corrupted
        // needs to fix order according to encyclopedia before using
    }

    public void WriteRunResult(RunResult result)
    {
        Debug.Log(result.GetState());
    }

    public CharacterProfile FirstCharacterProfile()
    {
        return _characterProfileList[0];
    }
}
