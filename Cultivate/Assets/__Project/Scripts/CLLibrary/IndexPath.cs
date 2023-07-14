
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using System.Text.RegularExpressions;

public class IndexPath : IEquatable<IndexPath>
{
    private static Regex Pattern = new Regex(@"\w+");

    private string _rawString;
    private readonly List<CLKey> _values;
    public List<CLKey> Values => _values;

    public IndexPath(string rawString)
    {
        _rawString = rawString;
        List<string> rawValues = Pattern.Matches(_rawString).Map(m => m.Value).ToList();
        _values = rawValues.Map<string, CLKey>(s =>
        {
            if (int.TryParse(s, out int i))
                return new IntKey(i);

            return new StringKey(s);
        }).ToList();
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
