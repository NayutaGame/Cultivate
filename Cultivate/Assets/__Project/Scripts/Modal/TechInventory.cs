using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class TechInventory : IInventory
{
    private List<RunTech> _list;

    private List<RunTech> _doneList;
    private List<RunTech> _currentList;
    private List<RunTech> _lockedList;

    public int GetCount() => _doneList.Count + _currentList.Count + _lockedList.Count;
    public string GetIndexPathString() => "GetRunTech";

    public RunTech this[int i] => _list[i];

    public TechInventory()
    {
        _list = new();
        _doneList = new();
        _currentList = new();
        _lockedList = new();

        Encyclopedia.TechCategory.Traversal.Do(techEntry =>
        {
            RunTech runTech = new RunTech(techEntry);
            _list.Add(runTech);
            _lockedList.Add(runTech);
        });

        RefreshStates();
    }

    public void RefreshStates()
    {
        List<RunTech> newlyMetList = _lockedList.FilterObj(locked =>
            null == locked.Prerequisites.FirstObj(req =>
                null == _doneList.FirstObj(done => done.Entry == req))).ToList();

        newlyMetList.Do(SetLockToCurrent);
    }

    public void SetDone(RunTech toDone)
    {
        switch (toDone.State)
        {
            case RunTech.RunTechState.Locked:
                _lockedList.Remove(toDone);
                break;
            case RunTech.RunTechState.Current:
                _lockedList.Remove(toDone);
                break;
            case RunTech.RunTechState.Done:
                return;
        }

        _doneList.Add(toDone);
        toDone.State = RunTech.RunTechState.Done;

        RefreshStates();
    }

    private void SetLockToCurrent(RunTech locked)
    {
        _lockedList.Remove(locked);
        _currentList.Add(locked);
        locked.State = RunTech.RunTechState.Current;
    }
}
