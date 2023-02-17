using System.Collections;
using System.Collections.Generic;
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

        _neiGongList = new StageNeiGong[_runEnemy.NeiGongCount];
        for (int i = 0; i < _neiGongList.Length; i++)
        {
            _neiGongList[i] = new StageNeiGong(_runEnemy.GetNeiGong(i));
        }
        _waiGongList = new StageWaiGong[_runEnemy.WaiGongCount];
        for (int i = 0; i < _waiGongList.Length; i++)
        {
            _waiGongList[i] = new StageWaiGong(_runEnemy.GetWaiGong(i));
        }

        _p = 0;
    }

    public override string GetName() => "        敌人";
    public override EntitySlot Slot() => StageManager.Instance._enemySlot;
    public override StageEntity Opponent() => StageManager.Instance._hero;
}
