using System;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class StageManager : CLLibrary.Singleton<StageManager>
{
    private readonly int MAX_ACTION_COUNT = 128;

    // public static event Action HeroStartTurnEvent;
    // public static void HeroStartTurn() => HeroStartTurnEvent?.Invoke();
    //
    // public static event Action HeroEndTurnEvent;
    // public static void HeroEndTurn() => HeroEndTurnEvent?.Invoke();
    //
    // public static event Action MonsterStartTurnEvent;
    // public static void MonsterStartTurn() => MonsterStartTurnEvent?.Invoke();
    //
    // public static event Action MonsterEndTurnEvent;
    // public static void MonsterEndTurn() => MonsterEndTurnEvent?.Invoke();
    //
    // public static FuncQueue<BuffDetails> AnyBuff = new();
    //
    // public static FuncQueue<BuffDetails> AnyBuffed = new();

    public void AttackProcedure
        (AttackDetails d
            // Action<DamageDetails> damaged = null,
            // Action undamage = null
            )
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        // if (source == null || source.IsDead() || target == null || target.IsDead())
        // {
        //     sequence.AppendInterval(0.5f);
        //     return;
        // }

        src.Attack(d);

        // if (source.IsDead() || target.IsDead())
        // {
        //     sequence.AppendInterval(0.5f);
        //     return;
        // }

        // 加攻 和 格挡
        // int power = (int)Mathf.Max(0, source.GetFinalPower() * (1 + magnification) - target.GetBlock());

        // 闪避
        // if (target.CanEvade)
        // {
        //     target.Evade(attackDetails);
        //     return;
        // }

        // 结算护甲
        (d.Value, tgt.Armor) = Numeric.Negate(d.Value, tgt.Armor);
        if (d.Value >= 0)
        {
            // undamage?.Invoke();
            return;
        }

        // 伤害Procedure
        DamageProcedure(new DamageDetails(d.Seq, d.Src, d.Tgt, d.Value));

        tgt.Attacked(d);

        // if (target.IsDead())
        // {
        //     target.Killed(attackDetails);
        //     AnyKilled(attackDetails);
        //
        //     kill?.Invoke();
        //     source.Kill(attackDetails);
        //     AnyKill(attackDetails);
        // }

        d.Seq.Append(src.Slot().GetAttackTween())
            .Join(tgt.Slot().GetAttackedTween())
            .AppendInterval(0.5f);
    }

    public static void DamageProcedure
        (DamageDetails d
            // Action<DamageDetails> damaged = null, Action undamage = null
            )
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        // 结算伤害免疫
        // int hpLose = (int)(1 - target.GetFinalDamageImmune());
        // if (hpLose == 0)
        // {
        //     undamage?.Invoke();
        //     return;
        // }

        // 结算不屈
        // bool canLaststand = target.CanLaststand;
        // if (target.GetCurrHP() > hpLose || !canLaststand)
        // {
        //     target.SetCurrHP(target.GetCurrHP() - hpLose);
        //     damageDetails = new DamageDetails(source, target, hpLose);
        // }
        // else
        // {
        //     target.SetCurrHP(1);
        //     damageDetails = new DamageDetails(source, target, target.GetCurrHP() - 1);
        //     target.Laststand(damageDetails);
        // }

        // 如果击伤
        // damaged?.Invoke(damageDetails);

        // 结算吸血


        src.Damage(d);
        tgt.Hp -= d.Value;
        tgt.Damaged(d);

        // 结算反伤
        // int blademailDamage = (int)(hpLose * target.GetFinalBlademail());
        // if (blademailDamage > 0)
        // {
        //     DamageDetails blademailDamageDetails = new DamageDetails(target, source, hpLose);
        //     BlademailDamageProcedure(target.Slot(), source.Slot(), blademailDamageDetails, ref sequence);
        // }
    }
    //
    // public static void BlademailDamageProcedure
    //     (EntitySlot sourceSlot, EntitySlot targetSlot, DamageDetails damageDetails, ref Sequence sequence)
    // {
    //     Entity source = damageDetails._source;
    //     Entity target = damageDetails._target;
    //     int hpLose = damageDetails._hpLose;
    //
    //     int blademailDamage = (int)(hpLose * source.GetFinalBlademail());
    //
    //     int shield = target.GetCurrShield();
    //     if (shield >= blademailDamage)
    //     {
    //         target.SetCurrShield(shield - blademailDamage);
    //         return;
    //     }
    //
    //     target.SetCurrShield(0);
    //     int blademailHpLose = (int)((blademailDamage - shield) * (1 - target.GetFinalDamageImmune()));
    //     DamageDetails blademailDamageDetails = new DamageDetails(source, target, blademailHpLose);
    //     target.SetCurrHP(target.GetCurrHP() - blademailHpLose);
    //
    //     source.Damage(blademailDamageDetails);
    //     target.Damaged(blademailDamageDetails);
    // }
    //
    public static void HealProcedure(HealDetails d)
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        src.Heal(d);
        tgt.Hp += d.Value;
        tgt.Healed(d);
    }

    public static void BuffProcedure(Sequence seq, StageEntity src, StageEntity tgt, string buffName, int stack = 1, bool recursive = true)
        => BuffProcedure(seq, src, tgt, Encyclopedia.BuffCategory.Find(buffName), stack, recursive);
    public static void BuffProcedure(Sequence seq, StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => BuffProcedure(new BuffDetails(seq, src, tgt, buffEntry, stack, recursive));
    public static void BuffProcedure(BuffDetails buffDetails)
    {
        // buffDetails = CleanProcedure(buffDetails);

        if (buffDetails._canceled) return;
        buffDetails = buffDetails.Src.Buff.Evaluate(buffDetails);

        if (buffDetails._canceled) return;
        buffDetails.Core();

        if (buffDetails._canceled) return;
        buffDetails = buffDetails.Tgt.Buffed.Evaluate(buffDetails);
    }

    // private static BuffDetails CleanProcedure(BuffDetails d)
    // {
    //     if (d._canceled) return d;
    //     if (d._buffEntry.Friendly) return d;
    //
    //     bool shouldCancel = d.Tgt.CleanStack >= d._stack;
    //     int toClean = Mathf.Min(d._stack, d.Tgt.CleanStack);
    //
    //     if (shouldCancel)
    //     {
    //         d._stack = 0;
    //         d.Tgt.Clean(toClean);
    //         d._canceled = true;
    //         return d;
    //     }
    //     else
    //     {
    //         d._stack -= toClean;
    //         d.Tgt.Clean(toClean);
    //         return d;
    //     }
    // }
    //
    // public static void DispelProcedure(StageEntity entity, int stack)
    // {
    //     while (stack > 0 && entity.FindBuff(b => !b.Friendly) is { } buff)
    //     {
    //         Negate(ref buff.Stack, ref stack);
    //     }
    // }
    //
    // public static void LoseHpProcedure(StageEntity entity, int amount)
    // {
    //     entity.SetCurrHP(entity.GetCurrHP() - amount);
    //     entity.LoseHp();
    // }

    public StageHero _hero;
    public StageEnemy _enemy;

    public HeroSlot _heroSlot;
    public EnemySlot _enemySlot;

    private Dictionary<string, Func<IndexPath, object>> _funcList;

    public override void DidAwake()
    {
        base.DidAwake();

        _hero = new StageHero(RunManager.Instance.Hero);
        _enemy = new StageEnemy(RunManager.Instance.Enemy);

        _funcList = new()
        {
            { "GetHeroStageNeiGong",       GetHeroStageNeiGong },
            { "GetHeroStageWaiGong",       GetHeroStageWaiGong },
            { "GetEnemyStageNeiGong",      GetEnemyStageNeiGong },
            { "GetEnemyStageWaiGong",      GetEnemyStageWaiGong },
            { "TryGetHeroBuff",            TryGetHeroBuff },
            { "TryGetEnemyBuff",           TryGetEnemyBuff },
        };
    }

    public static T Get<T>(IndexPath indexPath) => (T) Instance._funcList[indexPath._str](indexPath);
    public static T Get<T>(string funcName) => Get<T>(new IndexPath(funcName));
    private object GetHeroStageNeiGong(IndexPath indexPath) => _hero._neiGongList[indexPath._ints[0]];
    private object GetHeroStageWaiGong(IndexPath indexPath) => _hero._waiGongList[indexPath._ints[0]];
    private object GetEnemyStageNeiGong(IndexPath indexPath) => _enemy._neiGongList[indexPath._ints[0]];
    private object GetEnemyStageWaiGong(IndexPath indexPath) => _enemy._waiGongList[indexPath._ints[0]];
    private object TryGetHeroBuff(IndexPath indexPath) => _hero.TryGetBuff(indexPath._ints[0]);
    private object TryGetEnemyBuff(IndexPath indexPath) => _enemy.TryGetBuff(indexPath._ints[0]);

    public int GetHeroNeiGongCount() => _hero._neiGongList.Length;
    public int GetHeroWaiGongCount() => _hero._waiGongList.Length;
    public int GetEnemyNeiGongCount() => _enemy._neiGongList.Length;
    public int GetEnemyWaiGongCount() => _enemy._waiGongList.Length;

    public int GetHeroBuffCount() => _hero.GetBuffCount();
    public int GetEnemyBuffCount() => _enemy.GetBuffCount();

    public void Enter()
    {
        _hero = new StageHero(RunManager.Instance.Hero);
        _enemy = new StageEnemy(RunManager.Instance.Enemy);

        Sequence seq = DOTween.Sequence()
            .SetAutoKill()
            .OnComplete(() =>
            {
                Debug.Log("Animation is finished");
                // after animation is finished, exit
            });

        Simulate(seq);

        // config the scene
        StageCanvas.Instance.SetHeroHealth(_hero.Hp);
        StageCanvas.Instance.SetHeroArmor(0);
        StageCanvas.Instance.SetEnemyHealth(_enemy.Hp);
        StageCanvas.Instance.SetEnemyArmor(0);

        // animation
        // opening
        // show card
        // act
        // card back
        // end

        // bullet time at killing moment
        // accelerating
        // skip animation

        seq.Restart();
    }

    private void Simulate(Sequence seq)
    {
        bool heroTurn = true;

        // register passive chips

        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            if (heroTurn)
            {
                _hero.Execute(seq, _hero, _enemy);
            }
            else
            {
                _enemy.Execute(seq, _enemy, _hero);
            }

            heroTurn = !heroTurn;
        }
    }

    public void Exit()
    {
        // apply outcome to run manager
    }
}
