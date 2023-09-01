using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Range = CLLibrary.Range;

public class ArbitraryCardPickerPanelDescriptor : PanelDescriptor
{
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private SkillInventory _inventory;

    private Range _range;
    public Range Range => _range;

    public Action<List<RunSkill>> _action;

    public ArbitraryCardPickerPanelDescriptor(string detailedText = null, SkillInventory inventory = null, Range range = null, Action<List<RunSkill>> action = null)
    {
        _accessors = new()
        {
            { "Inventory",                () => _inventory },
        };
        _detailedText = detailedText ?? "请选择卡";
        _inventory = inventory ?? new SkillInventory();
        _range = range ?? new Range(1);
        _action = action;
    }

    public bool CanSelect(RunSkill skill)
    {
        return true;
    }

    public void ConfirmSelections(List<RunSkill> skills)
    {
        _action?.Invoke(skills);
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal) => null;
}
