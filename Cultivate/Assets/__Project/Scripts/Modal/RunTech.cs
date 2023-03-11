using System.Collections;
using System.Collections.Generic;
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
    }

    public string GetName() => _entry.Name;
    public int GetCost() => Mathf.FloorToInt((_hasEureka ? 0f : RunManager.EUREKA_DISCOUNT_RATE) * _entry.Cost);
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
}
