using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory<T> : List<T>, IInventory
{
    public T TryGet(int i)
    {
        if (i >= Count)
            return default;
        return this[i];
    }

    public bool MoveToEnd(int i)
    {
        T item = this[i];
        RemoveAt(i);
        Add(item);
        return true;
    }

    public bool MoveToEnd(T item)
    {
        Remove(item);
        Add(item);
        return true;
    }

    public bool Swap(int from, int to)
    {
        (this[from], this[to]) = (this[to], this[from]);
        return true;
    }

    public bool Swap(T from, T to) => Swap(IndexOf(from), IndexOf(to));

    public int GetCount() => Count;
    public abstract string GetIndexPathString();
}
