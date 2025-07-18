
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProfileList : ListModel<Profile>, Addressable, ISerializationCallbackReceiver
{
    public static readonly string Filename = "/ProfileList.json";

    [SerializeField] private string _version;

    [NonSerialized] private int CurrentIndex;
    public Profile GetCurrent() => Get(CurrentIndex) as Profile;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Current",                    thisObject => ((ProfileList)thisObject).GetCurrent() },
    };
    public object Get(string s) => Accessor[s](this);
    public ProfileList()
    {
        Add(Profile.Default());

        CurrentIndex = 0;

        _version = AppManager.Version;
    }

    public static ProfileList Default()
        => new();

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        // version migrating
    }
}
