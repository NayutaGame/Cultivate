using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;
using CLLibrary;
using Unity.VisualScripting;

public abstract class StageEntity
{
    public event Action StartStageEvent;
    public void StartStage() => StartStageEvent?.Invoke();

    public event Action EndStageEvent;
    public void EndStage() => EndStageEvent?.Invoke();

    public event Action StartTurnEvent;
    public void StartTurn() => StartTurnEvent?.Invoke();

    public event Action EndTurnEvent;
    public void EndTurn() => EndTurnEvent?.Invoke();

    public event Action StartRoundEvent;
    public void StartRound() => StartRoundEvent?.Invoke();

    public event Action EndRoundEvent;
    public void EndRound() => EndRoundEvent?.Invoke();

    public event Action StartStepEvent;
    public void StartStep() => StartStepEvent?.Invoke();

    public event Action<EndStepDetails> EndStepEvent;
    public void EndStep(EndStepDetails d) => EndStepEvent?.Invoke(d);

    public event Action<AttackDetails> AttackEvent;
    public void Attack(AttackDetails d) => AttackEvent?.Invoke(d);

    public event Action<AttackDetails> AttackedEvent;
    public void Attacked(AttackDetails d) => AttackedEvent?.Invoke(d);

    public event Action<DamageDetails> DamageEvent;
    public void Damage(DamageDetails d) => DamageEvent?.Invoke(d);

    public event Action<DamageDetails> DamagedEvent;
    public void Damaged(DamageDetails d) => DamagedEvent?.Invoke(d);

    public event Action<AttackDetails> KilledEvent;
    public void Killed(AttackDetails d) => KilledEvent?.Invoke(d);

    public event Action<AttackDetails> KillEvent;
    public void Kill(AttackDetails d) => KillEvent?.Invoke(d);

    public event Action<HealDetails> HealEvent;
    public void Heal(HealDetails d) => HealEvent?.Invoke(d);

    public event Action<HealDetails> HealedEvent;
    public void Healed(HealDetails d) => HealedEvent?.Invoke(d);

    public event Action<ArmorDetails> ArmorEvent;
    public void _Armor(ArmorDetails d) => ArmorEvent?.Invoke(d);

    public event Action<ArmorDetails> ArmoredEvent;
    public void Armored(ArmorDetails d) => ArmoredEvent?.Invoke(d);

    //
    // public event Action<DamageDetails> LaststandEvent;
    // public void Laststand(DamageDetails d) => LaststandEvent?.Invoke(d);
    //
    // public event Action<AttackDetails> EvadeEvent;
    // public void Evade(AttackDetails d) => EvadeEvent?.Invoke(d);
    //
    // public event Action<int> CleanEvent;
    // public void Clean(int stack) => CleanEvent?.Invoke(stack);

    public event Action LoseHpEvent;
    public void LoseHp() => LoseHpEvent?.Invoke();

    public FuncQueue<BuffDetails> Buff = new();

    public FuncQueue<BuffDetails> Buffed = new();
    //
    // public event Action OnStatsChangedEvent;
    // public void OnStatsChanged() => OnStatsChangedEvent?.Invoke();
    //
    // public event Action OnBuffChangedEvent;
    // public void OnBuffChanged() => OnBuffChangedEvent?.Invoke();

    private int _hp;
    public int Hp
    {
        get => _hp;
        set => _hp = Mathf.Clamp(value, 0, MaxHp);
        // OnStatsChanged()
    }

    private int _maxHp;
    public int MaxHp
    {
        get => _maxHp;
        set => _maxHp = Mathf.Max(value, 0);
        // OnStatsChanged()
    }

    public float HpPercent => (float)_hp / _maxHp;

    private int _armor;
    public int Armor
    {
        get => _armor;
        set => _armor = value;
        // OnStatsChanged()
    }

    public StageNeiGong[] _neiGongList;
    public StageNeiGong TryGetNeiGong(int i)
    {
        if (i < _neiGongList.Length)
            return _neiGongList[i];
        return null;
    }

    public StageWaiGong[] _waiGongList;
    public StageWaiGong TryGetWaiGong(int i)
    {
        if (i < _waiGongList.Length)
            return _waiGongList[i];
        return null;
    }

