
using System.Collections.Generic;

public class GachaPanelDescriptor : PanelDescriptor
{
    private SkillEntryDescriptorListModel _items;
    public SkillEntryDescriptorListModel GetItems() => _items;
    public void SetItems(SkillEntryDescriptorListModel items) => _items = items;

    private int _price;
    public int GetPrice() => _price;

    public GachaPanelDescriptor()
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Items",                    GetItems },
        };
    }

    public bool ItemsIsEmpty
        => _items.Count() <= 0;

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);

        _items = new();
        
        List<SkillEntry> entries1 = RunManager.Instance.Environment.LegacyDrawSkills(new(
            pred: e => e.LowestJingJie <= JingJie.ZhuJi,
            count: 7,
            consume: false));
        
        List<SkillEntry> entries2 = RunManager.Instance.Environment.LegacyDrawSkills(new(
            pred: e => JingJie.JinDan <= e.LowestJingJie && e.LowestJingJie <= JingJie.YuanYing,
            count: 2,
            consume: false));
        
        List<SkillEntry> entries3 = RunManager.Instance.Environment.LegacyDrawSkills(new(
            pred: e => JingJie.HuaShen <= e.LowestJingJie,
            count: 1,
            consume: false));

        foreach (SkillEntry e in entries1)
            _items.Add(SkillEntryDescriptor.FromEntryJingJie(e, e.LowestJingJie));

        foreach (SkillEntry e in entries2)
            _items.Add(SkillEntryDescriptor.FromEntryJingJie(e, e.LowestJingJie));

        foreach (SkillEntry e in entries3)
            _items.Add(SkillEntryDescriptor.FromEntryJingJie(e, e.LowestJingJie));

        _price = 0;

        foreach (var item in _items.Traversal())
            _price += (1 << item.JingJie.Value);

        _price /= 10;
    }

    public bool Buy()
    {
        if (_items.Count() <= 0)
            return false;

        if (RunManager.Instance.Environment.GetGold().Curr < _price)
            return false;

        RunManager.Instance.Environment.SetDGoldProcedure(-_price);

        int gachaIndex = RandomManager.Range(0, _items.Count());
        SkillEntryDescriptor item = _items.Get(gachaIndex) as SkillEntryDescriptor;
        _items.RemoveAt(gachaIndex);

        RunManager.Instance.Environment.GachaProcedure(item, gachaIndex);

        return true;
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is ExitShopSignal)
        {
            return null;
        }

        return this;
    }
}
