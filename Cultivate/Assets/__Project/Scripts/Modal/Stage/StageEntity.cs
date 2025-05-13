
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class StageEntity : Addressable, StageClosureListener
{
    public Memory Memory;
    
    public async UniTask TurnProcedure(int turnCount)
    {
        TurnDetails d = new TurnDetails(this, turnCount);
        ResetActionPoint();

        string thisTurnAttackedKey = "thisTurnAttacked";
        string thisTurnPreserveJianYiKey = "thisTurnPreserveJianYi";

        Memory.SetVariable(thisTurnAttackedKey, false);
        Memory.SetVariable(thisTurnPreserveJianYiKey, false);

        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_TURN, d);
        if (!d.Cancel)
            for (int i = 0; i < GetActionPoint(); i++)
                await ActionProcedure(i);

        await _env.ClosureDict.SendEvent(StageClosureDict.DID_TURN, d);
    }

    private async UniTask ActionProcedure(int currActionCount)
    {
        ActionDetails d = new ActionDetails(this, currActionCount);
        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_ACTION, d);
        if (d.Cancel)
            return;

        if (await CostProcedure()) return;
        await ExecuteProcedure();
        await StepProcedure();

        _costDefinition = null;
        _costDetails.Clear();
        _costDetails = null;

        await _env.ClosureDict.SendEvent(StageClosureDict.DID_ACTION, d);
    }

    private async UniTask<bool> CostProcedure()
    {
        if (_costDefinition == null)
        {
            StageSkill skill = _skills[_p];
            _costDetails = new(_env, this, skill);
            _costDefinition = skill.GetSkillDefinition().GetCostDefinition();
            _costDetails.CostDescription = _costDefinition.GetLiteralCostDescription();

            await _costDefinition.WillCostEvent(_costDetails);
        }

        await _costDefinition.ApplyCost(_costDetails);

        if (_costDetails.Blocking)
            return true;

        await _costDefinition.DidCostEvent(_costDetails);
        return false;
    }

    public async UniTask StartStageExecuteProcedure()
    {
        foreach (var skill in _skills)
        {
            if (!skill.Entry.HasStartStageCast()) continue;
            await StartStageCastProcedure(skill);
        }
    }

    private async UniTask ExecuteProcedure()
    {
        StageSkill skill = _skills[_p];
        ExecuteDetails d = new ExecuteDetails(this, skill);
        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_EXECUTE, d);

        for (int i = 0; i < d.CastTimes; i++)
            await CastProcedure(skill, shouldWriteToSlot: i == 0);

        await _env.ClosureDict.SendEvent(StageClosureDict.DID_EXECUTE, d);
    }

    private async UniTask StartStageCastProcedure(StageSkill skill, bool recursive = true)
    {
        ResultDict castResult = new();
        CastDetails castDetails = new CastDetails(_env, this, skill, recursive, fromWanJian: false, isStartStage: true, startStageCastTimes: 1, castResult: castResult);
        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_START_STAGE_CAST, castDetails);

        for (int i = 0; i < castDetails.StartStageCastTimes; i++)
        {
            await _env.PlayAsync(new ShiftAnimation());
            
            _env.Result.TryAppend($"{GetName()}使用了{castDetails.Skill.Entry.GetName()}的开局效果");
            SkillDefinition skillDefinition = skill.GetSkillDefinition();
            await skillDefinition.Cast(castDetails);

            CostDescription actualCostDescription = CostDescription.Empty;
            string actualDescription = skillDefinition.GetActualDescriptionHighlighted(_costDetails?.CostResult, castResult);
            _env.Result.TryAppendNote(Index, castDetails.Skill, actualCostDescription, actualDescription);
            _env.Result.TryAppend($"\n");
        }
        
        if (this == castDetails.Skill.Owner)
            castDetails.Skill.IncreaseRealCastedCount();
        
        await _env.ClosureDict.SendEvent(StageClosureDict.DID_START_STAGE_CAST, castDetails);

        castDetails.Clear();
    }

    public async UniTask CastProcedure(StageSkill skill, bool recursive = true, bool fromWanJian = false, bool shouldWriteToSlot = false)
    {
        ResultDict castResult = new();
        CastDetails castDetails = new CastDetails(_env, this, skill, recursive, fromWanJian, isStartStage: false, startStageCastTimes: 1, castResult: castResult);
        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_CAST, castDetails);
        
        await _env.PlayAsync(new ShiftAnimation());
        _env.Result.TryAppend($"{GetName()}使用了{castDetails.Skill.Entry.GetName()}");
        
        SkillDefinition skillDefinition = skill.GetSkillDefinition();
        await skillDefinition.Cast(castDetails);
        
        CostDescription actualCostDescription = _costDetails.CostDescription.Clone();
        string actualDescription = skillDefinition.GetActualDescriptionHighlighted(_costDetails?.CostResult, castResult);
        _env.Result.TryAppendNote(Index, castDetails.Skill, actualCostDescription, actualDescription);
        _env.Result.TryAppend($"\n");

        TryWriteResultToSlot(shouldWriteToSlot, skill, actualCostDescription, actualDescription);

        if (this == castDetails.Skill.Owner)
            castDetails.Skill.IncreaseRealCastedCount();
        
        await _env.ClosureDict.SendEvent(StageClosureDict.DID_CAST, castDetails);
        
        castDetails.Clear();
    }

    private void TryWriteResultToSlot(bool shouldWriteToSlot, StageSkill skill, CostDescription actualCostDescription, string actualDescription)
    {
        if (!shouldWriteToSlot) return;
        SkillSlot slot = skill.GetSlot();
        if (slot == null) return;
        if (!SlotIsUnwritten(slot)) return;
        
        WriteResultToSlot(slot, actualCostDescription, actualDescription);
    }

    private async UniTask StepProcedure()
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
                _skills[_p].IncreaseBonusCastedCount();
                continue;
            }

            if (await TryConsumeProcedure("跳卡牌"))
                continue;

            break;
        }

        await _env.ClosureDict.SendEvent(StageClosureDict.DID_STEP, new EndStepDetails(this, _p));
    }

    private bool SlotIsUnwritten(SkillSlot slot)
        => slot.ActualCostDescription == null;

    private void WriteResultToSlot(SkillSlot slot, CostDescription actualCostDescription, string actualDescription)
    {
        slot.ActualCostDescription = actualCostDescription;
        slot.ActualDescription = actualDescription;
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

    // public abstract GameObject GetPrefab();
    public string GetName() => _index == 0 ? "主场" : "客场";
    public StageEntity Opponent() => _env.Entities[1 - _index];
    public IStageModel Model() => _index == 0 ? StageManager.Instance.HomeModel : StageManager.Instance.AwayModel;

    public int _p;
    private int _actionPoint;
    public int GetActionPoint() => _actionPoint;
    public void SetActionPoint(int value) => _actionPoint = Mathf.Max(_actionPoint, value);
    public void ResetActionPoint() => _actionPoint = 1;
    private CostDetails _costDetails;
    private CostDefinition _costDefinition;

    public int GetFullHealthThreshold()
        => Mathf.RoundToInt((100 - GetStackOfBuff("锻体")) * 0.01f * MaxHp).Clamp(0, MaxHp);
    public int GetLowHealthThreshold()
        => Mathf.RoundToInt((25 + GetStackOfBuff("锻体")) * 0.01f * MaxHp).Clamp(0, MaxHp);

    public bool IsFullHealth
        => Hp >= GetFullHealthThreshold() || GetStackOfBuff("天人形态") > 0;
    public bool IsLowHealth
        => Hp <= GetLowHealthThreshold() || GetStackOfBuff("天人形态") > 0;
    public bool Forward
        => GetStackOfBuff("鹤回翔") == 0;
    public int ExhaustedCount
        => TraversalSkills().Count(skill => skill.Exhausted);
    public int AttackCount
        => TraversalSkills().Count(skill => skill.GetSkillType().Contains(SkillType.Attack));

    public async UniTask<bool> OppoHasFragile(bool useFocus = false)
    {
        bool oppoHasFragile = Opponent().Armor < 0;
        if (!oppoHasFragile)
            oppoHasFragile = useFocus && await IsFocused();

        return oppoHasFragile;
    }

    public bool HasChannelRecord;
    
    public bool HasZhiQiRecord;
    public bool HasChanRaoRecord;
    public bool HasRuanRuoRecord;
    public bool HasNeiShangRecord;
    public bool HasFuXiuRecord;

    public bool TriggeredEndRecord;
    public bool TriggeredFirstTimeRecord;

    public bool DeathCauseIsAttack;

    private int _index;
    public int Index => _index;

    private RunEntity _runEntity;
    public RunEntity RunEntity => _runEntity;

    public JingJie GetJingJie() => _runEntity.GetJingJie();

    public IEnumerable<RunFormation> RunFormations() => _runEntity.TraversalFormations;

    private StageEnvironment _env;
    public StageEnvironment Env => _env;

    private StageClosure[] _closures;

    private StageSkill _emptyAction;
    public StageSkill EmptyAction => _emptyAction;

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

        HasChannelRecord = false;
        
        HasZhiQiRecord = false;
        HasChanRaoRecord = false;
        HasRuanRuoRecord = false;
        HasNeiShangRecord = false;
        HasFuXiuRecord = false;
        
        TriggeredEndRecord = false;
        TriggeredFirstTimeRecord = false;

        DeathCauseIsAttack = false;

        _env = env;
        _runEntity = runEntity;
        _index = index;

        _formations = new();
        _buffs = new();

        _closures = new StageClosure[]
        {
            new(StageClosureDict.DID_GAIN_BUFF, -1, BuffRecorder),
            new(StageClosureDict.DID_CHANNEL, -1, ChannelRecorder),
        };
        
        _env.ClosureDict.Register(this, _closures);

        MingYuan = _runEntity.GetMingYuan().CloneMingYuan();
        MaxHp = _runEntity.GetHealth();
        Hp = _runEntity.GetHealth();
        Armor = 0;

        _skills = new StageSkill[_runEntity.GetSlotCount()];
        for (int i = 0; i < _skills.Length; i++)
        {
            SkillSlot slot = _runEntity.GetSlot(i + 0);
            _skills[i] = StageSkill.FromPlacedSkill(this, i, slot.PlacedSkill);
        }

        _emptyAction = StageSkill.FromSkillEntry(this, SkillEntry.FromName("发呆"));
        _manaShortageAction = StageSkill.FromSkillEntry(this, SkillEntry.FromName("灵气匮乏"));

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

    public async UniTask BuffRecorder(StageClosureListener listener, StageClosure closure, ClosureDetails closureDetails)
    {
        GainBuffDetails d = (GainBuffDetails)closureDetails;
        if (d.BuffEntry.GetName() == "滞气")
            HasZhiQiRecord = true;
        else if (d.BuffEntry.GetName() == "缠绕")
            HasChanRaoRecord = true;
        else if (d.BuffEntry.GetName() == "软弱")
            HasRuanRuoRecord = true;
        else if (d.BuffEntry.GetName() == "内伤")
            HasNeiShangRecord = true;
        else if (d.BuffEntry.GetName() == "腐朽")
            HasFuXiuRecord = true;
    }

    public async UniTask ChannelRecorder(StageClosureListener listener, StageClosure closure, ClosureDetails closureDetails)
    {
        ChannelDetails d = (ChannelDetails)closureDetails;
        HasChannelRecord = true;
    }

    #region Skill
    
    public StageSkill[] _skills;
    public IEnumerable<StageSkill> TraversalSkills()
        => _skills.Traversal();

    #endregion

    #region Formation

    private ListModel<Formation> _formations;
    public IEnumerable<Formation> TraversalFormations()
        => _formations.Traversal();

    public void AddFormation(Formation f)
    {
        f.Register();
        _formations.Add(f);
    }

    public void RemoveFormation(Formation f)
    {
        f.Unregister();
        _formations.Remove(f);
    }

    public async UniTask RemoveAllFormations()
    {
        await _formations.Traversal().Do(async f => f.Unregister());
        _formations.Clear();
    }

    #endregion

    #region Buff

    private ListModel<Buff> _buffs;
    public IEnumerable<Buff> TraversalBuffs()
        => _buffs.Traversal();
    public int IndexOfBuff(Buff b) => _buffs.IndexOf(b);

    public void AddBuff(Buff b)
    {
        b.Register();
        _buffs.Add(b);
    }

    public void RemoveBuff(Buff b)
    {
        b.Unregister();
        _buffs.Remove(b);
    }

    public async UniTask RemoveAllBuffs()
    {
        await _buffs.Traversal().Do(async b => b.Unregister());
        _buffs.Clear();
    }

    public Buff FindBuff(BuffEntry buffEntry) => TraversalBuffs().FirstObj(b => b.GetEntry() == buffEntry);

    public int GetStackOfBuff(BuffEntry entry) => FindBuff(entry)?.Stack ?? 0;

    public Buff GetHighestWuXingBuff()
    {
        Buff highestBuff = WuXing.Traversal
            .Map(wuXing => FindBuff(wuXing._elementaryBuff))
            .MinObj(b => -b.Stack);

        return highestBuff;
    }

    public WuXing? GetHighestWuXing()
    {
        WuXing highestWuXing = WuXing.Traversal.MinObj(wuXing => -GetStackOfBuff(wuXing._elementaryBuff));
        if (GetStackOfBuff(highestWuXing._elementaryBuff) == 0)
            return null;
        return highestWuXing;
    }

    public async UniTask<bool> IsFocused()
    {
        if (GetStackOfBuff("通透世界") > 0)
            return true;
        return await TryConsumeProcedure("集中");
    }

    #endregion

    #region Procedure
    
    public async UniTask AttackProcedure(
        int value,
        int times = 1,
        StageClosureListener initiator = null,
        WuXing? wuXing = null,
        bool crit = false,
        bool lifeSteal = false,
        bool penetrate = false,
        bool doesntConsumeJianYi = false,
        bool shatter = false,
        bool recursive = true,
        ResultDict castResult = null,
        StageClosure[] closures = null,
        bool induced = false)
        => await _env.AttackProcedure(new AttackDetails(this, Opponent(), value, times, initiator, wuXing, crit, lifeSteal, penetrate, doesntConsumeJianYi, shatter, false, recursive, castResult, closures, induced));
    
    public async UniTask IndirectProcedure(
        int value,
        StageSkill initiator = null,
        WuXing? wuXing = null,
        bool lifeSteal = false,
        bool recursive = true,
        ResultDict castResult = null,
        bool induced = false)
        => await _env.IndirectProcedure(new IndirectDetails(this, Opponent(), value, initiator, wuXing, lifeSteal, recursive, castResult, induced));
    
    public async UniTask DamageSelfProcedure(int value, StageSkill srcSkill = null, ResultDict castResult = null, bool recursive = true, bool induced = false)
        => await _env.DamageProcedure(new DamageDetails(this, this, value, crit: false, lifeSteal: false, false, recursive, srcSkill, null, castResult, induced));
    
    public async UniTask DamageOppoProcedure(int value, StageSkill srcSkill, ResultDict castResult, bool recursive = true, bool induced = false)
        => await _env.DamageProcedure(new DamageDetails(this, Opponent(), value, crit: false, lifeSteal: false, false, recursive, srcSkill, null, castResult, induced));
    
    public async UniTask LoseHealthProcedure(int value, bool causedByAttack, StageSkill srcSkill = null, ResultDict castResult = null, bool induced = false)
        => await _env.LoseHealthProcedure(new LoseHealthDetails(this, value, causedByAttack, srcSkill, null, castResult, induced));
    
    public async UniTask HealProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.HealProcedure(new HealDetails(this, this, value, false, null, castResult, null, induced));
    
    public async UniTask HealOppoProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.HealProcedure(new HealDetails(this, Opponent(), value, false, null, castResult, null, induced));
    
    public async UniTask GainArmorProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.GainArmorProcedure(new GainArmorDetails(this, this, value, null, castResult, null, induced));
    
    public async UniTask GiveArmorProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.GainArmorProcedure(new GainArmorDetails(this, Opponent(), value, null, castResult, null, induced));
    
    public async UniTask LoseArmorProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.LoseArmorProcedure(new LoseArmorDetails(this, this, value, null, null, castResult, induced));
    
    public async UniTask RemoveArmorProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.LoseArmorProcedure(new LoseArmorDetails(this, Opponent(), value, null, null, castResult, induced));
    
    public async UniTask GainBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, ResultDict castResult = null, bool induced = false)
        => await _env.GainBuffProcedure(new GainBuffDetails(this, this, buffEntry, stack, recursive, null, castResult, null, induced));
    
    public async UniTask GiveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, ResultDict castResult = null, bool induced = false)
        => await _env.GainBuffProcedure(new GainBuffDetails(this, Opponent(), buffEntry, stack, recursive, null, castResult, null, induced));
    
    public async UniTask LoseBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.LoseBuffProcedure(new LoseBuffDetails(this, this, buffEntry, stack, recursive, induced));
    
    public async UniTask RemoveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.LoseBuffProcedure(new LoseBuffDetails(this, Opponent(), buffEntry, stack, recursive, induced));
    
    public async UniTask CycleProcedure(WuXing wuXing, bool rotate = true, int gain = 0, int recover = 0, ResultDict castResult = null, bool induced = false)
        => await _env.CycleProcedure(new CycleDetails(this, rotate, wuXing, gain, recover, null, null, castResult, induced));
    
    public async UniTask DispelProcedure(int stack, ResultDict castResult = null, bool induced = false)
        => await _env.DispelProcedure(new DispelDetails(this, stack, null, null, castResult, induced));

    public async UniTask<bool> TryConsumeProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true)
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
    
    public async UniTask TransferProcedure(int fromStack, BuffEntry fromBuff, int toStack, BuffEntry toBuff, bool consuming, int? maxFlow = null, int? upperBound = null)
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
    
    public async UniTask BecomeLowHealth()
    {
        int gap = Hp - GetLowHealthThreshold();
        if (gap > 0)
            await LoseHealthProcedure(gap, false);
    }

    #endregion

    public void RegisterEntityClosures()
    {
        _env.ClosureDict.Register(this, RecordActualHeal);
        _env.ClosureDict.Register(this, RecordBurnTimes);
        _env.ClosureDict.Register(this, RecordHighestMana);
        _env.ClosureDict.Register(this, OppoLoseArmorTimes);
    }

    public void UnregisterEntityClosures()
    {
        _env.ClosureDict.Unregister(this, RecordActualHeal);
        _env.ClosureDict.Unregister(this, RecordBurnTimes);
        _env.ClosureDict.Unregister(this, RecordHighestMana);
        _env.ClosureDict.Unregister(this, OppoLoseArmorTimes);
    }

    public static string ActualHealKey = "ActualHeal";
    private static StageClosure RecordActualHeal =
        new(StageClosureDict.DID_HEAL, 1, async (listener, closure, closureDetails) =>
        {
            StageEntity entity = listener as StageEntity;
            HealDetails d = (HealDetails)closureDetails;

            if (entity != d.Tgt) return;
            entity.Memory.PerformOperation(ActualHealKey, 0, record => record + d.Value);
        });

    public static string BurnTimesKey = "BurnTimes";
    private static StageClosure RecordBurnTimes =
        new(StageClosureDict.DID_BURN, -1, async (listener, closure, closureDetails) =>
        {
            StageEntity entity = listener as StageEntity;
            BurnDetails d = (BurnDetails)closureDetails;

            if (entity != d.Owner) return;

            entity.Memory.PerformOperation(BurnTimesKey, 0, record => record + 1);
        });

    public static string HighestManaKey = "HighestMana";
    private static StageClosure RecordHighestMana =
        new(StageClosureDict.DID_GAIN_BUFF, -1, async (owner, closure, closureDetails) =>
        {
            StageEntity entity = owner as StageEntity;
            GainBuffDetails d = (GainBuffDetails)closureDetails;

            if (entity != d.Tgt) return;
            if (d.BuffEntry.GetName() != "灵气") return;
            
            entity.Memory.PerformOperation(HighestManaKey, 0, record => Mathf.Max(record, entity.GetStackOfBuff("灵气")));
        });

    public static string OppoLoseArmorTimesKey = "OppoLoseArmorTimes";
    private static StageClosure OppoLoseArmorTimes =
        new(StageClosureDict.DID_LOSE_ARMOR, -1, async (owner, closure, closureDetails) =>
        {
            StageEntity entity = owner as StageEntity;
            LoseArmorDetails d = (LoseArmorDetails)closureDetails;

            if (entity.Opponent() != d.Tgt) return;
            if (entity.Opponent().Armor >= 0) return;

            entity.Memory.PerformOperation(OppoLoseArmorTimesKey, 0, record => record += 1);
        });
}
