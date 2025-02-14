
using System;
using UnityEngine;

[Serializable]
public class CharacterProfile : ISerializationCallbackReceiver
{
    private const int PACK_SLOT_COUNT = 7;

    [SerializeField] private CharacterEntry _entry;
    [SerializeField] private bool _unlocked;

    [SerializeField] private int _level;
    [SerializeField] private int _experience;

    [SerializeField] private bool[] _packSlotIsLocked;

    public CharacterProfile(CharacterEntry entry, bool isDeveloper = false)
    {
        _entry = entry;
        _unlocked = isDeveloper;

        _level = isDeveloper ? 10 : 1;
        _experience = isDeveloper ? 1000 : 0;

        _packSlotIsLocked = new bool[PACK_SLOT_COUNT];
        for (int i = 0; i < PACK_SLOT_COUNT; i++)
            _packSlotIsLocked[i] = isDeveloper;
    }
    
    public CharacterEntry GetEntry() => _entry;
    public bool IsUnlocked() => _unlocked;
    public void SetUnlocked(bool value) => _unlocked = value;

    public bool SlotIsUnlocked(int slotIndex) => _packSlotIsLocked[slotIndex];
    public void SetSlotUnlocked(int slotIndex, bool value) => _packSlotIsLocked[slotIndex] = value;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.CharacterCategory[_entry.GetId()];
    }
}
