
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SettingsData : ISerializationCallbackReceiver
{
    public static readonly string Filename = "/SettingsData.json";

    [NonSerialized] public Dictionary<string, int> _dict;
    [SerializeField] private List<KVP> _data;

    public SettingsData()
    {
        _dict = new();
    }
    
    public void OnBeforeSerialize()
    {
        _data = new();
        foreach (KeyValuePair<string, int> kvp in _dict)
            _data.Add(new(kvp.Key, kvp.Value));
    }

    public void OnAfterDeserialize()
    {
        _dict = new();
        foreach (KVP row in _data)
            _dict[row.Key] = row.Value;
    }

    public int this[string key]
    {
        get => _dict[key];
        set => _dict[key] = value;
    }
}
