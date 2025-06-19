
using System;
using UnityEngine;

[Serializable]
public class AchievementProfile : ISerializationCallbackReceiver, RunClosureListener, StageClosureListener
{
    [SerializeField] private AchievementEntry _entry;
    [SerializeField] private bool _unlocked;

    [SerializeField] private SerializableDictionary _memory;

    public SerializableDictionary Memory => _memory;

    public AchievementProfile(AchievementEntry entry, bool isDeveloper = false)
    {
        _entry = entry;
        _unlocked = isDeveloper;

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
    
    public void Reset()
    {
        _unlocked = false;
        _memory.Clear();
    }

    // public int GetProgress() => _progress;
    // public void SetProgress(int value) => _progress = value;
    // public void AddProgress(int value = 1) => _progress += value;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.AchievementCategory[_entry.GetId()];
    }

    public override string ToString()
    {
        return $"{_entry.GetId()}: {_unlocked}";
    }
}
