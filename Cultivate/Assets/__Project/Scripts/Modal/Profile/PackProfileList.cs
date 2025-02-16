
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

    public PackProfile Find(PackEntry entry)
        => First(packProfile => packProfile.GetEntry() == entry);

    public static PackProfileList Default()
        => new();

    public static PackProfileList Developer()
        => new(true);

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
    }
}
