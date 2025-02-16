
using System;
using UnityEngine;

[Serializable]
public class AchievementProfile : ISerializationCallbackReceiver, RunClosureOwner, StageClosureOwner
{
    [SerializeField] private AchievementEntry _entry;
    [SerializeField] private bool _unlocked;
    // [SerializeField] private int _progress;

    [SerializeField] private Memory _memory;
    public Memory Memory => _memory;

    public AchievementProfile(AchievementEntry entry, bool isDeveloper = false)
    {
        _entry = entry;
        _unlocked = isDeveloper;
        // _progress = 0;
    }
    
    public AchievementEntry GetEntry() => _entry;
    public bool IsUnlocked() => _unlocked;
    public void SetUnlocked(bool value) => _unlocked = value;
    public void Unlock() => _unlocked = true;
    // public int GetProgress() => _progress;
    // public void SetProgress(int value) => _progress = value;
    // public void AddProgress(int value = 1) => _progress += value;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.AchievementCategory[_entry.GetId()];
    }
}
