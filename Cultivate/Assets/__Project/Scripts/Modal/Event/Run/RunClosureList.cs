
using System;
using System.Collections.Generic;

public class RunClosureList
{
    private List<Tuple<RunClosureOwner, RunClosure>> _list;

    public RunClosureList()
    {
        _list = new List<Tuple<RunClosureOwner, RunClosure>>();
    }

    public void Add(RunClosureOwner listener, RunClosure closure)
        => Add(new(listener, closure));
    public void Add(Tuple<RunClosureOwner, RunClosure> tuple)
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

    public void Remove(RunClosureOwner listener)
        => _list.RemoveAll(tuple => tuple.Item1 == listener);

    public IEnumerable<Tuple<RunClosureOwner, RunClosure>> Traversal()
    {
        Tuple<RunClosureOwner, RunClosure>[] list = new Tuple<RunClosureOwner, RunClosure>[_list.Count];
        for (int i = 0; i < list.Length; i++) list[i] = _list[i];
        foreach (var tuple in list) yield return tuple;
    }
}
