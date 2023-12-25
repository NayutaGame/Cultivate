using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoundedInt
{
    [SerializeField] private int _curr;
    [SerializeField] private int _max;

    public int GetCurr() => _curr;
    public void SetCurr(int value) => _curr = Mathf.Min(_max, value);
    public void SetDiff(int value = 1) => SetCurr(_curr + value);

    public int GetMax() => _max;
    public void SetMax(int value) => _max = value;
    public void SetDMax(int value) => SetMax(_max + value);

    public override string ToString()
        => $"{_curr}/{_max}";

    public BoundedInt(int curr, int max)
    {
        _curr = curr;
        _max = max;
    }
}
