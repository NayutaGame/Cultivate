
using System;
using System.Collections.Generic;

public class ProfileManager : Addressable
{
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public ProfileManager()
    {
        _accessors = new()
        {
            // { "Environment",           () => Environment },
            // { "Arena",                 () => Arena },
        };
    }
}
