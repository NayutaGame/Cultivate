using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoundedInt
{
    private int _curr;
    private int _max;

    public int GetCurr() => _curr;
    public void SetCurr(int value) => _curr = Mathf.Min(_max, value);
    public void SetDiff(int value = 1) => SetCurr(_curr + value);

    public int GetMax() => _max;
    public int SetMax(int value)
    {
        _max = value;
        SetCurr(_curr);
        return _max;
    }

    public override string ToString()
        => $"{_curr}/{_max}";

    public BoundedInt(int curr, int max)
    {
        _curr = curr;
        _max = max;
    }
}
