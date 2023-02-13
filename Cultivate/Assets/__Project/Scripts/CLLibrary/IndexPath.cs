
using System;

public class IndexPath : IEquatable<IndexPath>
{
    public readonly string _str;
    public readonly int[] _ints;

    public IndexPath(string str, params int[] ints)
    {
        _str = str;
        _ints = ints;
    }

    public bool Equals(IndexPath other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _str == other._str && Equals(_ints, other._ints);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((IndexPath)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_str, _ints);
    }
}
