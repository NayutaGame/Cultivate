using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct MechType : IEquatable<MechType>
{
    private static MechType[] _list;

    public readonly int _index;
    public readonly string _name;

    private MechType(int index, string name)
    {
        _index = index;
        _name = name;
    }

    static MechType()
    {
        _list = new MechType[]
        {
            new(0, "香"),
            new(1, "刃"),
            new(2, "匣"),
            new(3, "轮"),
        };
    }

    public static int Length => _list.Length;

    public static MechType Xiang => _list[0];
    public static MechType Ren => _list[1];
    public static MechType Xia => _list[2];
    public static MechType Lun => _list[3];

    public static IEnumerable<MechType> Traversal
    {
        get
        {
            foreach (var item in _list)
                yield return item;
        }
    }

    public static bool operator ==(MechType i0, MechType i1) => i0.Equals(i1);
    public static bool operator !=(MechType i0, MechType i1) => !i0.Equals(i1);

    public bool Equals(MechType other) => _index == other._index;
    public override bool Equals(object obj) => obj is MechType other && Equals(other);
    public override int GetHashCode() => _index;

    public static implicit operator int(MechType wuXing) => wuXing._index;
    public static implicit operator MechType(int index) => _list[index];

    public override string ToString() => _name;
}
