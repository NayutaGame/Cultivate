
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Profile : Addressable, ISerializationCallbackReceiver
{
    private CharacterProfileList _characterProfileList;
    public CharacterProfileList CharacterProfileList => _characterProfileList;
    private DifficultyProfileList _difficultyProfileList;
    public DifficultyProfileList DifficultyProfileList => _difficultyProfileList;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private Profile()
    {
        _accessors = new()
        {
            { "CharacterProfileList", () => _characterProfileList },
            { "DifficultyProfileList", () => _difficultyProfileList },
        };

        _characterProfileList = CharacterProfileList.Default();
        _difficultyProfileList = DifficultyProfileList.Default();
    }

    public static Profile Default()
        => new();

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        // when new entry is added, order will be corrupted
        // needs to fix order according to encyclopedia before using
    }

    public void WriteRunResult(RunResult result)
    {
        Debug.Log(result.State);
    }
}
