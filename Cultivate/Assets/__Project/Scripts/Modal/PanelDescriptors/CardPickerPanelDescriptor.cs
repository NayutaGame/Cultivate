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

    public bool CanSelect(RunSkill skill)
    {
        return true;
    }

    public bool CanSelect(SkillSlot slot)
        => slot.Skill != null && CanSelect(slot.Skill);

    public void ConfirmSelections(List<object> iRunSkillList)
    {
        _action?.Invoke(iRunSkillList);
        RunManager.Instance.Map.ReceiveSignal(new Signal());
        // ReceiveSignal(new Signal());
    }

    public override void DefaultReceiveSignal(Signal signal)
    {
        base.DefaultReceiveSignal(signal);
        RunManager.Instance.Map.TryFinishNode();
    }
}
