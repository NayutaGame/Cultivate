
using System;
using CLLibrary;
using UnityEngine;

// TODO, to struct, problem, MingYuan inherits, struct cannot have inheritance
[Serializable]
public class BoundedInt
{
    [SerializeField] private int _curr;
    [SerializeField] private int _max;

    public int Curr
    {
        get => _curr;
        set => _curr = value.ClampUpper(_max);
    }

    public int UpperBound
    {
        get => _max;
        set
        {
            _max = value;
            Curr = _curr;
        }
    }

    public override string ToString()
        => _max == int.MaxValue ? $"{_curr}" : $"{_curr}/{_max}";

    public BoundedInt Clone()
        => new(_curr, _max);

    public BoundedInt(int curr, int max = int.MaxValue)
    {
        _curr = curr;
        _max = max;
    }
}
