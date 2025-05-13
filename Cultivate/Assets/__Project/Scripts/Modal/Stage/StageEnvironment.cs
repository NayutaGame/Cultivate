
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using CLLibrary;
using UnityEngine;

public class StageEnvironment : Addressable, StageClosureListener
{
    private static readonly int MAX_TURN_COUNT = 120;

    #region Procedures

    // public async UniTask SimpleProcedure(Args args)
    //     => await SimpleProcedure(new SimpleDetails(args));
    // public async UniTask SimpleProcedure(SimpleDetails d)
    // {
    //     await _closureDict.SendEvent(StageClosureDict.WIL_SIMPLE, d);
    //     if (d.Cancel) return;
    //
    //     // actual work
    //
    //     await _closureDict.SendEvent(StageClosureDict.DID_SIMPLE, d);
    // }

    public async UniTask CoreProcedure()
    {
        StageClosure[] closures = new StageClosure[]
        {
            new(StageClosureDict.DID_MANA_SHORTAGE, 0, WriteShortage),
            new(StageClosureDict.DID_ARMOR_SHORTAGE, 0, WriteShortage),
            new(StageClosureDict.DID_MANA_COST, 0, WriteCost),
            new(StageClosureDict.DID_CHANNEL_COST, 0, WriteCost),
            new(StageClosureDict.DID_HEALTH_COST, 0, WriteCost),
            new(StageClosureDict.DID_ARMOR_COST, 0, WriteCost),
        };

        ClearResults();

        RegisterConfigClosures();
        RegisterAchievementClosures();
        RegisterEntityClosures();
        RegisterSkillClosures();

        await EnteringProcedure();

        await MingYuanPenaltyProcedure();
        await FormationProcedure();
        await StartStageProcedure();

        _closureDict.Register(this, closures);

        await BodyProcedure();

        _closureDict.Unregister(this, closures);

        await EndStageProcedure();

        await ForcedCommitProcedure();

        UnregisterSkillClosures();
        UnregisterEntityClosures();
        UnregisterAchievementClosures();
        UnregisterConfigClosures();

        await AnimationToFinishProcedure();
    }

    private async UniTask AnimationToFinishProcedure()
    {
        if (!_config.Animated)
            return;
        
        await PlayAsync(new WaitAnimation(3));
    }

    private async UniTask FormationProcedure()
    {
        List<GainFormationDetails> details = new();

        foreach (var entity in _entities)
        foreach (var runFormation in entity.RunFormations())
            if (runFormation.IsActivated())
                details.Add(new GainFormationDetails(entity, runFormation));

        details.Sort((lhs, rhs) => lhs._formation.GetEntry().GetOrder() - rhs._formation.GetEntry().GetOrder());

        foreach (var d in details)
            await GainFormationProcedure(d);
    }

    private async UniTask GainFormationProcedure(GainFormationDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_GAIN_FORMATION, d);
        if (d.Cancel) return;
        
        Formation formation = new Formation(d.Owner, d._formation);
        d.Owner.AddFormation(formation);
        await GainFormationStaging(d, formation);

