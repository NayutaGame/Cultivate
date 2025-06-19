using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary : ISerializationCallbackReceiver
{
    [NonSerialized] private Dictionary<string, int> _dict;
    [SerializeField] private List<KVP> _data;

    public void SetVariable(string key, int value)
        => _dict[key] = value;

    public int TryGetVariable(string key, int defaultValue)
    {
        _dict.TryAdd(key, defaultValue);
        return _dict[key];
    }

    public int PerformOperation(string key, int defaultValue, Func<int, int> operation)
    {
        int value = TryGetVariable(key, defaultValue);
        value = operation(value);
        SetVariable(key, value);
        return value;
    }

    public int PerformAggregate(string key, int value)
        => PerformOperation(key, 0, n => n + value);

    public int PerformAddOne(string key)
        => PerformOperation(key, 0, n => n + 1);

    public int PerformMax(string key, int value)
        => PerformOperation(key, 0, n => Mathf.Max(n, value));

    public SerializableDictionary()
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

    public void Clear()
    {
        _dict.Clear();
        _data.Clear();
    }
}