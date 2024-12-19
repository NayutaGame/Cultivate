
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
    [NonSerialized] public CastResult CastResult;
    [NonSerialized] public CostResult CostResult;
    
    [SerializeField] private int _index;
    [SerializeField] [OptionalField(VersionAdded = 4)] private bool _hidden;
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
        => DeckIndex.FromField(_index);
    
    #region Obsolete
    
    #endregion
}
