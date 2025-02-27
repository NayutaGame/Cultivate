
using System;
using System.Collections.Generic;

public class StageClosureRow
{
    private List<Tuple<StageClosureListener, StageClosure>> _list;

    public StageClosureRow()
    {
        _list = new List<Tuple<StageClosureListener, StageClosure>>();
    }

    public void Add(StageClosureListener listener, StageClosure closure)
        => Add(new(listener, closure));
    public void Add(Tuple<StageClosureListener, StageClosure> tuple)
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

    public void Remove(StageClosureListener listener)
        => _list.RemoveAll(tuple => tuple.Item1 == listener);

    public IEnumerable<Tuple<StageClosureListener, StageClosure>> Traversal()
    {
        Tuple<StageClosureListener, StageClosure>[] list = new Tuple<StageClosureListener, StageClosure>[_list.Count];
        for (int i = 0; i < list.Length; i++) list[i] = _list[i];
        foreach (var tuple in list) yield return tuple;
    }
}
