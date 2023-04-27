using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class StageHero : StageEntity
{
    public RunHero _runHero;

    public StageHero(StageEnvironment env, RunHero runHero, int index) : base(env, index)
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

    public override void WriteEffect()
    {
        base.WriteEffect();

        for (int i = 0; i < _waiGongList.Length; i++)
        {
            HeroChipSlot slot = _runHero.HeroSlotInventory[i + _runHero.HeroSlotInventory.Start];
            slot.RunConsumed = _waiGongList[i].RunConsumed;
        }
    }

    ~StageHero()
    {
        _waiGongList.Do(waiGong => waiGong.Unregister());
    }
}
