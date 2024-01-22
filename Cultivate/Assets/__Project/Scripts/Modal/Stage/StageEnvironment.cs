
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;
using DG.Tweening;

public class StageEnvironment : Addressable, StageEventListener
{
    private static readonly int MAX_ACTION_COUNT = 120;

    #region Procedures

    // public async Task SimpleProcedure(Args args)
    //     => await SimpleProcedure(new SimpleDetails(args));
    // public async Task SimpleProcedure(SimpleDetails d)
    // {
    //     await _eventDict.SendEvent(CLEventDict.WILL_SIMPLE, d);
    //
    //     // actual work
    //
    //     await _eventDict.SendEvent(CLEventDict.DID_SIMPLE, d);
    // }

    public async Task FormationProcedure(StageEntity owner, RunFormation formation, bool recursive = true)
        => await FormationProcedure(new FormationDetails(owner, formation, recursive));
    public async Task FormationProcedure(FormationDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.FORMATION_WILL_ADD, d);
        if (d.Cancel) return;

        await d.Owner.AddFormation(new GainFormationDetails(d._formation));

        // await TryPlayTween(new BuffTweenDescriptor(d));

        _result.TryAppend($"    {d._formation.GetName()} is set");

        if (d.Cancel) return;
        await _eventDict.SendEvent(StageEventDict.FORMATION_DID_ADD, d);
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
        if (await attackDetails.Src.TryConsumeProcedure("追击")) // 结算连击/追击
            times += 1;

        await TryPlayTween(new PreAttackTweenDescriptor(true, attackDetails.Src));

        for (int i = 0; i < times; i++)
        {
            AttackDetails d = attackDetails.Clone();

            StageEntity src = d.Src;
            StageEntity tgt = d.Tgt;

            await _eventDict.SendEvent(StageEventDict.WILL_ATTACK, d);
            // 添加穿透，应用吸血，应该在这个里面

            if (d.Cancel)
            {
                // 演出很麻烦。攻击被取消，应该对应的是空手夺白刃
                _result.TryAppend($"    攻击被取消");
                continue;
            }

            await TryPlayTween(new AttackTweenDescriptor(true, d));

            if (!d.Pierce && d.Evade)
            {
                await TryPlayTween(new EvadeTweenDescriptor(false, d.Tgt));
                _result.TryAppend($"    攻击被闪避");
                await _eventDict.SendEvent(StageEventDict.DID_EVADE, d);

                if (d.Undamaged != null)
                    await d.Undamaged(new DamageDetails(d.Src, d.Tgt, 0));
                await _eventDict.SendEvent(StageEventDict.DID_ATTACK, d);
                continue;
            }

            if (!d.Pierce && tgt.Armor >= 0)
            {
                int negate = Mathf.Min(d.Value, tgt.Armor);
                d.Value -= negate;
                await ArmorLoseProcedure(d.Src, d.Tgt, negate);
            }

            if (tgt.Armor < 0)
            {
                d.Value += -tgt.Armor;
                tgt.Armor = 0;
            }

            if (d.Value == 0)
            {
                _result.TryAppend($"    攻击为0");

                if (d.Undamaged != null)
                    await d.Undamaged(new DamageDetails(d.Src, d.Tgt, 0));
                await _eventDict.SendEvent(StageEventDict.DID_ATTACK, d);
                continue;
            }

            await TryPlayTween(new AttackedTweenDescriptor(false, d));

            if (d.Crit)
                d.Value *= 2;

            DamageDetails damageDetails = new DamageDetails(d.Src, d.Tgt, d.Value, damaged: d.Damaged, undamaged: d.Undamaged);
            await DamageProcedure(damageDetails);

            _result.TryAppend($"    敌方生命[护甲]变成了${tgt.Hp}[{tgt.Armor}]");

            if (!damageDetails.Cancel)
            {
                if (d.LifeSteal)
                    await HealProcedure(src, src, damageDetails.Value);
            }

            await _eventDict.SendEvent(StageEventDict.DID_ATTACK, d);
        }

