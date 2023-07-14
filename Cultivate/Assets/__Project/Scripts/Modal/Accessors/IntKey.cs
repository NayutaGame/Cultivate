using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntKey : CLKey
{
    private int _key;
    public int Key => _key;

    public IntKey(int key)
    {
        _key = key;
    }

    public static implicit operator IntKey(int key) => new(key);
}
