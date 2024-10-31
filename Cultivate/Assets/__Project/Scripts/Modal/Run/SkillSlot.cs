
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CLLibrary;
using UnityEngine;

[Serializable]
public class SkillSlot : Addressable, ISerializationCallbackReceiver
{
    [SerializeField] private int _index;
    public int Index => _index;

    [SerializeField] [OptionalField(VersionAdded = 4)] private bool _hidden;
    public bool Hidden
    {
        get => _hidden;
        set
        {
            _hidden = value;
            if (_hidden) Skill = null;
        }
    }

    [SerializeField] [OptionalField(VersionAdded = 4)] private bool _locked;
    public bool Locked
    {
        get => _locked;
        set => _locked = value;
    }

    [SerializeReference] private RunSkill _skill;
    public RunSkill Skill
    {
        get => _skill;
        set
        {
            _skill?.SetSkillSlot(null);
            _skill = value?.Clone();
            _skill?.SetSkillSlot(this);
            EnvironmentChangedNeuron.Invoke();
        }
    }

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public SkillSlot(int index)
    {
        _accessors = new()
        {
            { "Skill",         () => _skill },
        };
        EnvironmentChangedNeuron = new();

        _index = index;
        _hidden = true;
        _locked = false;
    }

    [NonSerialized] public Neuron EnvironmentChangedNeuron;
    [NonSerialized] public PlacedSkill PlacedSkill;
    [NonSerialized] public CastResult CastResult;
    [NonSerialized] public CostResult CostResult;
    
    public bool IsOccupied
        => !_hidden && _skill != null;

    public bool TryIncreaseJingJie(bool loop = true)
    {
        if (_skill == null)
            return false;

        bool success = _skill.TryIncreaseJingJie(loop);
        if (!success)
            return false;

        EnvironmentChangedNeuron.Invoke();
        return true;
    }

    public bool TryDeplete(DepleteDetails d)
    {
        if (Skill == null)
            return false;
        bool depleted = Skill.GetEntry().GetSkillTypeComposite().Contains(SkillType.Deplete);
        if (!depleted)
            return false;

        d.DepletedSkills.Add(Skill);
        Skill = null;
        return true;
    }

    public void ClearResults()
    {
        CastResult = null;
        CostResult = null;
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "Skill",         () => _skill },
        };
        EnvironmentChangedNeuron = new();
    }
    
    public DeckIndex ToDeckIndex()
        => RunManager.Instance.Environment.GetDeckIndexOfSkill(_skill).Value;
    
    #region Deprecated
    
    public enum SkillSlotState { Hidden, Empty, Occupied, }
    [SerializeField] private SkillSlotState _state;
    
    #endregion
}
