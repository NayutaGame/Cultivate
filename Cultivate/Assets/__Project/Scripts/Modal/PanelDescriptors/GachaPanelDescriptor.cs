
using System.Collections.Generic;
using CLLibrary;

public class GachaPanelDescriptor : PanelDescriptor
{
    private SkillEntryDescriptorListModel _items;
    public SkillEntryDescriptorListModel GetItems() => _items;
    public void SetItems(SkillEntryDescriptorListModel items) => _items = items;

    private int _price;
    public int GetPrice() => _price;

    private float _priceMultiplier;

    public GachaPanelDescriptor(float priceMultiplier)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Items",                    GetItems },
        };

        _priceMultiplier = priceMultiplier;
    }

    public bool ItemsIsEmpty
        => _items.Count() <= 0;

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);

        _items = new();

        SkillEntryCollectionDescriptor[] descriptors = new[]
        {
            new SkillEntryCollectionDescriptor(
                pred: e => e.LowestJingJie <= JingJie.ZhuJi,
                count: 7,
                consume: false),
            new(
                pred: e => JingJie.JinDan <= e.LowestJingJie && e.LowestJingJie <= JingJie.YuanYing,
                count: 2,
                consume: false),
            new(
                pred: e => JingJie.HuaShen <= e.LowestJingJie,
                count: 1,
                consume: false),
        };

        GainSkillBuilder b = new();
        foreach(SkillEntryCollectionDescriptor descriptor in descriptors)
            b.Draw(descriptor);
        
        foreach(SkillEntry skillEntry in b.DrawnSkillEntries)
            _items.Add(SkillEntryDescriptor.FromEntryJingJie(skillEntry, skillEntry.LowestJingJie));

        _price = 0;

        foreach (var item in _items.Traversal())
            _price += (1 << item.JingJie.Value);

        _price = (int) (_price * _priceMultiplier / _items.Count());
    }

    public void GachaProcedure()
    {
        if (_items.Count() <= 0)
            return;

        if (RunManager.Instance.Environment.GetGold().Curr < _price)
            return;

        RunManager.Instance.Environment.SetDGoldProcedure(-_price);

        int gachaIndex = RandomManager.Range(0, _items.Count());
        SkillEntryDescriptor skillEntryDescriptor = _items.Get(gachaIndex) as SkillEntryDescriptor;

        GachaDetails details = new(skillEntryDescriptor, gachaIndex);
        
        _items.RemoveAt(gachaIndex);

        RunManager.Instance.Environment.GachaProcedure(details);
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
