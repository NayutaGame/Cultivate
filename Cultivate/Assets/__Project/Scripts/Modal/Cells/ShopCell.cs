
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

    private bool _acceptGold;
    private bool _acceptHealth;

    private List<SkillEntryQuery> _drawStrategies;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((ShopCell)thisObject).GetGuideDescriptor() },
        { "Commodities",                thisObject => ((ShopCell)thisObject).GetCommodities() },
    };
    public override object Get(string s) => Accessor[s](this);
    private ShopCell(
        int ladder,
        float priceMultiplier,
        string title,
        string contentText,
        SpriteEntry spriteEntry,
        bool acceptGold,
        bool acceptHealth,
        List<SkillEntryQuery> drawStrategies)
    {
        _ladder = ladder;
        _priceMultiplier = priceMultiplier;
        _title = title;
        _contentText = contentText;
        _spriteEntry = spriteEntry;
        _acceptGold = acceptGold;
        _acceptHealth = acceptHealth;
        _drawStrategies = drawStrategies;
    }

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);

        _commodities = new CommodityListModel();
        
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(_ladder);
        
        GainSkillBuilder b = new();
        b.Draw(_drawStrategies, currJingJie, distinct: true, consume: false);
        
        foreach (GainingSkill g in b.GainingSkills)
        {
            int basePrice = RoomDefinition.GetCardBasePriceFromJingJie(currJingJie);
            int price = Mathf.RoundToInt(basePrice * _priceMultiplier * RandomManager.Range(0.8f, 1.2f));
            price = price.ClampLower(1);
            Commodity commodity = new Commodity(
                skill: SkillReference.FromGainingSkill(g), 
                price: price,
                payWithGoldFunc: _acceptGold ? PayWithGold : null,
                payWithHealthFunc: _acceptHealth ? PayWithHealth : null,
                discount: RandomManager.value < 0.2f ? 0.5f : 1f,
                acceptGold: _acceptGold,
                acceptHealth: _acceptHealth);
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
        => new(
            ladder,
            priceMultiplier: 2,
            title: "收藏家",
            contentText: "可以花钱购买卡牌",
            spriteEntry: Encyclopedia.SpriteCategory.FromName("收藏家"),
            acceptGold: true,
            acceptHealth: false,
            drawStrategies: SkillEntryQuery.FromBaseJingJieBound(new(JingJie.LianQi, RoomDefinition.GetJingJieFromLadder(ladder))).Stack(8));

    public static RequireCell FromYiBaoZhai(int ladder)
    {
        RequireCell requireCell = RequireCell.FromLiteral(
            titleText:          $"交易",
            getDetailedText:    GetDetailedText,
            requirements:            RunSkillQuery.AnySkill().Stack(1));

        ShopCell shopCell = new ShopCell(
            ladder,
            priceMultiplier: 2,
            title: "易宝斋",
            contentText: "可以花钱购买卡牌",
            spriteEntry: Encyclopedia.SpriteCategory.FromName("收藏家"),
            acceptGold: true,
            acceptHealth: false, 
            drawStrategies: SkillEntryQuery.FromBaseJingJieBound(new(JingJie.LianQi, RoomDefinition.GetJingJieFromLadder(ladder))).Stack(8));

        requireCell.SetSubmitOperation(SellCard);

        return requireCell;

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

        Cell SellCard(RequireCell cell)
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
        Bound baseJingJieBound = new((jingJieFromLadder + 2).ClampUpper(JingJie.HuaShen),
            (jingJieFromLadder + 3).ClampUpper(JingJie.HuaShen));

        ShopCell B = new(
            ladder,
            priceMultiplier: 1.5f,
            title: "黑市",
            contentText: "可以花钱购买卡牌\n如果钱不够，可以使用气血支付",
            spriteEntry: Encyclopedia.SpriteCategory.FromName("黑市"),
            acceptGold: true,
            acceptHealth: true,
            drawStrategies: SkillEntryQuery.FromBaseJingJieBound(baseJingJieBound).Stack(2));
        
        return B;
    }

    public static ShopCell FromFanXuHealthShop(int ladder)
    {
        JingJie jingJieFromLadder = RoomDefinition.GetJingJieFromLadder(ladder);

        ShopCell B = new(ladder, 1.5f, "气血商店", "可以使用气血购买卡牌", Encyclopedia.SpriteCategory.FromName("黑市"),
            acceptGold: false, acceptHealth: true, 
            drawStrategies: SkillEntryQuery.FromBaseJingJieBound(JingJie.JinDan2HuaShen).Stack(8));
        
        return B;
    }

    public static ShopCell FromEverything(int ladder, float priceMultiplier, string title, string contentText, SpriteEntry spriteEntry,
        bool acceptGold, bool acceptHealth, List<SkillEntryQuery> drawStrategies)
        => new(ladder, priceMultiplier, title, contentText, spriteEntry, acceptGold, acceptHealth, drawStrategies);
}
