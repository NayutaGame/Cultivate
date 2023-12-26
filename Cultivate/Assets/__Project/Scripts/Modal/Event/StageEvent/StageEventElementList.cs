
using System;
using System.Collections.Generic;

public class StageEventElementList
{
    private List<Tuple<StageEventListener, StageEventDescriptor>> _list;

    public StageEventElementList()
    {
        _list = new List<Tuple<StageEventListener, StageEventDescriptor>>();
    }

    public void Add(StageEventListener listener, StageEventDescriptor eventDescriptor)
        => Add(new(listener, eventDescriptor));
    public void Add(Tuple<StageEventListener, StageEventDescriptor> tuple)
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

    public void Remove(StageEventListener listener)
        => _list.RemoveAll(tuple => tuple.Item1 == listener);

    public IEnumerable<Tuple<StageEventListener, StageEventDescriptor>> Traversal()
    {
        Tuple<StageEventListener, StageEventDescriptor>[] list = new Tuple<StageEventListener, StageEventDescriptor>[_list.Count];
        for (int i = 0; i < list.Length; i++) list[i] = _list[i];
        foreach (var tuple in list) yield return tuple;
    }
}
