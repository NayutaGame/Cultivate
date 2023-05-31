using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commodity : GDictionary
{
    public RunSkill Skill;
    public int Price;
    public float Discount;
    public int FinalPrice;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;

    public Commodity(RunSkill skill, int price, float discount = 1f)
    {
        _accessors = new()
        {
            { "Skill",         () => Skill },
        };
        Skill = skill;
        Price = price;
        Discount = discount;

        FinalPrice = Mathf.FloorToInt(price * discount);
    }

    public bool Affordable()
    {
        return RunManager.Instance.XiuWei >= FinalPrice;
    }
}
