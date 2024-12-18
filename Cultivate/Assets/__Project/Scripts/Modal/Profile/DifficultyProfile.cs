
using System;
using UnityEngine;

[Serializable]
public class DifficultyProfile : ISerializationCallbackReceiver
{
    [SerializeField] private DifficultyEntry _entry;
    [SerializeField] private bool _unlocked;

    public DifficultyProfile(DifficultyEntry entry, bool unlocked = false)
    {
        _entry = entry;
        _unlocked = unlocked;
    }
    
    public DifficultyEntry GetEntry() => _entry;
    public bool IsUnlocked() => _unlocked;
    public void SetUnlocked(bool value) => _unlocked = value;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.DifficultyCategory[_entry.GetId()];
    }
}
