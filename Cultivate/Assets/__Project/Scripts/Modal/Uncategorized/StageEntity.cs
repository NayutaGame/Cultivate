
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;
using Unity.VisualScripting;

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

    public event Func<int, Task> ManaShortageEvent;
    public async Task ManaShortage(int p)
    {
        if (ManaShortageEvent != null) await ManaShortageEvent(p);
    }

    public event Func<Task> LoseHpEvent;
    public async Task LoseHp()
    {
        if (LoseHpEvent != null) await LoseHpEvent();
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

    public StageSkill[] _skills;
    public StageSkill TryGetSkill(int i)
    {
        if (i < _skills.Length)
            return _skills[i];
        return null;
    }

    public int _p;

    public async Task Turn()
    {
        UltraSwift = false;
        Swift = false;

        await StartTurn(new TurnDetails(this, _p));

        bool skipTurn = await TryConsumeProcedure("跳回合");
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

        StageSkill skill = _skills[_p];


        await _env.InvokeStageEvent("StartStep", new StepDetails(this, skill));

        int manaCost = skill.GetManaCost() - GetStackOfBuff("心斋");
        bool manaSufficient = skill.GetManaCost() == 0 || await TryConsumeProcedure("免费") || await TryConsumeProcedure("灵气", manaCost);
        _manaShortage = !manaSufficient;
        if(_manaShortage)
        {
            await ManaShortage(_p);
            await Encyclopedia.SkillCategory["聚气术"].Execute(this, null, true);
            await _env.InvokeStageEvent("EndStep", new StepDetails(this, null));
            return;
        }

        if (await TryConsumeProcedure("双发"))
        {
            await skill.Execute(this);
            await skill.Execute(this);
        }
        else
        {
            await skill.Execute(this);
        }

        await _env.InvokeStageEvent("EndStep", new StepDetails(this, skill));
    }

    private async Task MoveP()
    {
        int dir = Forward ? 1 : -1;
        for (int i = 0; i < _skills.Length; i++)
        {
            _p += dir;

            bool within = 0 <= _p && _p < _skills.Length;
            if (!within)
            {
                _p = (_p + _skills.Length) % _skills.Length;
                await _env.InvokeStageEvent("EndRound", new RoundDetails(this));
                await _env.InvokeStageEvent("StartRound", new RoundDetails(this));
            }

            if(_skills[_p].Exhausted)
                continue;

            if(await TryConsumeProcedure("跳卡牌"))
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
    public int ExhaustedCount
        => _skills.Count(skill => skill.Exhausted);

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

    public IEnumerable<FormationEntry> RunFormations() => _runEntity.TraversalActivatedFormations;

    private StageEnvironment _env;
    public StageEnvironment Env => _env;

    public StageEntity(StageEnvironment env, RunEntity runEntity, int index)
    {
        _accessors = new()
        {
            { "Skills", () => _skills },
            { "Buffs", () => _buffs },
        };

        _env = env;
        _runEntity = runEntity;
        _index = index;

        _formations = new List<Formation>();
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
        LoseHpEvent += DefaultLoseHp;

        MaxHp = _runEntity.GetFinalHealth();
        Hp = _runEntity.GetFinalHealth();
        Armor = 0;

        _skills = new StageSkill[_runEntity.Limit];
        for (int i = 0; i < _skills.Length; i++)
        {
            SkillSlot slot = _runEntity.GetSlot(i + _runEntity.Start);
            _skills[i] = new StageSkill(this, slot.Skill, i);
        }

        _p = 0;
    }

    ~StageEntity()
    {
        RemoveAllFormations().GetAwaiter().GetResult();
        RemoveAllBuffs();

        Buffed.Remove(HighestManaRecorder);
        Buffed.Remove(GainedEvadeRecorder);
        Buffed.Remove(GainedBurningRecorder);

        StartTurnEvent -= DefaultStartTurn;
        EndTurnEvent -= DefaultEndTurn;
        LoseHpEvent -= DefaultLoseHp;
    }

    public void WriteEffect()
    {
        for (int i = 0; i < _skills.Length; i++)
        {
            SkillSlot slot = _runEntity.GetSlot(i + _runEntity.Start);
            slot.RunExhausted = _skills[i].RunExhausted;
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

    protected async Task DefaultStartTurn(TurnDetails d) => await DesignerEnvironment.DefaultStartTurn(this);
    protected async Task DefaultEndTurn(TurnDetails d) { }
    protected async Task DefaultLoseHp() { }

    #region Formation

    private List<Formation> _formations;
    public IEnumerable<Formation> Formations => _formations.Traversal();

    public async Task AddFormation(Formation formation)
    {
        await formation.Gain();
        formation.Register();
        _formations.Add(formation);
    }

    public async Task RemoveFormation(Formation formation)
    {
        await formation.Lose();
        formation.Unregister();
        _formations.Remove(formation);
    }

    public async Task RemoveAllFormations()
    {
        await _formations.Do(async f =>
        {
            await f.Lose();
            f.Unregister();
        });
        _formations.RemoveAll(f => true);
    }

    #endregion

    #region Buff

    private List<Buff> _buffs;
    public IEnumerable<Buff> Buffs => _buffs.Traversal();

    public async Task AddBuff(Buff buff)
    {
        await buff.Gain(buff.Stack);
        buff.Register();
        _buffs.Add(buff);
        // OnBuffChangedEvent?.Invoke();
    }

    public async Task BuffGainStack(Buff buff, int gain)
    {
        await buff.Gain(gain);
        buff.Stack += gain;
        // OnBuffChangedEvent?.Invoke();
    }

    public async Task RemoveBuff(Buff buff)
    {
        await buff.Lose();
        buff.Unregister();
        _buffs.Remove(buff);
        // OnBuffChangedEvent?.Invoke();
    }

    public async Task TryRemoveBuff(string buffName)
    {
        Buff b = FindBuff(buffName);
        if (b != null)
            await RemoveBuff(b);
    }

    public async Task RemoveBuffs(Predicate<Buff> pred)
    {
        await _buffs.Do(async b =>
        {
            await b.Lose();
            b.Unregister();
        });
        _buffs.RemoveAll(pred);
        // OnBuffChangedEvent?.Invoke();
    }

    public async Task RemoveBuffs(params string[] names)
    {
        List<Buff> toRemove = _buffs.FilterObj(b => names.Contains(b.GetName())).ToList();
        await toRemove.Do(RemoveBuff);
    }

    public void RemoveAllBuffs() => RemoveBuffs(b => true);

    public Buff FindBuff(BuffEntry buffEntry) => Buffs.FirstObj(b => b.Entry == buffEntry);

    public int GetStackOfBuff(BuffEntry entry) => FindBuff(entry)?.Stack ?? 0;

    public int GetSumOfStackOfBuffs(params string[] names)
        => names.Map(name => GetStackOfBuff(name)).Aggregate((a, b) => a + b);

    public int GetMana() => GetStackOfBuff("灵气");

    public int GetBuffCount() => _buffs.Count;
    public Buff TryGetBuff(int i)
    {
        if (i < _buffs.Count)
            return _buffs[i];
        return null;
    }

    public async Task<bool> IsFocused()
    {
        if (GetStackOfBuff("永久集中") > 0) return true;
        return await TryConsumeProcedure("集中");
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

    public async Task ArmorGainSelfProcedure(int value)
        => await _env.ArmorGainProcedure(new ArmorGainDetails(this, this, value));

    public async Task ArmorGainOppoProcedure(int value)
        => await _env.ArmorGainProcedure(new ArmorGainDetails(this, Opponent(), value));

    public async Task ArmorLoseSelfProcedure(int value)
        => await _env.ArmorLoseProcedure(new ArmorLoseDetails(this, this, value));

    public async Task ArmorLoseOppoProcedure(int value)
        => await _env.ArmorLoseProcedure(new ArmorLoseDetails(this, Opponent(), value));

    public async Task<bool> TryConsumeProcedure(BuffEntry buffEntry, int stack = 1, bool friendly = true, bool recursive = true)
    {
        if (stack == 0)
            return true;

        Buff b = FindBuff(buffEntry);
        if (b != null && b.Stack >= stack)
        {
            await DispelSelfProcedure(buffEntry, stack, friendly, recursive);
            return true;
        }

        return false;
    }

    public async Task BuffSelfProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await _env.BuffProcedure(new BuffDetails(this, this, buffEntry, stack, recursive));

    public async Task BuffOppoProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await _env.BuffProcedure(new BuffDetails(this, Opponent(), buffEntry, stack, recursive));

    public async Task DispelSelfProcedure(BuffEntry buffEntry, int stack, bool friendly, bool recursive)
        => await _env.DispelProcedure(new DispelDetails(this, this, buffEntry, stack, friendly, recursive));

    public async Task DispelOppoProcedure(BuffEntry buffEntry, int stack, bool friendly, bool recursive)
        => await _env.DispelProcedure(new DispelDetails(this, Opponent(), buffEntry, stack, friendly, recursive));

    public async Task FormationProcedure(FormationEntry formationEntry, bool recursive = true)
        => await _env.FormationProcedure(this, formationEntry, recursive);

    #endregion
}
