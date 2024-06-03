
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

[Serializable]
public class SkillSlot : Addressable, ISerializationCallbackReceiver
{
    public Neuron EnvironmentChangedNeuron;

    [SerializeField] private int _index;
    public int GetIndex() => _index;

    public enum SkillSlotState
    {
        Locked,
        Empty,
        Occupied,
    }

    [SerializeField] private SkillSlotState _state;
    public SkillSlotState State => _state;
    public void SetLocked(bool locked)
    {
        bool stateIsLocked = _state == SkillSlotState.Locked;
        if (stateIsLocked == locked)
            return;

        if (!locked)
        {
            _state = SkillSlotState.Empty;
            return;
        }

        Skill = null;
        _state = SkillSlotState.Locked;
    }

    [SerializeReference] private RunSkill _skill;
    public RunSkill Skill
    {
        get => _skill;
        set
        {
            SetSkillWithoutInvokeChange(value);
            EnvironmentChangedNeuron.Invoke();
        }
    }

    public void SetSkillWithoutInvokeChange(RunSkill skill)
    {
        _skill?.SetSkillSlot(null);
        _skill = skill?.Clone();
        _skill?.SetSkillSlot(this);

        _state = _skill == null ? SkillSlotState.Empty : SkillSlotState.Occupied;
    }

    [NonSerialized] private PlacedSkill _placedSkill;

    public PlacedSkill PlacedSkill
    {
        get => _placedSkill;
        set => _placedSkill = value;
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
        _state = SkillSlotState.Locked;
    }

    public bool TryIncreaseJingJie(bool loop = true)
    {
        if (_skill == null)
            return false;

        RunSkill runSkill = _skill as RunSkill;
        if (runSkill == null)
            return false;

        bool success = runSkill.TryIncreaseJingJie(loop);
        if (!success)
            return false;

        EnvironmentChangedNeuron.Invoke();
        return true;
    }

    public bool TryDeplete(DepleteDetails d)
    {
        if (Skill == null)
            return false;
        bool sunHao = Skill.GetEntry().GetSkillTypeComposite().Contains(SkillType.SunHao);
        if (!sunHao)
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

    [NonSerialized] public CastResult CastResult;
    [NonSerialized] public CostResult CostResult;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "Skill",         () => _skill },
        };
        EnvironmentChangedNeuron = new();
    }
}
