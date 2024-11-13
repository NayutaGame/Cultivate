
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct JingJie : IEquatable<JingJie>
{
    private static JingJie[] _list;

    [SerializeField] private int _index;
    public int Index => _index;
    [SerializeField] public string _name;
    public string Name => _name;

    private JingJie(int index, string name)
    {
        _index = index;
        _name = name;
    }

    static JingJie()
    {
        _list = new JingJie[]
        {
            new(0, "练气"),
            new(1, "筑基"),
            new(2, "金丹"),
            new(3, "元婴"),
            new(4, "化神"),
            new(5, "返虚"),
        };
    }

    private static readonly string[] COLOR_NAMES = new string[] { "灰", "绿", "蓝", "紫", "金", "红" };

    public string GetColorName()
        => COLOR_NAMES[_index];

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
    public static implicit operator CLLibrary.Bound(JingJie jingJie) => new(jingJie._index);

    public override string ToString() => _name;

    public static CLLibrary.Bound LianQi2HuaShen => new(0, 5);
    public static CLLibrary.Bound LianQiOnly => new(0, 1);
    public static CLLibrary.Bound ZhuJi2HuaShen => new(1, 5);
    public static CLLibrary.Bound ZhuJiOnly => new(1, 2);
    public static CLLibrary.Bound JinDan2HuaShen => new(2, 5);
    public static CLLibrary.Bound YuanYing2HuaShen => new(3, 5);
    public static CLLibrary.Bound YuanYingOnly => new(3, 4);
    public static CLLibrary.Bound HuaShenOnly => new(4, 5);
    public static CLLibrary.Bound FanXuOnly => new(5, 6);
}
