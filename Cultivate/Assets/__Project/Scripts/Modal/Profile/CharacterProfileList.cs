
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

        // Find("徐福").SetUnlocked(true);
    }

    public CharacterProfile Find(CharacterEntry entry)
        => First(characterProfile => characterProfile.GetEntry() == entry);

    public static CharacterProfileList Default()
        => new();

    public static CharacterProfileList Developer()
        => new(true);

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        // when new entry is added, order will be corrupted
        // needs to fix order according to encyclopedia before using
    }
}
