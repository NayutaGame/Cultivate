
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class StageEntity : GDictionary
{
    public event Func<Task> StartStageEvent;
    public async Task StartStage()
    {
        if (StartStageEvent != null) await StartStageEvent();
    }

    public event Func<Task> EndStageEvent;
    public async Task EndStage()
    {
        if (EndStageEvent != null) await EndStageEvent();
    }

    public event Func<TurnDetails, Task> StartTurnEvent;
    public async Task StartTurn(TurnDetails d)
    {
        if (StartTurnEvent != null) await StartTurnEvent(d);
    }

    public event Func<TurnDetails, Task> EndTurnEvent;
    public async Task EndTurn(TurnDetails d)
    {
        if (EndTurnEvent != null) await EndTurnEvent(d);
    }

    public event Func<Task> StartRoundEvent;
    public async Task StartRound()
    {
        if (StartRoundEvent != null) await StartRoundEvent();
    }

    public event Func<Task> EndRoundEvent;
    public async Task EndRound()
    {
        if (EndRoundEvent != null) await EndRoundEvent();
    }

    public event Func<StepDetails, Task> StartStepEvent;
    public async Task StartStep(StepDetails d)
    {
        if (StartStepEvent != null) await StartStepEvent(d);
    }

    public event Func<StepDetails, Task> EndStepEvent;
    public async Task EndStep(StepDetails d)
    {
        if (EndStepEvent != null) await EndStepEvent(d);
    }

    public event Func<int, Task> ManaShortageEvent;
    public async Task ManaShortage(int p)
    {
        if (ManaShortageEvent != null) await ManaShortageEvent(p);
    }

    public event Func<AttackDetails, Task> AttackEvent;
    public async Task Attack(AttackDetails d)
    {
        if (AttackEvent != null) await AttackEvent(d);
    }

    public event Func<AttackDetails, Task> AttackedEvent;
    public async Task Attacked(AttackDetails d)
    {
        if (AttackedEvent != null) await AttackedEvent(d);
    }

    public event Func<DamageDetails, Task> DamageEvent;
    public async Task Damage(DamageDetails d)
    {
        if (DamageEvent != null) await DamageEvent(d);
    }

    public event Func<DamageDetails, Task> DamagedEvent;
    public async Task Damaged(DamageDetails d)
    {
        if (DamagedEvent != null) await DamagedEvent(d);
    }

    public event Func<Task> KillEvent;
    public async Task Kill()
    {
        if (KillEvent != null) await KillEvent();
    }

    public event Func<Task> KilledEvent;
    public async Task Killed()
    {
        if (KilledEvent != null) await KilledEvent();
    }

    public event Func<HealDetails, Task> HealEvent;
    public async Task Heal(HealDetails d)
    {
        if (HealEvent != null) await HealEvent(d);
    }

    public event Func<HealDetails, Task> HealedEvent;
    public async Task Healed(HealDetails d)
    {
        if (HealedEvent != null) await HealedEvent(d);
    }

    public event Func<ArmorGainDetails, Task> ArmorGainEvent;
    public async Task ArmorGain(ArmorGainDetails d)
    {
        if (ArmorGainEvent != null) await ArmorGainEvent(d);
    }

    public event Func<ArmorGainDetails, Task> ArmorGainedEvent;
    public async Task ArmorGained(ArmorGainDetails d)
    {
        if (ArmorGainedEvent != null) await ArmorGainedEvent(d);
    }

    public event Func<ArmorLoseDetails, Task> ArmorLoseEvent;
    public async Task ArmorLose(ArmorLoseDetails d)
    {
        if (ArmorLoseEvent != null) await ArmorLoseEvent(d);
    }

    public event Func<ArmorLoseDetails, Task> ArmorLostEvent;
    public async Task ArmorLost(ArmorLoseDetails d)
    {
        if (ArmorLostEvent != null) await ArmorLostEvent(d);
    }

    public event Func<EvadeDetails, Task> EvadedEvent;
    public async Task Evaded(EvadeDetails d)
    {
        if (EvadedEvent != null) await EvadedEvent(d);
    }

    public event Func<Task> LoseHpEvent;
    public async Task LoseHp()
    {
        if (LoseHpEvent != null) await LoseHpEvent();
    }

    public event Func<ConsumeDetails, Task> ConsumedEvent;
    public async Task Consumed(ConsumeDetails d)
    {
        if (ConsumedEvent != null) await ConsumedEvent(d);
    }

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

    public StageSkill[] _waiGongList;
    public StageSkill TryGetWaiGong(int i)
    {
        if (i < _waiGongList.Length)
            return _waiGongList[i];
        return null;
    }

    public int _p;

    public async Task Turn()
    {
        UltraSwift = false;
        Swift = false;

        await StartTurn(new TurnDetails(this, _p));

        bool skipTurn = TryConsumeBuff("跳回合");
        if (!skipTurn)
        {
            await Step();

            if (GetSumOfStackOfBuffs("不动明王咒", "缠绕") > 0)
            {

            }
            else if (UltraSwift)
            {
                await Step();
                await Step();
            }
            else if (Swift)
            {
                await Step();
            }
        }

        await EndTurn(new TurnDetails(this, _p));
    }

    private async Task Step()
    {
        if (!_manaShortage)
            await MoveP();

        StageSkill skill = _waiGongList[_p];

        await StartStep(new StepDetails(this, skill));
        // _env.Report.Seq?.
        // show waigong

        int manaCost = skill.GetManaCost() - GetStackOfBuff("心斋");
        bool manaSufficient = skill.GetManaCost() == 0 || TryConsumeBuff("免费") || TryConsumeMana(manaCost);
        _manaShortage = !manaSufficient;
        if(_manaShortage)
        {
            await ManaShortage(_p);
            await Encyclopedia.SkillCategory["聚气术"].Execute(this, null, true);
            await EndStep(new StepDetails(this, null));
            return;
        }

        if (TryConsumeBuff("双发"))
        {
            await skill.Execute(this);
            await skill.Execute(this);
        }
        else
        {
            await skill.Execute(this);
        }

        // hide waigong
        await EndStep(new StepDetails(this, skill));
    }

    private async Task MoveP()
    {
        int dir = Forward ? 1 : -1;
        for (int i = 0; i < _waiGongList.Length; i++)
        {
            _p += dir;

            bool within = 0 <= _p && _p < _waiGongList.Length;
            if (!within)
            {
                _p = (_p + _waiGongList.Length) % _waiGongList.Length;
                await EndRound();
                await StartRound();
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
    public StageEntity Opponent() => _env.Entities[1 - _index];

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
    public object Get(string s)
        => _accessors[s]();

    private int _index;
    public int Index => _index;

    private RunEntity _runEntity;
    public RunEntity RunEntity => _runEntity;

    private StageEnvironment _env;
    public StageEnvironment Env => _env;

    public StageEntity(StageEnvironment env, RunEntity runEntity, int index)
    {
        _accessors = new()
        {
            { "WaiGongs", () => _waiGongList },
            { "NeiGongs", () => _neiGongList },
            { "Buffs", () => _buffs },
        };

        _env = env;
        _runEntity = runEntity;
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

        StartTurnEvent += DefaultStartTurn;
        EndTurnEvent += DefaultEndTurn;
        DamageEvent += DefaultDamage;
        DamagedEvent += DefaultDamaged;
        LoseHpEvent += DefaultLoseHp;

        MaxHp = _runEntity.GetFinalHealth();
        Hp = _runEntity.GetFinalHealth();
        Armor = 0;

        _neiGongList = new StageNeiGong[4];

        _waiGongList = new StageSkill[_runEntity.Limit];
        for (int i = 0; i < _waiGongList.Length; i++)
        {
            SkillSlot slot = _runEntity.GetSlot(i + _runEntity.Start);
            _waiGongList[i] = new StageSkill(this, slot.Skill, i);
        }

        _p = 0;
    }

    public void WriteEffect()
    {
        for (int i = 0; i < _waiGongList.Length; i++)
        {
            SkillSlot slot = _runEntity.GetSlot(i + _runEntity.Start);
            slot.RunConsumed = _waiGongList[i].RunConsumed;
        }
    }

    public async Task<BuffDetails> HighestManaRecorder(BuffDetails d)
    {
        if (d._buffEntry.Name != "灵气")
            return d;

        HighestManaRecord = Mathf.Max(HighestManaRecord, GetStackOfBuff("灵气"));
        return d;
    }

    public async Task<BuffDetails> GainedEvadeRecorder(BuffDetails d)
    {
        if (d._buffEntry.Name != "闪避")
            return d;

        GainedEvadeRecord += d._stack;
        return d;
    }

    public async Task<BuffDetails> GainedBurningRecorder(BuffDetails d)
    {
        if (d._buffEntry.Name != "灼烧")
            return d;

        GainedBurningRecord += d._stack;
        return d;
    }

    ~StageEntity()
    {
        RemoveAllBuffs();

        Buffed.Remove(HighestManaRecorder);
        Buffed.Remove(GainedEvadeRecorder);
        Buffed.Remove(GainedBurningRecorder);

        StartTurnEvent -= DefaultStartTurn;
        EndTurnEvent -= DefaultEndTurn;
        DamageEvent -= DefaultDamage;
        DamagedEvent -= DefaultDamaged;
        LoseHpEvent -= DefaultLoseHp;
    }

    protected async Task DefaultStartTurn(TurnDetails d) => await DesignerEnvironment.DefaultStartTurn(this);
    protected async Task DefaultEndTurn(TurnDetails d) { }
    protected async Task DefaultDamage(DamageDetails damageDetails) { }
    protected async Task DefaultDamaged(DamageDetails damageDetails) { }
    protected async Task DefaultLoseHp() { }

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

    public async Task AttackProcedure(int value, WuXing? wuXing = null, int times = 1, bool lifeSteal = false, bool pierce = false, bool crit = false, bool recursive = true,
        Func<DamageDetails, Task> damaged = null, Func<DamageDetails, Task> undamaged = null)
        => await _env.AttackProcedure(new AttackDetails(this, Opponent(), value, wuXing, lifeSteal, pierce, crit, false, recursive, damaged, undamaged), times);

    public async Task DamageSelfProcedure(int value, bool recursive = true,
        Func<DamageDetails, Task> damaged = null, Func<DamageDetails, Task> undamaged = null)
        => await _env.DamageProcedure(new DamageDetails(this, this, value, recursive, damaged, undamaged));

    public async Task DamageOppoProcedure(int value, bool recursive = true,
        Func<DamageDetails, Task> damaged = null, Func<DamageDetails, Task> undamaged = null)
        => await _env.DamageProcedure(new DamageDetails(this, Opponent(), value, recursive, damaged, undamaged));

    public async Task HealProcedure(int value)
        => await _env.HealProcedure(new HealDetails(this, this, value));

    public async Task BuffSelfProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await _env.BuffProcedure(new BuffDetails(this, this, buffEntry, stack, recursive));

    public async Task BuffOppoProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await _env.BuffProcedure(new BuffDetails(this, Opponent(), buffEntry, stack, recursive));

    public async Task ArmorGainSelfProcedure(int value)
        => await _env.ArmorGainProcedure(new ArmorGainDetails(this, this, value));

    public async Task ArmorGainOppoProcedure(int value)
        => await _env.ArmorGainProcedure(new ArmorGainDetails(this, Opponent(), value));

    public async Task ArmorLoseSelfProcedure(int value)
        => await _env.ArmorLoseProcedure(new ArmorLoseDetails(this, this, value));

    public async Task ArmorLoseOppoProcedure(int value)
        => await _env.ArmorLoseProcedure(new ArmorLoseDetails(this, Opponent(), value));

    #endregion
}
