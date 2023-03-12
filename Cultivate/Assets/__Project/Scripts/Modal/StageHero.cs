using System.Collections;
using System.Collections.Generic;
using CLLibrary;
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

        _waiGongList = new StageWaiGong[_runHero.HeroSlotInventory.Limit];
        for (int i = 0; i < _waiGongList.Length; i++)
        {
            HeroChipSlot slot = _runHero.HeroSlotInventory[i + _runHero.HeroSlotInventory.Start];

            int[] powers = new int[WuXing.Length];
            WuXing.Traversal.Do(wuXing => powers[wuXing] = slot.GetPower(wuXing));

            _waiGongList[i] = new StageWaiGong(this, slot.RunChip, powers, i);
        }

        _p = 0;

        _waiGongList.Do(waiGong => waiGong.Register());
    }

    ~StageHero()
    {
        _waiGongList.Do(waiGong => waiGong.Unregister());
    }

    public override string GetName() => "玩家";
    public override EntitySlot Slot() => StageManager.Instance._heroSlot;
    public override StageEntity Opponent() => StageManager.Instance._enemy;
}
