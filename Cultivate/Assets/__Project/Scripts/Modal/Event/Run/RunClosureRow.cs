
using System;
using System.Collections.Generic;

public class RunClosureRow
{
    private List<Tuple<RunClosureListener, RunClosure>> _list;

    public RunClosureRow()
    {
        _list = new List<Tuple<RunClosureListener, RunClosure>>();
    }

    public void Add(RunClosureListener listener, RunClosure closure)
        => Add(new(listener, closure));
    public void Add(Tuple<RunClosureListener, RunClosure> tuple)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].Item2.Order > tuple.Item2.Order)
            {
                _list.Insert(i, tuple);
                return;
            }
        }

        _list.Add(tuple);
    }

    public void Remove(RunClosureListener listener)
        => _list.RemoveAll(tuple => tuple.Item1 == listener);

    public IEnumerable<Tuple<RunClosureListener, RunClosure>> Traversal()
    {
        Tuple<RunClosureListener, RunClosure>[] list = new Tuple<RunClosureListener, RunClosure>[_list.Count];
        for (int i = 0; i < list.Length; i++) list[i] = _list[i];
        foreach (var tuple in list) yield return tuple;
    }
}
