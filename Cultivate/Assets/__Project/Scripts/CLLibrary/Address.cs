
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using System.Text.RegularExpressions;
using UnityEngine;

public class Address : IEquatable<Address>
{
    private static Regex Pattern = new Regex(@"\w+");

    private static Dictionary<string, Func<object>> _root = new();

    private string _rawString;
    private readonly List<CLKey> _values;
    public List<CLKey> Values => _values;

    public Address(string rawString)
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

    public static void AddToRoot(string key, Func<object> value)
    {
        _root[key] = value;
    }

    public T Get<T>()
    {
        object curr = _root;
        foreach (var clKey in Values)
        {
            if (clKey is IntKey { Key: int i })
            {
                if (curr is IList l)
                {
                    if (l.Count <= i)
                        return default;
                    curr = l[i];
                }
                else if (curr is IListModel listModel)
                {
                    if (listModel.Count() <= i)
                        return default;
                    curr = listModel.Get(i);
                }
                else
                {
                    Debug.Log($"{this} @ {i}");
                }
            }
            else if (clKey is StringKey { Key: string s })
            {
                if (curr is Addressable addressable)
                {
                    curr = addressable.Get(s);
                }
                else if (curr == _root)
                {
                    curr = _root[s]();
                }
                else
                {
                    Debug.Log($"{this} @ {s}");
                }
            }
        }

        if (curr is T ret)
            return ret;
        else
            return default;
    }

    public Address Append(string s)
    {
        return new Address($"{this}{s}");
    }

    public bool Equals(Address other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(_rawString, other._rawString);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Address)obj);
    }

    public override int GetHashCode() => _values.GetHashCode();

    public override string ToString() => _rawString;

    public static implicit operator Address(string address) => new(address);
}
