
using System;
using System.Collections.Generic;

public class StageClosureList
{
    private List<Tuple<StageClosureOwner, StageClosure>> _list;

    public StageClosureList()
    {
        _list = new List<Tuple<StageClosureOwner, StageClosure>>();
    }

    public void Add(StageClosureOwner owner, StageClosure closure)
        => Add(new(owner, closure));
    public void Add(Tuple<StageClosureOwner, StageClosure> tuple)
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

    public void Remove(StageClosureOwner owner)
        => _list.RemoveAll(tuple => tuple.Item1 == owner);

    public IEnumerable<Tuple<StageClosureOwner, StageClosure>> Traversal()
    {
        Tuple<StageClosureOwner, StageClosure>[] list = new Tuple<StageClosureOwner, StageClosure>[_list.Count];
        for (int i = 0; i < list.Length; i++) list[i] = _list[i];
        foreach (var tuple in list) yield return tuple;
    }
}
