
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class StageEntity : Addressable, StageEventListener
{
    public MingYuan MingYuan;

    private int _hp;
    public int Hp
    {
        get => _hp;
        set
        {
            _hp = Mathf.Min(value, MaxHp);
        }
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
        set
        {
            _armor = value;
        }
    }

    public StageSkill[] _skills;
    public StageSkill TryGetSkill(int i)
    {
        if (i < _skills.Length)
            return _skills[i];
        return null;
    }

    public async Task Turn()
    {
        UltraSwift = false;
        Swift = false;

        await _env.EventDict.SendEvent(StageEventDict.START_TURN, new TurnDetails(this, _p));

        bool skipTurn = await TryConsumeProcedure("跳回合");
        if (skipTurn)
        {
            if (GetStackOfBuff("浮空艇") > 0)
                await BuffSelfProcedure("回合免疫");
            await _env.EventDict.SendEvent(StageEventDict.END_TURN, new TurnDetails(this, _p));
            return;
        }

        if (_channelDetails != null)
        {
            await ChannelProcedure();
            await _env.EventDict.SendEvent(StageEventDict.END_TURN, new TurnDetails(this, _p));
            return;
        }

        await Step();

        if (Swift || UltraSwift)
            await SwiftProcedure(new SwiftDetails(this, Swift, UltraSwift));

        await _env.EventDict.SendEvent(StageEventDict.END_TURN, new TurnDetails(this, _p));
    }

    private async Task SwiftProcedure(SwiftDetails d)
    {
        await _env.EventDict.SendEvent(StageEventDict.WILL_SWIFT, d);
        if (d.Cancel)
            return;

        if (d.Swift || d.UltraSwift)
            await Step();

        if (d.UltraSwift)
            await Step();

        await _env.EventDict.SendEvent(StageEventDict.DID_SWIFT, d);
    }

    private async Task Step()
    {
        if (!_manaShortage)
            await MoveP();

        StageSkill skill = _skills[_p];

        await _env.EventDict.SendEvent(StageEventDict.START_STEP, new StepDetails(this, skill));

        _manaShortage = await ManaCostProcedure(skill);
        if (_manaShortage)
        {
            await _env.EventDict.SendEvent(StageEventDict.END_STEP, new StepDetails(this, null));
            return;
        }

        if (skill.GetChannelTime() > 0)
        {
            _channelDetails = new ChannelDetails(this, skill);
            await skill.Channel(this, _channelDetails);
            await _env.EventDict.SendEvent(StageEventDict.END_STEP, new StepDetails(this, skill));
            return;
        }

        await Execute(skill);
        await _env.EventDict.SendEvent(StageEventDict.END_STEP, new StepDetails(this, skill));
    }

    private async Task<bool> ManaCostProcedure(StageSkill skill)
    {
        int actualCost = (skill.GetManaCost() - GetStackOfBuff("心斋")).ClampLower(0);
        if (actualCost > 0)
        {
            bool hasFree = GetStackOfBuff("永久免费") > 0 || await TryConsumeProcedure("免费");
            if (hasFree)
                actualCost = 0;
        }

        await _env.EventDict.SendEvent(StageEventDict.WILL_MANA_COST, new ManaCostDetails(this, skill, actualCost));

        bool manaSufficient = actualCost == 0 || await TryConsumeProcedure("灵气", actualCost);
        bool manaShortage = !manaSufficient;
        if (manaShortage)
        {
            await ManaShortageProcedure(_p, skill, actualCost);
            await ManaShortageAction.Execute(this);
        }
        else
        {
            await _env.EventDict.SendEvent(StageEventDict.DID_MANA_COST, new ManaCostDetails(this, skill, actualCost));
        }

        return manaShortage;
    }

    private async Task ChannelProcedure()
    {
        _channelDetails.IncrementProgress();
        if (_channelDetails.FinishedChannelling())
        {
            await _channelDetails._skill.ChannelWithoutTween(this, _channelDetails);
            await Execute(_channelDetails._skill);
            _channelDetails = null;
        }
        else
        {
            await _channelDetails._skill.Channel(this, _channelDetails);
        }
    }

    private async Task Execute(StageSkill skill)
    {
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
    }

    private async Task MoveP()
    {
        int dir = Forward ? 1 : -1;
        for (int i = 0; i < _skills.Length; i++)
        {
            // START_ROUND logic should be here
            // int prevP = _p - dir;

            _p += dir;

            bool within = 0 <= _p && _p < _skills.Length;
            if (!within)
            {
                _p = (_p + _skills.Length) % _skills.Length;
                await _env.EventDict.SendEvent(StageEventDict.END_ROUND, new RoundDetails(this));
                await _env.EventDict.SendEvent(StageEventDict.START_ROUND, new RoundDetails(this));
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

    public int _p;
    private ChannelDetails _channelDetails;
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

    private int _index;
    public int Index => _index;

    private RunEntity _runEntity;
    public RunEntity RunEntity => _runEntity;

    public IEnumerable<FormationEntry> RunFormations() => _runEntity.TraversalActivatedFormations;

    private StageEnvironment _env;
    public StageEnvironment Env => _env;

    private StageEventDescriptor[] _eventDescriptors;

    private StageSkill ManaShortageAction;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public StageEntity(StageEnvironment env, RunEntity runEntity, int index)
    {
        _accessors = new()
        {
            { "Skills", () => _skills },
            { "Formations", () => _formations },
            { "Buffs", () => _buffs },
        };

        _env = env;
        _runEntity = runEntity;
        _index = index;

        _formations = new();
        _buffs = new();

        _manaShortage = false;

        LostArmorRecord = 0;
        GeneratedManaRecord = 0;
        HighestManaRecord = 0;
        SelfDamageRecord = 0;
        HealedRecord = 0;

        _eventDescriptors = new StageEventDescriptor[]
        {
            new(StageEventDict.STAGE_ENTITY, StageEventDict.DID_BUFF, 0, HighestManaRecorder),
            new(StageEventDict.STAGE_ENTITY, StageEventDict.DID_BUFF, 0, GainedEvadeRecorder),
            new(StageEventDict.STAGE_ENTITY, StageEventDict.DID_BUFF, 0, GainedBurningRecorder),
            new(StageEventDict.STAGE_ENTITY, StageEventDict.START_TURN, 0, DefaultStartTurn),
        };

        foreach (var eventDescriptor in _eventDescriptors)
            _env.EventDict.Register(this, eventDescriptor);

        MingYuan = _runEntity.MingYuan.Clone();
        MaxHp = _runEntity.GetFinalHealth();
        Hp = _runEntity.GetFinalHealth();
        Armor = 0;

        _skills = new StageSkill[_runEntity.Limit];
        for (int i = 0; i < _skills.Length; i++)
        {
            SkillSlot slot = _runEntity.GetSlot(i + _runEntity.Start);

            // if (false)
            // {
            //     _skills[i] = StageSkill.FromPlaceSkill(this, slot.PlacedSkill, i);
            // }

            if (slot.Skill != null)
                _skills[i] = StageSkill.FromRunSkill(this, slot.Skill, i);
            else
                _skills[i] = StageSkill.FromSkillEntry(this, "聚气术", slotIndex: i);
        }

        ManaShortageAction = StageSkill.FromSkillEntry(this, "聚气术");

        _p = 0;
    }

    ~StageEntity()
    {
        RemoveAllFormations().GetAwaiter().GetResult();
        RemoveAllBuffs().GetAwaiter().GetResult();

        foreach (var eventDescriptor in _eventDescriptors)
            _env.EventDict.Unregister(this, eventDescriptor);
    }

    public void WriteResult()
    {
        // for (int i = 0; i < _skills.Length; i++)
        // {
        //     SkillSlot slot = _runEntity.GetSlot(i + _runEntity.Start);
        // }
    }

    public async Task<BuffDetails> HighestManaRecorder(StageEventListener listener, EventDetails eventDetails)
    {
        BuffDetails d = (BuffDetails)eventDetails;
        if (d._buffEntry.Name != "灵气")
            return d;

        HighestManaRecord = Mathf.Max(HighestManaRecord, GetStackOfBuff("灵气"));
        return d;
    }

    public async Task<BuffDetails> GainedEvadeRecorder(StageEventListener listener, EventDetails eventDetails)
    {
        BuffDetails d = (BuffDetails)eventDetails;
        if (d._buffEntry.Name != "闪避")
            return d;

        GainedEvadeRecord += d._stack;
        return d;
    }

    public async Task<BuffDetails> GainedBurningRecorder(StageEventListener listener, EventDetails eventDetails)
    {
        BuffDetails d = (BuffDetails)eventDetails;
        if (d._buffEntry.Name != "灼烧")
            return d;

        GainedBurningRecord += d._stack;
        return d;
    }

    protected async Task DefaultStartTurn(StageEventListener listener, EventDetails d)
        => await DesignerEnvironment.DefaultStartTurn(this, d);

    #region Formation

    private ListModel<Formation> _formations;

    public async Task AddFormation(GainFormationDetails d)
    {
        Formation formation = new Formation(this, d._entry);
        formation.Register();
        await formation._eventDict.SendEvent(StageEventDict.GAIN_FORMATION, d);
        _formations.Add(formation);
    }

    public async Task RemoveFormation(Formation f)
    {
        await f._eventDict.SendEvent(StageEventDict.LOSE_FORMATION, new LoseFormationDetails(f));
        f.Unregister();
        _formations.Remove(f);
    }

    public async Task RemoveAllFormations()
    {
        await _formations.Traversal().Do(async f =>
        {
            await f._eventDict.SendEvent(StageEventDict.LOSE_FORMATION, new LoseFormationDetails(f));
            f.Unregister();
        });
        _formations.Clear();
    }

    #endregion

    #region Buff

    private ListModel<Buff> _buffs;
    public IEnumerable<Buff> Buffs => _buffs.Traversal();

    public async Task AddBuff(GainBuffDetails d)
    {
        Buff buff = new Buff(this, d._entry);
        buff.Register();
        await buff._eventDict.SendEvent(StageEventDict.GAIN_BUFF, d);
        await buff.SetStack(d._initialStack);
        _buffs.Add(buff);
    }

    public async Task RemoveBuff(Buff b)
    {
        await b._eventDict.SendEvent(StageEventDict.LOSE_BUFF, new LoseBuffDetails(b));
        b.Unregister();
        _buffs.Remove(b);
    }

    public async Task TryRemoveBuff(string buffName)
    {
        Buff b = FindBuff(buffName);
        if (b != null)
            await RemoveBuff(b);
    }

    public async Task RemoveAllBuffs()
    {
        await _buffs.Traversal().Do(async b =>
        {
            await b._eventDict.SendEvent(StageEventDict.LOSE_BUFF, new LoseBuffDetails(b));
            b.Unregister();
        });
        _buffs.Clear();
    }

    // public async Task RemoveBuffs(params string[] names)
    // {
    //     List<Buff> toRemove = _buffs.FilterObj(b => names.Contains(b.GetName())).ToList();
    //     await toRemove.Do(RemoveBuff);
    // }

    public Buff FindBuff(BuffEntry buffEntry) => Buffs.FirstObj(b => b.GetEntry() == buffEntry);

    public int GetStackOfBuff(BuffEntry entry) => FindBuff(entry)?.Stack ?? 0;

    public int GetSumOfStackOfBuffs(params string[] names)
        => names.Map(name => GetStackOfBuff(name)).Aggregate((a, b) => a + b);

    public int GetMana() => GetStackOfBuff("灵气");

    public async Task<bool> IsFocused()
    {
        if (GetStackOfBuff("永久集中") > 0) return true;
        return await TryConsumeProcedure("集中");
    }

    #endregion

    #region Procedure

    public async Task AttackProcedure(int value, WuXing? wuXing = null, int times = 1, bool lifeSteal = false, bool pierce = false, bool crit = false, bool recursive = true,
        Func<DamageDetails, Task> damaged = null, Func<DamageDetails, Task> undamaged = null)
        => await _env.AttackProcedure(new AttackDetails(this, Opponent(), value, wuXing, lifeSteal, pierce, crit, false, recursive, damaged, undamaged), times);

    public async Task IndirectProcedure(int value, WuXing? wuXing = null, bool recursive = true)
        => await _env.IndirectProcedure(new IndirectDetails(this, Opponent(), value, wuXing, recursive));

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

    public async Task DispelSelfProcedure(BuffEntry buffEntry, int stack, bool friendly, bool recursive = true)
        => await _env.DispelProcedure(new DispelDetails(this, this, buffEntry, stack, friendly, recursive));

    public async Task DispelOppoProcedure(BuffEntry buffEntry, int stack, bool friendly, bool recursive = true)
        => await _env.DispelProcedure(new DispelDetails(this, Opponent(), buffEntry, stack, friendly, recursive));

    public async Task FormationProcedure(FormationEntry formationEntry, bool recursive = true)
        => await _env.FormationProcedure(this, formationEntry, recursive);

    public async Task ManaShortageProcedure(int position, StageSkill skill, int actualCost)
        => await _env.ManaShortageProcedure(this, position, skill, actualCost);

    #endregion
}
