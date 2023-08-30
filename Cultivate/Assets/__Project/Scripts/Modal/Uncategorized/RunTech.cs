using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class RunTech
{
    private TechEntry _entry;
    public TechEntry Entry => _entry;
    private RunTechState _state;

    public RunTechState State
    {
        get => _state;
        set
        {
            _state = value;
            if (_state == RunTechState.Done)
            {
                Unregister();
                _entry.Claim();
            }
        }
    }
    private bool _hasEureka;
    public bool HasEureka => _hasEureka;

    public RunTech(string entryName) : this(Encyclopedia.TechCategory[entryName]) { }
    public RunTech(TechEntry entry)
    {
        _entry = entry;
        _state = RunTechState.Locked;
        _hasEureka = false;

        Register();
    }

    ~RunTech()
    {
        Unregister();
    }

    public string GetName() => _entry.Name;
    public int GetCost() => Mathf.FloorToInt((_hasEureka ? RunManager.EUREKA_DISCOUNT_RATE : 1) * _entry.Cost);
    public string GetRewardsString() => _entry.GetRewardsString();
    public string GetEurekaString() => _entry.GetEurekaString();
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
        StageCommitRunEventDescriptor eureka = _entry.Eureka as StageCommitRunEventDescriptor;
        if (!eureka._cond(d, this)) return;

        _hasEureka = true;
        Unregister();
    }

    public void GainSkill(GainSkillDetails d)
    {
        GainSkillRunEventDescriptor eureka = _entry.Eureka as GainSkillRunEventDescriptor;
        if (!eureka._cond(d, this)) return;

        _hasEureka = true;
        Unregister();
    }

    public void StatusChanged(StatusChangedDetails d)
    {
        StatusChangedRunEventDescriptor eureka = _entry.Eureka as StatusChangedRunEventDescriptor;
        if (!eureka._cond(d, this)) return;

        _hasEureka = true;
        Unregister();
    }
}
