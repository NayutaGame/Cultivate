using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Range = CLLibrary.Range;

public class CardPickerPanelDescriptor : PanelDescriptor
{
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private Range _range;
    public Range Range => _range;

    public Action<List<object>> _action;

    public CardPickerPanelDescriptor(string detailedText = null, Range range = null, Action<List<object>> action = null)
    {
        _detailedText = detailedText ?? "请选择卡";
        _range = range ?? new Range(1);
        _action = action;
    }

    public bool CanSelect(EmulatedSkill skill)
    {
        return skill is RunSkill;
    }

    public bool CanSelect(SkillSlot slot)
        => slot.Skill != null && CanSelect(slot.Skill);

    public void ConfirmSelections(List<object> iRunSkillList)
    {
        _action?.Invoke(iRunSkillList);
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        RunManager.Instance.Map.TryFinishNode();
        return null;
    }
}
