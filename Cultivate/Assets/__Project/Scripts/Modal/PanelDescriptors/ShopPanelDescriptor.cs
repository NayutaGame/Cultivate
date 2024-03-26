
using System.Collections.Generic;
using UnityEngine;

public class ShopPanelDescriptor : PanelDescriptor
{
    private static readonly int[] PriceTable = new int[] { 39, 59, 99, 149, 249 };
    private JingJie _jingJie;
    private CommodityListModel _commodities;

    public ShopPanelDescriptor(JingJie jingJie)
    {
        _accessors = new()
        {
            { "Commodities",                () => _commodities },
        };
        _jingJie = jingJie;
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();

        _commodities = new CommodityListModel();

        bool success = RunManager.Instance.Environment.SkillPool.TryDrawSkills(out List<RunSkill> skills,
            jingJie: _jingJie,
            count: 6,
            consume: false);
        
        if (success)
        {
            foreach (RunSkill skill in skills)
            {
                int price = Mathf.RoundToInt(PriceTable[_jingJie] * RandomManager.Range(0.8f, 1.2f));
                float discount = RandomManager.value < 0.2f ? 0.5f : 1f;
                _commodities.Add(new Commodity(skill, price, discount));
            }
        }
    }

    public bool Buy(Commodity commodity)
    {
        if (!_commodities.Contains(commodity))
            return false;

        if (RunManager.Instance.Environment.Gold < commodity.FinalPrice)
            return false;

        RunManager.Instance.Environment.SetDGold(-commodity.FinalPrice);
        _commodities.Remove(commodity);

        RunManager.Instance.Environment.Hand.Add(commodity.Skill);

        return true;
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal) => null;
}
