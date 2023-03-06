using System;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using Sequence = DG.Tweening.Sequence;

public class StageManager : Singleton<StageManager>
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

    public void AttackProcedure(StringBuilder seq, StageEntity src, StageEntity tgt, int value, int times = 1, bool recursive = true,
        Action<DamageDetails> damaged = null, Action<DamageDetails> undamaged = null)
        => AttackProcedure(new AttackDetails(seq, src, tgt, value, times, recursive, damaged, undamaged));
    public void AttackProcedure(AttackDetails d)
    {
        for (int i = 0; i < d.Times; i++)
        {
            StageEntity src = d.Src;
            StageEntity tgt = d.Tgt;

            // if (source == null || source.IsDead() || target == null || target.IsDead())
            // {
            //     sequence.AppendInterval(0.5f);
            //     return;
            // }

            src.Attack(d);
            tgt.Attacked(d);
            if (d.Cancel)
            {
                d.Seq.Append($"    敌方生命[护甲]变成了${tgt.Hp}[{tgt.Armor}]");
                continue;
            }

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

            if (d.Value == 0)
            {
                // undamage?.Invoke();
                d.Seq.Append($"    敌方生命[护甲]变成了${tgt.Hp}[{tgt.Armor}]");
                continue;
            }

            // 伤害Procedure
            DamageProcedure(d.Seq, d.Src, d.Tgt, d.Value, damaged: d.Damaged, undamaged: d.Undamaged);


            // if (target.IsDead())
            // {
            //     target.Killed(attackDetails);
            //     AnyKilled(attackDetails);
            //
            //     kill?.Invoke();
            //     source.Kill(attackDetails);
            //     AnyKill(attackDetails);
            // }

            // d.Seq.Append(src.Slot().GetAttackTween())
            //     .Join(tgt.Slot().GetAttackedTween())
            //     .AppendInterval(0.5f);
            d.Seq.Append($"    敌方生命[护甲]变成了${tgt.Hp}[{tgt.Armor}]");
        }
    }

    public void DamageProcedure(StringBuilder seq, StageEntity src, StageEntity tgt, int value, bool recursive = true,
        Action<DamageDetails> damaged = null, Action<DamageDetails> undamaged = null)
        => DamageProcedure(new DamageDetails(seq, src, tgt, value, recursive, damaged, undamaged));
    public void DamageProcedure(DamageDetails d)
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
        tgt.Damaged(d);

        if (d.Cancel)
        {
            d.Undamaged?.Invoke(d);
            return;
        }

        tgt.Hp -= d.Value;

        if (d.Value == 0)
        {
            d.Undamaged?.Invoke(d);
        }
        else
        {
            d.Damaged?.Invoke(d);
        }
    }

    public void HealProcedure(StringBuilder seq, StageEntity src, StageEntity tgt, int value)
        => HealProcedure(new HealDetails(seq, src, tgt, value));
    public void HealProcedure(HealDetails d)
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        src.Heal(d);
        tgt.Healed(d);

        tgt.Hp += d.Value;

        d.Seq.Append($"    生命变成了${tgt.Hp}");
    }

    public void BuffProcedure(StringBuilder seq, StageEntity src, StageEntity tgt, string buffName, int stack = 1, bool recursive = true)
        => BuffProcedure(seq, src, tgt, Encyclopedia.BuffCategory[buffName], stack, recursive);
    public void BuffProcedure(StringBuilder seq, StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => BuffProcedure(new BuffDetails(seq, src, tgt, buffEntry, stack, recursive));
    public void BuffProcedure(BuffDetails buffDetails)
    {
        // buffDetails = CleanProcedure(buffDetails);

        if (buffDetails.Cancel) return;
        buffDetails = buffDetails.Src.Buff.Evaluate(buffDetails);

        if (buffDetails.Cancel) return;
        buffDetails.Core();

        if (buffDetails.Cancel) return;
        buffDetails = buffDetails.Tgt.Buffed.Evaluate(buffDetails);
    }

    public void ArmorProcedure(StringBuilder seq, StageEntity src, StageEntity tgt, int value)
        => ArmorProcedure(new ArmorDetails(seq, src, tgt, value));
    public void ArmorProcedure(ArmorDetails d)
    {
        d.Src._Armor(d);
        d.Tgt.Armored(d);
        if (d.Cancel)
            return;

        d.Tgt.Armor += d.Value;
        d.Seq.Append($"    护甲变成了[{d.Src.Armor}]");
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

    private Dictionary<string, Func<IndexPath, object>> _accessors;

    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new()
        {
            { "TryGetHeroStageNeiGong",    TryGetHeroStageNeiGong },
            { "TryGetHeroStageWaiGong",    TryGetHeroStageWaiGong },
            { "TryGetEnemyStageNeiGong",   TryGetEnemyStageNeiGong },
            { "TryGetEnemyStageWaiGong",   TryGetEnemyStageWaiGong },
            { "TryGetHeroBuff",            TryGetHeroBuff },
            { "TryGetEnemyBuff",           TryGetEnemyBuff },
        };
    }

    public static T Get<T>(IndexPath indexPath) => (T) Instance._accessors[indexPath._str](indexPath);
    public static T Get<T>(string funcName) => Get<T>(new IndexPath(funcName));
    private object TryGetHeroStageNeiGong(IndexPath indexPath) => _hero.TryGetNeiGong(indexPath._ints[0]);
    private object TryGetHeroStageWaiGong(IndexPath indexPath) => _hero.TryGetWaiGong(indexPath._ints[0]);
    private object TryGetEnemyStageNeiGong(IndexPath indexPath) => _enemy.TryGetNeiGong(indexPath._ints[0]);
    private object TryGetEnemyStageWaiGong(IndexPath indexPath) => _enemy.TryGetWaiGong(indexPath._ints[0]);
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

        CanvasManager.Instance.StageCanvas.Refresh();

        // Sequence seq = DOTween.Sequence()
        //     .SetAutoKill()
        //     .OnComplete(() =>
        //     {
        //         Debug.Log("Animation is finished");
        //         // after animation is finished, exit
        //     });

        StringBuilder seq = new StringBuilder();

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

        Debug.Log(seq.ToString());
        // seq.Restart();

        AppManager.Pop();
    }

    private void Simulate(StringBuilder seq)
    {
        bool heroTurn = true;
        _hero._p = -1;
        _enemy._p = -1;

        _hero.StartStage();
        _enemy.StartStage();

        // register nei gong

        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            if (heroTurn)
            {
                _hero.Execute(seq);
            }
            else
            {
                _enemy.Execute(seq);
            }

            if (TryCommit(seq, heroTurn))
                return;

            heroTurn = !heroTurn;
        }

        ForceCommit(seq);
    }

    public bool TryCommit(StringBuilder seq, bool heroTurn)
    {
        if (heroTurn)
        {
            if (_hero.Hp <= 0)
            {
                seq.Append($"玩家失败\n");
                return true;
            }

            if (_enemy.Hp <= 0)
            {
                seq.Append($"玩家胜利\n");
                return true;
            }
        }
        else
        {
            if (_enemy.Hp <= 0)
            {
                seq.Append($"玩家胜利\n");
                return true;
            }
            if (_hero.Hp <= 0)
            {
                seq.Append($"玩家失败\n");
                return true;
            }
        }

        return false;
    }

    public void ForceCommit(StringBuilder seq)
    {
        if (_hero.Hp > _enemy.Hp)
        {
            seq.Append($"玩家胜利\n");
            return;
        }
        seq.Append($"玩家失败\n");
    }

    public void Exit()
    {
        // apply outcome to run manager
    }
}
