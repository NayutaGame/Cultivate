
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;

public class StageEnvironment : Addressable, StageClosureOwner
{
    private static readonly int MAX_TURN_COUNT = 120;

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
        StageClosure writeManaShortage = new StageClosure(StageClosureDict.DID_MANA_SHORTAGE, 0, WriteShortage);
        StageClosure writeArmorShortage = new StageClosure(StageClosureDict.DID_ARMOR_SHORTAGE, 0, WriteShortage);
        StageClosure writeManaCost = new StageClosure(StageClosureDict.DID_MANA_COST, 0, WriteCost);
        StageClosure writeChannelCost = new StageClosure(StageClosureDict.DID_CHANNEL_COST, 0, WriteCost);
        StageClosure writeHealthCost = new StageClosure(StageClosureDict.DID_HEALTH_COST, 0, WriteCost);
        StageClosure writeArmorCost = new StageClosure(StageClosureDict.DID_ARMOR_COST, 0, WriteCost);

        ClearResults();

        RegisterConfig();
        RegisterSkillClosures();

        Opening();

        await MingYuanPenaltyProcedure();
        await FormationProcedure();
        await StartStageProcedure();

        _closureDict.Register(this, writeManaShortage);
        _closureDict.Register(this, writeArmorShortage);
        _closureDict.Register(this, writeManaCost);
        _closureDict.Register(this, writeChannelCost);
        _closureDict.Register(this, writeHealthCost);
        _closureDict.Register(this, writeArmorCost);

        await BodyProcedure();

        _closureDict.Unregister(this, writeManaShortage);
        _closureDict.Unregister(this, writeArmorShortage);
        _closureDict.Unregister(this, writeManaCost);
        _closureDict.Unregister(this, writeChannelCost);
        _closureDict.Unregister(this, writeHealthCost);
        _closureDict.Unregister(this, writeArmorCost);

        await EndStageProcedure();

        await _kernel.CommitProcedure(this, MAX_TURN_COUNT, 0, true);