        await _closureDict.SendEvent(StageClosureDict.DID_GAIN_FORMATION, d);
    }

    private async UniTask GainFormationStaging(GainFormationDetails d, Formation f)
    {
        _result.TryAppend($"    {d._formation.GetName()} is set");
        
        if (!_config.Animated)
            return;
        
        Play(new FormationVFXAnimation(d, false));
        Play(TextAnimation.FromGainFormationDetails(d));
        await PlayAsync(d.Owner.Model().GetAnimationFromBuffSelf(d.Induced));
        CanvasManager.Instance.StageCanvas.GainFormationStaging(d.Owner == _entities[0]);
        f.Emphasize();
    }

    public async UniTask GainBuffProcedure(GainBuffDetails d)
    {
        RegisterTempClosures(d.Listener, d.Closures);
        await _closureDict.SendEvent(StageClosureDict.WIL_GAIN_BUFF, d);
        d.Cancel |= d.Stack <= 0;
        if (d.Cancel)
        {
            UnregisterTempClosures(d.Listener, d.Closures);
            return;
        }

        Buff buff = d.Tgt.FindBuff(d.BuffEntry);
        
        bool generateNew = buff == null || d.BuffEntry.BuffStackRule == BuffStackRule.Individual;
        if (generateNew)
        {
            buff = new Buff(d.Tgt, d.BuffEntry);
            d.Tgt.AddBuff(buff);
            buff.SetStack(d.Stack);
            await GainBuffStaging(d, buff);
        }
        else
        {
            int newStack = buff.Stack;
            switch (d.BuffEntry.BuffStackRule)
            {
                case BuffStackRule.One:
                    return;
                    break;
                case BuffStackRule.Add:
                    newStack = buff.Stack + d.Stack;
                    break;
                case BuffStackRule.Min:
                    newStack = Mathf.Min(buff.Stack, d.Stack);
                    break;
                case BuffStackRule.Max:
                    newStack = Mathf.Max(buff.Stack, d.Stack);
                    break;
                case BuffStackRule.Overwrite:
                    newStack = d.Stack;
                    break;
            }

            buff.SetStack(newStack);
            await GainBuffStackStaging(d, buff);
        }

        await _closureDict.SendEvent(StageClosureDict.DID_GAIN_BUFF, d);
        UnregisterTempClosures(d.Listener, d.Closures);
    }

    public async UniTask LoseBuffProcedure(LoseBuffDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_LOSE_BUFF, d);
        d.Cancel |= d.Stack <= 0;
        if (d.Cancel)
            return;

        Buff b = d.Tgt.FindBuff(d.BuffEntry);
        if (b == null)
            return;
        
        int newStack = Mathf.Max(0, b.Stack - d.Stack);

        if (newStack > 0)
        {
            b.SetStack(newStack);
            await LoseBuffStackStaging(d, b);
        }
        else
        {
            int buffIndex = b.Owner.IndexOfBuff(b);
            
            b.SetStack(0);
            b.Owner.RemoveBuff(b);
            await LoseBuffStaging(d, buffIndex);
        }

        await _closureDict.SendEvent(StageClosureDict.DID_LOSE_BUFF, d);
    }

    private async UniTask GainBuffStaging(GainBuffDetails d, Buff buff)
    {
        _result.TryAppend($"    {d.BuffEntry.GetName()} +{d.Stack}");
        if (!_config.Animated)
            return;
        
        Play(BuffVFXAnimation.FromGainBuffDetails(d, false));
        Play(TextAnimation.FromGainBuffDetails(d));
        
        if (d.Src == d.Tgt)
        {
            await PlayAsync(d.Src.Model().GetAnimationFromBuffSelf(d.Induced));
        }
        else
        {
            await PlayAsync(d.Src.Model().GetAnimationFromBuffSelf(d.Induced));
        }
        
        CanvasManager.Instance.StageCanvas.GainBuffStaging(d.Tgt == _entities[0]);
        buff.Emphasize();
    }

    private async UniTask GainBuffStackStaging(GainBuffDetails d, Buff buff)
    {
        _result.TryAppend($"    {d.BuffEntry.GetName()} +{d.Stack}");
        if (!_config.Animated)
            return;
        
        Play(BuffVFXAnimation.FromGainBuffDetails(d, false));
        Play(TextAnimation.FromGainBuffDetails(d));
        
        if (d.Src == d.Tgt)
        {
            await PlayAsync(d.Tgt.Model().GetAnimationFromBuffSelf(d.Induced));
        }
        else
        {
            await PlayAsync(d.Tgt.Model().GetAnimationFromBuffSelf(d.Induced));
        }
        
        buff.Emphasize();
    }

    private async UniTask LoseBuffStaging(LoseBuffDetails d, int buffIndex)
    {
        _result.TryAppend($"    {d.BuffEntry.GetName()} losing:{d.Stack}");
        if (!_config.Animated)
            return;
        
        // Play(BuffVFXAnimation.FromLoseBuffDetails(d, false));
        Play(TextAnimation.FromLoseBuffDetails(d));
        // if (d.Src == d.Tgt)
        // {
        //     await PlayAsync(d.Tgt.Model().GetAnimationFromBuffSelf(d.Induced));
        // }
        // else
        // {
        //     await PlayAsync(d.Tgt.Model().GetAnimationFromBuffSelf(d.Induced));
        // }
        
        CanvasManager.Instance.StageCanvas.LoseBuffStaging(d.Tgt == _entities[0], buffIndex);
    }

    private async UniTask LoseBuffStackStaging(LoseBuffDetails d, Buff buff)
    {
        _result.TryAppend($"    {d.BuffEntry.GetName()} losing:{d.Stack}");
        if (!_config.Animated)
            return;
        
        // Play(BuffVFXAnimation.FromLoseBuffDetails(d, false));
        Play(TextAnimation.FromLoseBuffDetails(d));
        
        // if (d.Src == d.Tgt)
        // {
        //     await PlayAsync(d.Tgt.Model().GetAnimationFromBuffSelf(d.Induced));
        // }
        // else
        // {
        //     await PlayAsync(d.Tgt.Model().GetAnimationFromBuffSelf(d.Induced));
        // }
        
        buff.Emphasize();
    }

    public async UniTask AttackProcedure(AttackDetails attackDetails)
    {
        RegisterTempClosures(attackDetails.Listener, attackDetails.Closures);

        attackDetails.Value = Mathf.Max(1, attackDetails.Value);
        attackDetails.Times = Mathf.Max(1, attackDetails.Times);

        await _closureDict.SendEvent(StageClosureDict.WIL_FULL_ATTACK, attackDetails);
        await FullAttackStaging(attackDetails);

        if (!attackDetails.DoesntConsumeJianYi)
        {
            string thisTurnAttackedKey = "thisTurnAttacked";
            attackDetails.Src.Memory.SetVariable(thisTurnAttackedKey, true);
        }

        for (int i = 0; i < attackDetails.Times; i++)
        {
            AttackDetails d = attackDetails.ShallowClone();
            await _closureDict.SendEvent(StageClosureDict.WIL_ATTACK, d);
            await SingleAttackProcedure(d);
            await _closureDict.SendEvent(StageClosureDict.DID_ATTACK, d);
            await NextKey(d.Induced);
        }

        await _closureDict.SendEvent(StageClosureDict.DID_FULL_ATTACK, attackDetails);
        
        UnregisterTempClosures(attackDetails.Listener, attackDetails.Closures);
        
        // check win condition, but do not commit
        await RecoverStaging(attackDetails);
    }

    private async UniTask FullAttackStaging(AttackDetails attackDetails)
    {
        if (!_config.Animated)
            return;
        
        int armor = attackDetails.Tgt.Armor;
        
        if (armor > 0)
        {
            await PlayAsync(attackDetails.Tgt.Model().GetAnimationFromGuard(attackDetails.Induced));
        }
        else if (armor < 0)
        {
            await PlayAsync(attackDetails.Tgt.Model().GetAnimationFromUnguard(attackDetails.Induced));
        }
        
        await PlayAsync(attackDetails.Src.Model().GetAnimationFromAttack(attackDetails.Induced, attackDetails.Times));
    }

    private async UniTask RecoverStaging(AttackDetails attackDetails)
    {
        if (!_config.Animated)
            return;
        
        await PlayAsync(attackDetails.Tgt.Model().GetAnimationFromRecover());
    }

    private async UniTask SingleAttackProcedure(AttackDetails d)
    {
        await PlayAsync(new PiercingVFXAnimation(d, false));

        bool isEvaded = !d.Penetrate && d.Evade;
        if (isEvaded)
        {
            await EvadedProcedure(EvadedDetails.FromAttackDetails(d));
            await _closureDict.SendEvent(StageClosureDict.UNDAMAGED, DamageDetails.FromAttackDetailsUndamaged(d));
            return;
        }

        if (!d.Penetrate && d.Tgt.Armor >= 0)
        {
            int ratio = d.Shatter ? 2 : 1;
            int negate = Mathf.Min(ratio * d.Value, d.Tgt.Armor);
            d.Value -= negate / ratio;
            await LoseArmorProcedure(new LoseArmorDetails(d.Src, d.Tgt, negate, d.Listener, d.Closures, d.CastResult, d.Induced));
        }

        if (d.Tgt.Armor < 0)
        {
            d.Value += -d.Tgt.Armor;
            d.Tgt.Armor = 0;

            await PlayAsync(new FragileVFXAnimation(d, false));
        }

        await PlayAsync(new HitVFXAnimation(d, false));

        bool isGuarded = d.Value == 0;
        if (isGuarded)
        {
            await GuardedProcedure(GuardedDetails.FromAttackDetails(d));
            await _closureDict.SendEvent(StageClosureDict.UNDAMAGED, DamageDetails.FromAttackDetailsUndamaged(d));
            return;
        }

        await DamageProcedure(DamageDetails.FromAttackDetails(d));

        _result.TryAppend($"    敌方气血[护甲]变成了${d.Tgt.Hp}[{d.Tgt.Armor}]");
    }

    private async UniTask EvadedProcedure(EvadedDetails d)
    {
        if (_config.Animated)
            await PlayAsync(d.Tgt.Model().GetAnimationFromEvaded(d.Induced));
        _result.TryAppend($"    攻击被闪避");

        await _closureDict.SendEvent(StageClosureDict.DID_EVADE, d);
    }

    private async UniTask GuardedProcedure(GuardedDetails d)
    {
        await PlayAsync(new GuardedVFXAnimation(d, false));
        await PlayAsync(TextAnimation.FromGuardedDetails(d));
        _result.TryAppend($"    攻击被格挡");
    }

    public async UniTask IndirectProcedure(StageEntity src, StageEntity tgt, int value, StageSkill srcSkill,
        ResultDict castResult, WuXing? wuXing = null, bool lifesteal = false, bool recursive = true, bool induced = false)
        => await IndirectProcedure(new IndirectDetails(src, tgt, value, srcSkill, wuXing, lifesteal, recursive, castResult, induced));

    public async UniTask IndirectProcedure(IndirectDetails indirectDetails)
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
            await LoseArmorProcedure(new LoseArmorDetails(d.Src, d.Tgt, negate, d.SrcSkill, null, d.CastResult, d.Induced));
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

        await DamageProcedure(DamageDetails.FromIndirectDetails(d));

        // await TryPlayTween(new AttackTweenDescriptor(d));
        _result.TryAppend($"    敌方气血[护甲]变成了${d.Tgt.Hp}[{d.Tgt.Armor}]");

        await _closureDict.SendEvent(StageClosureDict.DID_INDIRECT, d);
    }

    public async UniTask DamageProcedure(DamageDetails d)
    {
        if (d.Crit)
            d.Value *= 2;

        await _closureDict.SendEvent(StageClosureDict.WIL_DAMAGE, d);

        if (d.Cancel || d.Value == 0)
        {
            await PlayAsync(TextAnimation.FromNoDamaged(d));
            await _closureDict.SendEvent(StageClosureDict.UNDAMAGED, d);
            return;
        }
        
        if (_config.Animated)
        {
            await PlayAsync(d.Tgt.Model().GetAnimationFromDamaged(d.Induced));
            await PlayAsync(TextAnimation.FromDamageDetails(d));
        }
        await LoseHealthProcedure(d.Tgt, d.Value, d.CausedByAttack, d.Listener, d.Closures, d.CastResult, d.Induced);

        await _closureDict.SendEvent(StageClosureDict.DID_DAMAGE, d);

        if (!d.Cancel && d.LifeSteal)
            await HealProcedure(d.Src, d.Src, d.Value, false, d.Listener, d.CastResult, null,true);
    }

    public async UniTask LoseHealthProcedure(
        StageEntity owner, int value, bool causedByAttack, StageClosureListener listener, StageClosure[] closures, ResultDict castResult, bool induced)
        => await LoseHealthProcedure(new LoseHealthDetails(owner, value, causedByAttack, listener, closures, castResult, induced));

    public async UniTask LoseHealthProcedure(LoseHealthDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_LOSE_HEALTH, d);
        if (d.Cancel)
            return;

        d.Victim.Hp -= d.Value;
        if (d.Victim.Hp <= 0 && d.CausedByAttack)
            d.Victim.DeathCauseIsAttack = true;

        await _closureDict.SendEvent(StageClosureDict.DID_LOSE_HEALTH, d);
    }

    public async UniTask HealProcedure(StageEntity src, StageEntity tgt, int value, bool penetrate,
        StageClosureListener initiator, ResultDict castResult, StageClosure[] closures, bool induced)
        => await HealProcedure(new(src, tgt, value, penetrate, initiator, castResult, closures, induced));

    public async UniTask HealProcedure(HealDetails d)
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

        if (_config.Animated)
        {
            await PlayAsync(d.Src.Model().GetAnimationFromHeal(d.Induced));
            await PlayAsync(new HealVFXAnimation(d, false));
            await PlayAsync(TextAnimation.FromHealDetails(d));
        }
        _result.TryAppend($"    气血变成了${tgt.Hp}");

        await _closureDict.SendEvent(StageClosureDict.DID_HEAL, d);
    }

    public async UniTask BurnProcedure(StageEntity owner, int value, bool induced)
        => await BurnProcedure(new BurnDetails(owner, value, induced));

    public async UniTask BurnProcedure(BurnDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_BURN, d);

        d.Cancel |= d.Value <= 0;
        if (d.Cancel)
            return;

        await DamageProcedure(DamageDetails.FromBurn(d));

        await _closureDict.SendEvent(StageClosureDict.DID_BURN, d);
    }

    public async UniTask GainArmorProcedure(GainArmorDetails d)
    {
        RegisterTempClosures(d.Listener, d.Closures);
        await _closureDict.SendEvent(StageClosureDict.WIL_GAIN_ARMOR, d);

        d.Cancel |= d.Value <= 0;

        if (d.Cancel)
        {
            UnregisterTempClosures(d.Listener, d.Closures);
            return;
        }

        d.Tgt.Armor += d.Value;

        if (_config.Animated)
        {
            await PlayAsync(d.Tgt.Model().GetAnimationFromGainArmor(d.Induced));
            await PlayAsync(TextAnimation.FromGainArmorDetails(d));
            await PlayAsync(new GainArmorVFXAnimation(d, false));
        }
        _result.TryAppend($"    护甲变成了[{d.Tgt.Armor}]");

        await _closureDict.SendEvent(StageClosureDict.DID_GAIN_ARMOR, d);
        UnregisterTempClosures(d.Listener, d.Closures);
    }

    public async UniTask LoseArmorProcedure(LoseArmorDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_LOSE_ARMOR, d);

        if (d.Cancel)
            return;

        d.Tgt.Armor -= d.Value;
        _result.TryAppend($"    护甲变成了[{d.Tgt.Armor}]");

        // if (_config.Animated)
        // {
        //     await PlayAsync(d.Tgt.Model().GetAnimationFromGainArmor(d.Induced));
        //     await PlayAsync(TextAnimation.FromLoseArmorDetails(d));
        //     await PlayAsync(new LoseArmorVFXAnimation(d, false));
        // }

        // 正变正，护甲伤害
        // 正变负，碎盾
        // 负变负，减甲

        await _closureDict.SendEvent(StageClosureDict.DID_LOSE_ARMOR, d);
    }

    public async UniTask ManaShortageProcedure(CostDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_MANA_SHORTAGE, d);

        if (d.Cancel)
            return;

        await _closureDict.SendEvent(StageClosureDict.DID_MANA_SHORTAGE, d);
    }

    public async UniTask ArmorShortageProcedure(CostDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_ARMOR_SHORTAGE, d);

        if (d.Cancel)
            return;

        await _closureDict.SendEvent(StageClosureDict.DID_ARMOR_SHORTAGE, d);
    }

    public async UniTask ExhaustProcedure(StageEntity owner, StageSkill skill)
        => await ExhaustProcedure(new ExhaustDetails(owner, skill));

    public async UniTask ExhaustProcedure(ExhaustDetails d)
    {
        if (d.Skill.Exhausted)
            return;

        await _closureDict.SendEvent(StageClosureDict.WIL_EXHAUST, d);

        d.Skill.Exhausted = true;

        await _closureDict.SendEvent(StageClosureDict.DID_EXHAUST, d);
    }
    
    public async UniTask CycleProcedure(CycleDetails d)
    {
        d.Rotate &= _config.RunConfig.DifficultyProfile.GetEntry().AllowRotate;
        
        RegisterTempClosures(d.Listener, d.Closures);
        await _closureDict.SendEvent(StageClosureDict.WIL_CYCLE, d);

        if (d.Cancel)
        {
            UnregisterTempClosures(d.Listener, d.Closures);
            return;
        }

        if (d.Rotate)
        {
            WuXing fromWuXing = d.WuXing;
            for (int i = 0; i < d.Step; i++)
                fromWuXing = fromWuXing.Prev;

            int flow = d.Owner.GetStackOfBuff(fromWuXing._elementaryBuff);
            d.Flow = flow;

            int consume = flow - Mathf.Min(flow, d.Recover);
            await d.Owner.TryConsumeProcedure(fromWuXing._elementaryBuff, consume);

            int gain = flow + d.Gain;
            await d.Owner.GainBuffProcedure(d.WuXing._elementaryBuff, gain, induced: d.Induced);
        }
        else
        {
            await d.Owner.GainBuffProcedure(d.WuXing._elementaryBuff, d.Gain, induced: d.Induced);
        }

        await _closureDict.SendEvent(StageClosureDict.DID_CYCLE, d);
        UnregisterTempClosures(d.Listener, d.Closures);
    }
    
    public async UniTask DispelProcedure(DispelDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_DISPEL, d);

        if (d.Cancel)
            return;

        List<Buff> buffs = d.Entity.TraversalBuffs().FilterObj(b => !b.GetEntry().Friendly && b.GetEntry().Dispellable).ToList();

        foreach (Buff b in buffs)
            await d.Entity.LoseBuffProcedure(b.GetEntry(), d.Value);

        await _closureDict.SendEvent(StageClosureDict.DID_DISPEL, d);
    }

    private async UniTask MingYuanPenaltyProcedure()
    {
        await _entities[0].MingYuan.MingYuanPenaltyProcedure(_entities[0]);
        await _entities[1].MingYuan.MingYuanPenaltyProcedure(_entities[1]);
    }

    private async UniTask StartStageProcedure()
    {
        foreach (var e in _entities)
            await _closureDict.SendEvent(StageClosureDict.WIL_STAGE, new StageDetails(e));

        foreach (var e in _entities)
            await e.StartStageExecuteProcedure();
    }

    private async UniTask BodyProcedure()
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
                foreach (Buff b in e.TraversalBuffs())
                    _result.TryAppend($"  {b.GetName()}*{b.Stack}");
                _result.TryAppend("\n");
            });

            if (0 != await CommitProcedure(turnCount, whosTurn))
                return;

            whosTurn = 1 - whosTurn;
        }
    }

    private async UniTask EndStageProcedure()
    {
        await _closureDict.SendEvent(StageClosureDict.DID_STAGE, new StageDetails(_entities[1]));
        await _closureDict.SendEvent(StageClosureDict.DID_STAGE, new StageDetails(_entities[0]));
    }

    private async UniTask<int> CommitProcedure(int turn, int whosTurn)
    {
        int flag = await _kernel.CommitProcedure(this, turn, whosTurn, false);
        
        if (!_config.Animated)
            return flag;

        if (flag == 1)
        {
            UniTask t1 = PlayAsync(Home.Model().GetAnimationFromWin());
            UniTask t2 = PlayAsync(Away.DeathCauseIsAttack ? Away.Model().GetAnimationFromDefeat() : Away.Model().GetAnimationFromLose());
            await UniTask.WhenAll(t1, t2);
        }
        else if (flag == 2)
        {
            UniTask t1 = PlayAsync(Home.DeathCauseIsAttack ? Home.Model().GetAnimationFromDefeat() : Home.Model().GetAnimationFromLose());
            UniTask t2 = PlayAsync(Away.Model().GetAnimationFromWin());
            await UniTask.WhenAll(t1, t2);
        }
        
        // await StartBulletTimeEffect();

        return flag;
    }

    private async UniTask<int> ForcedCommitProcedure()
    {
        if (Result.Flag != 0)
            return Result.Flag;
        
        int flag = await _kernel.CommitProcedure(this, MAX_TURN_COUNT, 0, true);
        
        if (!_config.Animated)
            return flag;

        if (flag == 1)
        {
            UniTask t1 = PlayAsync(_entities[0].Model().GetAnimationFromWin());
            UniTask t2 = PlayAsync(_entities[1].Model().GetAnimationFromLose());
            await UniTask.WhenAll(t1, t2);
        }
        else if (flag == 2)
        {
            UniTask t1 = PlayAsync(_entities[0].Model().GetAnimationFromLose());
            UniTask t2 = PlayAsync(_entities[1].Model().GetAnimationFromWin());
            await UniTask.WhenAll(t1, t2);
        }
        
        return flag;
    }

    #endregion

    private StageConfig _config;
    public StageConfig GetConfig() => _config;

    private StageClosureDict _closureDict;
    public StageClosureDict ClosureDict => _closureDict;

    private StageEntity[] _entities;
    public StageEntity[] Entities => _entities;

    public StageEntity Home => _entities[0];
    public StageEntity Away => _entities[1];

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
        AppManager.Instance.RunManager.Environment.SetGuideToFinish();
        CanvasManager.Instance.RefreshGuide();
        AppManager.Instance.Push(AppStateMachine.STAGE, config);
    }

    public async UniTask EnteringProcedure()
    {
        if (!_config.Animated)
            return;
        
        PlayAsync(_entities[0].Model().GetAnimationFromTrack0());
        PlayAsync(_entities[1].Model().GetAnimationFromTrack0());
        UniTask t1 = PlayAsync(_entities[0].Model().GetAnimationFromEntering());
        UniTask t2 = PlayAsync(_entities[1].Model().GetAnimationFromEntering());

        await UniTask.WhenAll(t1, t2);
    }

    public void Play(Animation animation)
    {
        if (!_config.Animated)
            return;

        if (animation.Induced && animation.InvolvesCharacterAnimation())
            return;

        StageManager.Instance.StageAnimationController.Play(animation);
    }

    public async UniTask PlayAsync(Animation animation)
    {
        if (!_config.Animated)
            return;

        if (animation.Induced && animation.InvolvesCharacterAnimation())
            return;

        await StageManager.Instance.StageAnimationController.Play(animation);
    }

    public async UniTask NextKey(bool induced)
    {
        if (!_config.Animated)
            return;

        if (induced)
            return;

        await StageManager.Instance.StageAnimationController.NextKey();
    }

    public void RecordResult(int flag)
    {
        Result.Flag = flag;
        Result.HomeLeftHp = Home.Hp;
        Result.AwayLeftHp = Away.Hp;
        Result.TryAppend(Result.Flag == 1 ? $"主场胜利\n" : $"客场胜利\n");
    }

    public void WriteResult()
    {
        _entities[0].WriteResult();
        RunManager.Instance.Environment.DepleteProcedure();
    }

    private async UniTask WriteShortage(StageClosureListener listener, StageClosure closure, ClosureDetails stageClosureDetails)
    {
        CostDetails d = (CostDetails)stageClosureDetails;
        d.State = CostState.Shortage;
    }

    private async UniTask WriteCost(StageClosureListener listener, StageClosure closure, ClosureDetails stageClosureDetails)
    {
        CostDetails d = (CostDetails)stageClosureDetails;
        if (d.State == CostState.Shortage)
            return;

        StageSkill skill = d.Skill;
        CostDescription costDescription = skill.Entry.GetLiteralCostDescription(skill.GetJingJie());
        int literalCost = costDescription.Value;

        CostState state = d.Value < literalCost
            ? CostState.Reduced
            : CostState.Normal;

        d.State = state;
    }

    private void RegisterConfigClosures()
    {
        if (_config.RunConfig == null)
            return;

        _closureDict.Register(this, _config.RunConfig.GetCharacter()._stageClosures);

        DifficultyEntry difficultyEntry = _config.RunConfig.DifficultyProfile.GetEntry();
        _closureDict.Register(this, difficultyEntry._stageClosures);
        foreach (var additionalDifficultyEntry in difficultyEntry.InheritedDifficulties)
            _closureDict.Register(this, additionalDifficultyEntry._stageClosures);
    }

    private void RegisterEntityClosures()
    {
        _entities.Do(e => e.RegisterEntityClosures());
    }

    private void RegisterSkillClosures()
    {
        _entities.Do(e => e._skills.Traversal().Do(s => _closureDict.Register(s, s.Entry.Closures)));
    }

    private void RegisterTempClosures(StageClosureListener listener, StageClosure[] closures)
    {
        // TODO: to be removed after guarantee all listeners is not null
        if (listener == null || closures == null)
            return;
        _closureDict.Register(listener, closures);
    }

    private void RegisterAchievementClosures()
    {
        if (!_config.EffectAchievements)
            return;
        AppManager.Instance.ProfileManager.GetCurrProfile().RegisterStageClosures(_closureDict);
    }

    private void UnregisterAchievementClosures()
    {
        if (!_config.EffectAchievements)
            return;
        AppManager.Instance.ProfileManager.GetCurrProfile().UnregisterStageClosures(_closureDict);
    }

    private void UnregisterConfigClosures()
    {
        if (_config.RunConfig == null)
            return;

        _closureDict.Unregister(this, _config.RunConfig.GetCharacter()._stageClosures);

        DifficultyEntry difficultyEntry = _config.RunConfig.DifficultyProfile.GetEntry();
        _closureDict.Unregister(this, difficultyEntry._stageClosures);
        foreach (var additionalDifficultyEntry in difficultyEntry.InheritedDifficulties)
            _closureDict.Unregister(this, additionalDifficultyEntry._stageClosures);
    }

    private void UnregisterEntityClosures()
    {
        _entities.Do(e => e.UnregisterEntityClosures());
    }

    private void UnregisterSkillClosures()
    {
        _entities.Do(e => e._skills.Traversal().Do(s => _closureDict.Unregister(s, s.Entry.Closures)));
    }

    private void UnregisterTempClosures(StageClosureListener listener, StageClosure[] closures)
    {
        // TODO: to be removed after guarantee all listeners is not null
        if (listener == null || closures == null)
            return;
        _closureDict.Unregister(listener, closures);
    }
    
    private void ClearResults()
    {
        _entities.Do(stageEntity => stageEntity.RunEntity.TraversalCurrentSlots().Do(s => s.ClearResults()));
    }
}