        await TryPlayTween(new PostAttackTweenDescriptor(true, attackDetails.Src));
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

        await _eventDict.SendEvent(StageEventDict.WILL_INDIRECT, d);

        if (d.Cancel)
        {
            _result.TryAppend($"    攻击被取消");
            return;
        }

        if (d.Tgt.Armor >= 0)
        {
            int negate = Mathf.Min(d.Value, d.Tgt.Armor);
            d.Value -= negate;
            await ArmorLoseProcedure(d.Src, d.Tgt, negate);
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
    /// <param name="recursive">是否会递归</param>
    /// <param name="damaged">如果造成伤害时候的额外行为</param>
    /// <param name="undamaged">如果未造成伤害的额外行为</param>
    public async Task DamageProcedure(StageEntity src, StageEntity tgt, int value, bool recursive = true,
        Func<DamageDetails, Task> damaged = null, Func<DamageDetails, Task> undamaged = null)
        => await DamageProcedure(new DamageDetails(src, tgt, value, recursive, damaged, undamaged));
    public async Task DamageProcedure(DamageDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WILL_DAMAGE, d);

        if (d.Cancel || d.Value == 0)
        {
            await TryPlayTween(new UndamagedTweenDescriptor(false, d));
            if (d.Undamaged != null)
                await d.Undamaged(d);
            return;
        }

        await TryPlayTween(new DamagedTweenDescriptor(false, d));
        await LoseHealthProcedure(d.Tgt, d.Value);
        if (d.Damaged != null)
            await d.Damaged(d);

        await _eventDict.SendEvent(StageEventDict.DID_DAMAGE, d);
    }

    public async Task LoseHealthProcedure(StageEntity owner, int value)
        => await LoseHealthProcedure(new LoseHealthDetails(owner, value));
    public async Task LoseHealthProcedure(LoseHealthDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WILL_LOSE_HEALTH, d);
        if (d.Cancel)
            return;

        d.Owner.Hp -= d.Value;

