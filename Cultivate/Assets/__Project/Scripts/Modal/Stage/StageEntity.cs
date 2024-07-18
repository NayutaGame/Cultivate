
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class StageEntity : Addressable, StageEventListener
{
    public async Task TurnProcedure(int turnCount)
    {
        TurnDetails d = new TurnDetails(this, turnCount);
        ResetActionPoint();

        await _env.EventDict.SendEvent(StageEventDict.WIL_TURN, d);
        if (!d.Cancel)
            for (int i = 0; i < GetActionPoint(); i++)
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
        StageSkill skill = _skills[_p];
        ExecuteDetails d = new ExecuteDetails(this, skill);
        await _env.EventDict.SendEvent(StageEventDict.WIL_EXECUTE, d);

        CastResult firstCastResult = null;

        for (int i = 0; i < d.CastTimes; i++)
            firstCastResult ??= await CastProcedure(skill);

        WriteResultToSlot(skill.GetSlot(), _costResult, firstCastResult);

        await _env.EventDict.SendEvent(StageEventDict.DID_EXECUTE, d);
    }

    public async Task<CastResult> CastProcedure(StageSkill skill, bool recursive = true)
    {
        CastDetails d = new CastDetails(this, skill);
        await _env.EventDict.SendEvent(StageEventDict.WIL_CAST, d);
        
        
        // will cast report
        await _env.Play(new ShiftAnimation(), false);
        _env.Result.TryAppend($"{GetName()}使用了{skill.Entry.GetName()}");
        
        
        CastResult castResult = await skill.Entry.Cast(_env, this, skill, recursive);
        _env.Result.TryAppendNote(Index, skill, _costResult, castResult);
        
        
        _env.Result.TryAppend($"\n");
        // did cast report
        

        if (this == skill.Owner)
            skill.IncreaseCastedCount();
        await _env.EventDict.SendEvent(StageEventDict.DID_CAST, d);

        return castResult;
    }

    public async Task<CastResult> CastProcedureNoTween(StageSkill skill, bool recursive = true)
    {
        CastDetails d = new CastDetails(this, skill);
        await _env.EventDict.SendEvent(StageEventDict.WIL_CAST, d);

        
        // will cast report
        _env.Result.TryAppend($"{GetName()}使用了{skill.Entry.GetName()}");

        
        CastResult castResult = await skill.Entry.Cast(_env, this, skill, recursive);
        
        
        _env.Result.TryAppend($"\n");
        // did cast report
        
        
        if (this == skill.Owner)
            skill.IncreaseCastedCount();
        await _env.EventDict.SendEvent(StageEventDict.DID_CAST, d);

        return castResult;
    }

    private async Task StepProcedure()
    {
        StartStepDetails startD = new StartStepDetails(this, _p);
        await _env.EventDict.SendEvent(StageEventDict.WIL_STEP, startD);
        if (startD.Cancel)
            return;

        int dir = Forward ? 1 : -1;
        for (int i = 0; i < _skills.Length; i++)
        {
            _p += dir;

            bool within = 0 <= _p && _p < _skills.Length;
            if (!within)
            {
                _p = (_p + _skills.Length) % _skills.Length;
                await _env.EventDict.SendEvent(StageEventDict.DID_ROUND, new RoundDetails(this));
                await _env.EventDict.SendEvent(StageEventDict.WIL_ROUND, new RoundDetails(this));
            }

            if (_skills[_p].Exhausted)
                continue;

            if (await TryConsumeProcedure("跳卡牌"))
                continue;

            break;
        }

        await _env.EventDict.SendEvent(StageEventDict.DID_STEP, new EndStepDetails(this, _p));
    }

    private void WriteResultToSlot(SkillSlot slot, CostResult costResult, CastResult castResult)
    {
        slot.CostResult ??= costResult;
        slot.CastResult ??= castResult;
    }

    public MingYuan MingYuan;

    public Neuron<int, int> HpChangedNeuron;
    public Neuron<int> ArmorChangedNeuron;

    private int _hp;
    public int Hp
    {
        get => _hp;
        set
        {
            _hp = Mathf.Min(value, MaxHp);
            HpChangedNeuron.Invoke(_hp, _maxHp);
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
            ArmorChangedNeuron.Invoke(_armor);
        }
    }

    public StageSkill[] _skills;

    // public abstract GameObject GetPrefab();
    public string GetName() => _index == 0 ? "主场" : "客场";
    public EntitySlot Slot() => StageManager.Instance._slots[_index];
    public StageEntity Opponent() => _env.Entities[1 - _index];

    public int _p;
    private int _actionPoint;
    public int GetActionPoint() => _actionPoint;
    public void SetActionPoint(int value) => _actionPoint = Mathf.Max(_actionPoint, value);
    public void ResetActionPoint() => _actionPoint = 1;
    private CostResult _costResult;

    public bool IsLowHp
        => (float)Hp / MaxHp <= 0.5f;
    public bool Forward
        => GetStackOfBuff("鹤回翔") == 0;
    public int ExhaustedCount
        => CountSuch(skill => skill.Exhausted);
    public int AttackCount
        => CountSuch(skill => skill.GetSkillType().Contains(SkillType.Attack));
    public int CountSuch(Func<StageSkill, bool> pred)
        => _skills.Count(pred);

    public async Task<bool> OppoHasFragile(bool useFocus = false)
    {
        bool oppoHasFragile = Opponent().Armor < 0;
        if (!oppoHasFragile)
            oppoHasFragile = useFocus && await IsFocused();

        return oppoHasFragile;
    }

    public int LostArmorRecord;
    public int GeneratedManaRecord;
    public int HighestManaRecord;
    public int SelfDamageRecord;
    public int HealedRecord;
    public int GainedEvadeRecord;

    public int GainedFengRuiRecord;
    public int GainedGeDangRecord;
    public int GainedLiLiangRecord;
    public int GainedZhuoShaoRecord;
    public int GainedRouRenRecord;

    public bool HasChannelRecord;
    
    public bool HasZhiQiRecord;
    public bool HasChanRaoRecord;
    public bool HasRuanRuoRecord;
    public bool HasNeiShangRecord;
    public bool HasFuXiuRecord;

    public bool TriggeredJiaShiRecord;
    public bool TriggeredEndRecord;
    public bool TriggeredFirstTimeRecord;

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

        HpChangedNeuron = new();
        ArmorChangedNeuron = new();

        LostArmorRecord = 0;
        GeneratedManaRecord = 0;
        HighestManaRecord = 0;
        SelfDamageRecord = 0;
        HealedRecord = 0;
        GainedEvadeRecord = 0;

        GainedFengRuiRecord = 0;
        GainedGeDangRecord = 0;
        GainedLiLiangRecord = 0;
        GainedZhuoShaoRecord = 0;
        GainedRouRenRecord = 0;

        HasChannelRecord = false;
        
        HasZhiQiRecord = false;
        HasChanRaoRecord = false;
        HasRuanRuoRecord = false;
        HasNeiShangRecord = false;
        HasFuXiuRecord = false;
        
        TriggeredJiaShiRecord = false;
        TriggeredEndRecord = false;
        TriggeredFirstTimeRecord = false;

        _env = env;
        _runEntity = runEntity;
        _index = index;

        _formations = new();
        _buffs = new();

        _eventDescriptors = new StageEventDescriptor[]
        {
            new(StageEventDict.STAGE_ENTITY, StageEventDict.DID_GAIN_BUFF, -1, HighestManaRecorder),
            new(StageEventDict.STAGE_ENTITY, StageEventDict.DID_GAIN_BUFF, -1, GainedEvadeRecorder),
            new(StageEventDict.STAGE_ENTITY, StageEventDict.DID_GAIN_BUFF, -1, BuffRecorder),
            new(StageEventDict.STAGE_ENTITY, StageEventDict.WIL_TURN, -1, DefaultStartTurn),
        };

        foreach (var eventDescriptor in _eventDescriptors)
            _env.EventDict.Register(this, eventDescriptor);

        MingYuan = _runEntity.MingYuan.CloneMingYuan();
        MaxHp = _runEntity.GetFinalHealth();
        Hp = _runEntity.GetFinalHealth();
        Armor = 0;

        _skills = new StageSkill[_runEntity.GetSlotCount()];
        for (int i = 0; i < _skills.Length; i++)
        {
            SkillSlot slot = _runEntity.GetSlot(i + 0);
            _skills[i] = StageSkill.FromPlacedSkill(this, slot.PlacedSkill, i);
        }

        _manaShortageAction = StageSkill.FromSkillEntry(this, "0001");

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

    public async Task<GainBuffDetails> BuffRecorder(StageEventListener listener, EventDetails eventDetails)
    {
        GainBuffDetails d = (GainBuffDetails)eventDetails;
        if (d._buffEntry.GetName() == "锋锐")
            GainedFengRuiRecord += d._stack;
        else if (d._buffEntry.GetName() == "格挡")
            GainedGeDangRecord += d._stack;
        else if (d._buffEntry.GetName() == "力量")
            GainedLiLiangRecord += d._stack;
        else if (d._buffEntry.GetName() == "灼烧")
            GainedZhuoShaoRecord += d._stack;
        else if (d._buffEntry.GetName() == "柔韧")
            GainedRouRenRecord += d._stack;
        else if (d._buffEntry.GetName() == "滞气")
            HasZhiQiRecord = true;
        else if (d._buffEntry.GetName() == "缠绕")
            HasChanRaoRecord = true;
        else if (d._buffEntry.GetName() == "软弱")
            HasRuanRuoRecord = true;
        else if (d._buffEntry.GetName() == "内伤")
            HasNeiShangRecord = true;
        else if (d._buffEntry.GetName() == "腐朽")
            HasFuXiuRecord = true;

        return d;
    }

    public async Task<ChannelDetails> ChannelRecorder(StageEventListener listener, EventDetails eventDetails)
    {
        ChannelDetails d = (ChannelDetails)eventDetails;

        HasChannelRecord = true;
        
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

    public async Task<Buff> AddBuff(BuffAppearDetails d)
    {
        Buff buff = new Buff(this, d.Entry);
        buff.Register();
        await buff._eventDict.SendEvent(StageEventDict.BUFF_APPEAR, d);
        await buff.SetStack(d.InitialStack);
        _buffs.Add(buff);
        return buff;
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
        Func<DamageDetails, Task> willDamage = null, Func<DamageDetails, Task> undamaged = null, Func<DamageDetails, Task> didDamage = null, bool fromSeamless = false, bool induced = false)
        => await _env.AttackProcedure(new AttackDetails(this, Opponent(), value, wuXing, lifeSteal, pierce, crit, false, recursive, willDamage, undamaged, didDamage, fromSeamless), times, induced);

    public async Task IndirectProcedure(int value, WuXing? wuXing = null, bool recursive = true, bool induced = false)
        => await _env.IndirectProcedure(new IndirectDetails(this, Opponent(), value, wuXing, recursive), induced);

    public async Task DamageSelfProcedure(int value, bool recursive = true,
        Func<DamageDetails, Task> willDamage = null, Func<DamageDetails, Task> undamaged = null, Func<DamageDetails, Task> didDamage = null, bool induced = false)
        => await _env.DamageProcedure(new DamageDetails(this, this, value, crit: false, lifeSteal: false, recursive, willDamage, undamaged, didDamage), induced);

    public async Task DamageOppoProcedure(int value, bool recursive = true,
        Func<DamageDetails, Task> willDamage = null, Func<DamageDetails, Task> undamaged = null, Func<DamageDetails, Task> didDamage = null, bool induced = false)
        => await _env.DamageProcedure(new DamageDetails(this, Opponent(), value, crit: false, lifeSteal: false, recursive, willDamage, undamaged, didDamage), induced);

    public async Task LoseHealthProcedure(int value)
        => await _env.LoseHealthProcedure(new LoseHealthDetails(this, value));

    public async Task HealProcedure(int value, bool induced)
        => await _env.HealProcedure(new HealDetails(this, this, value), induced);

    public async Task HealOppoProcedure(int value, bool induced)
        => await _env.HealProcedure(new HealDetails(this, Opponent(), value), induced);

    public async Task GainArmorProcedure(int value, bool induced)
        => await _env.GainArmorProcedure(new GainArmorDetails(this, this, value), induced);

    public async Task GiveArmorProcedure(int value, bool induced)
        => await _env.GainArmorProcedure(new GainArmorDetails(this, Opponent(), value), induced);

    public async Task LoseArmorProcedure(int value)
        => await _env.LoseArmorProcedure(new LoseArmorDetails(this, this, value));

    public async Task RemoveArmorProcedure(int value)
        => await _env.LoseArmorProcedure(new LoseArmorDetails(this, Opponent(), value));

    public async Task GainBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.GainBuffProcedure(new GainBuffDetails(this, this, buffEntry, stack, recursive), induced);

    public async Task GiveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.GainBuffProcedure(new GainBuffDetails(this, Opponent(), buffEntry, stack, recursive), induced);

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
        {
            TriggeredJiaShiRecord = true;
            return true;
        }

        if (GetStackOfBuff("架势") > 0)
        {
            await LoseBuffProcedure("架势");
            TriggeredJiaShiRecord = true;
            return true;
        }

        if (await IsFocused())
        {
            TriggeredJiaShiRecord = true;
            return true;
        }

        await GainBuffProcedure("架势");
        return false;
    }

    public async Task BecomeLowHealth()
    {
        int gap = Hp - MaxHp / 2;
        if (gap > 0)
            await LoseHealthProcedure(gap);
    }

    #endregion
}
