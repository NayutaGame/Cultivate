
using System;
using System.Collections.Generic;
using Range = CLLibrary.Range;

public class ArbitraryCardPickerPanelDescriptor : PanelDescriptor
{
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private SkillInventory _inventory;

    private Range _range;
    public Range Range => _range;

    private Func<List<RunSkill>, PanelDescriptor> _select;
    public ArbitraryCardPickerPanelDescriptor SetSelect(Func<List<RunSkill>, PanelDescriptor> select)
    {
        _select = select;
        return this;
    }

    public ArbitraryCardPickerPanelDescriptor(string detailedText = null, Range range = null, Func<List<RunSkill>, PanelDescriptor> select = null)
    {
        _accessors = new()
        {
            { "Inventory",                () => _inventory },
        };
        _detailedText = detailedText ?? "请选择卡";
        _inventory = new SkillInventory();
        _range = range ?? new Range(1);
        _select = select;
    }

    public bool CanSelect(RunSkill skill)
    {
        return true;
    }

    public void AddSkills(List<RunSkill> skills)
    {
        _inventory.AddSkills(skills);
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is SelectedSkillsSignal selectedSkillsSignal && _select != null)
        {
            return _select(selectedSkillsSignal.Selected);
        }

        return null;
    }
}
