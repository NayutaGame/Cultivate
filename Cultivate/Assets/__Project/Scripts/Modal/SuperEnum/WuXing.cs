
using System.Collections.Generic;
using System;

public struct WuXing : IEquatable<WuXing>
{
    private static WuXing[] _list;

    public readonly int _index;
    public readonly string _name;
    public readonly string _elementaryBuff;

    private WuXing(int index, string name, string elementaryBuff)
    {
        _index = index;
        _name = name;
        _elementaryBuff = elementaryBuff;
    }

    static WuXing()
    {
        _list = new WuXing[]
        {
            new(0, "金", "锋锐"),
            new(1, "水", "格挡"),
            new(2, "木", "力量"),
            new(3, "火", "灼烧"),
            new(4, "土", "坚毅"),
        };
    }

    public static int Length => _list.Length;

    public static WuXing Jin => _list[0];
    public static WuXing Shui => _list[1];
    public static WuXing Mu => _list[2];
    public static WuXing Huo => _list[3];
    public static WuXing Tu => _list[4];

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

    public static bool SameWuXing(WuXing? i0, WuXing? i1)
        => i0 != null && i1 != null && i0 == i1;

    public static bool XiangSheng(WuXing? i0, WuXing? i1)
        => i0 != null && i1 != null && (i0.Value.Next == i1 || i0.Value.Prev == i1);

    public static WuXing? XiangShengNext(WuXing? i0, WuXing? i1)
    {
        if (i0 == null || i1 == null)
            return null;
        if (i0.Value.Next == i1)
            return i1.Value.Next;
        if (i1.Value.Next == i0)
            return i0.Value.Next;
        return null;
    }
}