    public int _p;

    public void Execute(StringBuilder seq)
    {
        StartTurn();

        Buff skipTurnBuff = FindBuff("跳回合");
        if (skipTurnBuff is { Stack: > 0 })
        {
            skipTurnBuff.Stack -= 1;
        }
        else
        {
            Swift = false;
            Step(seq);
            if (Swift)
            {
                Step(seq);
            }
        }

        EndTurn();
    }

    private void Step(StringBuilder seq)
    {
        StartStep();
        if (!_manaShortage)
            MoveP();
        StageWaiGong waiGong = _waiGongList[_p];
        Buff mana = FindBuff("灵气");
        int manaCount = mana?.Stack ?? 0;
        int manaCost = waiGong.GetManaCost();

        if (manaCost - manaCount > 0)
        {
            _manaShortage = true;
            (Encyclopedia.ChipCategory["聚气术"] as WaiGongEntry).Execute(seq, this, null, true);
            EndStep(new EndStepDetails(seq, this, null));
            return;
        }

        _manaShortage = false;
        if (mana != null)
        {
            mana.Stack -= manaCost;
        }

        Buff b = FindBuff("双发");
        if (b is { Stack: >= 1 })
        {
            b.Stack -= 1;
            waiGong.Execute(seq, this);
            waiGong.Execute(seq, this);
        }
        else
        {
            waiGong.Execute(seq, this);
        }
        EndStep(new EndStepDetails(seq, this, waiGong));
    }

    private void MoveP()
    {
        for (int i = 0; i < _waiGongList.Length; i++)
        {
            _p = (_p + 1) % _waiGongList.Length;

            if(_waiGongList[_p].Consumed)
                continue;

            Buff skipChipBuff = FindBuff("跳卡牌");
            if (skipChipBuff is { Stack: > 0 })
            {
                skipChipBuff.Stack -= 1;
                continue;
            }

            return;
        }
    }

    // public abstract GameObject GetPrefab();
    public abstract string GetName();
    public abstract EntitySlot Slot();
    public abstract StageEntity Opponent();

    // private bool _canLaststand;
    // public bool CanLaststand { get => _canLaststand; set => _canLaststand = value; }
    //
    // private bool _canEvade;
    // public bool CanEvade { get => _canEvade; set => _canEvade = value; }
    //
    // public int CleanStack;

    // private bool CanCost(BuffEntry buffEntry, int stack = 1)
    // {
    //     Buff same = FindBuff(buffEntry);
    //     return same != null && same.Stack >= stack;
    // }
    //
    // private void Cost(BuffEntry buffEntry, int stack = 1)
    // {
    //     Buff same = FindBuff(buffEntry);
    //     same.Stack -= stack;
    // }
    //
    // public bool TryCost(string buffName, int stack = 1) => TryCost(Encyclopedia.BuffCategory.Find(buffName), stack);
    //
    // public bool TryCost(BuffEntry buffEntry, int stack = 1)
    // {
    //     if (!CanCost(buffEntry, stack)) return false;
    //     Cost(buffEntry, stack);
    //     return true;
    // }

    public bool Swift;
    private bool _manaShortage;

    public StageEntity()
    {
        _buffs = new List<Buff>();
        _manaShortage = false;
        // _modifier = Modifier.Default;
        // _notes = new List<INote>();

        StartTurnEvent += DefaultStartTurn;
        EndTurnEvent += DefaultEndTurn;
        DamageEvent += DefaultDamage;
        DamagedEvent += DefaultDamaged;
        LoseHpEvent += DefaultLoseHp;
    }

    public bool IsDead() => _hp <= 0;

    ~StageEntity()
    {
        RemoveAllBuffs();

        StartTurnEvent -= DefaultStartTurn;
        EndTurnEvent -= DefaultEndTurn;
        DamageEvent -= DefaultDamage;
        DamagedEvent -= DefaultDamaged;
        LoseHpEvent -= DefaultLoseHp;
    }

