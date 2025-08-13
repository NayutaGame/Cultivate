
using System;
using System.Collections.Generic;
using CLLibrary;

public class ArbitraryCardPickerCell : Cell
{
    private string _titleText;
    public string GetTitleText() => _titleText;
    
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private ListModel<SkillEntryDescriptor> _inventory;
    public ListModel<SkillEntryDescriptor> GetInventory() => _inventory;

    private Bound _bound;
    public Bound Bound => _bound;
    public bool HasSpace(int occupied)
        => _bound.End - 1 > occupied;

    private Func<List<SkillEntryDescriptor>, Cell> _confirmOperation;
    public ArbitraryCardPickerCell SetConfirmOperation(Func<List<SkillEntryDescriptor>, Cell> confirmOperation)
    {
        _confirmOperation = confirmOperation;
        return this;
    }

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((ArbitraryCardPickerCell)thisObject).GetGuideDescriptor() },
        { "Inventory",                  thisObject => ((ArbitraryCardPickerCell)thisObject).GetInventory() },
    };
    public override object Get(string s) => Accessor[s](this);
    public ArbitraryCardPickerCell(
        string titleText = null,
        string detailedText = null,
        Bound? bound = null,
        Func<List<SkillEntryDescriptor>, Cell> confirmOperation = null)
    {
        _titleText = titleText ?? "选牌";
        _detailedText = detailedText ?? "请选择卡";
        _bound = bound ?? new Bound(1);
        _confirmOperation = confirmOperation;
        
        _inventory = new ListModel<SkillEntryDescriptor>();
    }

    public void PopulateInventory(List<SkillEntryDescriptor> skills)
    {
        foreach(SkillEntryDescriptor skill in skills)
            _inventory.Add(skill);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ConfirmSkillsSignal selectedSkillsSignal && _confirmOperation != null)
        {
            return _confirmOperation(selectedSkillsSignal.Selected);
        }

        return this;
    }
}
