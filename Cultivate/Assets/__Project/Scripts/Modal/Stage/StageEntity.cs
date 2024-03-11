
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

    public async Task TurnProcedure()
    {
        Swift = false;
        TriSwift = false;
        OctSwift = false;

        await _env.EventDict.SendEvent(StageEventDict.START_TURN, new TurnDetails(this, _p));

        bool skipTurn = await TryConsumeProcedure("跳回合");
        if (skipTurn)
        {
            if (GetStackOfBuff("浮空艇") > 0)
                await GainBuffProcedure("回合免疫");
            await _env.EventDict.SendEvent(StageEventDict.END_TURN, new TurnDetails(this, _p));
            return;
        }

        await ActionProcedure();

        if (Swift || TriSwift || OctSwift)
            await SwiftProcedure(new SwiftDetails(this, Swift, TriSwift, OctSwift));

        await _env.EventDict.SendEvent(StageEventDict.END_TURN, new TurnDetails(this, _p));
    }

    private async Task ActionProcedure()
    {
        if (_costResult == null)
        {
            _costResult = await CostResult.FromEnvironment(_env, this, _skills[_p]);
            await _costResult.WillCostEvent(); // write value
        }
        
        await _costResult.ApplyCost(); // if shortage, write state

        if (_costResult.Blocking)
            return;
        await _costResult.DidCostEvent();
        
        await ExecuteProcedure(_skills[_p]);
        
        await StepProcedure();

        _costResult = null;
    }

    private async Task ExecuteProcedure(StageSkill skill)
    {
        bool duoCast = GetStackOfBuff("永久二重") > 0 || await TryConsumeProcedure("二重");

        int multiCast = GetStackOfBuff("多重");
        await RemoveBuffProcedure("多重");
        
        if (duoCast)
            await skill.Execute(this);

        for (int i = 0; i < multiCast; i++)
            await skill.Execute(this);
        
        await skill.Execute(this);
    }

    private async Task StepProcedure()
    {
        await _env.EventDict.SendEvent(StageEventDict.START_STEP, new StartStepDetails(this, _p));
        
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

            break;
        }
        
        await _env.EventDict.SendEvent(StageEventDict.END_STEP, new EndStepDetails(this, _p));
    }

    private async Task SwiftProcedure(SwiftDetails d)
    {
        await _env.EventDict.SendEvent(StageEventDict.WILL_SWIFT, d);
        if (d.Cancel)
            return;

        if (d.Swift || d.TriSwift || d.OctSwift)
            await ActionProcedure();

        if (d.TriSwift || d.OctSwift)
            await ActionProcedure();

        if (d.OctSwift)
        {
            await ActionProcedure();
            await ActionProcedure();
            await ActionProcedure();
            await ActionProcedure();
            await ActionProcedure();
        }

        await _env.EventDict.SendEvent(StageEventDict.DID_SWIFT, d);
    }

    // public abstract GameObject GetPrefab();
    public string GetName() => _index == 0 ? "主场" : "客场";
    public EntitySlot Slot() => StageManager.Instance._slots[_index];
    public StageEntity Opponent() => _env.Entities[1 - _index];

    public int _p;
    public bool Swift;
    public bool TriSwift;
    public bool OctSwift;
    private CostResult _costResult;

    public bool Forward
        => GetStackOfBuff("鹤回翔") == 0;
    public int ExhaustedCount
        => _skills.Count(skill => skill.Exhausted);
    public int AttackCount
        => _skills.Count(skill => skill.GetSkillType().Contains(SkillType.Attack));
    public bool HasJiaShi
        => GetStackOfBuff("架势") > 0;

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

    public IEnumerable<RunFormation> RunFormations() => _runEntity.TraversalFormations;

    private StageEnvironment _env;
    public StageEnvironment Env => _env;

    private StageEventDescriptor[] _eventDescriptors;

    private StageSkill _manaShortageAction;
    public StageSkill ManaShortageAction => _manaShortageAction;

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

        LostArmorRecord = 0;
        GeneratedManaRecord = 0;
        HighestManaRecord = 0;
        SelfDamageRecord = 0;
        HealedRecord = 0;

        _eventDescriptors = new StageEventDescriptor[]
        {
            new(StageEventDict.STAGE_ENTITY, StageEventDict.BUFF_DID_GAIN, 0, HighestManaRecorder),
            new(StageEventDict.STAGE_ENTITY, StageEventDict.BUFF_DID_GAIN, 0, GainedEvadeRecorder),
            new(StageEventDict.STAGE_ENTITY, StageEventDict.BUFF_DID_GAIN, 0, GainedBurningRecorder),
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
            _skills[i] = StageSkill.FromPlacedSkill(this, slot.PlacedSkill, i);
        }

        _manaShortageAction = StageSkill.FromSkillEntry(this, "聚气术");

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

    public async Task<GainBuffDetails> HighestManaRecorder(StageEventListener listener, EventDetails eventDetails)
    {
        GainBuffDetails d = (GainBuffDetails)eventDetails;
        if (d._buffEntry.GetName() != "灵气")
            return d;

        HighestManaRecord = Mathf.Max(HighestManaRecord, GetStackOfBuff("灵气"));
        return d;
    }

    public async Task<GainBuffDetails> GainedEvadeRecorder(StageEventListener listener, EventDetails eventDetails)
    {
        GainBuffDetails d = (GainBuffDetails)eventDetails;
        if (d._buffEntry.GetName() != "闪避")
            return d;

        GainedEvadeRecord += d._stack;
        return d;
    }

    public async Task<GainBuffDetails> GainedBurningRecorder(StageEventListener listener, EventDetails eventDetails)
    {
        GainBuffDetails d = (GainBuffDetails)eventDetails;
        if (d._buffEntry.GetName() != "灼烧")
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
        Formation formation = new Formation(this, d._formation);
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

    public async Task AddBuff(BuffAppearDetails d)
    {
        Buff buff = new Buff(this, d._entry);
        buff.Register();
        await buff._eventDict.SendEvent(StageEventDict.BUFF_APPEAR, d);
        await buff.SetStack(d._initialStack);
        _buffs.Add(buff);
    }

    public async Task RemoveBuff(Buff b)
    {
        await b._eventDict.SendEvent(StageEventDict.BUFF_DISAPPEAR, new BuffDisappearDetails(b));
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
            await b._eventDict.SendEvent(StageEventDict.BUFF_DISAPPEAR, new BuffDisappearDetails(b));
            b.Unregister();
        });
        _buffs.Clear();
    }

    public Buff FindBuff(BuffEntry buffEntry) => Buffs.FirstObj(b => b.GetEntry() == buffEntry);

    public int GetStackOfBuff(BuffEntry entry) => FindBuff(entry)?.Stack ?? 0;

    public int GetMana() => GetStackOfBuff("灵气");

    public async Task<bool> IsFocused()
    {
        if (GetStackOfBuff("永久集中") > 0)
            return true;
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

    public async Task HealOppoProcedure(int value)
        => await _env.HealProcedure(new HealDetails(this, Opponent(), value));

    public async Task GainArmorProcedure(int value)
        => await _env.GainArmorProcedure(new GainArmorDetails(this, this, value));

    public async Task GiveArmorProcedure(int value)
        => await _env.GainArmorProcedure(new GainArmorDetails(this, Opponent(), value));

    public async Task LoseArmorProcedure(int value)
        => await _env.LoseArmorProcedure(new LoseArmorDetails(this, this, value));

    public async Task RemoveArmorProcedure(int value)
        => await _env.LoseArmorProcedure(new LoseArmorDetails(this, Opponent(), value));

    public async Task GainBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await _env.GainBuffProcedure(new GainBuffDetails(this, this, buffEntry, stack, recursive));

    public async Task GiveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await _env.GainBuffProcedure(new GainBuffDetails(this, Opponent(), buffEntry, stack, recursive));

    public async Task LoseBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await _env.LoseBuffProcedure(new LoseBuffDetails(this, this, buffEntry, stack, recursive));

    public async Task RemoveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await _env.LoseBuffProcedure(new LoseBuffDetails(this, Opponent(), buffEntry, stack, recursive));

    public async Task FormationProcedure(RunFormation runFormation, bool recursive = true)
        => await _env.FormationProcedure(this, runFormation, recursive);

    public async Task ManaShortageProcedure(int position, StageSkill skill, int actualCost)
        => await _env.ManaShortageProcedure(this, position, skill, actualCost);

    public async Task CycleProcedure(WuXing wuXing, int gain = 0, int recover = 0)
        => await _env.CycleProcedure(this, wuXing, gain, recover);
    
    public async Task DispelProcedure(int stack)
        => await _env.DispelProcedure(this, stack);

    public async Task<bool> TryConsumeProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
    {
        if (stack == 0)
            return true;

        Buff b = FindBuff(buffEntry);
        if (b != null && b.Stack >= stack)
        {
            await LoseBuffProcedure(buffEntry, stack, recursive);
            return true;
        }

        return false;
    }

    public async Task TransferProcedure(int fromStack, BuffEntry fromBuff, int toStack, BuffEntry toBuff, bool consuming)
    {
        int flow = GetStackOfBuff(fromBuff) / fromStack;
        if (consuming)
            await LoseBuffProcedure(fromBuff, flow * fromStack);

        await GainBuffProcedure(toBuff, flow * toStack);
    }

    public async Task<bool> ToggleJiaShiProcedure()
    {
        if (GetStackOfBuff("天人合一") > 0)
            return true;

        if (GetStackOfBuff("架势") > 0)
        {
            await LoseBuffProcedure("架势");
            return true;
        }

        if (await IsFocused())
            return true;

        await GainBuffProcedure("架势");
        return false;
    }

    #endregion
}
