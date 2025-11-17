
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
        ResetDamageThreshold();

        RegisterConfigClosures();
        RegisterAchievementClosures();
        RegisterEntityClosures();
        RegisterSkillClosures();

        await EnteringStaging();

        await MingYuanPenaltyProcedure();
        
        await FormationProcedure();
        await StartStageProcedure();

        _closureDict.Register(this, closures);

        await BodyProcedure();

        _closureDict.Unregister(this, closures);

        if (!_shouldSkip)
        {
            await EndStageProcedure();
            await ForcedCommitProcedure();
        }

        UnregisterSkillClosures();
        UnregisterEntityClosures();
        UnregisterAchievementClosures();
        UnregisterConfigClosures();
        
        if (_config.WriteResult)
            RunManager.Instance.Environment.DepleteProcedure();

        if (!_shouldSkip)
            await ExitingStaging();
    }

    private async UniTask ExitingStaging()
    {
        if (!_config.Animated)
            return;

        AudioManager.PlayExitStage();
        await PlayAsync(new WaitAnimation(3));
    }

    private async UniTask FormationProcedure()
    {
        List<GainFormationDetails> details = new();

        foreach (var entity in EntitiesInitiateOrder)
        foreach (var runFormation in entity.RunFormations())
            if (runFormation.IsActivated())
                details.Add(new GainFormationDetails(this, entity, runFormation));

        details.Sort((lhs, rhs) => lhs._formation.GetEntry().GetOrder() - rhs._formation.GetEntry().GetOrder());

        foreach (var d in details)
        {
            if (_shouldSkip)
                return;
            await GainFormationProcedure(d);
        }
    }

    private async UniTask GainFormationProcedure(GainFormationDetails d)
    {
        await _closureDict.SendEvent(StageClosureDict.WIL_GAIN_FORMATION, d);
        if (d.Cancel)
            return;
        
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
        CanvasManager.Instance.StageCanvas.GainFormationStaging(d.Owner == Home);
        f.Emphasize();
    }

    public async UniTask GainBuffProcedure(GainBuffDetails d)
    {
        bool registeredHere = RegisterTempClosures(d);
        await _closureDict.SendEvent(StageClosureDict.WIL_GAIN_BUFF, d);
        d.Cancel |= d.Stack <= 0;
        if (d.Cancel)
        {
            UnregisterTempClosures(d, registeredHere);
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
        UnregisterTempClosures(d, registeredHere);
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
        
        CanvasManager.Instance.StageCanvas.GainBuffStaging(d.Tgt == Home);
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
            await PlayAsync(d.Src.Model().GetAnimationFromBuffSelf(d.Induced));
        }
        else
        {
            await PlayAsync(d.Src.Model().GetAnimationFromBuffSelf(d.Induced));
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
        
        CanvasManager.Instance.StageCanvas.LoseBuffStaging(d.Tgt == Home, buffIndex);
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
        bool registeredHere = RegisterTempClosures(attackDetails);

        attackDetails.Value = Mathf.Max(1, attackDetails.Value);
        attackDetails.Times = Mathf.Max(1, attackDetails.Times);
        
        bool shouldMoveCamera = attackDetails.CalcCritical(ref _damageThreshold);
        
        if (shouldMoveCamera)
        {
            float cameraDegree = GetCameraDegree(attackDetails);
            await CameraMovementStaging(cameraDegree);
        }

        await _closureDict.SendEvent(StageClosureDict.WIL_FULL_ATTACK, attackDetails);
        await FullAttackStaging(attackDetails);

        if (!attackDetails.DoesntConsumeJianYi)
            attackDetails.Src.Memory.SetVariable(StageEntity.ThisTurnAttackedKey, true);

        for (int i = 0; i < attackDetails.Times; i++)
        {
            AttackDetails d = attackDetails.ShallowClone();
            await _closureDict.SendEvent(StageClosureDict.WIL_ATTACK, d);
            if (!d.Cancel)
            {
                await SingleAttackProcedure(d);
                await _closureDict.SendEvent(StageClosureDict.DID_ATTACK, d);
            }
            await NextKey(d.Induced);
        }

        await _closureDict.SendEvent(StageClosureDict.DID_FULL_ATTACK, attackDetails);
        
        UnregisterTempClosures(attackDetails, registeredHere);
        
        // check win condition, but do not commit
        await RecoverStaging(attackDetails);
    }
    
    private float GetCameraDegree(AttackDetails attackDetails)
    {
        if (attackDetails.Src == Home)
            return 1f;
        
        if (attackDetails.Src == Away)
            return -1f;
        
        return 0f;
    }

    private float _damageThreshold;

    private void ResetDamageThreshold()
    {
        // 初始阈值 = (home.maxhp * away.maxhp)^(1/2) * 1/4
        int homeMaxHp = Home.MaxHp;
        int awayMaxHp = Away.MaxHp;
        _damageThreshold = Mathf.Sqrt(homeMaxHp * awayMaxHp) * 0.25f;
    }

    // -1 for home, 0 for center, 1 for away
    private async UniTask CameraMovementStaging(float degree)
    {
        if (!_config.Animated)
            return;
        
        CameraManager.Instance.CameraMovementAnimation(degree);
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
            await EvadedStaging(EvadedDetails.FromAttackDetails(d));
            await _closureDict.SendEvent(StageClosureDict.DID_EVADE, d);
            await _closureDict.SendEvent(StageClosureDict.UNDAMAGED, DamageDetails.FromAttackDetailsUndamaged(d));
            return;
        }

        if (!d.Penetrate && d.Tgt.Armor >= 0)
        {
            int ratio = d.Shatter ? 2 : 1;
            int negate = Mathf.Min(ratio * d.Value, d.Tgt.Armor);
            if (negate > 0)
            {
                d.Value -= negate / ratio;
                await LoseArmorProcedure(LoseArmorDetails.FromAttackDetails(d, negate));
            }
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
            await GuardedStaging(GuardedDetails.FromAttackDetails(d));
            await _closureDict.SendEvent(StageClosureDict.UNDAMAGED, DamageDetails.FromAttackDetailsUndamaged(d));
            return;
        }

        await DamageProcedure(DamageDetails.FromAttackDetails(d));

        _result.TryAppend($"    敌方气血[护甲]变成了${d.Tgt.Hp}[{d.Tgt.Armor}]");
    }

    private async UniTask EvadedStaging(EvadedDetails d)
    {
        if (_config.Animated)
        {
            await PlayAsync(new EvadedVFXAnimation(d, false));
            await PlayAsync(d.Tgt.Model().GetAnimationFromEvaded(d.Induced));
        }
        _result.TryAppend($"    攻击被闪避");
    }

    private async UniTask GuardedStaging(GuardedDetails d)
    {
        if (_config.Animated)
        {
            await PlayAsync(new GuardedVFXAnimation(d, false));
            await PlayAsync(TextAnimation.FromGuardedDetails(d));
        }
        _result.TryAppend($"    攻击被格挡");
    }

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
            if (negate > 0)
            {
                d.Value -= negate;
                await LoseArmorProcedure(LoseArmorDetails.FromIndirectDetails(d, negate));
            }
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
        await LoseHealthProcedure(LoseHealthDetails.FromDamageDetails(d));

        await _closureDict.SendEvent(StageClosureDict.DID_DAMAGE, d);

        if (!d.Cancel && d.LifeSteal)
            await HealProcedure(HealDetails.FromLifeSteal(d));
    }

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

    public async UniTask HealProcedure(HealDetails d)
    {
        bool registeredHere = RegisterTempClosures(d);
        await _closureDict.SendEvent(StageClosureDict.WIL_HEAL, d);

        if (d.Cancel)
        {
            UnregisterTempClosures(d, registeredHere);
            return;
        }

        int actualHealed;
        if (d.Penetrate)
        {
            int finalMaxHp = Mathf.Max(d.Tgt.MaxHp, d.Tgt.Hp + d.Value);
            int gap = finalMaxHp - d.Tgt.MaxHp;
            if (gap > 0)
            {
                await GainMaxHealthProcedure(GainMaxHealthDetails.FromHealPenetrate(d, gap));
            }
            
            actualHealed = d.Value;
        }
        else
        {
            actualHealed = Mathf.Min(d.Tgt.MaxHp - d.Tgt.Hp, d.Value);
        }

        d.Tgt.Hp += actualHealed;

        if (_config.Animated)
        {
            await PlayAsync(d.Src.Model().GetAnimationFromHeal(d.Induced));
            await PlayAsync(new HealVFXAnimation(d, false));
            await PlayAsync(TextAnimation.FromHealDetails(d));
        }
        _result.TryAppend($"    气血变成了${d.Tgt.Hp}");

        await _closureDict.SendEvent(StageClosureDict.DID_HEAL, d);
        UnregisterTempClosures(d, registeredHere);
    }

    public async UniTask GainMaxHealthProcedure(GainMaxHealthDetails d)
    {
        bool registeredHere = RegisterTempClosures(d);
        await _closureDict.SendEvent(StageClosureDict.WIL_GAIN_MAX_HEALTH, d);

        if (d.Cancel)
        {
            UnregisterTempClosures(d, registeredHere);
            return;
        }

        d.Entity.MaxHp += d.Value;
        
        await _closureDict.SendEvent(StageClosureDict.DID_GAIN_MAX_HEALTH, d);
        UnregisterTempClosures(d, registeredHere);
    }

    public async UniTask LoseMaxHealthProcedure(LoseMaxHealthDetails d)
    {
        bool registeredHere = RegisterTempClosures(d);
        await _closureDict.SendEvent(StageClosureDict.WIL_LOSE_MAX_HEALTH, d);

        if (d.Cancel)
        {
            UnregisterTempClosures(d, registeredHere);
            return;
        }

        d.Entity.MaxHp -= d.Value;
        
        await _closureDict.SendEvent(StageClosureDict.DID_LOSE_MAX_HEALTH, d);
        UnregisterTempClosures(d, registeredHere);
    }

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
        bool registeredHere = RegisterTempClosures(d);
        await _closureDict.SendEvent(StageClosureDict.WIL_GAIN_ARMOR, d);

        d.Cancel |= d.Value <= 0;

        if (d.Cancel)
        {
            UnregisterTempClosures(d, registeredHere);
            return;
        }

        d.Tgt.Armor += d.Value;

        if (_config.Animated)
        {
            await PlayAsync(d.Src.Model().GetAnimationFromGiveArmor(d.Induced));
            await PlayAsync(TextAnimation.FromGainArmorDetails(d));
            await PlayAsync(new GainArmorVFXAnimation(d, false));
        }
        _result.TryAppend($"    护甲变成了[{d.Tgt.Armor}]");

        await _closureDict.SendEvent(StageClosureDict.DID_GAIN_ARMOR, d);
        UnregisterTempClosures(d, registeredHere);
    }

    public async UniTask LoseArmorProcedure(LoseArmorDetails d)
    {
        bool registeredHere = RegisterTempClosures(d);
        await _closureDict.SendEvent(StageClosureDict.WIL_LOSE_ARMOR, d);

        d.Cancel |= d.Value <= 0;
        if (d.Cancel)
        {
            UnregisterTempClosures(d, registeredHere);
            return;
        }

        d.Tgt.Armor -= d.Value;
        _result.TryAppend($"    护甲变成了[{d.Tgt.Armor}]");

        if (_config.Animated)
        {
            await PlayAsync(d.Src.Model().GetAnimationFromRemoveArmor(d.Induced));
            await PlayAsync(TextAnimation.FromLoseArmorDetails(d));
            await PlayAsync(new LoseArmorVFXAnimation(d, false));
        }

        // 正变正，护甲伤害
        // 正变负，碎盾
        // 负变负，减甲

        await _closureDict.SendEvent(StageClosureDict.DID_LOSE_ARMOR, d);
        UnregisterTempClosures(d, registeredHere);
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
        => await ExhaustProcedure(new ExhaustDetails(this, owner, skill));

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
        
        bool registeredHere = RegisterTempClosures(d);
        await _closureDict.SendEvent(StageClosureDict.WIL_CYCLE, d);

        if (d.Cancel)
        {
            UnregisterTempClosures(d, registeredHere);
            return;
        }

        if (d.Rotate)
        {
            WuXing fromWuXing = d.WuXing;
            for (int i = 0; i < d.Step; i++)
                fromWuXing = fromWuXing.Prev;

            int flow = d.Owner.GetStackOfBuff(fromWuXing.GetElementaryBuff());
            d.Flow = flow;

            int consume = flow - Mathf.Min(flow, d.Recover);
            await d.Owner.TryConsumeProcedure(fromWuXing.GetElementaryBuff(), consume);

            int gain = flow + d.Gain;
            await d.Owner.GainBuffProcedure(d.WuXing.GetElementaryBuff(), gain, induced: d.Induced);
        }
        else
        {
            await d.Owner.GainBuffProcedure(d.WuXing.GetElementaryBuff(), d.Gain, induced: d.Induced);
        }

        await _closureDict.SendEvent(StageClosureDict.DID_CYCLE, d);
        UnregisterTempClosures(d, registeredHere);
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
        foreach (var entity in EntitiesInitiateOrder)
            await entity.MingYuan.MingYuanPenaltyProcedure(entity);
    }

    private async UniTask StartStageProcedure()
    {
        foreach (var e in EntitiesInitiateOrder)
        {
            if (_shouldSkip)
                return;
            await _closureDict.SendEvent(StageClosureDict.WIL_STAGE, new StageDetails(this, e));
        }

        foreach (var e in EntitiesInitiateOrder)
        {
            if (_shouldSkip)
                return;
            await e.StartStageExecuteProcedure();
        }
    }

    private async UniTask BodyProcedure()
    {
        int whosTurn = !_config.RunConfig.DifficultyProfile.GetEntry().EnemyInitiate ? 0 : 1;

        _turnCount = 0;
        for (; _turnCount < MAX_TURN_COUNT;)
        {
            if (_shouldSkip)
                return;
            StageEntity actor = _entities[whosTurn];

            _result.TryAppend($"--------第{_turnCount}回合, {actor.GetName()}行动--------\n");

            await actor.TurnProcedure(_turnCount);
            await CameraMovementStaging(0f);

            _entities.Do(e =>
            {
                _result.TryAppend($"{e.GetName()} {e.Hp}[{e.Armor}] Buff:");
                foreach (Buff b in e.TraversalBuffs())
                    _result.TryAppend($"  {b.GetName()}*{b.Stack}");
                _result.TryAppend("\n");
            });

            if (0 != await CommitProcedure(_turnCount, whosTurn))
                return;

            whosTurn = 1 - whosTurn;
            IncreaseTurnCount();
        }
    }

    private async UniTask EndStageProcedure()
    {
        await _closureDict.SendEvent(StageClosureDict.DID_STAGE, new StageDetails(this, Away));
        await _closureDict.SendEvent(StageClosureDict.DID_STAGE, new StageDetails(this, Home));
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
            UniTask t1 = PlayAsync(Home.Model().GetAnimationFromWin());
            UniTask t2 = PlayAsync(Away.Model().GetAnimationFromLose());
            await UniTask.WhenAll(t1, t2);
        }
        else if (flag == 2)
        {
            UniTask t1 = PlayAsync(Home.Model().GetAnimationFromLose());
            UniTask t2 = PlayAsync(Away.Model().GetAnimationFromWin());
            await UniTask.WhenAll(t1, t2);
        }
        
        return flag;
    }
    
    // public async UniTask TransferProcedure(
    //     int fromStack,
    //     BuffEntry fromBuff,
    //     int toStack,
    //     BuffEntry toBuff,
    //     bool consuming,
    //     int? maxFlow = null,
    //     int? upperBound = null)
    // {
    //     int flow = Caster.GetStackOfBuff(fromBuff) / fromStack;
    //     if (upperBound.HasValue)
    //     {
    //         int gap = upperBound.Value - Caster.GetStackOfBuff(toBuff);
    //         if (gap >= 0)
    //             flow = flow.ClampUpper(gap);
    //     }
    //     
    //     if (maxFlow.HasValue)
    //         flow = flow.ClampUpper(maxFlow.Value);
    //     
    //     if (consuming)
    //         await LoseBuffProcedure(fromBuff, flow * fromStack);
    //
    //     await GainBuffProcedure(toBuff, flow * toStack);
    // }
    
    // public async UniTask TransferProcedure(int fromStack, BuffEntry fromBuff, int toStack, BuffEntry toBuff, bool consuming, int? maxFlow = null, int? upperBound = null)
    // {
    //     int flow = GetStackOfBuff(fromBuff) / fromStack;
    //     if (upperBound.HasValue)
    //     {
    //         int gap = upperBound.Value - GetStackOfBuff(toBuff);
    //         if (gap >= 0)
    //             flow = flow.ClampUpper(gap);
    //     }
    //     
    //     if (maxFlow.HasValue)
    //         flow = flow.ClampUpper(maxFlow.Value);
    //     
    //     if (consuming)
    //         await LoseBuffProcedure(fromBuff, flow * fromStack);
    //
    //     await GainBuffProcedure(toBuff, flow * toStack);
    // }
    //
    // public async UniTask BecomeLowHealth()
    // {
    //     int gap = Hp - GetLowHealthThreshold();
    //     if (gap > 0)
    //         await LoseHealthProcedure(gap, false);
    // }

    #endregion

    private StageConfig _config;
    public StageConfig GetConfig() => _config;

    private int _turnCount;
    public void IncreaseTurnCount()
    {
        _turnCount++;
        
        if (!_config.Animated)
            return;
        
        CanvasManager.Instance.StageCanvas.TurnCountChangedStaging(_turnCount);
    }

    public int GetTurnCount() => _turnCount;

    private StageClosureDict _closureDict;
    public StageClosureDict ClosureDict => _closureDict;

    private StageEntity[] _entities;
    public StageEntity[] Entities => _entities;

    private StageEntity[] _entitiesInitiateOrder;
    public void CalcEntitiesInitiateOrder()
    {
        _entitiesInitiateOrder = !_config.RunConfig.DifficultyProfile.GetEntry().EnemyInitiate
            ? new[] { _entities[0], _entities[1] }
            : new[] { _entities[1], _entities[0] };
    }

    public StageEntity[] EntitiesInitiateOrder
        => _entitiesInitiateOrder;

    public StageEntity Home => _entities[0];
    public StageEntity Away => _entities[1];

    private StageKernel _kernel;

    private bool _shouldSkip;
    public void SetShouldSkip() => _shouldSkip = true;

    private StageResult _result;
    public StageResult Result => _result;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Home",                       thisObject => ((StageEnvironment)thisObject).Home },
        { "Away",                       thisObject => ((StageEnvironment)thisObject).Away },
        { "Report",                     thisObject => ((StageEnvironment)thisObject)._result },
    };
    public object Get(string s) => Accessor[s](this);
    private StageEnvironment(StageConfig config)
    {
        _config = config;

        _closureDict = new();

        _entities = new StageEntity[]
        {
            new(this, _config.Home, 0),
            new(this, _config.Away, 1),
        };
        
        CalcEntitiesInitiateOrder();

        _kernel = config.Kernel;

        _shouldSkip = false;

        _result = new(_config);
    }

    public static StageEnvironment FromConfig(StageConfig config)
        => new(config);

    public async UniTask EnteringStaging()
    {
        if (!_config.Animated)
            return;

        AudioManager.PlayEnterStage();
        PlayAsync(Home.Model().GetAnimationFromTrack0());
        PlayAsync(Away.Model().GetAnimationFromTrack0());
        UniTask t1 = PlayAsync(Home.Model().GetAnimationFromEntering());
        UniTask t2 = PlayAsync(Away.Model().GetAnimationFromEntering());

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

        _closureDict.Register(this, _config.RunConfig.GetCharacter().StageClosures);

        DifficultyEntry difficultyEntry = _config.RunConfig.DifficultyProfile.GetEntry();
        _closureDict.Register(this, difficultyEntry._stageClosures);
        foreach (var additionalDifficultyEntry in difficultyEntry.InheritedDifficulties)
            _closureDict.Register(this, additionalDifficultyEntry._stageClosures);
    }

    private void UnregisterConfigClosures()
    {
        if (_config.RunConfig == null)
            return;

        _closureDict.Unregister(this, _config.RunConfig.GetCharacter().StageClosures);

        DifficultyEntry difficultyEntry = _config.RunConfig.DifficultyProfile.GetEntry();
        _closureDict.Unregister(this, difficultyEntry._stageClosures);
        foreach (var additionalDifficultyEntry in difficultyEntry.InheritedDifficulties)
            _closureDict.Unregister(this, additionalDifficultyEntry._stageClosures);
    }

    private void RegisterSkillClosures()
    {
        EntitiesInitiateOrder.Do(e => e._skills.Do(s => _closureDict.Register(s, s.Entry.Closures)));
    }

    private void UnregisterSkillClosures()
    {
        EntitiesInitiateOrder.Do(e => e._skills.Do(s => _closureDict.Unregister(s, s.Entry.Closures)));
    }

    private void RegisterEntityClosures()
    {
        EntitiesInitiateOrder.Do(e => e.RegisterEntityClosures());
    }

    private void UnregisterEntityClosures()
    {
        EntitiesInitiateOrder.Do(e => e.UnregisterEntityClosures());
    }

    private bool RegisterTempClosures(NestedStageClosureDetails d)
    {
        if (d.ClosureHasRegistered)
            return false;
        
        // TODO: to be removed after guarantee all listeners is not null
        if (d.Listener == null || d.Closures == null)
            return false;
        
        _closureDict.Register(d.Listener, d.Closures);
        d.ClosureHasRegistered = true;
        return true;
    }

    private void UnregisterTempClosures(NestedStageClosureDetails d, bool registeredHere)
    {
        if (!registeredHere)
            return;
        // TODO: to be removed after guarantee all listeners is not null
        if (d.Listener == null || d.Closures == null)
            return;
        _closureDict.Unregister(d.Listener, d.Closures);
    }

    private void RegisterAchievementClosures()
    {
        if (!_config.WriteResult)
            return;
        AppManager.Instance.ProfileManager.GetCurrProfile().RegisterStageClosures(_closureDict);
    }

    private void UnregisterAchievementClosures()
    {
        if (!_config.WriteResult)
            return;
        AppManager.Instance.ProfileManager.GetCurrProfile().UnregisterStageClosures(_closureDict);
    }
    
    private void ClearResults()
    {
        Home.RunEntity.ClearSlotResults();
        Away.RunEntity.ClearSlotResults();
    }
}
