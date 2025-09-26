
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

    private string _contentText;
    public string GetContentText() => _contentText;
    
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
    private ShopCell(int ladder, float priceMultiplier, string title, string contentText, SpriteEntry spriteEntry)
    {
        _ladder = ladder;
        _priceMultiplier = priceMultiplier;
        _title = title;
        _contentText = contentText;
        _spriteEntry = spriteEntry;
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
            Commodity commodity = new Commodity(
                skill: SkillEntryDescriptor.FromEntryJingJie(e, currJingJie),
                price: price,
                payWithGoldFunc: PayWithGold,
                payWithHealthFunc: null,
                discount: RandomManager.value < 0.2f ? 0.5f : 1f,
                acceptGold: true,
                acceptHealth: false);
            _commodities.Add(commodity);
        }
    }

    private void PayWithGold(Commodity commodity)
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

    private void PayWithHealth(Commodity commodity)
    {
        BuySkillDetails details = new(commodity, _commodities.IndexOf(commodity));
        if (!_commodities.Contains(commodity))
            return;

        if (RunManager.Instance.Environment.Home.GetHealth() < commodity.FinalPrice)
            return;

        RunManager.Instance.Environment.LoseHealthProcedure(commodity.FinalPrice);
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
        => new(ladder, priceMultiplier: 2, "收藏家", "可以花钱购买卡牌", Encyclopedia.SpriteCategory.FromName("收藏家"));

    public static CardPickerCell FromYiBaoZhai(int ladder)
    {
        CardPickerCell cardPickerCell = CardPickerCell.FromLiteral(
            titleText:          $"交易",
            getDetailedText:    GetDetailedText,
            descriptor:         RunSkillDescriptorListModel.FromCount(1));

        ShopCell shopCell = new ShopCell(ladder, 2, "易宝斋", "可以花钱购买卡牌",Encyclopedia.SpriteCategory.FromName("收藏家"));

        cardPickerCell.SetSubmitOperation(SellCard);

        return cardPickerCell;

        string GetDetailedText(ListModel<RequirementSlot> requirementSlots)
        {
            int totalSkillValue = 0;
            requirementSlots.Do(slot =>
            {
                if (slot.Skill == null) return;
                JingJie jingJie = slot.Skill.GetJingJie();
                totalSkillValue += (2 << jingJie);
            });

            return totalSkillValue == 0 ? "请选择1张牌卖掉" : $"请选择1张牌卖掉\n有人愿意以{totalSkillValue}金收购您的卡牌";
        }

        Cell SellCard(CardPickerCell cell)
        {
            cell.RequirementSlotList.Do(slot =>
            {
                if (slot.Skill == null) return;

                JingJie jingJie = slot.Skill.GetJingJie();
                int skillValue = 2 << jingJie;
                RunManager.Instance.Environment.SetDGoldProcedure(skillValue);

                slot.Skill = null;
            });

            cell.WithdrawAll();
            return shopCell;
        }
    }

    public static ShopCell FromHeiShi(int ladder)
    {
        JingJie jingJieFromLadder = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound baseJingJieBound = new Bound((jingJieFromLadder + 2).ClampUpper(JingJie.HuaShen),
            (jingJieFromLadder + 3).ClampUpper(JingJie.HuaShen) + 1);

        ShopCell B = new(ladder, 1.5f, "黑市", "可以花钱购买卡牌\n如果钱不够，可以使用气血支付", Encyclopedia.SpriteCategory.FromName("黑市"));
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
                Commodity commodity = new Commodity(
                    skill:SkillEntryDescriptor.FromEntryJingJie(e, cardJingJie),
                    price: price,
                    payWithGoldFunc: shop.PayWithGold,
                    payWithHealthFunc: shop.PayWithHealth,
                    discount: RandomManager.value < 0.2f ? 0.5f : 1f,
                    acceptGold: true,
                    acceptHealth: true);
                commodities.Add(commodity);
            }

            B.SetCommodities(commodities);
        });
        
        return B;
    }

    public static ShopCell FromFanXuHealthShop(int ladder)
    {
        JingJie jingJieFromLadder = RoomDefinition.GetJingJieFromLadder(ladder);
        Bound baseJingJieBound = JingJie.JinDan2HuaShen;

        ShopCell B = new(ladder, 1.5f, "气血商店", "可以使用气血购买卡牌", Encyclopedia.SpriteCategory.FromName("黑市"));
        B.SetEnter(panelDescriptor =>
        {
            ShopCell shop = (ShopCell)panelDescriptor;
            CommodityListModel commodities = new CommodityListModel();

            SkillEntryCollectionDescriptor descriptor = new(
                pred: e => baseJingJieBound.Contains(e.LowestJingJie),
                count: 8,
                consume: false);

            GainSkillBuilder b = new();
            b.Draw(descriptor);

            foreach (SkillEntry e in b.DrawnSkillEntries)
            {
                int cardJingJie = e.LowestJingJie;
                int basePrice = RoomDefinition.GetCardBasePriceFromJingJie(cardJingJie);
                int price = Mathf.RoundToInt(basePrice * shop._priceMultiplier * RandomManager.Range(0.8f, 1.2f));
                price = price.ClampLower(1);
                Commodity commodity = new Commodity(
                    skill:SkillEntryDescriptor.FromEntryJingJie(e, cardJingJie),
                    price: price,
                    payWithGoldFunc: null,
                    payWithHealthFunc: shop.PayWithHealth,
                    discount: RandomManager.value < 0.2f ? 0.5f : 1f,
                    acceptGold: false,
                    acceptHealth: true);
                commodities.Add(commodity);
            }

            B.SetCommodities(commodities);
        });
        
        return B;
    }
}
