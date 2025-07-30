
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProfileList : ListModel<Profile>, Addressable, ISerializationCallbackReceiver
{
    public static readonly string Filename = "/ProfileList.json";

    [SerializeField] private Version _version;
    [SerializeField] private int _currentIndex;
    public Profile GetCurrent() => this[_currentIndex];

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Current",                    thisObject => ((ProfileList)thisObject).GetCurrent() },
    };
    public object Get(string s) => Accessor[s](this);
    public ProfileList()
    {
        _version = AppManager.Version;
        _currentIndex = 0;
        Add(new Profile(0));
    }

    public static ProfileList Default()
        => new();

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        // version migrating
    }

    public bool IsCompatible()
        => AppManager.Version.IsProfileCompatible(_version);
}
