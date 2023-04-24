using System;
using System.Collections.Generic;

public class FuncQueue<T0>
{
    private List<Tuple<int, Func<T0, T0>>> _list;

    public FuncQueue()
    {
        _list = new List<Tuple<int, Func<T0, T0>>>();
    }

    public void Add(int priority, Func<T0, T0> func)
        => Add(new Tuple<int, Func<T0, T0>>(priority, func));
    public void Add(Tuple<int, Func<T0, T0>> item)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].Item1 > item.Item1)
            {
                _list.Insert(i, item);
                return;
            }
        }

        _list.Add(item);
    }

    public void Remove(Tuple<int, Func<T0, T0>> t) => Remove(t.Item2);
    public void Remove(Func<T0, T0> func)
    {
        _list.RemoveAll(t => t.Item2 == func);
    }

    public T0 Evaluate(T0 val)
    {
        foreach (var t in _list) val = t.Item2(val);

        return val;
    }
}
