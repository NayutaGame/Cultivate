
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class PackProfileList : ListModel<PackProfile>, ISerializationCallbackReceiver
{
    private PackProfileList()
    {
        Encyclopedia.PackCategory.Do(entry => Add(new PackProfile(entry)));
    }

    public PackProfile Find(PackEntry entry)
        => First(packProfile => packProfile.GetEntry() == entry);

    public static PackProfileList Default()
        => new();

    public void UnlockEverything()
    {
        
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
    }
}
