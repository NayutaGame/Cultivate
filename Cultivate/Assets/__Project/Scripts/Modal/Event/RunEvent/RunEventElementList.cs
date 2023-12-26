
using System;
using System.Collections.Generic;

public class RunEventElementList
{
    private List<Tuple<RunEventListener, RunEventDescriptor>> _list;

    public RunEventElementList()
    {
        _list = new List<Tuple<RunEventListener, RunEventDescriptor>>();
    }

    public void Add(RunEventListener listener, RunEventDescriptor eventDescriptor)
        => Add(new(listener, eventDescriptor));
    public void Add(Tuple<RunEventListener, RunEventDescriptor> tuple)
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

    public void Remove(RunEventListener listener)
        => _list.RemoveAll(tuple => tuple.Item1 == listener);

    public IEnumerable<Tuple<RunEventListener, RunEventDescriptor>> Traversal()
    {
        Tuple<RunEventListener, RunEventDescriptor>[] list = new Tuple<RunEventListener, RunEventDescriptor>[_list.Count];
        for (int i = 0; i < list.Length; i++) list[i] = _list[i];
        foreach (var tuple in list) yield return tuple;
    }
}
