
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class PackProfileList : ListModel<PackProfile>, ISerializationCallbackReceiver
{
    private PackProfileList(bool isDeveloper = false)
    {
        Encyclopedia.PackCategory.Traversal.Do(entry => Add(new PackProfile(entry, isDeveloper)));
    }

    private PackProfile Find(PackEntry entry)
        => First(packProfile => packProfile.GetEntry() == entry);

    public static PackProfileList Default()
        => new();

    public static PackProfileList Developer()
        => new(true);

    public bool IsUnlocked(PackEntry entry)
        => Find(entry).IsUnlocked();

    public void SetUnlocked(PackEntry entry, bool unlocked)
        => Find(entry).SetUnlocked(unlocked);

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
    }
}
