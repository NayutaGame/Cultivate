
using System;
using System.Collections;
using System.Collections.Generic;

public class Category<T> : IEnumerable<T>, Addressable where T : Entry
{
    private ListModel<T> _list;
    protected ListModel<T> List => _list;

    private Dictionary<string, T> _dict;

    public void AddRange(IEnumerable<T> collection)
    {
        List.AddRange(collection);
        RefreshDict();
    }

    public void Add(T item)
    {
        List.Add(item);
    }

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "List",                       thisObject => ((Category<T>)thisObject)._list },
    };
    public object Get(string s) => Accessor[s](this);
    public Category()
    {
        _list = new();
        _dict = new();
    }

    public void RefreshDict()
    {
        _dict.Clear();

        foreach (T item in _list)
            _dict[item.GetId()] = item;
    }

    public int Count() => _list.Count();

    public T this[int i] => _list[i];
    public T this[string key]
    {
        get
        {
            bool success = _dict.TryGetValue(key, out T value);
            if (success)
                return value;
            else
                return DefaultEntry();
        }
    }

    public bool ContainsKey(string key)
        => _dict.ContainsKey(key);

    public virtual T DefaultEntry() => null;

    public int IndexOf(T item)
        => _list.IndexOf(item);
    
    public IEnumerator<T> GetEnumerator()
        => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
