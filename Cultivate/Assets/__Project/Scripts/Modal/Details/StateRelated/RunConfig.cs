
using System;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

[Serializable]
public class RunConfig : Addressable, ISerializationCallbackReceiver
{
    [SerializeReference] public CharacterProfile CharacterProfile;
    [SerializeReference] public DifficultyProfile DifficultyProfile;
    [SerializeField] public MapEntry MapEntry;
    
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

        MapEntry = "标准";
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "CharacterProfile", () => CharacterProfile },
        };
        MapEntry = string.IsNullOrEmpty(MapEntry.GetId()) ? null : Encyclopedia.MapCategory[MapEntry.GetId()];
    }
}
