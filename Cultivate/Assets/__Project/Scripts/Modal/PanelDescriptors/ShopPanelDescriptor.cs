
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ShopPanelDescriptor : PanelDescriptor
{
    private int _ladder;
    private float _priceMultiplier;

    private string _title;
    public string GetTitle() => _title;
    
    private CommodityListModel _commodities;
    public CommodityListModel GetCommodities() => _commodities;
    public void SetCommodities(CommodityListModel commodities) => _commodities = commodities;

    private SpriteEntry _spriteEntry;
    public SpriteEntry GetSprite() => _spriteEntry;

    private ShopPanelDescriptor(int ladder, float priceMultiplier = 1, string title = null, SpriteEntry spriteEntry = null)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Commodities",              GetCommodities },
        };

        _ladder = ladder;
        _priceMultiplier = priceMultiplier;
        _title = title ?? "商店";
        _spriteEntry = spriteEntry ?? "收藏家";
    }

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);

        _commodities = new CommodityListModel();
        
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(_ladder);

        List<SkillEntry> entries = RunManager.Instance.Environment.InnerDrawSkills(new(
            jingJie: currJingJie,
            count: 8,
            consume: false));
        
        foreach (SkillEntry e in entries)
        {
            int basePrice = RoomDefinition.GetCardBasePriceFromJingJie(currJingJie);
            int price = Mathf.RoundToInt(basePrice * _priceMultiplier * RandomManager.Range(0.8f, 1.2f));
            price = price.ClampLower(1);
            float discount = RandomManager.value < 0.2f ? 0.5f : 1f;
            _commodities.Add(new Commodity(SkillEntryDescriptor.FromEntryJingJie(e, currJingJie), price, discount));
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

    public static ShopPanelDescriptor FromDefault(int ladder)
        => FromShouCangJia(ladder);

    public static ShopPanelDescriptor FromShouCangJia(int ladder)
        => new(ladder, priceMultiplier: 2, "收藏家", "收藏家");

    public static ShopPanelDescriptor FromYiBaoZhai(int ladder)
    {
        int goldReward = RoomDefinition.GetGoldRewardFromLadder(ladder);

        ShopPanelDescriptor B = new ShopPanelDescriptor(ladder, 2, "易宝斋", "收藏家");
        B.SetEnter(panelDescriptor =>
        {
            panelDescriptor.DefaultEnter(panelDescriptor);
            RunManager.Instance.Environment.SetDGoldProcedure(goldReward);
        });
        return B;
    }

    public static ShopPanelDescriptor FromHeiShi(int ladder)
    {
        int priceMultiplier = 2;
        JingJie jingJieFromLadder = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound baseJingJieBound = new Bound((jingJieFromLadder + 2).ClampUpper(JingJie.HuaShen),
            (jingJieFromLadder + 3).ClampUpper(JingJie.HuaShen) + 1);

        ShopPanelDescriptor B = new(ladder, priceMultiplier, "黑市", "黑市");
        B.SetEnter(panelDescriptor =>
        {
            CommodityListModel commodities = new CommodityListModel();

            List<SkillEntry> entries = RunManager.Instance.Environment.InnerDrawSkills(new(
                pred: e => baseJingJieBound.Contains(e.LowestJingJie),
                count: 2,
                consume: false));

            foreach (SkillEntry e in entries)
            {
                int cardJingJie = e.LowestJingJie;
                int basePrice = RoomDefinition.GetCardBasePriceFromJingJie(cardJingJie);
                int price = Mathf.RoundToInt(basePrice * priceMultiplier * RandomManager.Range(0.8f, 1.2f));
                price = price.ClampLower(1);
                float discount = RandomManager.value < 0.2f ? 0.5f : 1f;
                commodities.Add(new Commodity(SkillEntryDescriptor.FromEntryJingJie(e, cardJingJie), price,
                    discount));
            }

            B.SetCommodities(commodities);
        });
        
        return B;
    }

    public static ShopPanelDescriptor FromBiYeJi(int ladder)
    {
        int priceMultiplier = 2;
        JingJie jingJieFromLadder = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound baseJingJieBound = new Bound(JingJie.LianQi,
            (jingJieFromLadder - 2).ClampLower(JingJie.LianQi) + 1);

        ShopPanelDescriptor B = new(ladder, 0.5f, "毕业季", "毕业季");
        B.SetEnter(panelDescriptor =>
        {
            CommodityListModel commodities = new CommodityListModel();

            List<SkillEntry> entries = RunManager.Instance.Environment.InnerDrawSkills(new(
                pred: e => baseJingJieBound.Contains(e.LowestJingJie),
                count: 4,
                consume: false));

            foreach (SkillEntry e in entries)
            {
                int cardJingJie = e.LowestJingJie;
                int basePrice = RoomDefinition.GetCardBasePriceFromJingJie(cardJingJie);
                int price = Mathf.RoundToInt(basePrice * priceMultiplier * RandomManager.Range(0.8f, 1.2f));
                price = price.ClampLower(1);
                float discount = RandomManager.value < 0.2f ? 0.5f : 1f;
                commodities.Add(new Commodity(SkillEntryDescriptor.FromEntryJingJie(e, e.LowestJingJie), price,
                    discount));
            }

            B.SetCommodities(commodities);
        });
        return B;
    }
}
