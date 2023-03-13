using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CLLibrary;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class RunTech
{
    private TechEntry _entry;
    public TechEntry Entry => _entry;
    public RunTechState State;
    private bool _hasEureka;
    public bool HasEureka => _hasEureka;

    public RunTech(string entryName) : this(Encyclopedia.TechCategory[entryName]) { }
    public RunTech(TechEntry entry)
    {
        _entry = entry;
        State = RunTechState.Locked;
        _hasEureka = false;

        Register();
    }

    ~RunTech()
    {
        Unregister();
    }

    public string GetName() => _entry.Name;
    public int GetCost() => Mathf.FloorToInt((_hasEureka ? RunManager.EUREKA_DISCOUNT_RATE : 1) * _entry.Cost);
    public string GetRewardString() => "Reward String";
    public string GetEurekaString() => "Eureka String";
    public Vector2Int GetPosition() => _entry.Position;

    public IEnumerable<TechEntry> Prerequisites
    {
        get
        {
            foreach (TechEntry e in _entry.Prerequisites)
                yield return e;
        }
    }

    public enum RunTechState
    {
        Done,
        Current,
        Locked,
    }

    private void Register()
    {
        _entry.Eureka?.Register(this);
    }

    private void Unregister()
    {
        _entry.Eureka?.Unregister(this);
    }

    public void StageCommit(StageCommitDetails d)
    {
        StageCommitEventDescriptor eureka = _entry.Eureka as StageCommitEventDescriptor;

        if (eureka._cond(d, this))
            _hasEureka = true;
    }

    public void Acquire(AcquireDetails d)
    {
        AcquireEventDescriptor eureka = _entry.Eureka as AcquireEventDescriptor;

        if (eureka._cond(d, this))
            _hasEureka = true;
    }

    public void Build(BuildDetails d)
    {
        BuildEventDescriptor eureka = _entry.Eureka as BuildEventDescriptor;

        if (eureka._cond(d, this))
            _hasEureka = true;
    }

    public void PowerChanged(PowerChangedDetails d)
    {
        PowerChangedEventDescriptor eureka = _entry.Eureka as PowerChangedEventDescriptor;

        if (eureka._cond(d, this))
            _hasEureka = true;
    }

    public void StatusChanged(StatusChangedDetails d)
    {
        StatusChangedEventDescriptor eureka = _entry.Eureka as StatusChangedEventDescriptor;

        if (eureka._cond(d, this))
            _hasEureka = true;
    }
}
