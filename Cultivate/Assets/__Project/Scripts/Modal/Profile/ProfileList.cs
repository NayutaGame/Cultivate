
using System;
using System.Collections.Generic;

[Serializable]
public class ProfileList : ListModel<Profile>, Addressable
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
}
