using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FuncQueue<T>
{
    private List<Tuple<int, Func<T, Task<T>>>> _list;

    public FuncQueue()
    {
        _list = new List<Tuple<int, Func<T, Task<T>>>>();
    }

    public void Add(int priority, Func<T, Task<T>> func)
        => Add(new Tuple<int, Func<T, Task<T>>>(priority, func));
    public void Add(Tuple<int, Func<T, Task<T>>> item)
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

    public void Remove(Tuple<int, Func<T, Task<T>>> t) => Remove(t.Item2);
    public void Remove(Func<T, Task<T>> func)
    {
        _list.RemoveAll(t => t.Item2 == func);
    }

    public async Task<T> Evaluate(T val)
    {
        foreach (var t in _list) val = await t.Item2(val);

        return val;
    }
}
