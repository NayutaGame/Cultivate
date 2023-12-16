
using System;
using System.Collections.Generic;

[Serializable]
public class ProfileList : ListModel<Profile>, Addressable
{
    public static readonly string Filename = "/ProfileList.json";

    private int CurrentIndex;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public ProfileList()
    {
        _accessors = new()
        {
            { "Current",           () => Get(CurrentIndex) },
        };

        Add(Profile.Default());
        CurrentIndex = 0;
    }
}
