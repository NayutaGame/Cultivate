
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Profile : Addressable, ISerializationCallbackReceiver
{
    private CharacterProfileList _characterProfileList;
    // private List<DifficultyProfile> _difficultyProfiles;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private Profile()
    {
        _accessors = new()
        {
            { "CharacterProfileList", () => _characterProfileList },
        };

        _characterProfileList = CharacterProfileList.Default();
    }

    public static Profile Default()
        => new();

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        // when new character entry is added, auto complete profile for the new character
    }
}
