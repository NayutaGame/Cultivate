using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHero : StageEntity
{
    public RunHero _runHero;

    public StageHero(RunHero runHero)
    {
        _runHero = runHero;
        MaxHp = _runHero.Health;
        Hp = _runHero.Health;
        Armor = 0;

        _neiGongList = new StageNeiGong[4];

        _waiGongList = new StageWaiGong[_runHero.WaiGongLimit];
        for (int i = 0; i < _waiGongList.Length; i++)
        {
            _waiGongList[i] = new StageWaiGong(_runHero.GetWaiGong(i).RunChip);
        }

        _p = 0;
    }

    public override string GetName() => "玩家";
    public override EntitySlot Slot() => StageManager.Instance._heroSlot;
    public override StageEntity Opponent() => StageManager.Instance._enemy;
}
