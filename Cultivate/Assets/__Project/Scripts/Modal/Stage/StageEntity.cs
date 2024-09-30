
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class StageEntity : Addressable, StageClosureOwner
{
    public Memory Memory;
    
    public async Task TurnProcedure(int turnCount)
    {
        TurnDetails d = new TurnDetails(this, turnCount);
        ResetActionPoint();

        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_TURN, d);
        if (!d.Cancel)
            for (int i = 0; i < GetActionPoint(); i++)
                await ActionProcedure(i);

        await _env.ClosureDict.SendEvent(StageClosureDict.DID_TURN, d);
    }

    private async Task ActionProcedure(int currActionCount)
    {
        ActionDetails d = new ActionDetails(this, currActionCount);
        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_ACTION, d);
        if (d.Cancel)
            return;

        if (!await CostProcedure()) return;
        await ExecuteProcedure();
        await StepProcedure();

        _costResult = null;

        await _env.ClosureDict.SendEvent(StageClosureDict.DID_ACTION, d);
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

    public async Task StartStageExecuteProcedure()
    {
        foreach (var skill in _skills)
        {
            if (!skill.Entry.HasStartStageCast()) continue;
            await StartStageCastProcedure(skill);
        }
    }

    private async Task ExecuteProcedure()
    {
        StageSkill skill = _skills[_p];
        ExecuteDetails d = new ExecuteDetails(this, skill);
        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_EXECUTE, d);

        CastResult firstCastResult = await CastProcedure(skill);
        for (int i = 1; i < d.CastTimes; i++) await CastProcedure(skill);

        WriteResultToSlot(skill.GetSlot(), _costResult, firstCastResult);

        await _env.ClosureDict.SendEvent(StageClosureDict.DID_EXECUTE, d);
    }

    private async Task StartStageCastProcedure(StageSkill skill, bool recursive = true)
    {
        StartStageCastDetails d = new StartStageCastDetails(_env, this, skill, recursive, new());
        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_START_STAGE_CAST, d);

        for (int i = 0; i < d.Times; i++)
        {
            await _env.Play(new ShiftAnimation(), false);
            _env.Result.TryAppend($"{GetName()}使用了{skill.Entry.GetName()}的开局效果");

            await skill.Entry.StartStageCast(d);
            _env.Result.TryAppendNote(Index, skill, _costResult, null);
            _env.Result.TryAppend($"\n");
        }
        
        await _env.ClosureDict.SendEvent(StageClosureDict.DID_START_STAGE_CAST, d);
    }

    public async Task<CastResult> CastProcedure(StageSkill skill, bool recursive = true)
    {
        CastResult castResult = new();
        CastDetails d = new CastDetails(_env, this, skill, recursive, castResult);
        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_CAST, d);
        
        
        // will cast report
        await _env.Play(new ShiftAnimation(), false);
        _env.Result.TryAppend($"{GetName()}使用了{skill.Entry.GetName()}");
        
        await skill.Entry.Cast(d);
        _env.Result.TryAppendNote(Index, skill, _costResult, castResult);
        
        
        _env.Result.TryAppend($"\n");
        // did cast report
        

        if (this == skill.Owner)
            skill.IncreaseCastedCount();
        await _env.ClosureDict.SendEvent(StageClosureDict.DID_CAST, d);

        return castResult;
    }

    public async Task<CastResult> CastProcedureNoTween(StageSkill skill, bool recursive = true)
    {
        CastResult castResult = new();
        CastDetails d = new CastDetails(_env, this, skill, recursive, castResult);
        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_CAST, d);

        
        // will cast report
        _env.Result.TryAppend($"{GetName()}使用了{skill.Entry.GetName()}");

        
        await skill.Entry.Cast(d);
        
        _env.Result.TryAppend($"\n");
        // did cast report
        
        
        if (this == skill.Owner)
            skill.IncreaseCastedCount();
        await _env.ClosureDict.SendEvent(StageClosureDict.DID_CAST, d);

        return castResult;
    }

    private async Task StepProcedure()
    {
        StartStepDetails startD = new StartStepDetails(this, _p);
        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_STEP, startD);
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
                await _env.ClosureDict.SendEvent(StageClosureDict.DID_ROUND, new RoundDetails(this));
                await _env.ClosureDict.SendEvent(StageClosureDict.WIL_ROUND, new RoundDetails(this));
            }

            if (_skills[_p].Exhausted)
                continue;

            if (await TryConsumeProcedure("飞龙在天"))
            {
                _skills[_p].IncreaseCastedCount();
                continue;
            }

            if (await TryConsumeProcedure("跳卡牌"))
                continue;

            break;
        }

        await _env.ClosureDict.SendEvent(StageClosureDict.DID_STEP, new EndStepDetails(this, _p));
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
    // TODO proceduralize
    public void SetActionPoint(int value) => _actionPoint = Mathf.Max(_actionPoint, value);
    public void ResetActionPoint() => _actionPoint = 1;
    private CostResult _costResult;

    public int GetLowHealthThreshold()
        => Mathf.RoundToInt((25 + GetStackOfBuff("锻体")) * 0.01f * MaxHp).Clamp(0, MaxHp);

    public bool IsLowHealth
        => Hp <= GetLowHealthThreshold();
    public bool IsOverpower
        => Hp >= Opponent().Hp;
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

    private StageClosure[] _closures;

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

        Memory = new();

        HpChangedNeuron = new();
        ArmorChangedNeuron = new();

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

        _closures = new StageClosure[]
        {
            new(StageClosureDict.DID_GAIN_BUFF, -1, BuffRecorder),
        };
        
        _env.ClosureDict.Register(this, _closures);

        MingYuan = _runEntity.MingYuan.CloneMingYuan();
        MaxHp = _runEntity.GetFinalHealth();
        Hp = _runEntity.GetFinalHealth();
        Armor = 0;

        _skills = new StageSkill[_runEntity.GetSlotCount()];
        for (int i = 0; i < _skills.Length; i++)
        {
            SkillSlot slot = _runEntity.GetSlot(i + 0);
            _skills[i] = StageSkill.FromPlacedSkill(this, i, slot.PlacedSkill);
        }

        _manaShortageAction = StageSkill.FromSkillEntry(this, "0001");

        _p = 0;
    }

    ~StageEntity()
    {
        RemoveAllFormations().GetAwaiter().GetResult();
        RemoveAllBuffs().GetAwaiter().GetResult();
        
        _env.ClosureDict.Unregister(this, _closures);
    }

    public void WriteResult()
    {
        // for (int i = 0; i < _skills.Length; i++)
        // {
        //     SkillSlot slot = _runEntity.GetSlot(i + _runEntity.Start);
        // }
    }

    public async Task BuffRecorder(StageClosureOwner listener, ClosureDetails closureDetails)
    {
        GainBuffDetails d = (GainBuffDetails)closureDetails;
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
    }

    public async Task ChannelRecorder(StageClosureOwner owner, ClosureDetails closureDetails)
    {
        ChannelDetails d = (ChannelDetails)closureDetails;
        HasChannelRecord = true;
    }

    #region Formation

    private ListModel<Formation> _formations;

    public async Task AddFormation(GainFormationDetails d)
    {
        Formation formation = new Formation(this, d._formation);
        formation.Register();
        await Env.ClosureDict.SendEvent(StageClosureDict.GAIN_FORMATION, d);
        _formations.Add(formation);
    }

    public async Task RemoveFormation(Formation f)
    {
        await Env.ClosureDict.SendEvent(StageClosureDict.LOSE_FORMATION, new LoseFormationDetails(f));
        f.Unregister();
        _formations.Remove(f);
    }

    public async Task RemoveAllFormations()
    {
        await _formations.Traversal().Do(async f =>
        {
            await Env.ClosureDict.SendEvent(StageClosureDict.LOSE_FORMATION, new LoseFormationDetails(f));
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
        await Env.ClosureDict.SendEvent(StageClosureDict.BUFF_APPEAR, d);
        await buff.SetStack(d.InitialStack);
        _buffs.Add(buff);
        return buff;
    }

    public async Task RemoveBuff(Buff b)
    {
        await Env.ClosureDict.SendEvent(StageClosureDict.BUFF_DISAPPEAR, new BuffDisappearDetails(b));
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
            await Env.ClosureDict.SendEvent(StageClosureDict.BUFF_DISAPPEAR, new BuffDisappearDetails(b));
            b.Unregister();
        });
        _buffs.Clear();
    }

    public Buff FindBuff(BuffEntry buffEntry) => Buffs.FirstObj(b => b.GetEntry() == buffEntry);

    public int GetStackOfBuff(BuffEntry entry) => FindBuff(entry)?.Stack ?? 0;

    public async Task<bool> IsFocused()
    {
        if (GetStackOfBuff("永久集中") > 0 || GetStackOfBuff("通透世界") > 0)
            return true;
        return await TryConsumeProcedure("集中");
    }

    #endregion

    #region Procedure
    
    public async Task AttackProcedure(int value,
        int times = 1,
        StageSkill srcSkill = null,
        WuXing? wuXing = null,
        bool crit = false,
        bool lifeSteal = false,
        bool penetrate = false,
        bool recursive = true,
        CastResult castResult = null,
        StageClosure[] closures = null,
        bool induced = false)
        => await _env.AttackProcedure(new AttackDetails(this, Opponent(), value, times, srcSkill, wuXing, crit, lifeSteal, penetrate, false, recursive, castResult, closures), induced);
    
    public async Task IndirectProcedure(int value, StageSkill srcSkill = null, CastResult castResult = null, WuXing? wuXing = null, bool recursive = true, bool induced = false)
        => await _env.IndirectProcedure(new IndirectDetails(this, Opponent(), value, srcSkill, wuXing, recursive), castResult, induced);
    
    public async Task DamageSelfProcedure(int value, StageSkill srcSkill = null, CastResult castResult = null, bool recursive = true, bool induced = false)
        => await _env.DamageProcedure(new DamageDetails(this, this, value, srcSkill, crit: false, lifeSteal: false, recursive, castResult), induced);
    
    public async Task DamageOppoProcedure(int value, StageSkill srcSkill, CastResult castResult, bool recursive = true, bool induced = false)
        => await _env.DamageProcedure(new DamageDetails(this, Opponent(), value, srcSkill, crit: false, lifeSteal: false, recursive, castResult), induced);
    
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
    
    public async Task CycleProcedure(WuXing wuXing, int gain = 0, int recover = 0, bool induced = false)
        => await _env.CycleProcedure(this, wuXing, gain, recover, induced);
    
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
    
    public async Task TransferProcedure(int fromStack, BuffEntry fromBuff, int toStack, BuffEntry toBuff, bool consuming, int? maxFlow = null, int? upperBound = null)
    {
        int flow = GetStackOfBuff(fromBuff) / fromStack;
        if (upperBound.HasValue)
        {
            int gap = upperBound.Value - GetStackOfBuff(toBuff);
            if (gap >= 0)
                flow = flow.ClampUpper(gap);
        }
        
        if (maxFlow.HasValue)
            flow = flow.ClampUpper(maxFlow.Value);
        
        if (consuming)
            await LoseBuffProcedure(fromBuff, flow * fromStack);
    
        await GainBuffProcedure(toBuff, flow * toStack);
    }
    
    public async Task<bool> JiaShiProcedure()
    {
        // if (GetStackOfBuff("天人合一") > 0)
        // {
        //     TriggeredJiaShiRecord = true;
        //     return true;
        // }
    
        if (GetStackOfBuff("架势") > 0)
        {
            await LoseBuffProcedure("架势");
            TriggeredJiaShiRecord = true;
            return true;
        }
    
        // if (await IsFocused())
        // {
        //     TriggeredJiaShiRecord = true;
        //     return true;
        // }
    
        await GainBuffProcedure("架势");
        return false;
    }
    
    public async Task BecomeLowHealth()
    {
        int gap = Hp - GetLowHealthThreshold();
        if (gap > 0)
            await LoseHealthProcedure(gap);
    }

    #endregion
}
