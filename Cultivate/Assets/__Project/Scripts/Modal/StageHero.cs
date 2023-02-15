using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHero : StageEntity
{
    public RunHero _runHero;

    public StageHero(RunHero runHero)
    {
        _runHero = runHero;
        Health = _runHero.Health;
        Armor = 0;

        _neiGongList = new StageNeiGong[_runHero.NeiGongCount];
        for (int i = 0; i < _neiGongList.Length; i++)
        {
            _neiGongList[i] = new StageNeiGong(_runHero.GetNeiGong(i));
        }
        _waiGongList = new StageWaiGong[_runHero.WaiGongCount];
        for (int i = 0; i < _waiGongList.Length; i++)
        {
            _waiGongList[i] = new StageWaiGong(_runHero.GetWaiGong(i));
        }

        _p = 0;
    }

    public override EntitySlot Slot() => StageManager.Instance._heroSlot;
}
