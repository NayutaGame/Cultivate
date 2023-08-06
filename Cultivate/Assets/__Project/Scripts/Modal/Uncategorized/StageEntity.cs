
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class StageEntity : GDictionary
{
    public event Func<int, Task> ManaShortageEvent;
    public async Task ManaShortage(int p)
    {
        if (ManaShortageEvent != null) await ManaShortageEvent(p);
    }

    private int _hp;
    public int Hp
    {
        get => _hp;
        set => _hp = Mathf.Min(value, MaxHp);
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
    }

    private int _armor;
    public int Armor
    {
        get => _armor;
        set => _armor = value;
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

        await _env._eventDict.FireEvent(CLEventDict.START_TURN, new TurnDetails(this, _p));

        bool skipTurn = await TryConsumeProcedure("跳回合");
        if (skipTurn)
        {
            if (GetStackOfBuff("浮空艇") > 0)
                await BuffSelfProcedure("回合免疫");
        }
        else
        {
            await Step();

            if (Swift || UltraSwift)
                await SwiftProcedure(new SwiftDetails(this, Swift, UltraSwift));
        }

        await _env._eventDict.FireEvent(CLEventDict.END_TURN, new TurnDetails(this, _p));
    }

    private async Task SwiftProcedure(SwiftDetails d)
    {
        await _env._eventDict.FireEvent(CLEventDict.WILL_SWIFT, d);
        if (d.Cancel)
            return;

        if (d.Swift || d.UltraSwift)
            await Step();

        if (d.UltraSwift)
            await Step();

        await _env._eventDict.FireEvent(CLEventDict.DID_SWIFT, d);
    }

    private async Task Step()
    {
        if (!_manaShortage)
            await MoveP();

        StageSkill skill = _skills[_p];

        await _env._eventDict.FireEvent(CLEventDict.START_STEP, new StepDetails(this, skill));

        int manaCost = skill.GetManaCost() - GetStackOfBuff("心斋");
        bool manaSufficient = skill.GetManaCost() == 0 || GetStackOfBuff("永久免费") > 0 || await TryConsumeProcedure("免费") || await TryConsumeProcedure("灵气", manaCost);
        _manaShortage = !manaSufficient;
        if(_manaShortage)
        {
            await ManaShortage(_p);
            await Encyclopedia.SkillCategory["聚气术"].Execute(this, null, true);
            await _env._eventDict.FireEvent(CLEventDict.END_STEP, new StepDetails(this, null));
            return;
        }

        bool multicast = GetStackOfBuff("永久双发") > 0 || await TryConsumeProcedure("双发");
        if (multicast)
        {
            await skill.Execute(this);
            await skill.Execute(this);
        }
        else
        {
            await skill.Execute(this);
        }

        await _env._eventDict.FireEvent(CLEventDict.END_STEP, new StepDetails(this, skill));
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
                await _env._eventDict.FireEvent(CLEventDict.END_ROUND, new RoundDetails(this));
                await _env._eventDict.FireEvent(CLEventDict.START_ROUND, new RoundDetails(this));
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

    public bool Forward
        => GetStackOfBuff("鹤回翔") == 0;
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

        _env._eventDict.AddCallback(CLEventDict.DID_BUFF, 0, HighestManaRecorder);
        _env._eventDict.AddCallback(CLEventDict.DID_BUFF, 0, GainedEvadeRecorder);
        _env._eventDict.AddCallback(CLEventDict.DID_BUFF, 0, GainedBurningRecorder);
        _env._eventDict.AddCallback(CLEventDict.START_TURN, 0, DefaultStartTurn);

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
        RemoveAllBuffs().GetAwaiter().GetResult();

        _env._eventDict.RemoveCallback(CLEventDict.DID_BUFF, HighestManaRecorder);
        _env._eventDict.RemoveCallback(CLEventDict.DID_BUFF, GainedEvadeRecorder);
        _env._eventDict.RemoveCallback(CLEventDict.DID_BUFF, GainedBurningRecorder);
        _env._eventDict.RemoveCallback(CLEventDict.START_TURN, DefaultStartTurn);
    }

    public void WriteEffect()
    {
        for (int i = 0; i < _skills.Length; i++)
        {
            SkillSlot slot = _runEntity.GetSlot(i + _runEntity.Start);
            slot.RunExhausted = _skills[i].RunExhausted;
        }
    }

    public async Task<BuffDetails> HighestManaRecorder(StageEventDetails stageEventDetails)
    {
        BuffDetails d = (BuffDetails)stageEventDetails;
        if (d._buffEntry.Name != "灵气")
            return d;

        HighestManaRecord = Mathf.Max(HighestManaRecord, GetStackOfBuff("灵气"));
        return d;
    }

    public async Task<BuffDetails> GainedEvadeRecorder(StageEventDetails stageEventDetails)
    {
        BuffDetails d = (BuffDetails)stageEventDetails;
        if (d._buffEntry.Name != "闪避")
            return d;

        GainedEvadeRecord += d._stack;
        return d;
    }

    public async Task<BuffDetails> GainedBurningRecorder(StageEventDetails stageEventDetails)
    {
        BuffDetails d = (BuffDetails)stageEventDetails;
        if (d._buffEntry.Name != "灼烧")
            return d;

        GainedBurningRecord += d._stack;
        return d;
    }

    protected async Task DefaultStartTurn(StageEventDetails d)
        => await DesignerEnvironment.DefaultStartTurn(this, d);

    #region Formation

    private List<Formation> _formations;
    public IEnumerable<Formation> Formations => _formations.Traversal();

    public async Task AddFormation(GainFormationDetails d)
    {
        Formation formation = new Formation(this, d._entry);
        formation.Register();
        await formation._eventDict.FireEvent(CLEventDict.GAIN_FORMATION, d);
        _formations.Add(formation);
    }

    public async Task RemoveFormation(Formation f)
    {
        await f._eventDict.FireEvent(CLEventDict.LOSE_FORMATION, new LoseFormationDetails(f));
        f.Unregister();
        _formations.Remove(f);
    }

    public async Task RemoveAllFormations()
    {
        await _formations.Do(async f =>
        {
            await f._eventDict.FireEvent(CLEventDict.LOSE_FORMATION, new LoseFormationDetails(f));
            f.Unregister();
        });
        _formations.RemoveAll(f => true);
    }

    #endregion

    #region Buff

    private List<Buff> _buffs;
    public IEnumerable<Buff> Buffs => _buffs.Traversal();

    public async Task AddBuff(GainBuffDetails d)
    {
        Buff buff = new Buff(this, d._entry);
        buff.Register();
        await buff._eventDict.FireEvent(CLEventDict.GAIN_BUFF, d);
        await buff.SetStack(d._initialStack);
        _buffs.Add(buff);
    }

    public async Task RemoveBuff(Buff b)
    {
        await b._eventDict.FireEvent(CLEventDict.LOSE_BUFF, new LoseBuffDetails(b));
        b.Unregister();
        _buffs.Remove(b);
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
            await b._eventDict.FireEvent(CLEventDict.LOSE_BUFF, new LoseBuffDetails(b));
            b.Unregister();
        });
        _buffs.RemoveAll(pred);
    }

    public async Task RemoveBuffs(params string[] names)
    {
        List<Buff> toRemove = _buffs.FilterObj(b => names.Contains(b.GetName())).ToList();
        await toRemove.Do(RemoveBuff);
    }

    public async Task RemoveAllBuffs() => await RemoveBuffs(b => true);

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

    public async Task LoseHealthProcedure(int value)
        => await _env.LoseHealthProcedure(new LoseHealthDetails(this, value));

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
