using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEnemy
{
    public int Health { get; private set; }

    private RunChip[] _waiGongList;
    public int WaiGongLimit;

    public RunEnemy(int health = 40) : this(health, new RunChip[RunManager.WaiGongLimit])
    {
    }

    public RunEnemy(int health, RunChip[] waiGongList)
    {
        Health = health;
        _waiGongList = waiGongList;
        WaiGongLimit = RunManager.WaiGongLimit;
    }

    public void SetJingJie(JingJie jingJie)
    {
        WaiGongLimit = RunManager.WaiGongLimitFromJingJie[jingJie];
    }

    public RunChip GetWaiGong(int i) => _waiGongList[i];
    public void SetWaiGong(int i, RunChip waiGong) => _waiGongList[i] = waiGong;
}
