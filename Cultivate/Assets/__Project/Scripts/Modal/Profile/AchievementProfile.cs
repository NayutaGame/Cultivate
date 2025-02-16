
using System;
using UnityEngine;

[Serializable]
public class AchievementProfile : ISerializationCallbackReceiver, RunClosureOwner, StageClosureOwner
{
    [SerializeField] private AchievementEntry _entry;
    [SerializeField] private bool _unlocked;
    // [SerializeField] private int _progress;

    // @TODO serialize
    [NonSerialized] private Memory _memory;

    public AchievementEntry Entry => _entry;
    public Memory Memory => _memory;

    public AchievementProfile(AchievementEntry entry, bool isDeveloper = false)
    {
        _entry = entry;
        _unlocked = isDeveloper;
        // _progress = 0;

        _memory = new();
    }
    
    public AchievementEntry GetEntry() => _entry;
    public bool IsUnlocked() => _unlocked;
    public void Unlock()
    {
        if (_unlocked) return;
        _unlocked = true;

        RunEnvironment env = RunManager.Instance.Environment;
        if (env != null)
            env.RecordNewlyUnlockedAchievement(_entry);
    }

    public void SetUnlockedQuietly(bool value)
    {
        _unlocked = value;
    }

    // public int GetProgress() => _progress;
    // public void SetProgress(int value) => _progress = value;
    // public void AddProgress(int value = 1) => _progress += value;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.AchievementCategory[_entry.GetId()];

        _memory = new();
    }

    public override string ToString()
    {
        return $"{_entry.GetId()}: {_unlocked}";
    }
}
