using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using CLLibrary;
using Unity.VisualScripting;

public abstract class StageEntity : GDictionary
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

    public event Action<StepDetails> StartStepEvent;
    public void StartStep(StepDetails d) => StartStepEvent?.Invoke(d);

    public event Action<StepDetails> EndStepEvent;
    public void EndStep(StepDetails d) => EndStepEvent?.Invoke(d);

    public event Action<int> ManaShortageEvent;
    public void ManaShortage(int p) => ManaShortageEvent?.Invoke(p);

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
        set => _hp = Mathf.Min(value, MaxHp);
        // OnStatsChanged()
    }

    private int _maxHp;
    public int MaxHp
    {
        get => _maxHp;
        set
        {
            _maxHp = Mathf.Max(value, 0);
            Hp = Hp;
        }
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

    public void Turn()
    {
        UltraSwift = false;
        Swift = false;

        if (GetStackOfBuff("激活的不屈") > 0 && GetStackOfBuff("强化不屈") > 0)
            StageManager.Instance.BuffProcedure(this, Opponent(), "跳回合");

        TryConsumeBuff("激活的不屈");

        StartTurn();

        if (TryConsumeBuff("跳回合"))
        {

        }
        else
        {
            Step();

            if (GetSumOfStackOfBuffs("不屈", "激活的不屈") == 0)
            {
                if (UltraSwift)
                {
                    Step();
                    Step();
                }
                else if (Swift)
                {
                    Step();
                }
            }
        }

        EndTurn();
    }

    private void Step()
    {
        if (!_manaShortage)
            MoveP();

        StageWaiGong waiGong = _waiGongList[_p];

        StartStep(new StepDetails(this, waiGong));
        // StageManager.Instance.Report.Seq?.
        // show waigong

        _manaShortage = !TryConsumeMana(waiGong.GetManaCost());
        if(_manaShortage)
        {
            ManaShortage(_p);
            (Encyclopedia.ChipCategory["聚气术"] as WaiGongEntry).Execute(this, null, true);
            EndStep(new StepDetails(this, null));
            return;
        }

        if (TryConsumeBuff("双发"))
        {
            waiGong.Execute(this);
            waiGong.Execute(this);
        }
        else
        {
            waiGong.Execute(this);
        }

        // hide waigong
        EndStep(new StepDetails(this, waiGong));
    }

    private void MoveP()
    {
        for (int i = 0; i < _waiGongList.Length; i++)
        {
            _p++;
            if (_p >= _waiGongList.Length)
            {
                _p %= _waiGongList.Length;
                EndRound();
                StartRound();
            }

            if(_waiGongList[_p].Consumed)
                continue;

            if(TryConsumeBuff("跳卡牌"))
                continue;

            return;
        }
    }

    // public abstract GameObject GetPrefab();
    public string GetName() => _index == 0 ? "主场" : "客场";
    public EntitySlot Slot() => StageManager.Instance._slots[_index];
    public StageEntity Opponent() => StageManager.Instance._entities[1 - _index];

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

    public bool UltraSwift;
    public bool Swift;
    private bool _manaShortage;

    public int LostArmorRecord;
    public int GeneratedManaRecord;
    public int SelfDamageRecord;
    public int HealedRecord;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;

    private int _index;

    public StageEntity(int index)
    {
        _accessors = new()
        {
            { "WaiGongs", () => _waiGongList },
            { "NeiGongs", () => _neiGongList },
            { "Buffs", () => _buffs },
        };

        _index = index;

        _buffs = new List<Buff>();
        _manaShortage = false;

        LostArmorRecord = 0;
        GeneratedManaRecord = 0;
        SelfDamageRecord = 0;
        HealedRecord = 0;

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

    protected virtual void DefaultStartTurn() => DesignerEnvironment.DefaultStartTurn(this);
    protected virtual void DefaultEndTurn() { }
    protected virtual void DefaultDamage(DamageDetails damageDetails) { }
    protected virtual void DefaultDamaged(DamageDetails damageDetails) { }
    protected virtual void DefaultLoseHp() { }

    #region Buff

    private List<Buff> _buffs;
    public IEnumerable<Buff> Buffs => _buffs.Traversal();

    public void AddBuff(Buff buff)
    {
        buff.Gain(buff.Stack);
        buff.Register();
        _buffs.Add(buff);
        // OnBuffChangedEvent?.Invoke();
    }

    public void BuffGainStack(Buff buff, int gain)
    {
        buff.Gain(gain);
        buff.Stack += gain;
        // OnBuffChangedEvent?.Invoke();
    }

    public void RemoveBuff(Buff buff)
    {
        buff.Unregister();
        _buffs.Remove(buff);
        buff.Lose();
        // OnBuffChangedEvent?.Invoke();
    }

    public void RemoveBuff(string buffName)
    {
        RemoveBuff(FindBuff(buffName));
    }

    public void RemoveAllBuffs() => RemoveBuffs(b => true);
    public void RemoveBuffs(Predicate<Buff> pred)
    {
        _buffs.Do(b =>
        {
            b.Unregister();
            b.Lose();
        });
        _buffs.RemoveAll(pred);
        // OnBuffChangedEvent?.Invoke();
    }

    public void RemoveBuffs(params string[] names)
    {
        List<Buff> toRemove = _buffs.FilterObj(b => names.Contains(b.GetName())).ToList();
        toRemove.Do(RemoveBuff);
    }

    public Buff FindBuff(string name) => Buffs.FirstObj(b => b.BuffEntry.Name == name);
    public Buff FindBuff(BuffEntry buffEntry) => Buffs.FirstObj(b => b.BuffEntry == buffEntry);
    public Buff FindBuff(Predicate<Buff> pred) => Buffs.FirstObj(pred);

    public int GetStackOfBuff(string name) => FindBuff(name)?.Stack ?? 0;
    public int GetStackOfBuff(BuffEntry entry) => FindBuff(entry)?.Stack ?? 0;

    public int GetSumOfStackOfBuffs(params string[] names)
        => names.Map(GetStackOfBuff).Aggregate((a, b) => a + b);

    public bool TryConsumeBuff(string buffName, int stack = 1)
    {
        if (stack == 0)
            return true;

        Buff b = FindBuff(buffName);
        if (b != null && b.Stack >= stack)
        {
            b.Stack -= stack;
            return true;
        }

        return false;
    }

    public bool TryConsumeBuff(BuffEntry buffEntry, int stack = 1)
    {
        if (stack == 0)
            return true;

        Buff b = FindBuff(buffEntry);
        if (b != null && b.Stack >= stack)
        {
            b.Stack -= stack;
            return true;
        }

        return false;
    }

    public int GetMana() => GetStackOfBuff("灵气");
    public bool TryConsumeMana(int stack = 1) => TryConsumeBuff("灵气", stack);

    public int GetBuffCount() => _buffs.Count;
    public Buff TryGetBuff(int i)
    {
        if (i < _buffs.Count)
            return _buffs[i];
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
