using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLEventElement
{
    private List<Tuple<int, CLEventListener, CLEventDescriptor>> _list;

    public CLEventElement()
    {
        _list = new List<Tuple<int, CLEventListener, CLEventDescriptor>>();
    }

    public void Add(int priority, CLEventListener listener, CLEventDescriptor eventDescriptor)
        => Add(new(priority, listener, eventDescriptor));
    public void Add(Tuple<int, CLEventListener, CLEventDescriptor> tuple)
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

    public void Remove(CLEventListener listener)
        => _list.RemoveAll(tuple => tuple.Item2 == listener);

    public IEnumerable<Tuple<int, CLEventListener, CLEventDescriptor>> Traversal()
    {
        Tuple<int, CLEventListener, CLEventDescriptor>[] list = new Tuple<int, CLEventListener, CLEventDescriptor>[_list.Count];
        for (int i = 0; i < list.Length; i++) list[i] = _list[i];
        foreach (var tuple in list) yield return tuple;
    }
}
