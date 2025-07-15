using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commodity : Addressable
{
    public SkillEntryDescriptor Skill;
    public int Price;
    public float Discount;
    private Action<Commodity> BuyFunc;
    public int FinalPrice;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Commodity(SkillEntryDescriptor skill, int price, Action<Commodity> buyFunc, float discount = 1f)
    {
        _accessors = new()
        {
            { "Skill",         () => Skill },
        };
        Skill = skill;
        Price = price;
        Discount = discount;
        BuyFunc = buyFunc;
        FinalPrice = Mathf.FloorToInt(price * discount);
    }

    public bool Affordable()
    {
        return RunManager.Instance.Environment.GetGold().Curr >= FinalPrice;
    }

    public void Buy()
        => BuyFunc.Invoke(this);
}
