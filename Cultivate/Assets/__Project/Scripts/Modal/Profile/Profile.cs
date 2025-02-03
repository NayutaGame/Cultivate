
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Profile : Addressable, ISerializationCallbackReceiver
{
    [SerializeField] private bool _finishedFirstRun;
    
    private CharacterProfileList _characterProfileList;
    public CharacterProfileList CharacterProfileList => _characterProfileList;
    private DifficultyProfileList _difficultyProfileList;
    public DifficultyProfileList DifficultyProfileList => _difficultyProfileList;
    
    // LevelProfile, used to track unlocked skills
    // ResultProfile

    [SerializeField] private RunEnvironment _runEnvironment;

    public void WriteRunEnvironment(RunEnvironment env)
    {
        _runEnvironment = JsonUtility.FromJson<RunEnvironment>(JsonUtility.ToJson(env));
    }

    public RunEnvironment ReadRunEnvironment()
    {
        return JsonUtility.FromJson<RunEnvironment>(JsonUtility.ToJson(_runEnvironment));
    }

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
        _finishedFirstRun = false;
    }

    public static Profile Default()
        => new();

    public bool IsFirstRunFinished()
        => _finishedFirstRun;

    public void SetFirstRunFinished(bool value)
        => _finishedFirstRun = value;

    public bool HasSave()
        => _runEnvironment != null;

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
        Debug.Log(result.GetState());
    }
}
