
using CLLibrary;
using UnityEngine;

public class ShopCell : Cell
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

    private ShopCell(int ladder, float priceMultiplier = 1, string title = null, SpriteEntry spriteEntry = null)
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

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);

        _commodities = new CommodityListModel();
        
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(_ladder);

        SkillEntryCollectionDescriptor descriptor = new(
            jingJie: currJingJie,
            count: 8,
            consume: false);

        GainSkillBuilder b = new();
        b.Draw(descriptor);
        
        foreach (SkillEntry e in b.DrawnSkillEntries)
        {
            int basePrice = RoomDefinition.GetCardBasePriceFromJingJie(currJingJie);
            int price = Mathf.RoundToInt(basePrice * _priceMultiplier * RandomManager.Range(0.8f, 1.2f));
            price = price.ClampLower(1);
            float discount = RandomManager.value < 0.2f ? 0.5f : 1f;
            _commodities.Add(new Commodity(SkillEntryDescriptor.FromEntryJingJie(e, currJingJie), price, Buy, discount));
        }
    }

    private void Buy(Commodity commodity)
    {
        BuySkillDetails details = new(commodity, _commodities.IndexOf(commodity));
        if (!_commodities.Contains(commodity))
            return;

        if (RunManager.Instance.Environment.GetGold().Curr < commodity.FinalPrice)
            return;

        RunManager.Instance.Environment.SetDGoldProcedure(-commodity.FinalPrice);
        _commodities.Remove(commodity);

        RunManager.Instance.Environment.BuySkillProcedure(details);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ExitShopSignal)
        {
            return null;
        }

        return this;
    }

    public static ShopCell FromDefault(int ladder)
        => FromShouCangJia(ladder);

    public static ShopCell FromShouCangJia(int ladder)
        => new(ladder, priceMultiplier: 2, "收藏家", "收藏家");

    public static ShopCell FromYiBaoZhai(int ladder)
    {
        int goldReward = RoomDefinition.GetGoldRewardFromLadder(ladder);

        ShopCell B = new ShopCell(ladder, 2, "易宝斋", "收藏家");
        B.SetEnter(panelDescriptor =>
        {
            panelDescriptor.DefaultEnter(panelDescriptor);
            RunManager.Instance.Environment.SetDGoldProcedure(goldReward);
        });
        return B;
    }

    public static ShopCell FromHeiShi(int ladder)
    {
        JingJie jingJieFromLadder = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound baseJingJieBound = new Bound((jingJieFromLadder + 2).ClampUpper(JingJie.HuaShen),
            (jingJieFromLadder + 3).ClampUpper(JingJie.HuaShen) + 1);

        ShopCell B = new(ladder, 2, "黑市", "黑市");
        B.SetEnter(panelDescriptor =>
        {
            ShopCell shop = (ShopCell)panelDescriptor;
            CommodityListModel commodities = new CommodityListModel();

            SkillEntryCollectionDescriptor descriptor = new(
                pred: e => baseJingJieBound.Contains(e.LowestJingJie),
                count: 2,
                consume: false);

            GainSkillBuilder b = new();
            b.Draw(descriptor);

            foreach (SkillEntry e in b.DrawnSkillEntries)
            {
                int cardJingJie = e.LowestJingJie;
                int basePrice = RoomDefinition.GetCardBasePriceFromJingJie(cardJingJie);
                int price = Mathf.RoundToInt(basePrice * shop._priceMultiplier * RandomManager.Range(0.8f, 1.2f));
                price = price.ClampLower(1);
                float discount = RandomManager.value < 0.2f ? 0.5f : 1f;
                commodities.Add(new Commodity(SkillEntryDescriptor.FromEntryJingJie(e, cardJingJie), price, shop.Buy, discount));
            }

            B.SetCommodities(commodities);
        });
        
        return B;
    }

    public static ShopCell FromBiYeJi(int ladder)
    {
        JingJie jingJieFromLadder = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound baseJingJieBound = new Bound(JingJie.LianQi,
            (jingJieFromLadder - 2).ClampLower(JingJie.LianQi) + 1);

        ShopCell B = new(ladder, 0.5f, "毕业季", "毕业季");
        B.SetEnter(panelDescriptor =>
        {
            ShopCell shop = (ShopCell)panelDescriptor;
            CommodityListModel commodities = new CommodityListModel();

            SkillEntryCollectionDescriptor descriptor = new(
                pred: e => baseJingJieBound.Contains(e.LowestJingJie),
                count: 4,
                consume: false);

            GainSkillBuilder b = new();
            b.Draw(descriptor);

            foreach (SkillEntry e in b.DrawnSkillEntries)
            {
                int cardJingJie = e.LowestJingJie;
                int basePrice = RoomDefinition.GetCardBasePriceFromJingJie(cardJingJie);
                int price = Mathf.RoundToInt(basePrice * shop._priceMultiplier * RandomManager.Range(0.8f, 1.2f));
                price = price.ClampLower(1);
                float discount = RandomManager.value < 0.2f ? 0.5f : 1f;
                commodities.Add(new Commodity(SkillEntryDescriptor.FromEntryJingJie(e, e.LowestJingJie), price, shop.Buy, discount));
            }

            B.SetCommodities(commodities);
        });
        return B;
    }
}
