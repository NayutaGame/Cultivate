
using System.Collections.Generic;
using UnityEngine;

public class ShopPanelDescriptor : PanelDescriptor
{
    private static readonly int[] PriceTable = new int[] { 1, 2, 4, 8, 16 };
    private JingJie _jingJie;
    private CommodityListModel _commodities;
    public CommodityListModel GetCommodities() => _commodities;
    public void SetCommodities(CommodityListModel commodities) => _commodities = commodities;

    private SpriteEntry _spriteEntry;
    public SpriteEntry GetSprite() => _spriteEntry;

    public ShopPanelDescriptor(JingJie jingJie, SpriteEntry spriteEntry)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Commodities",              GetCommodities },
        };
        
        _jingJie = jingJie;
        _spriteEntry = spriteEntry;
    }

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);

        _commodities = new CommodityListModel();

        List<SkillEntry> entries = RunManager.Instance.Environment.InnerDrawSkills(new(
            jingJie: _jingJie,
            count: 8,
            consume: false));
        
        foreach (SkillEntry e in entries)
        {
            int price = Mathf.RoundToInt(PriceTable[_jingJie] * RandomManager.Range(0.8f, 1.2f));
            float discount = RandomManager.value < 0.2f ? 0.5f : 1f;
            _commodities.Add(new Commodity(SkillEntryDescriptor.FromEntryJingJie(e, _jingJie), price, discount));
        }
    }

    public void BuySkillProcedure(BuySkillDetails d)
    {
        Commodity commodity = d.Commodity;
        if (!_commodities.Contains(commodity))
            return;

        if (RunManager.Instance.Environment.GetGold().Curr < commodity.FinalPrice)
            return;

        RunManager.Instance.Environment.SetDGoldProcedure(-commodity.FinalPrice);
        _commodities.Remove(commodity);

        RunManager.Instance.Environment.BuySkillProcedure(d);
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
