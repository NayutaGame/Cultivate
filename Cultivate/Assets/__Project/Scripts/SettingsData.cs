
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SettingsData
{
    public static readonly string Filename = "/SettingsData.json";

    [SerializeField] public SerializableDictionary _dict;

    public SettingsData()
    {
        _dict = new();
    }

    public int this[string key]
    {
        get => _dict[key];
        set => _dict[key] = value;
    }
}
