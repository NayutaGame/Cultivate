using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringKey : CLKey
{
    private string _key;
    public string Key => _key;

    public StringKey(string key)
    {
        _key = key;
    }

    public static implicit operator StringKey(string key) => new(key);
}
