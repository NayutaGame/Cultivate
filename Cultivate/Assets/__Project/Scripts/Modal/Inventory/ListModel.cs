
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class ListModel<T> : IListModel
{
    private List<T> _list;

    public T this[int index] => _list[index];

    public event Func<int, object, Task> InsertEvent;
    public event Func<int, Task> RemoveAtEvent;
    public event Func<int, Task> ModifiedEvent;
    public int Count() => _list.Count;
    public object Get(int index) => _list[index];

    public virtual void Add(T item)
    {
        _list.Add(item);

        if (InsertEvent != null)
            InsertEvent(Count() - 1, item).GetAwaiter().GetResult();
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

    public ListModel()
    {
        _list = new();
    }

    public IEnumerable<T> Traversal()
    {
        foreach (T item in _list)
            yield return item;
    }

    public bool Swap(int from, int to)
    {
        (_list[from], _list[to]) = (_list[to], _list[from]);
        return true;
    }

    public bool Swap(T from, T to) => Swap(_list.IndexOf(from), _list.IndexOf(to));

    public void Replace(T from, T to)
    {
        int index = _list.IndexOf(from);
        _list[index] = to;
        SetModified(index);
    }
}

public interface IListModel
{
    event Func<int, object, Task> InsertEvent;
    event Func<int, Task> RemoveAtEvent;
    event Func<int, Task> ModifiedEvent;
    int Count();
    object Get(int index);
}
