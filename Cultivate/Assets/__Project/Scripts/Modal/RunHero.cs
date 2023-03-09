using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class RunHero
{
    public int Health { get; private set; }
    public int Mana { get; private set; }

    private HeroRunChip[] _waiGongList;
    public int WaiGongLimit;
    public HeroRunChip GetWaiGong(int i) => _waiGongList[i];
    public HeroRunChip SetWaiGong(int i, HeroRunChip runChip) => _waiGongList[i] = runChip;

    public int? FindWaiGongIdx(AcquiredRunChip acquiredRunChip) => _waiGongList.FirstIdx(item => item.AcquiredRunChip == acquiredRunChip);

    public void SetJingJie(JingJie jingJie)
    {
        WaiGongLimit = RunManager.WaiGongLimitFromJingJie[jingJie];
    }

    public RunHero(int health = 40, int mana = 0)
    {
        Health = health;
        Mana = mana;

        _waiGongList = new HeroRunChip[RunManager.WaiGongLimit];

        WaiGongLimit = RunManager.WaiGongLimit;
    }

    public void Swap(int from, int to)
    {
        (_waiGongList[from], _waiGongList[to]) = (_waiGongList[to], _waiGongList[from]);
    }
}
