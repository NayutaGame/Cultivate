
using System;

[Serializable]
public class KVP
{
    public string Key;
    public int Value;

    public KVP(string key, int value)
    {
        Key = key;
        Value = value;
    }
}