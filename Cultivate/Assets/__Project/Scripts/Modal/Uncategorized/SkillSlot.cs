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
            _skill = value?.Clone();
            _state = _skill == null ? SkillSlotState.Empty : SkillSlotState.Occupied;
            EnvironmentChanged();
        }
    }

    [SerializeReference] private MechComposite _mechComposite;

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

        bool success = _skill.TryIncreaseJingJie();
        if (!success)
            return false;

        EnvironmentChanged();
        return true;
    }

    [NonSerialized] public bool RunExhausted;

    public bool TryExhaust()
    {
        if (!RunExhausted)
            return false;

        Skill = null;
        return true;
    }

    [NonSerialized] public bool IsManaShortage;
}
