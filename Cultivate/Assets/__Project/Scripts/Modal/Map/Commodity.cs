
using System;
using System.Collections.Generic;
using UnityEngine;

public class Commodity : Addressable
{
    public SkillEntryDescriptor Skill;
    public int Price;
    public float Discount;
    private Action<Commodity> PayWithGoldFunc;
    private Action<Commodity> PayWithHealthFunc;
    public int FinalPrice;

    private bool _acceptGold;
    private bool _acceptHealth;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Skill",         thisObject => ((Commodity)thisObject).Skill },
    };
    public object Get(string s) => Accessor[s](this);
    public Commodity(
        SkillEntryDescriptor skill,
        int price,
        Action<Commodity> payWithGoldFunc,
        Action<Commodity> payWithHealthFunc,
        float discount,
        bool acceptGold,
        bool acceptHealth)
    {
        Skill = skill;
        Price = price;
        Discount = discount;
        PayWithGoldFunc = payWithGoldFunc;
        PayWithHealthFunc = payWithHealthFunc;
        FinalPrice = Mathf.FloorToInt(price * discount);
        _acceptGold = acceptGold;
        _acceptHealth = acceptHealth;
    }

    public string GetGoldPrice()
        => $"{FinalPrice}金";

    public string GetHealthPrice()
        => $"{FinalPrice}气血";

    public bool AcceptGold()
        => _acceptGold;

    public bool AcceptHealth()
        => _acceptHealth;
    
    public bool GoldAffordable()
        => RunManager.Instance.Environment.GetGold().Curr >= FinalPrice;
    
    public bool HealthAffordable()
        => RunManager.Instance.Environment.Home.GetHealth() >= FinalPrice;

    public void PayWithGold()
        => PayWithGoldFunc.Invoke(this);

    public void PayWithHealth()
        => PayWithHealthFunc.Invoke(this);
}
