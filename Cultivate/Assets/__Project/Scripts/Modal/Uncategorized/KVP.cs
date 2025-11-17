
using System;
using UnityEngine;

[Serializable]
public class KVP
{
    [SerializeField] public string Key;
    [SerializeField] public int Value;

    public KVP(string key, int value)
    {
        Key = key;
        Value = value;
    }
}