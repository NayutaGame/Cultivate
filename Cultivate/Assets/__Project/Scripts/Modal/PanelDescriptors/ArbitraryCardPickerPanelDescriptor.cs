
using System;
using System.Collections.Generic;
using CLLibrary;

public class ArbitraryCardPickerPanelDescriptor : PanelDescriptor
{
    private string _titleText;
    public string GetTitleText() => _titleText;
    
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private ListModel<SkillEntryDescriptor> _inventory;
    public ListModel<SkillEntryDescriptor> GetInventory() => _inventory;

    private SkillEntryDescriptor _descriptor;

    private Bound _bound;
    public Bound Bound => _bound;
    public bool HasSpace(int occupied)
        => _bound.End - 1 > occupied;

    private Func<List<SkillEntryDescriptor>, PanelDescriptor> _confirmOperation;
    public ArbitraryCardPickerPanelDescriptor SetConfirmOperation(Func<List<SkillEntryDescriptor>, PanelDescriptor> confirmOperation)
    {
        _confirmOperation = confirmOperation;
        return this;
    }

    public ArbitraryCardPickerPanelDescriptor(
        string titleText = null,
        string detailedText = null,
        Bound? bound = null,
        SkillEntryDescriptor descriptor = null,
        Func<List<SkillEntryDescriptor>, PanelDescriptor> confirmOperation = null)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Inventory",                GetInventory },
        };

        _titleText = titleText ?? "选牌";
        _detailedText = detailedText ?? "请选择卡";
        _bound = bound ?? new Bound(1);
        _confirmOperation = confirmOperation;
        _descriptor = descriptor;
        
        _inventory = new ListModel<SkillEntryDescriptor>();
    }

    public bool CanSelect(SkillEntryDescriptor skill)
        => _descriptor?.Contains(skill) ?? true;

    public void PopulateInventory(List<SkillEntryDescriptor> skills)
    {
        foreach(SkillEntryDescriptor skill in skills)
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
