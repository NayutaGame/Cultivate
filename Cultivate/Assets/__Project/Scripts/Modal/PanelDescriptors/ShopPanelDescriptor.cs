
using System.Collections.Generic;
using UnityEngine;

public class ShopPanelDescriptor : PanelDescriptor
{
    private static readonly int[] PriceTable = new int[] { 1, 2, 4, 8, 16 };
    private JingJie _jingJie;
    private CommodityListModel _commodities;
    public CommodityListModel GetCommodities() => _commodities;
    public void SetCommodities(CommodityListModel commodities) => _commodities = commodities;

    public ShopPanelDescriptor(JingJie jingJie)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Commodities",              GetCommodities },
        };
        
        _jingJie = jingJie;
    }

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);

        _commodities = new CommodityListModel();

        List<SkillEntry> entries = RunManager.Instance.Environment.DrawSkills(new(
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

    public bool Buy(Commodity commodity)
    {
        if (!_commodities.Contains(commodity))
            return false;

        if (RunManager.Instance.Environment.GetGold().Curr < commodity.FinalPrice)
            return false;

        RunManager.Instance.Environment.SetDGoldProcedure(-commodity.FinalPrice);
        _commodities.Remove(commodity);
        
        RunManager.Instance.Environment.AddSkillProcedure(commodity.Skill.Entry, commodity.Skill.JingJie);

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
