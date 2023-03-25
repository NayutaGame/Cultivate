using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;

public class Category<T> where T : Entry
{
    private List<T> _list;

    protected List<T> List
    {
        get => _list;
        set
        {
            _list = value;
            RefreshDict();
        }
    }

    private Dictionary<string, T> _dict;
    private void RefreshDict()
    {
        _dict = new Dictionary<string, T>();

        foreach (T item in _list)
            _dict[item.Name] = item;
    }
    public IEnumerable<T> Traversal
    {
        get
        {
            foreach (var item in _list)
                yield return item;
        }
    }

    public int GetCount() => _list.Count;

    public T this[int i] => _list[i];
    public T this[string key] => _dict[key];

    public int IndexOf(T item)
        => _list.IndexOf(item);
}
