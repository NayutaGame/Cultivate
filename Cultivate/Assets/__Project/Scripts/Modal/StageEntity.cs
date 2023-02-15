using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class StageEntity
{
    public event Action StartTurnEvent;
    public void StartTurn() => StartTurnEvent?.Invoke();

    public event Action EndTurnEvent;
    public void EndTurn() => EndTurnEvent?.Invoke();

    public event Action<AttackDetails> AttackEvent;
    public void Attack(AttackDetails d) => AttackEvent?.Invoke(d);

    public event Action<AttackDetails> AttackedEvent;
    public void Attacked(AttackDetails d) => AttackedEvent?.Invoke(d);

    public event Action<DamageDetails> DamageEvent;
    public void Damage(DamageDetails d) => DamageEvent?.Invoke(d);

    public event Action<DamageDetails> DamagedEvent;
    public void Damaged(DamageDetails d) => DamagedEvent?.Invoke(d);
    //
    // public event Action<AttackDetails> KilledEvent;
    // public void Killed(AttackDetails d) => KilledEvent?.Invoke(d);
    //
    // public event Action<AttackDetails> KillEvent;
    // public void Kill(AttackDetails d) => KillEvent?.Invoke(d);
    //
    // public event Action<HealDetails> HealEvent;
    // public void Heal(HealDetails d) => HealEvent?.Invoke(d);
    //
    // public event Action<HealDetails> HealedEvent;
    // public void Healed(HealDetails d) => HealedEvent?.Invoke(d);
    //
    // public event Action<DamageDetails> LaststandEvent;
    // public void Laststand(DamageDetails d) => LaststandEvent?.Invoke(d);
    //
    // public event Action<AttackDetails> EvadeEvent;
    // public void Evade(AttackDetails d) => EvadeEvent?.Invoke(d);
    //
    // public event Action<int> CleanEvent;
    // public void Clean(int stack) => CleanEvent?.Invoke(stack);
    //
    // public event Action LoseHpEvent;
    // public void LoseHp() => LoseHpEvent?.Invoke();
    //
    // public FuncQueue<BuffDetails> Buff = new();
    //
    // public FuncQueue<BuffDetails> Buffed = new();
    //
    // public event Action OnStatsChangedEvent;
    // public void OnStatsChanged() => OnStatsChangedEvent?.Invoke();
    //
    // public event Action OnBuffChangedEvent;
    // public void OnBuffChanged() => OnBuffChangedEvent?.Invoke();

    public int Health;
    public int Armor;

    public StageNeiGong[] _neiGongList;
    public StageWaiGong[] _waiGongList;
    public int _p;

    public void Execute(Sequence seq, StageEntity src, StageEntity tgt)
    {
        StageWaiGong chip = _waiGongList[_p];
        chip.Execute(seq, src, tgt);
        _p = (_p + 1) % _waiGongList.Length;
    }

    public abstract EntitySlot Slot();
}
