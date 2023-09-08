
using System;
using System.Collections.Generic;

public abstract class ListModel<T> : List<T>
{
    public bool Swap(int from, int to)
    {
        (this[from], this[to]) = (this[to], this[from]);
        return true;
    }

    public bool Swap(T from, T to) => Swap(IndexOf(from), IndexOf(to));

    public void Replace(T from, T to)
    {
        this[IndexOf(from)] = to;
    }
}
