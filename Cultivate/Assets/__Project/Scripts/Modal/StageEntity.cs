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

    public event Action<TurnDetails> StartTurnEvent;
    public void StartTurn(TurnDetails d) => StartTurnEvent?.Invoke(d);

    public event Action<TurnDetails> EndTurnEvent;
    public void EndTurn(TurnDetails d) => EndTurnEvent?.Invoke(d);

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

    public event Action<ArmorGainDetails> ArmorGainEvent;
    public void ArmorGain(ArmorGainDetails d) => ArmorGainEvent?.Invoke(d);

    public event Action<ArmorGainDetails> ArmorGainedEvent;
    public void ArmorGained(ArmorGainDetails d) => ArmorGainedEvent?.Invoke(d);

    public event Action<ArmorLoseDetails> ArmorLoseEvent;
    public void ArmorLose(ArmorLoseDetails d) => ArmorLoseEvent?.Invoke(d);

    public event Action<ArmorLoseDetails> ArmorLostEvent;
    public void ArmorLost(ArmorLoseDetails d) => ArmorLostEvent?.Invoke(d);

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

        StartTurn(new TurnDetails(this, _p));

        bool skipTurn = TryConsumeBuff("跳回合");
        if (!skipTurn)
        {
            Step();

            // if (GetStackOfBuff("缠绕") == 0)
            // {
            if (UltraSwift)
            {
                Step();
                Step();
            }
            else if (Swift)
            {
                Step();
            }
            // }
        }

        EndTurn(new TurnDetails(this, _p));
    }

    private void Step()
    {
        if (!_manaShortage)
            MoveP();

        StageWaiGong waiGong = _waiGongList[_p];

        StartStep(new StepDetails(this, waiGong));
        // StageManager.Instance.Report.Seq?.
        // show waigong

        bool manaSufficient = waiGong.GetManaCost() == 0 || TryConsumeBuff("免费") || TryConsumeMana(waiGong.GetManaCost());
        _manaShortage = !manaSufficient;
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
        int dir = Forward ? 1 : -1;
        for (int i = 0; i < _waiGongList.Length; i++)
        {
            _p += dir;

            bool within = 0 <= _p && _p < _waiGongList.Length;
            if (!within)
            {
                _p = (_p + _waiGongList.Length) % _waiGongList.Length;
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

    public bool UltraSwift;
    public bool Swift;
    private bool _manaShortage;

    public bool IsFullHp
        => Hp == MaxHp;
    public bool Forward
        => GetStackOfBuff("鹤回翔") == 0;
    public bool IsDead()
        => _hp <= 0;
    public int ConsumedCount
        => _waiGongList.Count(waiGong => waiGong.Consumed);

    public int LostArmorRecord;
    public int GeneratedManaRecord;
    public int HighestManaRecord;
    public int SelfDamageRecord;
    public int HealedRecord;
    public int GainedEvadeRecord;
    public int GainedBurningRecord;

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
        HighestManaRecord = 0;
        SelfDamageRecord = 0;
        HealedRecord = 0;

        Buffed.Add(0, HighestManaRecorder);
        Buffed.Add(0, GainedEvadeRecorder);
        Buffed.Add(0, GainedBurningRecorder);

        // _modifier = Modifier.Default;
        // _notes = new List<INote>();

        StartTurnEvent += DefaultStartTurn;
        EndTurnEvent += DefaultEndTurn;
        DamageEvent += DefaultDamage;
        DamagedEvent += DefaultDamaged;
        LoseHpEvent += DefaultLoseHp;
    }

    public BuffDetails HighestManaRecorder(BuffDetails d)
    {
        if (d._buffEntry.Name != "灵气")
            return d;

        HighestManaRecord = Mathf.Max(HighestManaRecord, GetStackOfBuff("灵气"));
        return d;
    }

    public BuffDetails GainedEvadeRecorder(BuffDetails d)
    {
        if (d._buffEntry.Name != "闪避")
            return d;

        GainedEvadeRecord += d._stack;
        return d;
    }

    public BuffDetails GainedBurningRecorder(BuffDetails d)
    {
        if (d._buffEntry.Name != "灼热")
            return d;

        GainedBurningRecord += d._stack;
        return d;
    }

    public virtual void WriteEffect() { }

    ~StageEntity()
    {
        RemoveAllBuffs();

        StartTurnEvent -= DefaultStartTurn;
        EndTurnEvent -= DefaultEndTurn;
        DamageEvent -= DefaultDamage;
        DamagedEvent -= DefaultDamaged;
        LoseHpEvent -= DefaultLoseHp;
    }

    protected virtual void DefaultStartTurn(TurnDetails d) => DesignerEnvironment.DefaultStartTurn(this);
    protected virtual void DefaultEndTurn(TurnDetails d) { }
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

    public void TryRemoveBuff(string buffName)
    {
        Buff b = FindBuff(buffName);
        if (b != null)
            RemoveBuff(b);
    }

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

    public void RemoveAllBuffs() => RemoveBuffs(b => true);

    public Buff FindBuff(BuffEntry buffEntry) => Buffs.FirstObj(b => b.BuffEntry == buffEntry);

    public int GetStackOfBuff(BuffEntry entry) => FindBuff(entry)?.Stack ?? 0;

    public int GetSumOfStackOfBuffs(params string[] names)
        => names.Map(name => GetStackOfBuff(name)).Aggregate((a, b) => a + b);

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

    #region Procedure

    public void AttackProcedure(int value, int times = 1, bool lifeSteal = false, bool pierce = false, bool crit = false, bool recursive = true,
        Action<DamageDetails> damaged = null, Action<DamageDetails> undamaged = null)
        => StageManager.Instance.AttackProcedure(new AttackDetails(this, Opponent(), value, lifeSteal, pierce, crit, false, recursive, damaged, undamaged), times);

    public void DamageSelfProcedure(int value, bool recursive = true,
        Action<DamageDetails> damaged = null, Action<DamageDetails> undamaged = null)
        => StageManager.Instance.DamageProcedure(new DamageDetails(this, this, value, recursive, damaged, undamaged));

    public void DamageOppoProcedure(int value, bool recursive = true,
        Action<DamageDetails> damaged = null, Action<DamageDetails> undamaged = null)
        => StageManager.Instance.DamageProcedure(new DamageDetails(this, Opponent(), value, recursive, damaged, undamaged));

    public void HealProcedure(int value)
        => StageManager.Instance.HealProcedure(new HealDetails(this, this, value));

    public void BuffSelfProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => StageManager.Instance.BuffProcedure(new BuffDetails(this, this, buffEntry, stack, recursive));

    public void BuffOppoProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => StageManager.Instance.BuffProcedure(new BuffDetails(this, Opponent(), buffEntry, stack, recursive));

    public void ArmorGainSelfProcedure(int value)
        => StageManager.Instance.ArmorGainProcedure(new ArmorGainDetails(this, this, value));

    public void ArmorGainOppoProcedure(int value)
        => StageManager.Instance.ArmorGainProcedure(new ArmorGainDetails(this, Opponent(), value));

    public void ArmorLoseSelfProcedure(int value)
        => StageManager.Instance.ArmorLoseProcedure(new ArmorLoseDetails(this, this, value));

    public void ArmorLoseOppoProcedure(int value)
        => StageManager.Instance.ArmorLoseProcedure(new ArmorLoseDetails(this, Opponent(), value));

    #endregion
}
