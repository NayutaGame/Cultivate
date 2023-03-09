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

    public void MoveToEnd(int i)
    {
        T item = this[i];
        RemoveAt(i);
        Add(item);
    }

    public void Swap(int from, int to)
    {
        (this[from], this[to]) = (this[to], this[from]);
    }

    public int GetCount() => Count;
    public abstract string GetIndexPathString();
}
