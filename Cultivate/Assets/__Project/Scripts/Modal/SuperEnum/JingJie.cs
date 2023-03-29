using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct JingJie : IEquatable<JingJie>
{
    private static JingJie[] _list;

    public readonly int _index;
    public readonly string _name;

    private JingJie(int index, string name)
    {
        _index = index;
        _name = name;
    }

    static JingJie()
    {
        _list = new JingJie[]
        {
            new JingJie(0, "练气"),
            new JingJie(1, "筑基"),
            new JingJie(2, "金丹"),
            new JingJie(3, "元婴"),
            new JingJie(4, "化神"),
            new JingJie(5, "返虚"),
        };
    }

    public static int Length => _list.Length;

    public static JingJie LianQi => _list[0];
    public static JingJie ZhuJi  => _list[1];
    public static JingJie JinDan  => _list[2];
    public static JingJie YuanYing => _list[3];
    public static JingJie HuaShen  => _list[4];
    public static JingJie FanXu => _list[5];

    public static IEnumerable<JingJie> Traversal
    {
        get
        {
            foreach (var item in _list)
                yield return item;
        }
    }

    public static bool operator ==(JingJie i0, JingJie i1) => i0.Equals(i1);
    public static bool operator !=(JingJie i0, JingJie i1) => !i0.Equals(i1);

    public bool Equals(JingJie other) => _index == other._index;
    public override bool Equals(object obj) => obj is JingJie other && Equals(other);
    public override int GetHashCode() => _index;

    public static implicit operator int(JingJie jingJie) => jingJie._index;
    public static implicit operator JingJie(int index) => _list[index];
    public static implicit operator CLLibrary.Range(JingJie jingJie) => new(jingJie._index);

    public override string ToString() => _name;
}
