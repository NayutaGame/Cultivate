
using System;
using System.Collections.Generic;
using CLLibrary;

public class GachaCell : Cell
{
    private float _priceMultiplier;
    private List<SkillEntryQuery> _drawStrategies;
    private JingJie _preferredJingJie;

    private ListModel<GachaItem> _items;
    public ListModel<GachaItem> GetItems() => _items;
    public void SetItems(ListModel<GachaItem> items) => _items = items;

    private int _price;
    public int GetPrice() => _price;

    private bool _picking;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((GachaCell)thisObject).GetGuideDescriptor() },
        { "Items",                      thisObject => ((GachaCell)thisObject).GetItems() },
    };
    public override object Get(string s) => Accessor[s](this);
    private GachaCell(
        float priceMultiplier,
        List<SkillEntryQuery> drawStrategies,
        JingJie preferredJingJie)
    {
        _priceMultiplier = priceMultiplier;
        _drawStrategies = drawStrategies ?? DefaultDrawStrategies();
        _preferredJingJie = preferredJingJie ?? JingJie.LianQi;
        _picking = false;
    }

    public static GachaCell FromPriceMultiplier(float priceMultiplier)
        => new(priceMultiplier, null, null);

    public static GachaCell FromEverything(
        float priceMultiplier,
        List<SkillEntryQuery> drawStrategies,
        JingJie preferredJingJie)
        => new(priceMultiplier, drawStrategies, preferredJingJie);

    public bool ItemsIsEmpty
        => _items.Count() <= 0;

    public List<SkillEntryQuery> DefaultDrawStrategies()
    {
        List<SkillEntryQuery> toRet = new();

        toRet.AddRange(SkillEntryQuery.FromBaseJingJieBound(JingJie.LianQi2ZhuJi).Stack(7));
        toRet.AddRange(SkillEntryQuery.FromBaseJingJieBound(JingJie.JinDan2YuanYing).Stack(2));
        toRet.Add(SkillEntryQuery.FromBaseJingJieBound(JingJie.HuaShenOnly));

        return toRet;
    }

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);

        _items = new();

        GainSkillBuilder b = new();
        b.Draw(_drawStrategies, _preferredJingJie);
        b.GainingSkills.Do(g => _items.Add(new(SkillGhost.FromGainingSkill(g), GachaProcedure)));

        _price = 0;

        foreach (var item in _items)
            _price += (1 << item.Skill.GetJingJie());

        _price = (int) (_price * _priceMultiplier / _items.Count());
    }

    public bool IsAffordable()
        => RunManager.Instance.Environment.GetGold().Curr >= _price;

    public void Shuffle()
    {
        _items.Shuffle();
    }

    public bool CanCall()
    {
        return !ItemsIsEmpty && IsAffordable() && !_picking;
    }

    public void CallProcedure()
    {
        if (!CanCall())
            return;
        _picking = true;
        RunManager.Instance.Environment.LoseGoldProcedure(_price);
    }

    private void GachaProcedure(GachaItem gachaItem)
    {
        _picking = false;
        int gachaIndex = _items.IndexOf(gachaItem);
        SkillGhost skillGhost = _items[gachaIndex].Skill;

        GachaDetails details = new(skillGhost, gachaIndex);
        
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
