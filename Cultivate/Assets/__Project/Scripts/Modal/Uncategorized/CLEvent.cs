using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CLEvent<T>
{
    private List<Tuple<int, Func<T, Task>>> _list;

    public CLEvent()
    {
        _list = new List<Tuple<int, Func<T, Task>>>();
    }

    public void Add(int priority, Func<T, Task> callback)
        => Add(new Tuple<int, Func<T, Task>>(priority, callback));
    public void Add(Tuple<int, Func<T, Task>> callback)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].Item1 > callback.Item1)
            {
                _list.Insert(i, callback);
                return;
            }
        }

        _list.Add(callback);
    }

    public void Remove(Tuple<int, Func<T, Task>> t) => Remove(t.Item2);
    public void Remove(Func<T, Task> func)
    {
        _list.RemoveAll(t => t.Item2 == func);
    }

    public IEnumerable<Func<T, Task>> Traversal()
    {
        Tuple<int, Func<T, Task>>[] list = new Tuple<int, Func<T, Task>>[_list.Count];
        for (int i = 0; i < list.Length; i++)
            list[i] = _list[i];
        foreach (var t in list) yield return t.Item2;
    }
}
