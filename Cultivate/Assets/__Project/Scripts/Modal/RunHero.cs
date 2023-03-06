using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunHero
{
    public int Health { get; private set; }
    public int Mana { get; private set; }

    private RunChip[] _waiGongList;
    public int WaiGongLimit;
    public RunChip GetWaiGong(int i) => _waiGongList[i];
    public RunChip SetWaiGong(int i, RunChip runChip) => _waiGongList[i] = runChip;

    public void SetJingJie(JingJie jingJie)
    {
        WaiGongLimit = RunManager.WaiGongLimitFromJingJie[jingJie];
    }

    public RunHero(int health = 40, int mana = 0)
    {
        Health = health;
        Mana = mana;

        _waiGongList = new RunChip[RunManager.WaiGongLimit];

        WaiGongLimit = RunManager.WaiGongLimit;
    }

    public void Swap(int from, int to)
    {
        (_waiGongList[from], _waiGongList[to]) = (_waiGongList[to], _waiGongList[from]);
    }
}
