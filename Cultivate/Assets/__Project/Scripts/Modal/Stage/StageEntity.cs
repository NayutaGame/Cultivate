
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class StageEntity : Addressable, StageEventListener
{
    public async Task TurnProcedure()
    {
        TurnDetails d = new TurnDetails(this);
        ActionPoint = 1;
        
        await _env.EventDict.SendEvent(StageEventDict.WIL_TURN, d);
        if (!d.Cancel)
            for (int i = 0; i < ActionPoint; i++)
                await ActionProcedure(i);
        
        await _env.EventDict.SendEvent(StageEventDict.DID_TURN, d);
    }

    private async Task ActionProcedure(int currActionCount)
    {
        ActionDetails d = new ActionDetails(this, currActionCount);
        await _env.EventDict.SendEvent(StageEventDict.WIL_ACTION, d);
        if (d.Cancel)
            return;

        if (!await CostProcedure()) return;
        await ExecuteProcedure();
        await StepProcedure();

        _costResult = null;
        
        await _env.EventDict.SendEvent(StageEventDict.DID_ACTION, d);
    }

    private async Task<bool> CostProcedure()
    {
        if (_costResult == null)
        {
            _costResult = await CostResult.FromEnvironment(_env, this, _skills[_p]);
            await _costResult.WillCostEvent();
        }
        
        await _costResult.ApplyCost();

        if (_costResult.Blocking)
            return false;
        
        await _costResult.DidCostEvent();
        return true;
    }

    private async Task ExecuteProcedure()
    {
        bool duoCast = GetStackOfBuff("永久二重") > 0 || await TryConsumeProcedure("二重");

        int multiCast = GetStackOfBuff("多重");
        await RemoveBuffProcedure("多重");

        int castCount = 1 + (duoCast ? 1 : 0) + multiCast;
        
        for (int i = 0; i < castCount; i++)
            await _skills[_p].Execute(this);
        
        SkillSlot slot = _skills[_p].GetSlot();
        slot.CostResult = _costResult;
        slot.ExecuteResult = null;
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
                await _env.EventDict.SendEvent(StageEventDict.DID_ROUND, new RoundDetails(this));
                await _env.EventDict.SendEvent(StageEventDict.WIL_ROUND, new RoundDetails(this));
            }

            if(_skills[_p].Exhausted)
                continue;

            if(await TryConsumeProcedure("跳卡牌"))
                continue;

            break;
        }
        
        await _env.EventDict.SendEvent(StageEventDict.END_STEP, new EndStepDetails(this, _p));
    }
    
    public MingYuan MingYuan;

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

    // public abstract GameObject GetPrefab();
    public string GetName() => _index == 0 ? "主场" : "客场";
    public EntitySlot Slot() => StageManager.Instance._slots[_index];
    public StageEntity Opponent() => _env.Entities[1 - _index];

    public int _p;
    private int _actionPoint;
    public int ActionPoint
    {
        get => _actionPoint;
        set => _actionPoint = Mathf.Max(_actionPoint, value);
    }
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
            new(StageEventDict.STAGE_ENTITY, StageEventDict.WIL_TURN, 0, DefaultStartTurn),
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
