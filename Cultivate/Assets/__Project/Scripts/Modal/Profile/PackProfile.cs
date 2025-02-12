
using UnityEngine;

public class PackProfile : ISerializationCallbackReceiver
{
    [SerializeField] private PackEntry _entry;
    [SerializeField] private bool _unlocked;

    public PackProfile(PackEntry entry, bool isDeveloper = false)
    {
        _entry = entry;
        _unlocked = isDeveloper;
    }
    
    public PackEntry GetEntry() => _entry;
    public bool IsUnlocked() => _unlocked;
    public void SetUnlocked(bool value) => _unlocked = value;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.PackCategory[_entry.GetId()];
    }
}
