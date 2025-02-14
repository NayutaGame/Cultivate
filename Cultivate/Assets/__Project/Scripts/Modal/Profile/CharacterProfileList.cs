
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class CharacterProfileList : ListModel<CharacterProfile>, ISerializationCallbackReceiver
{
    private CharacterProfileList(bool isDeveloper = false)
    {
        Encyclopedia.CharacterCategory.Traversal.Do(entry => Add(
            new CharacterProfile(entry, isDeveloper)));

        Find("徐福").SetUnlocked(true);
    }

    private CharacterProfile Find(CharacterEntry entry)
        => First(characterProfile => characterProfile.GetEntry() == entry);

    public static CharacterProfileList Default()
        => new();

    public static CharacterProfileList Developer()
        => new(true);

    public bool IsUnlocked(CharacterEntry entry)
        => Find(entry).IsUnlocked();

    public void SetUnlocked(CharacterEntry entry, bool value)
        => Find(entry).SetUnlocked(value);

    public bool SlotIsUnlocked(CharacterEntry entry, int slotIndex)
        => Find(entry).SlotIsUnlocked(slotIndex);

    public void SetSlotUnlocked(CharacterEntry entry, int slotIndex, bool unlocked)
        => Find(entry).SetSlotUnlocked(slotIndex, unlocked);

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        // when new entry is added, order will be corrupted
        // needs to fix order according to encyclopedia before using
    }
}
