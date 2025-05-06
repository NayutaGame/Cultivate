
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CLLibrary;
using UnityEngine;

[Serializable]
public class SkillSlot : Addressable, ISerializationCallbackReceiver
{
    [NonSerialized] public Neuron EnvironmentChangedNeuron = new();
    [NonSerialized] public PlacedSkill PlacedSkill;

    [NonSerialized] public CostDescription ActualCostDescription;
    [NonSerialized] public string ActualDescription;
    
    [SerializeField] private int _index;
    [SerializeField] private bool _hidden;
    [SerializeReference] private RunSkill _skill;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public SkillSlot(int index)
    {
        _accessors = new()
        {
            { "Skill",         () => _skill },
        };

        _index = index;
        _hidden = true;
    }
    
    public int GetIndex() => _index;
    public bool Hidden
    {
        get => _hidden;
        set
        {
            _hidden = value;
            if (_hidden) Skill = null;
        }
    }
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
    
    public bool IsOccupied()
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

    public void ClearResults()
    {
        ActualCostDescription = null;
        ActualDescription = null;
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
        => DeckIndex.FromField(_index);
}