        await _eventDict.SendEvent(StageEventDict.DID_LOSE_HEALTH, d);
    }

    public async Task HealProcedure(StageEntity src, StageEntity tgt, int value)
        => await HealProcedure(new HealDetails(src, tgt, value));
    public async Task HealProcedure(HealDetails d)
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        await _eventDict.SendEvent(StageEventDict.WILL_HEAL, d);

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

        await TryPlayTween(new HealTweenDescriptor(true, d));
        _result.TryAppend($"    生命变成了${tgt.Hp}");

        await _eventDict.SendEvent(StageEventDict.DID_HEAL, d);
    }

    public async Task BuffProcedure(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await BuffProcedure(new BuffDetails(src, tgt, buffEntry, stack, recursive));
    public async Task BuffProcedure(BuffDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WILL_BUFF, d);

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

            await TryPlayTween(new BuffTweenDescriptor(true, d));
            _result.TryAppend($"    {d._buffEntry.GetName()}: {oldStack} -> {same.Stack}");
        }
        else
        {
            await d.Tgt.AddBuff(new GainBuffDetails(d._buffEntry, d._stack));

            await TryPlayTween(new BuffTweenDescriptor(true, d));
            _result.TryAppend($"    {d._buffEntry.GetName()}: 0 -> {d._stack}");
        }

        await _eventDict.SendEvent(StageEventDict.DID_BUFF, d);
    }

    public async Task ArmorGainProcedure(StageEntity src, StageEntity tgt, int value)
        => await ArmorGainProcedure(new ArmorGainDetails(src, tgt, value));
    public async Task ArmorGainProcedure(ArmorGainDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.ARMOR_WILL_GAIN, d);

        if (d.Cancel)
            return;

        d.Tgt.Armor += d.Value;
        _result.TryAppend($"    护甲变成了[{d.Tgt.Armor}]");

        await _eventDict.SendEvent(StageEventDict.ARMOR_DID_GAIN, d);
    }

    public async Task ArmorLoseProcedure(StageEntity src, StageEntity tgt, int value)
        => await ArmorLoseProcedure(new ArmorLoseDetails(src, tgt, value));
    public async Task ArmorLoseProcedure(ArmorLoseDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.ARMOR_WILL_LOSE, d);

        if (d.Cancel)
            return;

        if (d.Tgt.Armor >= 0)
            d.Tgt.LostArmorRecord += Mathf.Min(d.Tgt.Armor, d.Value);

        d.Tgt.Armor -= d.Value;
        _result.TryAppend($"    护甲变成了[{d.Tgt.Armor}]");

        // 正变正，护甲伤害
        // 正变负，碎盾
        // 负变负，减甲

        await _eventDict.SendEvent(StageEventDict.ARMOR_DID_LOSE, d);
    }

    public async Task DispelProcedure(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool friendly = true, bool recursive = true)
        => await DispelProcedure(new DispelDetails(src, tgt, buffEntry, stack, friendly, recursive));
    public async Task DispelProcedure(DispelDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WILL_DISPEL, d);

        if (d.Cancel)
            return;

        Buff b = d.Tgt.FindBuff(d._buffEntry);
        if (b != null)
            await b.SetStack(Mathf.Max(0, b.Stack - d._stack));

        await _eventDict.SendEvent(StageEventDict.DID_DISPEL, d);
    }

    public async Task ManaShortageProcedure(StageEntity owner, int position, StageSkill skill, int actualCost)
        => await ManaShortageProcedure(new ManaShortageDetails(owner, position, skill, actualCost));
    public async Task ManaShortageProcedure(ManaShortageDetails d)
    {
        await _eventDict.SendEvent(StageEventDict.WILL_MANA_SHORTAGE, d);

        if (d.Cancel)
            return;

        await _eventDict.SendEvent(StageEventDict.DID_MANA_SHORTAGE, d);
    }

    public async Task ExhaustProcedure(StageEntity owner, StageSkill skill)
        => await ExhaustProcedure(new ExhaustDetails(owner, skill));
    public async Task ExhaustProcedure(ExhaustDetails d)
    {
        if (d.Skill.Exhausted)
            return;

        await _eventDict.SendEvent(StageEventDict.WILL_EXHAUST, d);

        d.Skill.Exhausted = true;

        await _eventDict.SendEvent(StageEventDict.DID_EXHAUST, d);
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

        _result = new(_config.GenerateReport, _config.GenerateTimeline);
    }

    public static StageEnvironment FromConfig(StageConfig config)
        => new(config);

    public void Execute()
    {
        if (!_config.Animated)
        {
            Simulate().GetAwaiter().GetResult();

            if (_config.WriteResult)
                WriteResult();

            return;
        }

        StageEnvironment futureEnvironment =
            FromConfig(new StageConfig(false, false, false, true, _config.Home, _config.Away));
        futureEnvironment.Simulate().GetAwaiter().GetResult();

        StageManager.Instance.Environment = this;
        StageManager.Instance.Timeline = futureEnvironment.Result.Timeline;

        AppManager.Push(new StageAppS());
    }

    public async Task TryPlayTween(StageTweenDescriptor descriptor)
    {
        if (!_config.Animated)
            return;
        await StageManager.Instance.Anim.PlayTween(descriptor);
    }

    public async Task TryPlayTween(Tween tween)
    {
        if (!_config.Animated)
            return;
        await StageManager.Instance.Anim.PlayTween(true, tween);
    }

    public void WriteResult()
    {
        _entities[0].WriteResult();
        _config.Home.TryExhaust();
    }

    public async Task Simulate()
    {
        StageEventDescriptor writeManaShortage = new StageEventDescriptor(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_MANA_SHORTAGE, 0,
            async (listener, d) =>
            {
                ManaShortageDetails manaShortageDetails = (ManaShortageDetails)d;
                RunEntity entity = manaShortageDetails.Owner.RunEntity;
                int index = manaShortageDetails.Position + RunManager.SkillStartFromJingJie[entity.GetJingJie()];
                SkillSlot slot = entity.GetSlot(index);

                if (slot.ManaIndicator.State != ManaIndicator.ManaCostState.Unwritten)
                    return;

                slot.ManaIndicator = new ManaIndicator(ManaIndicator.ManaCostState.Shortage,
                    manaShortageDetails.LiteralCost, manaShortageDetails.ActualCost);
            });

        StageEventDescriptor writeManaCost = new StageEventDescriptor(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_MANA_COST, 0,
            async (listener, d) =>
            {
                ManaCostDetails manaCostDetails = (ManaCostDetails)d;

                RunEntity entity = manaCostDetails.Caster.RunEntity;
                int index = manaCostDetails.Skill.SlotIndex + RunManager.SkillStartFromJingJie[entity.GetJingJie()];
                SkillSlot slot = entity.GetSlot(index);

                if (slot.ManaIndicator.State != ManaIndicator.ManaCostState.Unwritten)
                    return;

                ManaIndicator.ManaCostState state = (manaCostDetails.ActualCost < manaCostDetails.LiteralCost)
                    ? ManaIndicator.ManaCostState.Reduced
                    : ManaIndicator.ManaCostState.Normal;

                slot.ManaIndicator = new ManaIndicator(state,
                    manaCostDetails.LiteralCost, manaCostDetails.ActualCost);
            });

        ClearManaIndicator();

        await MingYuanPenaltyProcedure();
        await FormationProcedure();
        await StartStageProcedure();

        _eventDict.Register(this, writeManaShortage);
        _eventDict.Register(this, writeManaCost);

        await TurnProcedure();

        _eventDict.Unregister(this, writeManaShortage);
        _eventDict.Unregister(this, writeManaCost);

        await EndStageProcedure();

        _result.HomeLeftHp = _entities[0].Hp;
        _result.AwayLeftHp = _entities[1].Hp;
        _result.TryAppend(_result.HomeVictory ? $"主场胜利\n" : $"主场失败\n");
    }

    private void ClearManaIndicator()
    {
        _entities.Do(stageEntity =>
        {
            RunEntity e = stageEntity.RunEntity;
            e.TraversalCurrentSlots().Do(s =>
            {
                s.ManaIndicator = ManaIndicator.Default();
            });
        });
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
        {
            e._p = -1;
            await _eventDict.SendEvent(StageEventDict.START_STAGE, new StageDetails(e));
        }
    }

    private async Task TurnProcedure()
    {
        int whosTurn = 0;
        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            StageEntity actor = _entities[whosTurn];

            _result.TryAppend($"--------第{i}回合, {actor.GetName()}行动--------\n");
            await actor.Turn();

            _entities.Do(e =>
            {
                _result.TryAppend($"{e.GetName()} {e.Hp}[{e.Armor}] Buff:");
                foreach (Buff b in e.Buffs)
                    _result.TryAppend($"  {b.GetName()}*{b.Stack}");
                _result.TryAppend("\n");
            });

            if (TryCommit(whosTurn))
                return;

            whosTurn = 1 - whosTurn;
        }
    }

    private async Task EndStageProcedure()
    {
        await _eventDict.SendEvent(StageEventDict.END_STAGE, new StageDetails(_entities[1]));
        await _eventDict.SendEvent(StageEventDict.END_STAGE, new StageDetails(_entities[0]));
        ForceCommit();
    }

    private bool TryCommit(int whosTurn)
    {
        if (whosTurn == 0)
        {
            if (_entities[whosTurn].Hp <= 0)
            {
                _result.SetHomeVictory(false);
                return true;
            }

            if (_entities[1 - whosTurn].Hp <= 0)
            {
                _result.SetHomeVictory(true);
                return true;
            }
        }
        else
        {
            if (_entities[whosTurn].Hp <= 0)
            {
                _result.SetHomeVictory(true);
                return true;
            }
            if (_entities[1 - whosTurn].Hp <= 0)
            {
                _result.SetHomeVictory(false);
                return true;
            }
        }

        return false;
    }

    private void ForceCommit()
    {
        _result.SetHomeVictory(_entities[0].Hp >= _entities[1].Hp);
    }
}
