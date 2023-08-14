
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class SuperEnum<T> : IEquatable<SuperEnum<T>>
    where T : SuperEnum<T>
{
    protected static T[] _list;

    [SerializeField]
    public readonly int _index;
    [SerializeField]
    public readonly string _name;

    protected SuperEnum(int index, string name)
    {
        _index = index;
        _name = name;
    }

    public static int Length => _list.Length;

    public static IEnumerable<T> Traversal
    {
        get
        {
            foreach (var item in _list)
                yield return item;
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_index, _name);
    }

    public override string ToString() => _name;

    public bool Equals(SuperEnum<T> other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _index == other._index && _name == other._name;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SuperEnum<T>)obj);
    }
}
