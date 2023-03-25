
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using System.Text.RegularExpressions;

public class IndexPath : IEquatable<IndexPath>
{
    private static Regex Pattern = new Regex(@"\w+");

    private string _rawString;
    private readonly List<string> _values;
    public List<string> Values => _values;

    public IndexPath(string rawString)
    {
        _rawString = rawString;
        _values = Pattern.Matches(_rawString).Map(m => m.Value).ToList();
    }

    public bool Equals(IndexPath other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(_values, other._values);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((IndexPath)obj);
    }

    public override int GetHashCode() => _values.GetHashCode();

    public override string ToString() => _rawString;
}
