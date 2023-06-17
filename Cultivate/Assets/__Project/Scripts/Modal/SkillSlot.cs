using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using Unity.VisualScripting;
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
        if (_state == SkillSlotState.Locked)
        {
            if (locked)
                return;

            _state = SkillSlotState.Empty;
            return;
        }

        if (!locked)
            return;

        Skill = null;
        _state = SkillSlotState.Locked;
    }

    [SerializeReference] private RunSkill _skill;
    public RunSkill Skill
    {
        get => _skill;
        set
        {
            _skill = value?.Clone();
            _state = _skill == null ? SkillSlotState.Empty : SkillSlotState.Occupied;
            EnvironmentChanged();
        }
    }

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
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

        bool success = _skill.TryIncreaseJingJie();
        if (!success)
            return false;

        EnvironmentChanged();
        return true;
    }

    [NonSerialized] public bool RunConsumed;

    public bool TryConsume()
    {
        if (!RunConsumed)
            return false;

        Skill = null;
        return true;
    }

    [NonSerialized] public bool IsManaShortage;
}
