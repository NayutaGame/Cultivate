
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GachaCell : Cell
{
    private ListModel<SkillReference> _items;
    public ListModel<SkillReference> GetItems() => _items;
    public void SetItems(ListModel<SkillReference> items) => _items = items;

    private int _price;
    public int GetPrice() => _price;

    private float _priceMultiplier;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((GachaCell)thisObject).GetGuideDescriptor() },
        { "Items",                      thisObject => ((GachaCell)thisObject).GetItems() },
    };
    public override object Get(string s) => Accessor[s](this);
    public GachaCell(float priceMultiplier)
    {
        _priceMultiplier = priceMultiplier;
    }

    public bool ItemsIsEmpty
        => _items.Count() <= 0;

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);

        _items = new();

        GainSkillBuilder b = new();
        
        b.Draw(SkillEntryQuery.FromBaseJingJieBound(JingJie.LianQi2ZhuJi).Stack(7), JingJie.LianQi);
        b.Draw(SkillEntryQuery.FromBaseJingJieBound(JingJie.JinDan2YuanYing).Stack(2), JingJie.JinDan);
        b.Draw(SkillEntryQuery.FromBaseJingJieBound(JingJie.HuaShenOnly), JingJie.HuaShen);
        
        foreach(SkillReference skillReference in b.DrawnSkills)
            _items.Add(skillReference.Clone());

        _price = 0;

        foreach (var item in _items)
            _price += (1 << item.GetJingJie());

        _price = (int) (_price * _priceMultiplier / _items.Count());
    }

    public bool IsAffordable()
        => RunManager.Instance.Environment.GetGold().Curr < _price;

    public void GachaProcedure()
    {
        if (_items.Count() <= 0)
            return;

        if (IsAffordable())
            return;

        RunManager.Instance.Environment.SetDGoldProcedure(-_price);

        int gachaIndex = RandomManager.Range(0, _items.Count());
        SkillReference skillReference = _items.Get(gachaIndex) as SkillReference;

        GachaDetails details = new(skillReference, gachaIndex);
        
        _items.RemoveAt(gachaIndex);

        RunManager.Instance.Environment.GachaProcedure(details);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ExitShopSignal)
        {
            return null;
        }

        return this;
    }
}
