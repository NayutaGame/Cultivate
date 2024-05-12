

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLLibrary;
using DG.Tweening;
using FMOD;
using UnityEngine;

public class StageEnvironment : Addressable, StageEventListener
{
    private static readonly int MAX_ACTION_COUNT = 120;

    #region Procedures

    // public async Task SimpleProcedure(Args args)
    //     => await SimpleProcedure(new SimpleDetails(args));
    // public async Task SimpleProcedure(SimpleDetails d)
    // {
    //     await _eventDict.SendEvent(StageEventDict.WILL_SIMPLE, d);
    //     if (d.Cancel)
    //         return;
    //
    //     // actual work
    //
    //     await _eventDict.SendEvent(StageEventDict.DID_SIMPLE, d);
    // }

    public async Task CoreProcedure()
    {
        StageEventDescriptor writeManaShortage = new StageEventDescriptor(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_MANA_SHORTAGE, 0, WriteShortage);
        StageEventDescriptor writeArmorShortage = new StageEventDescriptor(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_ARMOR_SHORTAGE, 0, WriteShortage);
        StageEventDescriptor writeManaCost = new StageEventDescriptor(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_MANA_COST, 0, WriteCost);
        StageEventDescriptor writeChannelCost = new StageEventDescriptor(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_CHANNEL_COST, 0, WriteCost);
        StageEventDescriptor writeHealthCost = new StageEventDescriptor(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_HEALTH_COST, 0, WriteCost);
        StageEventDescriptor writeArmorCost = new StageEventDescriptor(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_ARMOR_COST, 0, WriteCost);

        ClearResults();

        Register();
        
        Opening();

        await MingYuanPenaltyProcedure();
        await FormationProcedure();
        await StartStageProcedure();

        _eventDict.Register(this, writeManaShortage);
        _eventDict.Register(this, writeArmorShortage);
        _eventDict.Register(this, writeManaCost);
        _eventDict.Register(this, writeChannelCost);
        _eventDict.Register(this, writeHealthCost);
        _eventDict.Register(this, writeArmorCost);

        await BodyProcedure();

        _eventDict.Unregister(this, writeManaShortage);
        _eventDict.Unregister(this, writeArmorShortage);
        _eventDict.Unregister(this, writeManaCost);
        _eventDict.Unregister(this, writeChannelCost);
        _eventDict.Unregister(this, writeHealthCost);
        _eventDict.Unregister(this, writeArmorCost);

        await EndStageProcedure();

        Unregister();

        _result.HomeLeftHp = _entities[0].Hp;
        _result.AwayLeftHp = _entities[1].Hp;
        _result.TryAppend(_result.HomeVictory ? $"主场胜利\n" : $"主场失败\n");
    }

    public async Task FormationProcedure(StageEntity owner, RunFormation formation, bool recursive = true)
        => await FormationProcedure(new FormationDetails(owner, formation, recursive));
    public async Task FormationProcedure(FormationDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WIL_ADD_FORMATION, d);
        if (d.Cancel) return;

        await d.Owner.AddFormation(new GainFormationDetails(d._formation));

        // await TryPlayTween(new BuffTweenDescriptor(d));

        _result.TryAppend($"    {d._formation.GetName()} is set");

