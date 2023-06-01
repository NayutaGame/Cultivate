using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ShopPanelDescriptor : PanelDescriptor
{
    private CommodityInventory _commodities;

    public ShopPanelDescriptor()
    {
        _accessors = new()
        {
            { "Commodities",                () => _commodities },
        };
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();

        _commodities = new CommodityInventory();

        bool success = RunManager.Instance.SkillPool.TryDrawSkills(out List<RunSkill> skills, jingJie: RunManager.Instance.Map.JingJie, count: 6, consume: false);
        if (success)
        {
            foreach (RunSkill skill in skills)
            {
                _commodities.Add(new Commodity(skill, 20));
            }
        }
    }

    public bool Buy(Commodity commodity)
    {
        if (!_commodities.Contains(commodity))
            return false;

        if (RunManager.Instance.XiuWei < commodity.FinalPrice)
            return false;

        RunManager.Instance.RemoveXiuWei(commodity.FinalPrice);
        _commodities.Remove(commodity);

        RunManager.Instance.Battle.SkillInventory.AddSkill(commodity.Skill);

        return true;
    }

    public override void DefaultReceiveSignal(Signal signal)
    {
        base.DefaultReceiveSignal(signal);
        RunManager.Instance.Map.TryFinishNode();
    }
}
