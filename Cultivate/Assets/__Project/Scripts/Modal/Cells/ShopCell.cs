
using System;
using System.Collections.Generic;
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

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((ShopCell)thisObject).GetGuideDescriptor() },
        { "Commodities",                thisObject => ((ShopCell)thisObject).GetCommodities() },
    };
    public override object Get(string s) => Accessor[s](this);
    private ShopCell(int ladder, float priceMultiplier = 1, string title = null, string spriteName = null)
        : this(ladder, priceMultiplier, title, Encyclopedia.SpriteCategory.FromName(spriteName ?? "收藏家")) { }
    private ShopCell(int ladder, float priceMultiplier = 1, string title = null, SpriteEntry spriteEntry = null)
    {
        _ladder = ladder;
        _priceMultiplier = priceMultiplier;
        _title = title ?? "商店";
        _spriteEntry = spriteEntry ?? Encyclopedia.SpriteCategory.FromName("收藏家");
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

    public static CardPickerCell FromYiBaoZhai(int ladder)
    {
        CardPickerCell cardPickerCell = new(
            titleText:          $"交易",
            detailedText:       $"选择1张牌卖掉",
            descriptor:         RunSkillDescriptorListModel.FromCount(1));

        ShopCell shopCell = new ShopCell(ladder, 2, "易宝斋", "收藏家");

        cardPickerCell.SetSubmitOperation(cell =>
        {
            cell.RequirementSlotList.Do(slot =>
            {
                if (slot.Skill == null)
                    return;

                JingJie jingJie = slot.Skill.GetJingJie();
                int skillValue = 2 << jingJie;
                RunManager.Instance.Environment.SetDGoldProcedure(skillValue);
                
                slot.Skill = null;
            });
            
            cell.WithdrawAll();
            return shopCell;
        });

        return cardPickerCell;
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
}
