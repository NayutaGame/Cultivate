
using System;
using System.Collections.Generic;

public class RunConfig : Config, Addressable
{
    public CharacterProfile CharacterProfile;
    public DifficultyProfile DifficultyProfile;

    public MapEntry MapEntry;
    
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public RunConfig(RunConfigForm runConfigForm)
    {
        _accessors = new()
        {
            { "CharacterProfile", () => CharacterProfile },
        };
        
        CharacterProfile = runConfigForm.CharacterProfile;
        DifficultyProfile = runConfigForm.DifficultyProfile;

        MapEntry = "测试";
    }
}
