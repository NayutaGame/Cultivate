
using System;
using UnityEngine;

[Serializable]
public class CharacterProfile : ISerializationCallbackReceiver
{
    [SerializeField] private CharacterEntry _entry;
    [SerializeField] private bool _unlocked;

    public CharacterProfile(CharacterEntry entry, bool unlocked = false)
    {
        _entry = entry;
        _unlocked = unlocked;
    }
    
    public CharacterEntry GetEntry() => _entry;
    public bool IsUnlocked() => _unlocked;
    public void SetUnlocked(bool value) => _unlocked = value;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.CharacterCategory[_entry.GetId()];
    }
}