        if (d.Cancel) return;
        await _eventDict.SendEvent(StageEventDict.DID_ADD_FORMATION, d);
    }

    /// <summary>
    /// 发起一次攻击行为，会结算目标的护甲
    /// </summary>
    /// <param name="src">攻击者</param>
    /// <param name="tgt">受攻击者</param>
    /// <param name="value">攻击数值</param>
    /// <param name="wuXing">攻击特效的五行</param>
    /// <param name="times">攻击次数</param>
    /// <param name="lifeSteal">是否吸血</param>
    /// <param name="pierce">是否穿透</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="damaged">如果造成伤害时候的额外行为</param>
    /// <param name="undamaged">如果未造成伤害的额外行为</param>
    public async Task AttackProcedure(StageEntity src, StageEntity tgt, int value, WuXing? wuXing = null, int times = 1,
        bool lifeSteal = false, bool pierce = false, bool crit = false, bool recursive = true,
        Func<DamageDetails, Task> damaged = null, Func<DamageDetails, Task> undamaged = null)
        => await AttackProcedure(new AttackDetails(src, tgt, value, wuXing, lifeSteal, pierce, crit, false, recursive, damaged, undamaged), times);
    public async Task AttackProcedure(AttackDetails attackDetails, int times)
    {
        /*
         * Buff引起的
         * primary 延迟攻，天衣无缝
         * derived 回马枪
         * dont care 看破
         */
        if (await attackDetails.Src.TryConsumeProcedure("追击"))
            times += 1;

        await Play(new PreAttackedAnimation(false, attackDetails));
        await Play(new AttackAnimation(true, attackDetails));
        
        for (int i = 0; i < times; i++)
        {
            AttackDetails d = attackDetails.Clone();
            await _eventDict.SendEvent(StageEventDict.WIL_ATTACK, d);
            await SingleAttackProcedure(d);
            await _eventDict.SendEvent(StageEventDict.DID_ATTACK, d);
            await NextKey();
        }
    }

    private async Task SingleAttackProcedure(AttackDetails d)
    {
        await Play(new PiercingVFXAnimation(false, d));

        bool isEvaded = !d.Pierce && d.Evade;
        if (isEvaded)
        {
            await EvadedProcedure(new EvadedDetails(d.Src, d.Tgt, d.Value));

            if (d.Undamaged != null)
                await d.Undamaged(new DamageDetails(d.Src, d.Tgt, 0, crit: d.Crit, lifeSteal: d.LifeSteal));
            return;
        }

        if (!d.Pierce && d.Tgt.Armor >= 0)
        {
            int negate = Mathf.Min(d.Value, d.Tgt.Armor);
            d.Value -= negate;
            await LoseArmorProcedure(d.Src, d.Tgt, negate);
        }

        if (d.Tgt.Armor < 0)
        {
            d.Value += -d.Tgt.Armor;
            d.Tgt.Armor = 0;
        }
        
        await Play(new HitVFXAnimation(false, d));

        bool isGuarded = d.Value == 0;
        if (isGuarded)
        {
            await GuardedProcedure(new GuardedDetails(d.Src, d.Tgt, d.Value));

            if (d.Undamaged != null)
                await d.Undamaged(new DamageDetails(d.Src, d.Tgt, 0, crit: d.Crit, lifeSteal: d.LifeSteal));
            return;
        }

        await DamageProcedure(new DamageDetails(d.Src, d.Tgt, d.Value, crit: d.Crit, lifeSteal: d.LifeSteal, damaged: d.Damaged, undamaged: d.Undamaged));
        
        _result.TryAppend($"    敌方生命[护甲]变成了${d.Tgt.Hp}[{d.Tgt.Armor}]");
    }

    private async Task EvadedProcedure(EvadedDetails d)
    {
        await Play(new EvadedAnimation(false, d.Tgt));
        _result.TryAppend($"    攻击被闪避");
        
        await _eventDict.SendEvent(StageEventDict.DID_EVADE, d);
    }

    private async Task GuardedProcedure(GuardedDetails d)
    {
        await Play(new GuardedAnimation(false, d.Tgt));
        _result.TryAppend($"    攻击被格挡");
    }

    /// <summary>
    /// 发起一次间接攻击行为，例如锋锐，灼烧，会结算目标的护甲，不会继承攻击词条
    /// </summary>
    /// <param name="src">攻击者</param>
    /// <param name="tgt">受攻击者</param>
    /// <param name="value">攻击数值</param>
    /// <param name="wuXing">攻击特效的五行</param>
    /// <param name="recursive">是否会递归</param>
    public async Task IndirectProcedure(StageEntity src, StageEntity tgt, int value, WuXing? wuXing = null, bool recursive = true)
        => await IndirectProcedure(new IndirectDetails(src, tgt, value, wuXing, recursive));
    public async Task IndirectProcedure(IndirectDetails indirectDetails)
    {
        IndirectDetails d = indirectDetails.Clone();

        await _eventDict.SendEvent(StageEventDict.WIL_INDIRECT, d);

        if (d.Cancel)
        {
            _result.TryAppend($"    攻击被取消");
            return;
        }

        if (d.Tgt.Armor >= 0)
        {
            int negate = Mathf.Min(d.Value, d.Tgt.Armor);
            d.Value -= negate;
            await LoseArmorProcedure(d.Src, d.Tgt, negate);
        }

        if (d.Tgt.Armor < 0)
        {
            d.Value += -d.Tgt.Armor;
            d.Tgt.Armor = 0;
        }

        if (d.Value == 0)
        {
            _result.TryAppend($"    攻击为0");
            await _eventDict.SendEvent(StageEventDict.DID_INDIRECT, d);
            return;
        }

        DamageDetails damageDetails = new DamageDetails(d.Src, d.Tgt, d.Value);
        await DamageProcedure(damageDetails);

        // await TryPlayTween(new AttackTweenDescriptor(d));
        _result.TryAppend($"    敌方生命[护甲]变成了${d.Tgt.Hp}[{d.Tgt.Armor}]");

        await _eventDict.SendEvent(StageEventDict.DID_INDIRECT, d);
    }

    /// <summary>
    /// 发起一次伤害行为，不会结算目标的护甲
    /// </summary>
    /// <param name="src">伤害者</param>
    /// <param name="tgt">受伤害者</param>
    /// <param name="value">伤害数值</param>
    /// <param name="crit">是否暴击</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="damaged">如果造成伤害时候的额外行为</param>
    /// <param name="undamaged">如果未造成伤害的额外行为</param>
    public async Task DamageProcedure(StageEntity src, StageEntity tgt, int value, bool crit = false, bool lifeSteal = false, bool recursive = true,
        Func<DamageDetails, Task> damaged = null, Func<DamageDetails, Task> undamaged = null)
        => await DamageProcedure(new DamageDetails(src, tgt, value, crit, lifeSteal, recursive, damaged, undamaged));
    public async Task DamageProcedure(DamageDetails d)
    {
        if (d.Crit)
            d.Value *= 2;
        
        await _eventDict.SendEvent(StageEventDict.WIL_DAMAGE, d);

        if (d.Cancel || d.Value == 0)
        {
            await Play(new UndamagedAnimation(false, d));
            if (d.Undamaged != null)
                await d.Undamaged(d);
            return;
        }

        await Play(new DamagedAnimation(false, d));
        await Play(new DamagedTextAnimation(false, d));
        await LoseHealthProcedure(d.Tgt, d.Value);
        if (d.Damaged != null)
            await d.Damaged(d);

        await _eventDict.SendEvent(StageEventDict.DID_DAMAGE, d);

        if (!d.Cancel && d.LifeSteal)
            await HealProcedure(d.Src, d.Src, d.Value);
    }

    public async Task LoseHealthProcedure(StageEntity owner, int value)
        => await LoseHealthProcedure(new LoseHealthDetails(owner, value));
    public async Task LoseHealthProcedure(LoseHealthDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WIL_LOSE_HEALTH, d);
        if (d.Cancel)
            return;

        // hpBar change
        d.Owner.Hp -= d.Value;

        await _eventDict.SendEvent(StageEventDict.DID_LOSE_HEALTH, d);
    }

    public async Task HealProcedure(StageEntity src, StageEntity tgt, int value)
        => await HealProcedure(new HealDetails(src, tgt, value));
    public async Task HealProcedure(HealDetails d)
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        await _eventDict.SendEvent(StageEventDict.WIL_HEAL, d);

        if (d.Cancel)
            return;

        int actualHealed;
        if (d.Penetrate)
        {
            tgt.MaxHp = Mathf.Max(tgt.MaxHp, tgt.Hp + d.Value);
            actualHealed = d.Value;
        }
        else
        {
            actualHealed = Mathf.Min(tgt.MaxHp - tgt.Hp, d.Value);
        }

        tgt.Hp += actualHealed;
        tgt.HealedRecord += actualHealed;

        await Play(new HealAnimation(true, d));
        _result.TryAppend($"    生命变成了${tgt.Hp}");

        await _eventDict.SendEvent(StageEventDict.DID_HEAL, d);
    }

    public async Task GainBuffProcedure(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await GainBuffProcedure(new GainBuffDetails(src, tgt, buffEntry, stack, recursive));
    public async Task GainBuffProcedure(GainBuffDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WIL_GAIN_BUFF, d);

        d.Cancel |= d._stack <= 0;
        if (d.Cancel) return;

        Buff same = d.Tgt.FindBuff(d._buffEntry);

        if (same != null && d._buffEntry.BuffStackRule != BuffStackRule.Individual)
        {
            int oldStack = same.Stack;

            switch (d._buffEntry.BuffStackRule)
            {
                case BuffStackRule.One:
                    break;
                case BuffStackRule.Add:
                    await same.SetStack(same.Stack + d._stack);
                    break;
                case BuffStackRule.Min:
                    await same.SetStack(Mathf.Min(same.Stack, d._stack));
                    break;
                case BuffStackRule.Max:
                    await same.SetStack(Mathf.Max(same.Stack, d._stack));
                    break;
                case BuffStackRule.Overwrite:
                    await same.SetStack(d._stack);
                    break;
            }

            await Play(new BuffAnimation(true, d));
            _result.TryAppend($"    {d._buffEntry.GetName()}: {oldStack} -> {same.Stack}");
        }
        else
        {
            await d.Tgt.AddBuff(new BuffAppearDetails(d.Tgt, d._buffEntry, d._stack));

            await Play(new BuffAnimation(true, d));
            _result.TryAppend($"    {d._buffEntry.GetName()}: 0 -> {d._stack}");
        }

        await _eventDict.SendEvent(StageEventDict.DID_GAIN_BUFF, d);
    }

    public async Task LoseBuffProcedure(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await LoseBuffProcedure(new LoseBuffDetails(src, tgt, buffEntry, stack, recursive));
    public async Task LoseBuffProcedure(LoseBuffDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WIL_LOSE_BUFF, d);

        if (d.Cancel)
            return;

        Buff b = d.Tgt.FindBuff(d._buffEntry);
        if (b != null)
            await b.SetStack(Mathf.Max(0, b.Stack - d._stack));

        await _eventDict.SendEvent(StageEventDict.DID_LOSE_BUFF, d);
    }

    public async Task GainArmorProcedure(StageEntity src, StageEntity tgt, int value)
        => await GainArmorProcedure(new GainArmorDetails(src, tgt, value));
    public async Task GainArmorProcedure(GainArmorDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WIL_GAIN_ARMOR, d);

        if (d.Cancel)
            return;

        d.Tgt.Armor += d.Value;
        _result.TryAppend($"    护甲变成了[{d.Tgt.Armor}]");

        await _eventDict.SendEvent(StageEventDict.DID_GAIN_ARMOR, d);
    }

    public async Task LoseArmorProcedure(StageEntity src, StageEntity tgt, int value)
        => await LoseArmorProcedure(new LoseArmorDetails(src, tgt, value));
    public async Task LoseArmorProcedure(LoseArmorDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WIL_LOSE_ARMOR, d);

        if (d.Cancel)
            return;

        if (d.Tgt.Armor >= 0)
            d.Tgt.LostArmorRecord += Mathf.Min(d.Tgt.Armor, d.Value);

        d.Tgt.Armor -= d.Value;
        _result.TryAppend($"    护甲变成了[{d.Tgt.Armor}]");

        // 正变正，护甲伤害
        // 正变负，碎盾
        // 负变负，减甲

        await _eventDict.SendEvent(StageEventDict.DID_LOSE_ARMOR, d);
    }

    public async Task ManaShortageProcedure(ManaCostResult d)
    {
        await _eventDict.SendEvent(StageEventDict.WIL_MANA_SHORTAGE, d);

        if (d.Cancel)
            return;

        await _eventDict.SendEvent(StageEventDict.DID_MANA_SHORTAGE, d);
    }

    public async Task ArmorShortageProcedure(ArmorCostResult d)
    {
        await _eventDict.SendEvent(StageEventDict.WIL_ARMOR_SHORTAGE, d);

        if (d.Cancel)
            return;

        await _eventDict.SendEvent(StageEventDict.DID_ARMOR_SHORTAGE, d);
    }

    public async Task ExhaustProcedure(StageEntity owner, StageSkill skill)
        => await ExhaustProcedure(new ExhaustDetails(owner, skill));
    public async Task ExhaustProcedure(ExhaustDetails d)
    {
        if (d.Skill.Exhausted)
            return;

        await _eventDict.SendEvent(StageEventDict.WIL_EXHAUST, d);

        d.Skill.Exhausted = true;

        await _eventDict.SendEvent(StageEventDict.DID_EXHAUST, d);
    }

    public async Task CycleProcedure(StageEntity owner, WuXing wuXing, int gain, int recover)
        => await CycleProcedure(new CycleDetails(owner, wuXing, gain, recover));
    public async Task CycleProcedure(CycleDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WIL_CYCLE, d);

        if (d.Cancel)
            return;

        WuXing fromWuXing = d.WuXing.Prev;
        int flow = d.Owner.GetStackOfBuff(fromWuXing._elementaryBuff);
        await d.Owner.TryConsumeProcedure(fromWuXing._elementaryBuff, flow);

        await d.Owner.GainBuffProcedure(fromWuXing._elementaryBuff, Mathf.Min(flow, d.Recover));
        await d.Owner.GainBuffProcedure(d.WuXing._elementaryBuff, flow + Mathf.Min(flow, d.Gain));

        await _eventDict.SendEvent(StageEventDict.DID_CYCLE, d);
    }

    public async Task DispelProcedure(StageEntity owner, int stack)
        => await DispelProcedure(new DispelDetails(owner, stack));
    public async Task DispelProcedure(DispelDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WIL_DISPEL, d);

        if (d.Cancel)
            return;

        List<Buff> buffs = d.Owner.Buffs.FilterObj(b => !b.Friendly && b.Dispellable).ToList();

        foreach (Buff b in buffs)
            await d.Owner.RemoveBuffProcedure(b.GetEntry(), d.Stack);

        await _eventDict.SendEvent(StageEventDict.DID_DISPEL, d);
    }

    #endregion

    private StageConfig _config;
    public StageConfig Config => _config;

    private StageEventDict _eventDict;
    public StageEventDict EventDict => _eventDict;

    private StageEntity[] _entities;
    public StageEntity[] Entities => _entities;

    private StageResult _result;
    public StageResult Result => _result;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private StageEnvironment(StageConfig config)
    {
        _accessors = new()
        {
            { "Home",                  () => _entities[0] },
            { "Away",                  () => _entities[1] },
            { "Report",                () => _result },
        };

        _config = config;

        _eventDict = new();

        _entities = new StageEntity[]
        {
            new(this, _config.Home, 0),
            new(this, _config.Away, 1),
        };

        _result = new(_config);
    }

    public static StageEnvironment FromConfig(StageConfig config)
        => new(config);

    public static StageTimeline CalcTimeline(StageConfig config)
        => CalcSimulateResult(StageConfig.ForTimeline(config.Home, config.Away, config.RunConfig)).Timeline;

    public static StageResult CalcSimulateResult(StageConfig config)
    {
        StageEnvironment env = FromConfig(config);
        env.CoreProcedure().GetAwaiter().GetResult();
        if (env._result.WriteResult)
            env.WriteResult();
        return env._result;
    }

    public static void Combat(StageConfig config)
    {
        AppManager.Push(new StageAppS(config));
    }

    public void Opening()
    {
        if (!_config.Animated)
            return;
        StageManager.Instance.StageAnimationController.Opening();
    }

    public async Task Play(Animation animation)
    {
        if (!_config.Animated)
            return;
        await StageManager.Instance.StageAnimationController.Play(animation);
    }

    public async Task NextKey()
    {
        if (!_config.Animated)
            return;
        await StageManager.Instance.StageAnimationController.NextKey();
    }

    public void WriteResult()
    {
        _entities[0].WriteResult();
        _config.Home.DepleteProcedure();
    }

    private async Task WriteShortage(StageEventListener listener, EventDetails stageEventDetails)
    {
        CostResult d = (CostResult)stageEventDetails;
        d.State = CostResult.CostState.Shortage;
    }

    private async Task WriteCost(StageEventListener listener, EventDetails stageEventDetails)
    {
        CostResult d = (CostResult)stageEventDetails;
        if (d.State == CostResult.CostState.Shortage)
            return;

        StageSkill skill = d.Skill;
        CostDescription costDescription = skill.Entry.GetCostDescription(skill.GetJingJie());
        int literalCost = costDescription.Value;

        CostResult.CostState state = d.Value < literalCost
            ? CostResult.CostState.Reduced
            : CostResult.CostState.Normal;

        d.State = state;
    }

    private void Register()
    {
        if (_config.RunConfig == null)
            return;

        RegisterList(_config.RunConfig.CharacterProfile.GetEntry()._stageEventDescriptors);

        DifficultyEntry difficultyEntry = _config.RunConfig.DifficultyProfile.GetEntry();
        RegisterList(difficultyEntry._stageEventDescriptors);
        foreach (var additionalDifficultyEntry in difficultyEntry.AdditionalDifficulties)
            RegisterList(additionalDifficultyEntry._stageEventDescriptors);

        RegisterList(_config.RunConfig.DesignerConfig._stageEventDescriptors);
    }

    private void RegisterList(StageEventDescriptor[] list)
    {
        list.FilterObj(d => d.ListenerId == StageEventDict.STAGE_ENVIRONMENT)
            .Do(e => _eventDict.Register(this, e));
    }

    private void Unregister()
    {
        if (_config.RunConfig == null)
            return;

        UnregisterList(_config.RunConfig.CharacterProfile.GetEntry()._stageEventDescriptors);

        DifficultyEntry difficultyEntry = _config.RunConfig.DifficultyProfile.GetEntry();
        UnregisterList(difficultyEntry._stageEventDescriptors);
        foreach (var additionalDifficultyEntry in difficultyEntry.AdditionalDifficulties)
            UnregisterList(additionalDifficultyEntry._stageEventDescriptors);

        UnregisterList(_config.RunConfig.DesignerConfig._stageEventDescriptors);
    }

    private void UnregisterList(StageEventDescriptor[] list)
    {
        list.FilterObj(d => d.ListenerId == StageEventDict.STAGE_ENVIRONMENT)
            .Do(e => _eventDict.Unregister(this, e));
    }

    private void ClearResults()
    {
        _entities.Do(stageEntity => stageEntity.RunEntity.TraversalCurrentSlots().Do(s => s.ClearResults()));
    }

    private async Task MingYuanPenaltyProcedure()
    {
        await _entities[0].MingYuan.MingYuanPenaltyProcedure(_entities[0]);
        await _entities[1].MingYuan.MingYuanPenaltyProcedure(_entities[1]);
    }

    private async Task FormationProcedure()
    {
        List<FormationDetails> details = new List<FormationDetails>();

        foreach (var entity in _entities)
        foreach (var runFormation in entity.RunFormations())
            if(runFormation.IsActivated())
                details.Add(new FormationDetails(entity, runFormation));

        details.Sort((lhs, rhs) => lhs._formation.GetEntry().GetOrder() - rhs._formation.GetEntry().GetOrder());

        foreach (var d in details)
            await FormationProcedure(d);
    }

    private async Task StartStageProcedure()
    {
        foreach (var e in _entities)
            await _eventDict.SendEvent(StageEventDict.WIL_STAGE, new StageDetails(e));
    }

    private async Task BodyProcedure()
    {
        int whosTurn = 0;
        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            StageEntity actor = _entities[whosTurn];

            _result.TryAppend($"--------第{i}回合, {actor.GetName()}行动--------\n");
            await actor.TurnProcedure();

            _entities.Do(e =>
            {
                _result.TryAppend($"{e.GetName()} {e.Hp}[{e.Armor}] Buff:");
                foreach (Buff b in e.Buffs)
                    _result.TryAppend($"  {b.GetName()}*{b.Stack}");
                _result.TryAppend("\n");
            });

            if (await TryCommit(whosTurn))
                return;

            whosTurn = 1 - whosTurn;
        }
    }

    private async Task EndStageProcedure()
    {
        await _eventDict.SendEvent(StageEventDict.DID_STAGE, new StageDetails(_entities[1]));
        await _eventDict.SendEvent(StageEventDict.DID_STAGE, new StageDetails(_entities[0]));
        ForceCommit();
    }

    private async Task<bool> TryCommit(int whosTurn)
    {
        TryCommitDetails d = new TryCommitDetails(_entities[whosTurn]);
        
        await _eventDict.SendEvent(StageEventDict.WIL_TRY_COMMIT, d);

        if (d.Cancel)
            return false;
        
        if (whosTurn == 0)
        {
            if (_entities[whosTurn].Hp <= 0)
            {
                d.State = TryCommitDetails.StageState.HomeFailure;
            }

            if (_entities[1 - whosTurn].Hp <= 0)
            {
                d.State = TryCommitDetails.StageState.HomeVictory;
            }
        }
        else
        {
            if (_entities[whosTurn].Hp <= 0)
            {
                d.State = TryCommitDetails.StageState.HomeVictory;
            }
            if (_entities[1 - whosTurn].Hp <= 0)
            {
                d.State = TryCommitDetails.StageState.HomeFailure;
            }
        }

        if (d.State == TryCommitDetails.StageState.HomeVictory)
            _result.SetHomeVictory(true);
        else if (d.State == TryCommitDetails.StageState.HomeFailure)
            _result.SetHomeVictory(false);
        
        await _eventDict.SendEvent(StageEventDict.DID_TRY_COMMIT, d);
        
        return d.State != TryCommitDetails.StageState.OnGoing;
    }

    private void ForceCommit()
    {
        _result.SetHomeVictory(_entities[0].Hp >= _entities[1].Hp);
    }
}
