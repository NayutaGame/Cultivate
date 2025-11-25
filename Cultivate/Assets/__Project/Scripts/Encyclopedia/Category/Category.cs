
using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;

public abstract class Category<T> : ICategory<T>, Addressable where T : Entry
{
    private ListModel<T> _list;
    private Dictionary<string, T> _dict;
    private Dictionary<string, T> _nameDict;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "List",                       thisObject => ((Category<T>)thisObject)._list },
        { "Dict",                       thisObject => ((Category<T>)thisObject)._dict },
    };
    public object Get(string s) => Accessor[s](this);
    public Category()
    {
        _list = new();
        _dict = new();
        _nameDict = new();
    }
    
    public ListModel<T> List
        => _list;

    public T this[int index] => _list[index];

    public void AddRange(IEnumerable<T> collection)
    {
        List.AddRange(collection);
        RefreshDict();
    }

    public void Add(T item)
    {
        List.Add(item);
    }

    public void RefreshDict()
    {
        _dict.Clear();

        foreach (T item in _list)
            _dict[item.GetId()] = item;
        
        _nameDict.Clear();

        foreach (T item in _list)
            _nameDict[item.GetName()] = item;
    }

    public int Count() => _list.Count();
    public int IndexOf(T item) => _list.IndexOf(item);
    
    public IEnumerator<T> GetEnumerator()
        => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public bool ContainsId(string id)
        => _dict.ContainsKey(id);
    
    public T FromId(string id)
    {
        bool success = _dict.TryGetValue(id, out T value);
        return success ? value : null;
    }

    public bool ContainsName(string name)
        => _nameDict.ContainsKey(name);

    public T FromName(string name)
    {
        bool success = _nameDict.TryGetValue(name, out T value);
        return success ? value : null;
    }

    public virtual void Init()
    {
        List.Do(entry =>
        {
            entry.Init();
        });
    }
}
