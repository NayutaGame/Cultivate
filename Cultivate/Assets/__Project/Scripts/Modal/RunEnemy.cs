using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class RunEnemy
{
    public int Health;

    private EnemyChipSlot[] _slots;
    public int Start;
    public int Limit;

    public RunEnemy(int health = 40)
    {
        Health = health;
        _slots = new EnemyChipSlot[RunManager.WaiGongLimit];
        for (int i = 0; i < _slots.Length; i++)
            _slots[i] = new EnemyChipSlot(i);

        Start = 0;
        Limit = RunManager.WaiGongLimit;
    }

    public void SetJingJie(JingJie jingJie)
    {
        Start = RunManager.WaiGongStartFromJingJie[jingJie];
        Limit = RunManager.WaiGongLimitFromJingJie[jingJie];
    }

    public EnemyChipSlot GetSlot(int i) => _slots[i];
    public void SetSlotContent(int i, RunChip waiGong, int[] powers)
    {
        _slots[i].Chip = waiGong;
        WuXing.Traversal.Do(wuXing => _slots[i].SetPower(wuXing, powers[wuXing]));
    }
}
