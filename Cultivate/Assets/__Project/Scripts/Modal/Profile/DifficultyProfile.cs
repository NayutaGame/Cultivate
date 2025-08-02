
using System;
using UnityEngine;

[Serializable]
public class DifficultyProfile : ISerializationCallbackReceiver
{
    [SerializeField] private DifficultyEntry _entry;
    [SerializeField] private bool _unlocked;

    public DifficultyProfile(DifficultyEntry entry)
    {
        _entry = entry;
        _unlocked = false;
    }
    
    public DifficultyEntry GetEntry() => _entry;
    public bool IsUnlocked() => _unlocked;
    public void SetUnlocked(bool value) => _unlocked = value;
    
    public bool IsDemoLocked()
        => _entry._order >= 3 && AppManager.Instance.PackageIsDemo();

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.DifficultyCategory.FromId(_entry.GetId());
    }
}
