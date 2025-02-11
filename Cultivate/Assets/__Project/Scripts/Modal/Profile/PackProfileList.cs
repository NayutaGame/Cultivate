
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class PackProfileList : ListModel<PackProfile>, ISerializationCallbackReceiver
{
    private PackProfileList()
    {
        Encyclopedia.PackCategory.Traversal.Do(entry => Add(new PackProfile(entry, true)));

        // Encyclopedia.CharacterCategory.Traversal.Do(entry => Add(new CharacterProfile(entry)));
        // Find("徐福").SetUnlocked(true);
    }

    private PackProfile Find(PackEntry entry)
        => First(packProfile => packProfile.GetEntry() == entry);

    public static PackProfileList Default()
        => new();

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
    }
}
