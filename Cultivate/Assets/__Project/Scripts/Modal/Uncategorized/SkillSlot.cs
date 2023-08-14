
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillSlot : GDictionary
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

    [SerializeReference] private RunEntity _owner;
    public RunEntity Owner => _owner;
    [SerializeField] private int _index;

    public enum SkillSlotState
    {
        Locked,
        Empty,
        Occupied,
    }

    private SkillSlotState _state;
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

    [SerializeReference] private EmulatedSkill _skill;
    public EmulatedSkill Skill
    {
        get => _skill;
        set
        {
            if (_skill != null) _skill.SetSkillSlot(null);

            if (value is RunSkill runSkill)
                _skill = runSkill.Clone();
            else
                _skill = value;

            if (_skill != null) _skill.SetSkillSlot(this);

            _state = _skill == null ? SkillSlotState.Empty : SkillSlotState.Occupied;
            EnvironmentChanged();
        }
    }

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public SkillSlot(RunEntity owner, int index)
    {
        _accessors = new()
        {
            { "Skill",         () => _skill },
        };

        _owner = owner;
        _index = index;
        _state = SkillSlotState.Locked;
    }

    public bool TryIncreaseJingJie()
    {
        if (_skill == null)
            return false;

        RunSkill runSkill = _skill as RunSkill;
        if (runSkill == null)
            return false;

        bool success = runSkill.TryIncreaseJingJie();
        if (!success)
            return false;

        EnvironmentChanged();
        return true;
    }

    public bool TryExhaust()
    {
        bool sunHao = Skill.GetEntry().SkillTypeComposite.Contains(SkillType.SunHao);
        if (!sunHao)
            return false;

        Skill = null;
        return true;
    }

    [NonSerialized] public bool IsManaShortage;
    [NonSerialized] public bool IsJiaShi;
}
