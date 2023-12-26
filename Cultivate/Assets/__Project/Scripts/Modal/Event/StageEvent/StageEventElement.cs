
using System;
using System.Collections.Generic;

public class StageEventElement
{
    private List<Tuple<int, StageEventListener, StageEventDescriptor>> _list;

    public StageEventElement()
    {
        _list = new List<Tuple<int, StageEventListener, StageEventDescriptor>>();
    }

    public void Add(int priority, StageEventListener listener, StageEventDescriptor eventDescriptor)
        => Add(new(priority, listener, eventDescriptor));
    public void Add(Tuple<int, StageEventListener, StageEventDescriptor> tuple)
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

    public void Remove(StageEventListener listener)
        => _list.RemoveAll(tuple => tuple.Item2 == listener);

    public IEnumerable<Tuple<int, StageEventListener, StageEventDescriptor>> Traversal()
    {
        Tuple<int, StageEventListener, StageEventDescriptor>[] list = new Tuple<int, StageEventListener, StageEventDescriptor>[_list.Count];
        for (int i = 0; i < list.Length; i++) list[i] = _list[i];
        foreach (var tuple in list) yield return tuple;
    }
}
