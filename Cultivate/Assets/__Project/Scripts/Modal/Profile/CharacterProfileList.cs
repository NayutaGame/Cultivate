
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class CharacterProfileList : ListModel<CharacterProfile>, ISerializationCallbackReceiver
{
    private CharacterProfileList()
    {
        Encyclopedia.CharacterCategory.Traversal.Do(entry => Add(new CharacterProfile(entry)));
        Find("徐福").SetUnlocked(true);
    }

    private CharacterProfile Find(CharacterEntry entry)
        => First(characterProfile => characterProfile.GetEntry() == entry);

    public static CharacterProfileList Default()
        => new();

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        // if loaded, needs to fix order according to encyclopedia before using
    }
}
