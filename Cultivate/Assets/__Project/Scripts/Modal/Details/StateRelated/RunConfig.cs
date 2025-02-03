
using System;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

[Serializable]
public class RunConfig : Addressable, ISerializationCallbackReceiver
{
    // this form should contains: selected character, selected difficulty, selected mutators, selected seed
    [SerializeReference] public CharacterProfile CharacterProfile;
    [SerializeReference] public DifficultyProfile DifficultyProfile;
    [SerializeField] public MapEntry MapEntry;
    
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public RunConfig(CharacterProfile characterProfile, DifficultyProfile difficultyProfile, MapEntry mapEntry = null)
    {
        _accessors = new()
        {
            { "CharacterProfile", () => CharacterProfile },
        };
        
        CharacterProfile = characterProfile;
        DifficultyProfile = difficultyProfile;
        // MapEntry = mapEntry ?? "标准";
        // MapEntry = mapEntry ?? "发现";
        MapEntry = mapEntry ?? "多段测试";
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

    public static RunConfig FirstRun()
    {
        Profile profile = AppManager.Instance.ProfileManager.ProfileList[0];
        return new(profile.CharacterProfileList[0], profile.DifficultyProfileList[0], "序章");
    }
}
