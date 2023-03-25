using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class RunHero : GDictionary
{
    public int Health;
    public int Mana { get; private set; }

    public HeroSlotInventory HeroSlotInventory { get; private set; }

    public void SetJingJie(JingJie jingJie)
    {
        HeroSlotInventory.SetJingJie(jingJie);
    }

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;

    public RunHero(int health = 40, int mana = 0)
    {
        _accessors = new()
        {
            { "HeroSlotInventory", () => HeroSlotInventory },
        };

        Health = health;
        Mana = mana;

        HeroSlotInventory = new();
    }
}
