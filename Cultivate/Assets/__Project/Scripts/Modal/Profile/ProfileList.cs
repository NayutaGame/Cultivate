
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProfileList : ListModel<Profile>, Addressable, ISerializationCallbackReceiver
{
    public static readonly string Filename = "/ProfileList.json";

    private int CurrentIndex;
    public Profile GetCurrent() => Get(CurrentIndex) as Profile;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public ProfileList()
    {
        _accessors = new()
        {
            { "Current",           GetCurrent },
        };

        Add(Profile.Default());
        CurrentIndex = 0;
    }
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "Current",           GetCurrent },
        };
    }
}
