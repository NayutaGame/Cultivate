
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

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public ProfileList()
    {
        _accessors = new()
        {
            { "Current",           GetCurrent },
        };

        if (AppManager.Instance.IsDeveloperMode)
        {
            Add(Profile.Developer());
        }
        else
        {
            Add(Profile.Default());
        }

        CurrentIndex = 0;

        _version = AppManager.Version;
    }

    public static ProfileList Default()
        => new();

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "Current",           GetCurrent },
        };
        
        // version migrating
    }
}
