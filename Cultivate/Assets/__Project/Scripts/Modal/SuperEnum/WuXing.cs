using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct WuXing : IEquatable<WuXing>
{
    private static WuXing[] _list;

    public readonly int _index;
    public readonly string _name;

    private WuXing(int index, string name)
    {
        _index = index;
        _name = name;
    }

    static WuXing()
    {
        _list = new WuXing[]
        {
            new WuXing(0, "金"),
            new WuXing(1, "水"),
            new WuXing(2, "木"),
            new WuXing(3, "火"),
            new WuXing(4, "土"),
        };
    }

    public static int Length => _list.Length;

    public static WuXing Jin => _list[0];
    public static WuXing Shui  => _list[1];
    public static WuXing Mu  => _list[2];
    public static WuXing Huo => _list[3];
    public static WuXing Tu  => _list[4];

    public WuXing Shift(int i)
    {
        return _list[(_index + i) % _list.Length];
    }

    public WuXing Next => Shift(1);
    public WuXing Prev => Shift(4);

    public static IEnumerable<WuXing> Traversal
    {
        get
        {
            foreach (var item in _list)
                yield return item;
        }
    }

    public static bool operator ==(WuXing i0, WuXing i1) => i0.Equals(i1);
    public static bool operator !=(WuXing i0, WuXing i1) => !i0.Equals(i1);

    public bool Equals(WuXing other) => _index == other._index;
    public override bool Equals(object obj) => obj is WuXing other && Equals(other);
    public override int GetHashCode() => _index;

    public static implicit operator int(WuXing wuXing) => wuXing._index;
    public static implicit operator WuXing(int index) => _list[index];

    public override string ToString() => _name;
}
