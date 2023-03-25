
using System;
using CLLibrary;
using System.Text.RegularExpressions;

public class IndexPath : IEquatable<IndexPath>
{


    // private static Regex Pattern = new Regex(@"\w+");
    //
    // private string _rawString;
    // private List<string> _values;
    //
    // public NewIndexPath(string rawString)
    // {
    //     _rawString = rawString;
    //     _values = Pattern.Matches(_rawString).Map(m => m.Value).ToList();
    // }


    public readonly string _str;
    public readonly int[] _ints;

    public IndexPath(string str, int[] parentInts, params int[] ints)
    {
        _str = str;
        _ints = new int[parentInts.Length + ints.Length];
        parentInts.Length.Do(i => _ints[i] = parentInts[i]);
        ints.Length.Do(i => _ints[i + parentInts.Length] = ints[i]);
    }

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
