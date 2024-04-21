
using System;
using System.Collections.Generic;
using Range = CLLibrary.Range;

public class ArbitraryCardPickerPanelDescriptor : PanelDescriptor
{
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private ListModel<SkillDescriptor> _inventory;
    public ListModel<SkillDescriptor> GetInventory() => _inventory;

    private SkillDescriptor _descriptor;

    private Range _range;
    public Range Range => _range;
    public bool HasSpace(int occupied)
        => _range.End - 1 > occupied;

    private Func<List<SkillDescriptor>, PanelDescriptor> _confirmOperation;
    public ArbitraryCardPickerPanelDescriptor SetConfirmOperation(Func<List<SkillDescriptor>, PanelDescriptor> confirmOperation)
    {
        _confirmOperation = confirmOperation;
        return this;
    }

    public ArbitraryCardPickerPanelDescriptor(
        string detailedText = null,
        Range range = null,
        SkillDescriptor descriptor = null,
        Func<List<SkillDescriptor>, PanelDescriptor> confirmOperation = null)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Inventory",                GetInventory },
        };
        
        _detailedText = detailedText ?? "请选择卡";
        _range = range ?? new Range(1);
        _confirmOperation = confirmOperation;
        _descriptor = descriptor;
        
        _inventory = new ListModel<SkillDescriptor>();
    }

    public bool CanSelect(SkillDescriptor skill)
        => _descriptor?.Contains(skill) ?? true;

    public void PopulateInventory(List<SkillDescriptor> skills)
    {
        foreach(SkillDescriptor skill in skills)
            _inventory.Add(skill);
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is ConfirmSkillsSignal selectedSkillsSignal && _confirmOperation != null)
        {
            return _confirmOperation(selectedSkillsSignal.Selected);
        }

        return this;
    }
}
