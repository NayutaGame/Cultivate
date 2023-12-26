
using System;
using System.Collections.Generic;

public class RunEventElement
{
    private List<Tuple<int, RunEventListener, RunEventDescriptor>> _list;

    public RunEventElement()
    {
        _list = new List<Tuple<int, RunEventListener, RunEventDescriptor>>();
    }

    public void Add(int priority, RunEventListener listener, RunEventDescriptor eventDescriptor)
        => Add(new(priority, listener, eventDescriptor));
    public void Add(Tuple<int, RunEventListener, RunEventDescriptor> tuple)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].Item1 > tuple.Item1)
            {
                _list.Insert(i, tuple);
                return;
            }
        }

        _list.Add(tuple);
    }

    public void Remove(RunEventListener listener)
        => _list.RemoveAll(tuple => tuple.Item2 == listener);

    public IEnumerable<Tuple<int, RunEventListener, RunEventDescriptor>> Traversal()
    {
        Tuple<int, RunEventListener, RunEventDescriptor>[] list = new Tuple<int, RunEventListener, RunEventDescriptor>[_list.Count];
        for (int i = 0; i < list.Length; i++) list[i] = _list[i];
        foreach (var tuple in list) yield return tuple;
    }
}
