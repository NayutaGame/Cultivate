
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillSlot : Addressable, ISerializationCallbackReceiver
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

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

    [SerializeReference] private EmulatedSkill _skill;
    public EmulatedSkill Skill
    {
        get => _skill;
        set
        {
            SetSkillWithoutInvokeChange(value);
            EnvironmentChanged();
        }
    }

    public void SetSkillWithoutInvokeChange(EmulatedSkill skill)
    {
        if (_skill != null) _skill.SetSkillSlot(null);

        if (skill is RunSkill runSkill)
            _skill = runSkill.Clone();
        else
            _skill = skill;

        if (_skill != null) _skill.SetSkillSlot(this);

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

    public bool TryDeplete(DepleteDetails d)
    {
        if (Skill == null)
            return false;
        bool sunHao = Skill.GetEntry().SkillTypeComposite.Contains(SkillType.SunHao);
        if (!sunHao)
            return false;

        d.DepletedSkills.Add(Skill);
        Skill = null;
        return true;
    }

    [NonSerialized] public ManaIndicator ManaIndicator;
    [NonSerialized] public bool JiaShiIndicator;
    [NonSerialized] public Dictionary<string, string> Indicator;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "Skill",         () => _skill },
        };
    }
}
