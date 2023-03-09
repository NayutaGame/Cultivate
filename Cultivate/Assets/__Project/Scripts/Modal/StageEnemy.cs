using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class StageEnemy : StageEntity
{
    public RunEnemy _runEnemy;

    public StageEnemy(RunEnemy runEnemy)
    {
        _runEnemy = runEnemy;
        MaxHp = _runEnemy.Health;
        Hp = _runEnemy.Health;
        Armor = 0;

        _neiGongList = new StageNeiGong[4];

        _waiGongList = new StageWaiGong[_runEnemy.Limit];
        for (int i = 0; i < _waiGongList.Length; i++)
        {
            EnemyChipSlot slot = _runEnemy.GetSlot(i + _runEnemy.Start);

            int[] powers = new int[WuXing.Length];
            WuXing.Traversal.Do(wuXing => powers[wuXing] = slot.GetPower(wuXing));

            _waiGongList[i] = new StageWaiGong(this, _runEnemy.GetSlot(i).Chip, powers, i);
        }

        _p = 0;
    }

    public override string GetName() => "        敌人";
    public override EntitySlot Slot() => StageManager.Instance._enemySlot;
    public override StageEntity Opponent() => StageManager.Instance._hero;
}
