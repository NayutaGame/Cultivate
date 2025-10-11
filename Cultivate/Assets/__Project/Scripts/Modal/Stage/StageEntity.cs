
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class StageEntity : Addressable, StageClosureListener
{
    public async UniTask TurnProcedure(int turnCount)
    {
        TurnDetails d = new TurnDetails(_env, this, turnCount);
        ResetActionPoint();

        Memory.SetVariable(ThisTurnAttackedKey, false);
        Memory.SetVariable(ThisTurnPreserveJianYiKey, false);

        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_TURN, d);
        if (!d.Cancel)
            for (int i = 0; i < GetActionPoint(); i++)
                await ActionProcedure(i);

        await _env.ClosureDict.SendEvent(StageClosureDict.DID_TURN, d);
    }

    private async UniTask ActionProcedure(int currActionCount)
    {
        ActionDetails d = new ActionDetails(_env, this, currActionCount);
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
            StageSkill skill = Skills[_p];

            SetCostDefinitionDetails d = new(_env, this, skill);
            
            await _env.ClosureDict.SendEvent(StageClosureDict.WIL_SET_COST_DEFINITION, d);
            _costDefinition = d.CostDefinition;
            await _env.ClosureDict.SendEvent(StageClosureDict.DID_SET_COST_DEFINITION, d);
            
            _costDetails = new(_env, this, skill, _costDefinition.GetLiteralCostDescription());

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
        foreach (var skill in Skills)
        {
            if (!skill.GetSkillDefinition().HasStartStageCast) continue;
            await StartStageCastProcedure(skill);
        }
    }

    private async UniTask ExecuteProcedure()
    {
        StageSkill skill = Skills[_p];
        ExecuteDetails d = new ExecuteDetails(_env, this, skill);
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
            SkillDefinition skillDefinition = castDetails.Skill.GetSkillDefinition();
            await skillDefinition.Cast(castDetails);

            CostDescription actualCostDescription = CostDescription.Empty;
            Description actualDescription = skillDefinition.GetActualDescription(_costDetails?.CostResult, castResult);
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
        
        SkillDefinition skillDefinition = castDetails.Skill.GetSkillDefinition();
        await skillDefinition.Cast(castDetails);
        
        CostDescription actualCostDescription = _costDetails?.CostDescription.Clone() ?? CostDescription.Empty;
        Description actualDescription = skillDefinition.GetActualDescription(_costDetails?.CostResult, castResult);
        _env.Result.TryAppendNote(Index, castDetails.Skill, actualCostDescription, actualDescription);
        _env.Result.TryAppend($"\n");

        TryWriteResultToSlot(shouldWriteToSlot, castDetails.Skill, actualCostDescription, actualDescription);

        if (this == castDetails.Skill.Owner)
            castDetails.Skill.IncreaseRealCastedCount();
        
        await _env.ClosureDict.SendEvent(StageClosureDict.DID_CAST, castDetails);
        
        castDetails.Clear();
    }

    private async UniTask StepProcedure()
    {
        StartStepDetails startD = new StartStepDetails(_env, this, _p);
        await _env.ClosureDict.SendEvent(StageClosureDict.WIL_STEP, startD);
        if (startD.Cancel)
            return;

        int dir = Forward ? 1 : -1;
        for (int i = 0; i < Skills.Length; i++)
        {
            _p += dir;

            bool within = 0 <= _p && _p < Skills.Length;
            if (!within)
            {
                _p = (_p + Skills.Length) % Skills.Length;
                await _env.ClosureDict.SendEvent(StageClosureDict.DID_ROUND, new RoundDetails(_env, this));
                await _env.ClosureDict.SendEvent(StageClosureDict.WIL_ROUND, new RoundDetails(_env, this));
            }

            if (Skills[_p].Exhausted)
                continue;

            if (await TryConsumeProcedure("飞龙在天"))
            {
                Skills[_p].IncreaseBonusCastedCount();
                continue;
            }

            if (await TryConsumeProcedure("跳卡牌"))
                continue;

            break;
        }

        await _env.ClosureDict.SendEvent(StageClosureDict.DID_STEP, new EndStepDetails(_env, this, _p));
    }

    public Memory Memory;
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

    public int MaxHpDiff
        => Mathf.Abs(MaxHp - Opponent().MaxHp);

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

    public string GetName() => _index == 0 ? "主场" : "客场";
    public StageEntity Opponent() => _env.Entities[1 - _index];
    public IStageModel Model() => _index == 0 ? StageManager.Instance.HomeModel : StageManager.Instance.AwayModel;

    public int _p;
    private int _actionPoint;
    
    private CostDefinition _costDefinition;
    private CostDetails _costDetails;
    
    public int GetActionPoint() => _actionPoint;
    public void SetActionPoint(int value) => _actionPoint = Mathf.Max(_actionPoint, value);
    public void ResetActionPoint() => _actionPoint = 1;

    public int GetFullHealthThreshold()
        => Mathf.RoundToInt((100 - GetStackOfBuff("锻体")) * 0.01f * MaxHp).Clamp(0, MaxHp);
    public int GetLowHealthThreshold()
        => Mathf.RoundToInt((25 + GetStackOfBuff("锻体")) * 0.01f * MaxHp).Clamp(0, MaxHp);

    private Hint GetArmorDescription()
    {
        if (Armor > 0)
        {
            return new("可以抵消受到的攻击伤害");
        }
        else if (Armor == 0)
        {
            return new("没有护甲时，受到的攻击伤害不变");
        }
        else // armor < 0
        {
            return new("会加深下一次受到的攻击伤害");
        }
    }

    public bool IsFullHealth => Hp >= GetFullHealthThreshold() || GetStackOfBuff("天人形态") > 0;
    public bool IsLowHealth => Hp <= GetLowHealthThreshold() || GetStackOfBuff("天人形态") > 0;
    public bool Forward => GetStackOfBuff("鹤回翔") == 0;
    public int ExhaustedCount => Skills.Count(skill => skill.Exhausted);
    public int AttackCount => Skills.Count(skill => skill.GetTagComposite().Contains(TagCategory.Attack));

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

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Skills",                     thisObject => ((StageEntity)thisObject).Skills },
        { "Formations",                 thisObject => ((StageEntity)thisObject)._formations },
        { "Buffs",                      thisObject => ((StageEntity)thisObject)._buffs },
        { "ArmorDescription",           thisObject => ((StageEntity)thisObject).GetArmorDescription() },
    };
    public object Get(string s) => Accessor[s](this);
    public StageEntity(StageEnvironment env, RunEntity runEntity, int index)
    {
        Memory = new();

        HpChangedNeuron = new();
        ArmorChangedNeuron = new();

        HasChannelRecord = false;
        
        HasZhiQiRecord = false;
        HasChanRaoRecord = false;
        HasRuanRuoRecord = false;
        HasNeiShangRecord = false;
        HasFuXiuRecord = false;
        
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

        Skills = new StageSkill[_runEntity.GetSlotCount()];
        for (int i = 0; i < Skills.Length; i++)
        {
            SkillSlot slot = _runEntity.GetSlot(i + 0);
            Skills[i] = StageSkill.FromPlacedSkill(this, i, slot.PlacedSkill);
        }

        _emptyAction = StageSkill.FromSkillEntry(this, Encyclopedia.SkillCategory.FromName("伺机而动"));
        _manaShortageAction = StageSkill.FromSkillEntry(this, Encyclopedia.SkillCategory.FromName("灵气匮乏"));

        _p = 0;
    }

    ~StageEntity()
    {
        RemoveAllFormations().GetAwaiter().GetResult();
        RemoveAllBuffs().GetAwaiter().GetResult();
        
        _env.ClosureDict.Unregister(this, _closures);
    }
    
    private void TryWriteResultToSlot(bool shouldWriteToSlot, StageSkill skill, CostDescription actualCostDescription, Description actualDescription)
    {
        if (!shouldWriteToSlot) return;
        SkillSlot slot = skill.GetSlot();
        if (slot == null) return;
        if (!SlotIsUnwritten(slot)) return;
        
        WriteResultToSlot(slot, actualCostDescription, actualDescription);
    }

    private bool SlotIsUnwritten(SkillSlot slot)
        => slot.ActualCostDescription == null;

    private void WriteResultToSlot(SkillSlot slot, CostDescription actualCostDescription, Description actualDescription)
    {
        slot.ActualCostDescription = actualCostDescription;
        slot.ActualDescription = actualDescription;
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
    public StageSkill[] Skills
    {
        get => _skills;
        set => _skills = value;
    }

    public IEnumerable<StageSkill> NextSkills(int index, bool loop = false)
    {
        int p = index;
        for (int i = 0; i < Skills.Length - 1; i++)
        {
            int? nextP = Next(p, loop);
            if (!nextP.HasValue)
                yield break;
            
            p = nextP.Value;
            yield return Skills[p];
        }
    }

    public IEnumerable<StageSkill> PrevSkills(int index, bool loop = false)
    {
        int p = index;
        for (int i = 0; i < Skills.Length - 1; i++)
        {
            int? prevP = Prev(p, loop);
            if (!prevP.HasValue)
                yield break;
            
            p = prevP.Value;
            yield return Skills[p];
        }
    }

    public StageSkill NextSkill(int index, bool loop = false)
    {
        int? p = Next(index, loop);
        if (!p.HasValue)
            return null;
        
        return Skills[p.Value];
    }

    public StageSkill PrevSkill(int index, bool loop = false)
    {
        int? p = Prev(index, loop);
        if (!p.HasValue)
            return null;
        
        return Skills[p.Value];
    }

    private int? Next(int index, bool loop)
    {
        int p = index + 1;
        if (loop)
            p %= Skills.Length;

        if (p >= Skills.Length)
            return null;

        return p;
    }

    private int? Prev(int index, bool loop)
    {
        int p = index - 1;
        if (loop)
            p = (p + Skills.Length) % Skills.Length;

        if (p < 0)
            return null;

        return p;
    }

    #endregion

    #region Formation

    private ListModel<Formation> _formations;
    public IEnumerable<Formation> TraversalFormations() => _formations;

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
        await _formations.Do(async f => f.Unregister());
        _formations.Clear();
    }

    #endregion

    #region Buff

    private ListModel<Buff> _buffs;
    public IEnumerable<Buff> TraversalBuffs() => _buffs;
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
        await _buffs.Do(async b => b.Unregister());
        _buffs.Clear();
    }

    public Buff FindBuff(string buffName) => FindBuff(Encyclopedia.BuffCategory.FromName(buffName));
    public Buff FindBuff(BuffEntry buffEntry) => TraversalBuffs().FirstObj(b => b.GetEntry() == buffEntry);

    public int GetStackOfBuff(string buffName) => GetStackOfBuff(Encyclopedia.BuffCategory.FromName(buffName));
    public int GetStackOfBuff(BuffEntry entry) => FindBuff(entry)?.Stack ?? 0;

    // public Buff GetHighestWuXingBuff()
    // {
    //     Buff highestBuff = LegacyWuXing.Traversal
    //         .Map(wuXing => FindBuff(wuXing._elementaryBuff))
    //         .MinObj(b => -b.Stack);
    //
    //     return highestBuff;
    // }
    //
    // public LegacyWuXing? GetHighestWuXing()
    // {
    //     LegacyWuXing highestWuXing = LegacyWuXing.Traversal.MinObj(wuXing => -GetStackOfBuff(wuXing._elementaryBuff));
    //     if (GetStackOfBuff(highestWuXing._elementaryBuff) == 0)
    //         return null;
    //     return highestWuXing;
    // }

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
        WuXing wuXing = null,
        bool recursive = true,
        StageClosureListener listener = null,
        StageClosure[] closures = null,
        bool induced = false)
        => await _env.AttackProcedure(AttackDetails.FromEntity(this, value, times, wuXing, recursive, listener, closures, induced));
    
    public async UniTask IndirectProcedure(
        int value,
        StageSkill initiator = null,
        WuXing wuXing = null,
        bool lifeSteal = false,
        bool recursive = true,
        ResultDict castResult = null,
        bool induced = false)
        => await _env.IndirectProcedure(new IndirectDetails(_env, this, Opponent(), value, initiator, wuXing, lifeSteal, recursive, castResult, induced));
    
    public async UniTask DamageSelfProcedure(int value, StageSkill listener = null, bool recursive = true, bool induced = false)
        => await _env.DamageProcedure(DamageDetails.FromEntity(this, true, value, recursive, listener, null, induced));
    
    public async UniTask DamageOppoProcedure(int value, StageSkill listener = null, bool recursive = true, bool induced = false)
        => await _env.DamageProcedure(DamageDetails.FromEntity(this, false, value, recursive, listener, null, induced));
    
    public async UniTask LoseHealthProcedure(int value, bool causedByAttack, StageSkill srcSkill = null, ResultDict castResult = null, bool induced = false)
        => await _env.LoseHealthProcedure(new LoseHealthDetails(_env, this, value, causedByAttack, srcSkill, null, castResult, false, induced));
    
    public async UniTask HealProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.HealProcedure(new HealDetails(_env, this, this, value, false, null, null, castResult, false, induced));
    
    public async UniTask HealOppoProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.HealProcedure(new HealDetails(_env, this, Opponent(), value, false, null, null, castResult, false, induced));
    
    public async UniTask GainArmorProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.GainArmorProcedure(new GainArmorDetails(_env, this, this, value, null, null, castResult, false, induced));
    
    public async UniTask GiveArmorProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.GainArmorProcedure(new GainArmorDetails(_env, this, Opponent(), value, null, null, castResult, false, induced));
    
    public async UniTask LoseArmorProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.LoseArmorProcedure(new LoseArmorDetails(_env, this, this, value, null, null, castResult, false, induced, true));
    
    public async UniTask RemoveArmorProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.LoseArmorProcedure(new LoseArmorDetails(_env, this, Opponent(), value, null, null, castResult, false, induced, true));

    public async UniTask GainBuffProcedure(string buffName, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.GainBuffProcedure(GainBuffDetails.FromEntity(this, true, Encyclopedia.BuffCategory.FromName(buffName), stack, recursive, induced));
    public async UniTask GainBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.GainBuffProcedure(GainBuffDetails.FromEntity(this, true, buffEntry, stack, recursive, induced));
    public async UniTask GiveBuffProcedure(string buffName, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.GainBuffProcedure(GainBuffDetails.FromEntity(this, false, Encyclopedia.BuffCategory.FromName(buffName), stack, recursive, induced));
    public async UniTask GiveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.GainBuffProcedure(GainBuffDetails.FromEntity(this, false, buffEntry, stack, recursive, induced));
    public async UniTask LoseBuffProcedure(string buffName, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.LoseBuffProcedure(LoseBuffDetails.FromEntity(this, true, Encyclopedia.BuffCategory.FromName(buffName), stack, recursive, induced));
    public async UniTask LoseBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.LoseBuffProcedure(LoseBuffDetails.FromEntity(this, true, buffEntry, stack, recursive, induced));
    public async UniTask RemoveBuffProcedure(string buffName, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.LoseBuffProcedure(LoseBuffDetails.FromEntity(this, false, Encyclopedia.BuffCategory.FromName(buffName), stack, recursive, induced));
    public async UniTask RemoveBuffProcedure(BuffEntry buffEntry, int stack = 1, bool recursive = true, bool induced = false)
        => await _env.LoseBuffProcedure(LoseBuffDetails.FromEntity(this, false, buffEntry, stack, recursive, induced));
    
    public async UniTask CycleProcedure(WuXing wuXing, bool rotate = true, int gain = 0, int recover = 0, ResultDict castResult = null, bool induced = false)
        => await _env.CycleProcedure(new CycleDetails(_env, this, rotate, wuXing, gain, recover, null, null, castResult, false, induced));
    
    public async UniTask DispelProcedure(int stack, ResultDict castResult = null, bool induced = false)
        => await _env.DispelProcedure(new DispelDetails(_env, this, stack, null, null, castResult, false, induced));

    public async UniTask LoseMaxHealthProcedure(int value, ResultDict castResult = null, bool induced = false)
        => await _env.LoseMaxHealthProcedure(new LoseMaxHealthDetails(_env, this, value, null, null, castResult, false, induced));

    public async UniTask<bool> TryConsumeProcedure(string buffName, int stack = 1, bool recursive = true)
        => await TryConsumeProcedure(Encyclopedia.BuffCategory.FromName(buffName), stack, recursive);
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

    #endregion

    public void RegisterEntityClosures()
    {
        _env.ClosureDict.Register(this, EntityClosures);
    }

    public void UnregisterEntityClosures()
    {
        _env.ClosureDict.Unregister(this, EntityClosures);
    }
    
    public static string ThisTurnAttackedKey = "ThisTurnAttacked";
    public static string ThisTurnPreserveJianYiKey = "ThisTurnPreserveJianYi";
    public static string TriggeredCritTimesKey = "TriggeredCritTimes";
    public static string TriggeredLifestealTimesKey = "TriggeredLifestealTimes";
    public static string TriggeredPenetrateTimesKey = "TriggeredPenetrateTimes";
    public static string ActualHealKey = "ActualHeal";
    public static string BurnTimesKey = "BurnTimes";
    public static string HighestAttackKey = "HighestAttack";
    public static string HighestManaKey = "HighestMana";
    public static string HighestJianYiKey = "HighestJianYi";
    public static string OppoLoseArmorTimesKey = "OppoLoseArmorTimes";
    public static string LastRotatedWuXingKey = "LastRotatedWuXing";

    private static StageClosure[] EntityClosures = new StageClosure[]
    {
        new(StageClosureDict.WIL_DAMAGE, 1, async (listener, closure, closureDetails) =>
        {
            StageEntity entity = listener as StageEntity;
            DamageDetails d = (DamageDetails)closureDetails;

            if (entity != d.Src) return;
            entity.Memory.PerformOperation(TriggeredCritTimesKey, 0, record => record += d.Crit ? 1 : 0);
        }, "RecordTriggeredCritTimesFromWilDamage"),
        
        new(StageClosureDict.DID_LOSE_BUFF, 1, async (listener, closure, closureDetails) =>
        {
            StageEntity entity = listener as StageEntity;
            LoseBuffDetails d = (LoseBuffDetails)closureDetails;

            if (entity != d.Tgt) return;
            if (d.BuffEntry != Encyclopedia.BuffCategory.FromName("暴击")) return;
            entity.Memory.PerformOperation(TriggeredCritTimesKey, 0, record => record += d.Stack);
        }, "RecordTriggeredCritTimesFromLoseBuff"),
        
        new(StageClosureDict.WIL_DAMAGE, 1, async (listener, closure, closureDetails) =>
        {
            StageEntity entity = listener as StageEntity;
            DamageDetails d = (DamageDetails)closureDetails;
            
            if (entity != d.Src) return;
            entity.Memory.PerformOperation(TriggeredLifestealTimesKey, 0, record => record += d.LifeSteal ? 1 : 0);
        }, "RecordTriggeredLifestealTimes"),
        
        new(StageClosureDict.WIL_ATTACK, 1, async (listener, closure, closureDetails) =>
        {
            StageEntity entity = listener as StageEntity;
            AttackDetails d = (AttackDetails)closureDetails;
            
            if (entity != d.Src) return;
            entity.Memory.PerformOperation(TriggeredPenetrateTimesKey, 0, record => record += d.Penetrate ? 1 : 0);
        }, "RecordTriggeredPenetrateTimes"),
        
        new(StageClosureDict.DID_HEAL, 1, async (listener, closure, closureDetails) =>
        {
            StageEntity entity = listener as StageEntity;
            HealDetails d = (HealDetails)closureDetails;

            if (entity != d.Tgt) return;
            entity.Memory.PerformOperation(ActualHealKey, 0, record => record + d.Value);
        }, "RecordActualHeal"),
        
        new(StageClosureDict.DID_BURN, -1, async (listener, closure, closureDetails) =>
        {
            StageEntity entity = listener as StageEntity;
            BurnDetails d = (BurnDetails)closureDetails;

            if (entity != d.Owner) return;

            entity.Memory.PerformOperation(BurnTimesKey, 0, record => record + 1);
        }, "RecordBurnTimes"),
        
        new(StageClosureDict.DID_ATTACK, -1, async (owner, closure, closureDetails) =>
        {
            StageEntity entity = owner as StageEntity;
            AttackDetails d = (AttackDetails)closureDetails;
            
            if (entity != d.Src) return;
            
            entity.Memory.PerformOperation(HighestAttackKey, 0, record => Mathf.Max(record, d.Value));
        }, "RecordHighestAttack"),
        
        new(StageClosureDict.DID_GAIN_BUFF, -1, async (owner, closure, closureDetails) =>
        {
            StageEntity entity = owner as StageEntity;
            GainBuffDetails d = (GainBuffDetails)closureDetails;

            if (entity != d.Tgt) return;
            if (d.BuffEntry.GetName() != "灵气") return;
            
            entity.Memory.PerformOperation(HighestManaKey, 0, record => Mathf.Max(record, entity.GetStackOfBuff("灵气")));
        }, "RecordHighestMana"),
        
        new(StageClosureDict.DID_GAIN_BUFF, -1, async (owner, closure, closureDetails) =>
        {
            StageEntity entity = owner as StageEntity;
            GainBuffDetails d = (GainBuffDetails)closureDetails;

            if (entity != d.Tgt) return;
            if (d.BuffEntry != Encyclopedia.BuffCategory.FromName("剑意")) return;
            
            entity.Memory.PerformOperation(HighestJianYiKey, 0, record => Mathf.Max(record, entity.GetStackOfBuff("剑意")));
        }, "RecordHighestJianYi"),
        
        new(StageClosureDict.DID_LOSE_ARMOR, -1, async (owner, closure, closureDetails) =>
        {
            StageEntity entity = owner as StageEntity;
            LoseArmorDetails d = (LoseArmorDetails)closureDetails;

            if (entity.Opponent() != d.Tgt) return;
            if (entity.Opponent().Armor >= 0) return;

            entity.Memory.PerformOperation(OppoLoseArmorTimesKey, 0, record => record += 1);
        }, "OppoLoseArmorTimes"),
        
        new(StageClosureDict.DID_CYCLE, -1, async (owner, closure, closureDetails) =>
        {
            StageEntity entity = owner as StageEntity;
            CycleDetails d = (CycleDetails)closureDetails;

            if (entity != d.Owner) return;
            if (!d.Rotate) return;

            WuXing wuXing = d.WuXing;
            entity.Memory.PerformOperation<WuXing>(LastRotatedWuXingKey, null, record => record = wuXing);
        }, "LastRotatedWuXing"),
    };
}
