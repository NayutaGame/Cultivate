
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using CLLibrary;
using UnityEngine;

[Serializable]
public class ListModel<T> : IListModel
{
    [SerializeReference] private List<T> _list;

    public T this[int index]
    {
        get => _list[index];
        set
        {
            _list[index] = value;
            SetModified(_list[index]);
        }
    }

    public event Func<int, object, UniTask> InsertEvent;
    public event Func<int, UniTask> RemoveAtEvent;
    public event Func<int, UniTask> ModifiedEvent;
    public event Func<UniTask> ResyncEvent;
    public int Count() => _list.Count;
    public object Get(int index) => _list[index];

    public virtual void Add(T item)
    {
        _list.Add(item);

        if (InsertEvent != null)
            InsertEvent(Count() - 1, item).GetAwaiter().GetResult();
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach(T item in items)
            Add(item);
    }

    public virtual void Insert(int index, T item)
    {
        _list.Insert(index, item);

        if (InsertEvent != null)
            InsertEvent(index, item).GetAwaiter().GetResult();
    }

    public virtual void Remove(T item)
    {
        RemoveAt(_list.IndexOf(item));
    }

    public virtual void RemoveAt(int index)
    {
        _list.RemoveAt(index);

        if (RemoveAtEvent != null)
            RemoveAtEvent(index).GetAwaiter().GetResult();
    }

    public void Clear()
    {
        while (Count() > 0)
        {
            RemoveAt(0);
        }
    }

    public virtual void SetModified(T item)
    {
        SetModified(_list.IndexOf(item));
    }

    public void SetModified(int index)
    {
        if (ModifiedEvent != null)
            ModifiedEvent(index).GetAwaiter().GetResult();
    }

    public void Sort(Comparison<T> comparison)
        => _list.Sort(comparison);

    public bool Contains(T item)
        => _list.Contains(item);

    public List<T> Filter(Predicate<T> filter)
        => _list.FilterObj(filter).ToList();

    public int CountSuch(Func<T, bool> filter)
        => _list.Count(filter);

    public ListModel()
    {
        _list = new();
    }

    public ListModel(T[] initialItems)
    {
        _list = new List<T>(initialItems);
    }

    public IEnumerable<T> Traversal()
    {
        foreach (T item in _list)
            yield return item;
    }

    public T First(Predicate<T> pred)
        => _list.FirstObj(pred);

    public bool Swap(int from, int to)
    {
        (_list[from], _list[to]) = (_list[to], _list[from]);
        return true;
    }

    public bool Swap(T from, T to) => Swap(_list.IndexOf(from), _list.IndexOf(to));

    public void Replace(T from, T to)
        => Replace(_list.IndexOf(from), to);
    public void Replace(int from, T to)
    {
        _list[from] = to;
        SetModified(from);
    }

    public int IndexOf(T item)
        => _list.IndexOf(item);
}

public interface IListModel
{
    event Func<int, object, UniTask> InsertEvent;
    event Func<int, UniTask> RemoveAtEvent;
    event Func<int, UniTask> ModifiedEvent;
    event Func<UniTask> ResyncEvent;
    int Count();
    object Get(int index);
}