        UnregisterSkillClosures();
        UnregisterConfig();
    }

    public async Task FormationProcedure(StageEntity owner, RunFormation formation, bool recursive = true)
        => await FormationProcedure(new FormationDetails(owner, formation, recursive));

    public async Task FormationProcedure(FormationDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_ADD_FORMATION, d);
        if (d.Cancel) return;

        await d.Owner.AddFormation(new GainFormationDetails(d._formation));

        // await TryPlayTween(new BuffTweenDescriptor(d));

        _result.TryAppend($"    {d._formation.GetName()} is set");

        if (d.Cancel) return;
        await _closureDict.SendEvent(StageClosureDict.DID_ADD_FORMATION, d);
    }

    /// <summary>
    /// 发起一次攻击行为，会结算目标的护甲
    /// </summary>
    /// <param name="src">攻击者</param>
    /// <param name="tgt">受攻击者</param>
    /// <param name="value">攻击数值</param>
    /// <param name="times">攻击次数</param>
    /// <param name="srcSkill">技能来源</param>
    /// <param name="castResult">结果描述</param>
    /// <param name="wuXing">攻击特效的五行</param>
    /// <param name="crit">是否吸血</param>
    /// <param name="lifeSteal">是否吸血</param>
    /// <param name="penetrate">是否穿透</param>
    /// <param name="closures">额外行为</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="induced">该行为是间接行为，不会引起额外的角色动画</param>
    public async Task AttackProcedure(StageEntity src, StageEntity tgt, int value, int times, StageSkill srcSkill,
        WuXing? wuXing = null, bool crit = false, bool lifeSteal = false, bool penetrate = false, bool recursive = true,
        CastResult castResult = null,
        StageClosure[] closures = null, bool induced = false)
        => await AttackProcedure(
            new AttackDetails(src, tgt, value, times, srcSkill, wuXing, crit, lifeSteal, penetrate, false, recursive,
                castResult, closures), induced);

    public async Task AttackProcedure(AttackDetails attackDetails, bool induced)
    {
        RegisterAttackClosure(attackDetails);

        await _closureDict.SendEvent(StageClosureDict.WIL_FULL_ATTACK, attackDetails);
        await Play(new PreAttackedCharacterAnimation(false, attackDetails), induced);
        await Play(new AttackCharacterAnimation(true, attackDetails), induced);

        for (int i = 0; i < attackDetails.Times; i++)
        {
            AttackDetails d = attackDetails.ShallowClone();
            await _closureDict.SendEvent(StageClosureDict.WIL_ATTACK, d);
            await SingleAttackProcedure(d, induced);
            await _closureDict.SendEvent(StageClosureDict.DID_ATTACK, d);
            await NextKey(induced);
        }

        await _closureDict.SendEvent(StageClosureDict.DID_FULL_ATTACK, attackDetails);

        UnregisterAttackClosure(attackDetails);
    }

    private async Task SingleAttackProcedure(AttackDetails d, bool induced)
    {
        await Play(new PiercingVFXAnimation(false, d), induced);

        bool isEvaded = !d.Penetrate && d.Evade;
        if (isEvaded)
        {
            await EvadedProcedure(EvadedDetails.FromAttackDetails(d), induced);
            await _closureDict.SendEvent(StageClosureDict.UNDAMAGED, DamageDetails.FromAttackDetailsUndamaged(d));
            return;
        }

        if (!d.Penetrate && d.Tgt.Armor >= 0)
        {
            int negate = Mathf.Min(d.Value, d.Tgt.Armor);
            d.Value -= negate;
            await LoseArmorProcedure(d.Src, d.Tgt, negate);
        }

        if (d.Tgt.Armor < 0)
        {
            d.Value += -d.Tgt.Armor;
            d.Tgt.Armor = 0;

            await Play(new FragileVFXAnimation(false, d), induced);
        }

        await Play(new HitVFXAnimation(false, d), induced);

        bool isGuarded = d.Value == 0;
        if (isGuarded)
        {
            await GuardedProcedure(GuardedDetails.FromAttackDetails(d), induced);
            await _closureDict.SendEvent(StageClosureDict.UNDAMAGED, DamageDetails.FromAttackDetailsUndamaged(d));
            return;
        }

        await DamageProcedure(DamageDetails.FromAttackDetails(d), induced);

        _result.TryAppend($"    敌方气血[护甲]变成了${d.Tgt.Hp}[{d.Tgt.Armor}]");
    }

    private async Task EvadedProcedure(EvadedDetails d, bool induced)
    {
        await Play(new EvadedCharacterAnimation(false, d.Tgt), induced);
        _result.TryAppend($"    攻击被闪避");

        await _closureDict.SendEvent(StageClosureDict.DID_EVADE, d);
    }

    private async Task GuardedProcedure(GuardedDetails d, bool induced)
    {
        await Play(new GuardedVFXAnimation(false, d), induced);
        await Play(new GuardedTextAnimation(false, d), induced);
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
    public async Task IndirectProcedure(StageEntity src, StageEntity tgt, int value, StageSkill srcSkill,
        CastResult castResult, WuXing? wuXing = null, bool recursive = true, bool induced = false)
        => await IndirectProcedure(new IndirectDetails(src, tgt, value, srcSkill, wuXing, recursive), castResult,
            induced);

    public async Task IndirectProcedure(IndirectDetails indirectDetails, CastResult castResult, bool induced)
    {
        IndirectDetails d = indirectDetails.Clone();

        await _closureDict.SendEvent(StageClosureDict.WIL_INDIRECT, d);

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
            await _closureDict.SendEvent(StageClosureDict.DID_INDIRECT, d);
            return;
        }

        DamageDetails damageDetails = new DamageDetails(d.Src, d.Tgt, d.Value, d.SrcSkill, castResult: castResult);
        await DamageProcedure(damageDetails, induced);

        // await TryPlayTween(new AttackTweenDescriptor(d));
        _result.TryAppend($"    敌方气血[护甲]变成了${d.Tgt.Hp}[{d.Tgt.Armor}]");

        await _closureDict.SendEvent(StageClosureDict.DID_INDIRECT, d);
    }

    /// <summary>
    /// 发起一次伤害行为，不会结算目标的护甲
    /// </summary>
    /// <param name="src">伤害者</param>
    /// <param name="tgt">受伤害者</param>
    /// <param name="value">伤害数值</param>
    /// <param name="srcSkill">技能来源</param>
    /// <param name="crit">是否暴击</param>
    /// <param name="lifeSteal">是否吸血</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="castResult">结果描述</param>
    /// <param name="induced">是否是间接行为</param>
    public async Task DamageProcedure(StageEntity src, StageEntity tgt, int value, StageSkill srcSkill,
        bool crit = false, bool lifeSteal = false, bool recursive = true, CastResult castResult = null,
        bool induced = false)
        => await DamageProcedure(new DamageDetails(src, tgt, value, srcSkill, crit, lifeSteal, recursive, castResult),
            induced);

    public async Task DamageProcedure(DamageDetails d, bool induced)
    {
        if (d.Crit)
            d.Value *= 2;

        await _closureDict.SendEvent(StageClosureDict.WIL_DAMAGE, d);

        if (d.Cancel || d.Value == 0)
        {
            await Play(new UndamagedTextAnimation(false, d), induced);
            await _closureDict.SendEvent(StageClosureDict.UNDAMAGED, d);
            return;
        }

        await Play(new DamagedCharacterAnimation(false, d), induced);
        await Play(new DamagedTextAnimation(false, d), induced);
        await LoseHealthProcedure(d.Tgt, d.Value);

        await _closureDict.SendEvent(StageClosureDict.DID_DAMAGE, d);

        if (!d.Cancel && d.LifeSteal)
            await HealProcedure(d.Src, d.Src, d.Value, true);
    }

    public async Task LoseHealthProcedure(StageEntity owner, int value)
        => await LoseHealthProcedure(new LoseHealthDetails(owner, value));

    public async Task LoseHealthProcedure(LoseHealthDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_LOSE_HEALTH, d);
        if (d.Cancel)
            return;

        d.Owner.Hp -= d.Value;
        // d.Owner.TriggeredLowHealth |= d.Owner.IsLowHealth;

        await _closureDict.SendEvent(StageClosureDict.DID_LOSE_HEALTH, d);
    }

    public async Task HealProcedure(StageEntity src, StageEntity tgt, int value, bool induced)
        => await HealProcedure(new HealDetails(src, tgt, value), induced);

    public async Task HealProcedure(HealDetails d, bool induced)
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        await _closureDict.SendEvent(StageClosureDict.WIL_HEAL, d);

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
        // tgt.HealedRecord += actualHealed;

        await Play(new HealCharacterAnimation(true, d), induced);
        await Play(new HealVFXAnimation(false, d), induced);
        await Play(new HealTextAnimation(false, d), induced);
        _result.TryAppend($"    气血变成了${tgt.Hp}");

        await _closureDict.SendEvent(StageClosureDict.DID_HEAL, d);
    }

    public async Task GainBuffProcedure(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1,
        bool recursive = true, bool induced = false)
        => await GainBuffProcedure(new GainBuffDetails(src, tgt, buffEntry, stack, recursive), induced);

    public async Task GainBuffProcedure(GainBuffDetails d, bool induced)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_GAIN_BUFF, d);

        d.Cancel |= d._stack <= 0;
        if (d.Cancel) return;

        await Play(new BuffCharacterAnimation(true, d), induced);
        await Play(new BuffVFXAnimation(false, d), induced);
        await Play(new BuffTextAnimation(false, d), induced);
        _result.TryAppend($"    {d._buffEntry.GetName()} + {d._stack}");

        Buff buff = d.Tgt.FindBuff(d._buffEntry);
        if (buff != null && d._buffEntry.BuffStackRule != BuffStackRule.Individual)
        {
            switch (d._buffEntry.BuffStackRule)
            {
                case BuffStackRule.One:
                    break;
                case BuffStackRule.Add:
                    await buff.SetStack(buff.Stack + d._stack);
                    break;
                case BuffStackRule.Min:
                    await buff.SetStack(Mathf.Min(buff.Stack, d._stack));
                    break;
                case BuffStackRule.Max:
                    await buff.SetStack(Mathf.Max(buff.Stack, d._stack));
                    break;
                case BuffStackRule.Overwrite:
                    await buff.SetStack(d._stack);
                    break;
            }
        }
        else
        {
            buff = await d.Tgt.AddBuff(new BuffAppearDetails(d.Tgt, d._buffEntry, d._stack));
        }

        await _closureDict.SendEvent(StageClosureDict.DID_GAIN_BUFF, d);
        buff.PlayPingAnimation();
    }

    public async Task LoseBuffProcedure(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1,
        bool recursive = true)
        => await LoseBuffProcedure(new LoseBuffDetails(src, tgt, buffEntry, stack, recursive));

    public async Task LoseBuffProcedure(LoseBuffDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_LOSE_BUFF, d);

        if (d.Cancel)
            return;

        Buff b = d.Tgt.FindBuff(d._buffEntry);
        if (b != null)
            await b.SetStack(Mathf.Max(0, b.Stack - d._stack));

        await _closureDict.SendEvent(StageClosureDict.DID_LOSE_BUFF, d);
    }

    public async Task GainArmorProcedure(StageEntity src, StageEntity tgt, int value, bool induced)
        => await GainArmorProcedure(new GainArmorDetails(src, tgt, value), induced);

    public async Task GainArmorProcedure(GainArmorDetails d, bool induced)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_GAIN_ARMOR, d);

        if (d.Cancel)
            return;

        d.Tgt.Armor += d.Value;

        await Play(new GainArmorCharacterAnimation(true, d), induced);
        await Play(new GainArmorTextAnimation(false, d), induced);
        await Play(new GainArmorVFXAnimation(false, d), induced);
        _result.TryAppend($"    护甲变成了[{d.Tgt.Armor}]");

        await _closureDict.SendEvent(StageClosureDict.DID_GAIN_ARMOR, d);
    }

    public async Task LoseArmorProcedure(StageEntity src, StageEntity tgt, int value)
        => await LoseArmorProcedure(new LoseArmorDetails(src, tgt, value));

    public async Task LoseArmorProcedure(LoseArmorDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_LOSE_ARMOR, d);

        if (d.Cancel)
            return;

        // if (d.Tgt.Armor >= 0)
        //     d.Tgt.LostArmorRecord += Mathf.Min(d.Tgt.Armor, d.Value);

        d.Tgt.Armor -= d.Value;
        _result.TryAppend($"    护甲变成了[{d.Tgt.Armor}]");

        // 正变正，护甲伤害
        // 正变负，碎盾
        // 负变负，减甲

        await _closureDict.SendEvent(StageClosureDict.DID_LOSE_ARMOR, d);
    }

    public async Task ManaShortageProcedure(ManaCostResult d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_MANA_SHORTAGE, d);

        if (d.Cancel)
            return;

        await _closureDict.SendEvent(StageClosureDict.DID_MANA_SHORTAGE, d);
    }

    public async Task ArmorShortageProcedure(ArmorCostResult d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_ARMOR_SHORTAGE, d);

        if (d.Cancel)
            return;

        await _closureDict.SendEvent(StageClosureDict.DID_ARMOR_SHORTAGE, d);
    }

    public async Task ExhaustProcedure(StageEntity owner, StageSkill skill)
        => await ExhaustProcedure(new ExhaustDetails(owner, skill));

    public async Task ExhaustProcedure(ExhaustDetails d)
    {
        if (d.Skill.Exhausted)
            return;

        await _closureDict.SendEvent(StageClosureDict.WIL_EXHAUST, d);

        d.Skill.Exhausted = true;

        await _closureDict.SendEvent(StageClosureDict.DID_EXHAUST, d);
    }

    public async Task CycleProcedure(StageEntity owner, WuXing wuXing, int gain, int recover, bool induced)
        => await CycleProcedure(new CycleDetails(owner, wuXing, gain, recover), induced);

    public async Task CycleProcedure(CycleDetails d, bool induced)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_CYCLE, d);

        if (d.Cancel)
            return;

        WuXing fromWuXing = d.WuXing;
        for (int i = 0; i < d.Step; i++)
            fromWuXing = fromWuXing.Prev;

        int flow = d.Owner.GetStackOfBuff(fromWuXing._elementaryBuff);

        int consume = flow - Mathf.Min(flow, d.Recover);
        await d.Owner.TryConsumeProcedure(fromWuXing._elementaryBuff, consume);

        int gain = flow + d.Gain;
        await d.Owner.GainBuffProcedure(d.WuXing._elementaryBuff, gain, induced: induced);

        await _closureDict.SendEvent(StageClosureDict.DID_CYCLE, d);
    }

    public async Task DispelProcedure(StageEntity owner, int stack)
        => await DispelProcedure(new DispelDetails(owner, stack));

    public async Task DispelProcedure(DispelDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_DISPEL, d);

        if (d.Cancel)
            return;

        List<Buff> buffs = d.Owner.Buffs.FilterObj(b => !b.Friendly && b.Dispellable).ToList();

        foreach (Buff b in buffs)
            await d.Owner.LoseBuffProcedure(b.GetEntry(), d.Stack);

        await _closureDict.SendEvent(StageClosureDict.DID_DISPEL, d);
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
            if (runFormation.IsActivated())
                details.Add(new FormationDetails(entity, runFormation));

        details.Sort((lhs, rhs) => lhs._formation.GetEntry().GetOrder() - rhs._formation.GetEntry().GetOrder());

        foreach (var d in details)
            await FormationProcedure(d);
    }

    private async Task StartStageProcedure()
    {
        foreach (var e in _entities)
            await _closureDict.SendEvent(StageClosureDict.WIL_STAGE, new StageDetails(e));

        foreach (var e in _entities)
            await e.StartStageExecuteProcedure();
    }

    private async Task BodyProcedure()
    {
        int whosTurn = 0;
        for (int turnCount = 0; turnCount < MAX_TURN_COUNT; turnCount++)
        {
            StageEntity actor = _entities[whosTurn];

            _result.TryAppend($"--------第{turnCount}回合, {actor.GetName()}行动--------\n");
            await actor.TurnProcedure(turnCount);

            _entities.Do(e =>
            {
                _result.TryAppend($"{e.GetName()} {e.Hp}[{e.Armor}] Buff:");
                foreach (Buff b in e.Buffs)
                    _result.TryAppend($"  {b.GetName()}*{b.Stack}");
                _result.TryAppend("\n");
            });

            if (await _kernel.CommitProcedure(this, turnCount, whosTurn, false) != 0)
                return;

            whosTurn = 1 - whosTurn;
        }
    }

    private async Task EndStageProcedure()
    {
        await _closureDict.SendEvent(StageClosureDict.DID_STAGE, new StageDetails(_entities[1]));
        await _closureDict.SendEvent(StageClosureDict.DID_STAGE, new StageDetails(_entities[0]));
    }

    #endregion

    private StageConfig _config;
    public StageConfig Config => _config;

    private StageClosureDict _closureDict;
    public StageClosureDict ClosureDict => _closureDict;

    private StageEntity[] _entities;
    public StageEntity[] Entities => _entities;

    private StageKernel _kernel;

    private StageResult _result;
    public StageResult Result => _result;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();

    private StageEnvironment(StageConfig config)
    {
        _accessors = new()
        {
            { "Home", () => _entities[0] },
            { "Away", () => _entities[1] },
            { "Report", () => _result },
        };

        _config = config;

        _closureDict = new();

        _entities = new StageEntity[]
        {
            new(this, _config.Home, 0),
            new(this, _config.Away, 1),
        };

        _kernel = config.Kernel;

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

    public async Task Play(Animation animation, bool induced)
    {
        if (!_config.Animated)
            return;

        if (induced && animation.InvolvesCharacterAnimation())
            return;

        await StageManager.Instance.StageAnimationController.Play(animation);
    }

    public async Task NextKey(bool induced)
    {
        if (!_config.Animated)
            return;

        if (induced)
            return;

        await StageManager.Instance.StageAnimationController.NextKey();
    }

    public void WriteResult()
    {
        _entities[0].WriteResult();
        _config.Home.DepleteProcedure();
    }

    private async Task WriteShortage(StageClosureOwner listener, ClosureDetails stageClosureDetails)
    {
        CostResult d = (CostResult)stageClosureDetails;
        d.State = CostResult.CostState.Shortage;
    }

    private async Task WriteCost(StageClosureOwner listener, ClosureDetails stageClosureDetails)
    {
        CostResult d = (CostResult)stageClosureDetails;
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

    private void RegisterConfig()
    {
        if (_config.RunConfig == null)
            return;

        _closureDict.Register(this, _config.RunConfig.CharacterProfile.GetEntry()._stageClosures);

        DifficultyEntry difficultyEntry = _config.RunConfig.DifficultyProfile.GetEntry();
        _closureDict.Register(this, difficultyEntry._stageClosures);
        foreach (var additionalDifficultyEntry in difficultyEntry.AdditionalDifficulties)
            _closureDict.Register(this, additionalDifficultyEntry._stageClosures);
    }

    private void RegisterSkillClosures()
    {
        _entities.Do(e => e._skills.Traversal().Do(s => _closureDict.Register(s, s.Entry.Closures)));
    }

    private void RegisterAttackClosure(AttackDetails attackDetails)
    {
        _closureDict.Register(attackDetails.SrcSkill, attackDetails.Closures);
    }

    private void UnregisterConfig()
    {
        if (_config.RunConfig == null)
            return;

        _closureDict.Unregister(this, _config.RunConfig.CharacterProfile.GetEntry()._stageClosures);

        DifficultyEntry difficultyEntry = _config.RunConfig.DifficultyProfile.GetEntry();
        _closureDict.Unregister(this, difficultyEntry._stageClosures);
        foreach (var additionalDifficultyEntry in difficultyEntry.AdditionalDifficulties)
            _closureDict.Unregister(this, additionalDifficultyEntry._stageClosures);
    }

    private void UnregisterSkillClosures()
    {
        _entities.Do(e => e._skills.Traversal().Do(s => _closureDict.Unregister(s, s.Entry.Closures)));
    }

    private void UnregisterAttackClosure(AttackDetails attackDetails)
    {
        _closureDict.Unregister(attackDetails.SrcSkill, attackDetails.Closures);
    }
    
    private void ClearResults()
    {
        _entities.Do(stageEntity => stageEntity.RunEntity.TraversalCurrentSlots().Do(s => s.ClearResults()));
    }
}
