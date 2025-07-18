
using System;
using System.Collections.Generic;
using UnityEngine;

public class Commodity : Addressable
{
    public SkillEntryDescriptor Skill;
    public int Price;
    public float Discount;
    private Action<Commodity> BuyFunc;
    public int FinalPrice;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Skill",         thisObject => ((Commodity)thisObject).Skill },
    };
    public object Get(string s) => Accessor[s](this);
    public Commodity(SkillEntryDescriptor skill, int price, Action<Commodity> buyFunc, float discount = 1f)
    {
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
