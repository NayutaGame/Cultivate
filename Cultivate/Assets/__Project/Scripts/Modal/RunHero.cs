using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class RunHero
{
    public int Health;
    public int Mana { get; private set; }

    public HeroSlotInventory HeroSlotInventory { get; private set; }

    public void SetJingJie(JingJie jingJie)
    {
        HeroSlotInventory.SetJingJie(jingJie);
    }

    public RunHero(int health = 40, int mana = 0)
    {
        Health = health;
        Mana = mana;

        HeroSlotInventory = new();
    }
}