    protected virtual void DefaultStartTurn() { } // 比如护甲每回合开始自动减半，可以做在这里，每回合开始不减或者只减20%做在modifier里面
    protected virtual void DefaultEndTurn() { }
    protected virtual void DefaultDamage(DamageDetails damageDetails) { }
    protected virtual void DefaultDamaged(DamageDetails damageDetails) { }
    protected virtual void DefaultLoseHp() { }

    #region Buff

    private List<Buff> _buffs;
    public List<Buff> Buffs => _buffs;

    public void AddBuff(Buff buff)
    {
        buff.Gain(buff.Stack);
        buff.Register();
        Buffs.Add(buff);
        // OnBuffChangedEvent?.Invoke();
    }

    public void StackBuff(Buff buff, int stack)
    {
        buff.Gain(stack);
        buff.Stack += stack;
        // OnBuffChangedEvent?.Invoke();
    }

    public void RemoveBuff(Buff buff)
    {
        buff.Unregister();
        Buffs.Remove(buff);
        buff.Lose();
        // OnBuffChangedEvent?.Invoke();
    }

    public void RemoveAllBuffs()
    {
        _buffs.Do(b =>
        {
            b.Unregister();
            b.Lose();
        });
        Buffs.RemoveAll(b => true);
        // OnBuffChangedEvent?.Invoke();
    }

    public Buff FindBuff(string name) => Buffs.FirstObj(b => b.BuffEntry.Name == name);
    public Buff FindBuff(BuffEntry buffEntry) => Buffs.FirstObj(b => b.BuffEntry == buffEntry);
    public Buff FindBuff(Predicate<Buff> pred) => Buffs.FirstObj(pred);

    public int GetStackOfBuff(string name)
    {
        Buff b = FindBuff(name);
        if (b != null)
        {
            return b.Stack;
        }
        else
        {
            return 0;
        }
    }

    public int GetStackOfBuff(BuffEntry buffEntry)
    {
        Buff b = FindBuff(buffEntry);
        if (b != null)
        {
            return b.Stack;
        }
        else
        {
            return 0;
        }
    }

    public int GetMana()
    {
        return GetStackOfBuff("灵气");
    }

    public int GetBuffCount() => Buffs.Count;
    public Buff TryGetBuff(int i)
    {
        if (i < Buffs.Count)
            return Buffs[i];
        return null;
    }

    #endregion

    // #region Modifier
    //
    // private Modifier _modifier;
    // public Modifier Modifier => _modifier;
    //
    // public int GetFinalPower() =>
    //     (int)Mathf.Max(0, ((
    //                            GetCurrPower()
    //                            + GetModifierProperty(Constants.PowerAdd)
    //                            + GetModifierProperty(Constants.LostHP2Power) * (GetMaxHP() - GetCurrHP())
    //                        )
    //                        * (1 + GetModifierProperty(Constants.PowerMul))
    //                        ));
    //
    // public float GetModifierProperty(string key) => Modifier.Value.ContainsKey(key) ? Modifier.Value[key] : 0;
    //
    // public float GetFinalLifesteal() => GetModifierProperty(Constants.Lifesteal);
    // public float GetFinalDamageImmune() => GetModifierProperty(Constants.DamageImmune);
    // public float GetFinalBlademail() => GetModifierProperty(Constants.Blademail);
    // public float GetFinalArmorKeep() => GetModifierProperty(Constants.ArmorKeep);
    // public float GetFinalExtraGather() => GetModifierProperty(Constants.ExtraGather);
    // public float GetBlock() => GetModifierProperty(Constants.Block);
    //
    // #endregion

    // private List<INote> _notes;
    //
    // public void ConfigureNote(StringBuilder sb)
    // {
    //     sb.Append($"<style=\"EntityName\">{GetName()}</style>\n\n\n");
    //
    //     FetchNotes(_notes);
    //     foreach (INote n in _notes)
    //     {
    //         n.ConfigureNote(sb);
    //         sb.Append("\n\n");
    //     }
    //
    //     ConfigureEntityDescription(sb);
    // }
    //
    // protected abstract void ConfigureEntityDescription(StringBuilder sb);
    //
    // protected virtual void FetchNotes(List<INote> notes)
    // {
    //     notes.Clear();
    // }
}
