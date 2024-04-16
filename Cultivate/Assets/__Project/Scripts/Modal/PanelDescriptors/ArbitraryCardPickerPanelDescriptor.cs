
using System;
using System.Collections.Generic;
using Range = CLLibrary.Range;

public class ArbitraryCardPickerPanelDescriptor : PanelDescriptor
{
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private ListModel<SkillDescriptor> _inventory;
    public ListModel<SkillDescriptor> GetInventory() => _inventory;

    private Range _range;
    public Range Range => _range;

    private Func<List<SkillDescriptor>, PanelDescriptor> _select;
    public ArbitraryCardPickerPanelDescriptor SetSelect(Func<List<SkillDescriptor>, PanelDescriptor> select)
    {
        _select = select;
        return this;
    }

    public ArbitraryCardPickerPanelDescriptor(string detailedText = null, Range range = null, Func<List<SkillDescriptor>, PanelDescriptor> select = null)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Inventory",                GetInventory },
        };
        _detailedText = detailedText ?? "请选择卡";
        _inventory = new ListModel<SkillDescriptor>();
        _range = range ?? new Range(1);
        _select = select;
    }

    public bool CanSelect(SkillDescriptor skill)
    {
        return true;
    }

    public void AddSkills(List<SkillDescriptor> skills)
    {
        foreach(SkillDescriptor skill in skills)
            _inventory.Add(skill);
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
