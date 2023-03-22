using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using Sequence = DG.Tweening.Sequence;

public class StageManager : Singleton<StageManager>
{
    private readonly int MAX_ACTION_COUNT = 128;

    /// <summary>
    /// 发起一次攻击行为，会结算目标的护甲
    /// </summary>
    /// <param name="src">攻击者</param>
    /// <param name="tgt">受攻击者</param>
    /// <param name="value">攻击数值</param>
    /// <param name="times">攻击次数</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="damaged">如果造成伤害时候的额外行为</param>
    /// <param name="undamaged">如果未造成伤害的额外行为</param>
    public void AttackProcedure(StageEntity src, StageEntity tgt, int value, int times = 1, bool recursive = true,
        Action<DamageDetails> damaged = null, Action<DamageDetails> undamaged = null)
        => AttackProcedure(new AttackDetails(src, tgt, value, times, recursive, damaged, undamaged));
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
                Report.Append($"    敌方生命[护甲]变成了${tgt.Hp}[{tgt.Armor}]");
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
            int negate = Mathf.Min(d.Value, tgt.Armor);
            d.Value -= negate;
            ArmorLoseProcedure(d.Tgt, negate);

            if (d.Value == 0)
            {
                // undamage?.Invoke();
                Report.Append($"    敌方生命[护甲]变成了${tgt.Hp}[{tgt.Armor}]");
                continue;
            }

            // 伤害Procedure
            DamageProcedure(d.Src, d.Tgt, d.Value, damaged: d.Damaged, undamaged: d.Undamaged);


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
            Report.Append($"    敌方生命[护甲]变成了${tgt.Hp}[{tgt.Armor}]");
        }
    }

    /// <summary>
    /// 发起一次直接伤害行为，不会结算目标的护甲
    /// </summary>
    /// <param name="src">伤害者</param>
    /// <param name="tgt">受伤害者</param>
    /// <param name="value">伤害数值</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="damaged">如果造成伤害时候的额外行为</param>
    /// <param name="undamaged">如果未造成伤害的额外行为</param>
    public void DamageProcedure(StageEntity src, StageEntity tgt, int value, bool recursive = true,
        Action<DamageDetails> damaged = null, Action<DamageDetails> undamaged = null)
        => DamageProcedure(new DamageDetails(src, tgt, value, recursive, damaged, undamaged));
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

    public void HealProcedure(StageEntity src, StageEntity tgt, int value)
        => HealProcedure(new HealDetails(src, tgt, value));
    public void HealProcedure(HealDetails d)
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        src.Heal(d);
        tgt.Healed(d);

        int space = tgt.MaxHp - tgt.Hp;
        tgt.Hp += d.Value;

        int actualHealed = Mathf.Min(space, d.Value);
        tgt.HealedRecord += actualHealed;

        Report.Append($"    生命变成了${tgt.Hp}");
    }

    public void BuffProcedure(StageEntity src, StageEntity tgt, string buffName, int stack = 1, bool recursive = true)
        => BuffProcedure(src, tgt, Encyclopedia.BuffCategory[buffName], stack, recursive);
    public void BuffProcedure(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => BuffProcedure(new BuffDetails(src, tgt, buffEntry, stack, recursive));
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

    public void ArmorGainProcedure(StageEntity src, StageEntity tgt, int value)
        => ArmorGainProcedure(new ArmorDetails(src, tgt, value));
    public void ArmorGainProcedure(ArmorDetails d)
    {
        d.Src._Armor(d);
        d.Tgt.Armored(d);
        if (d.Cancel)
            return;

        d.Tgt.Armor += d.Value;
        Report.Append($"    护甲变成了[{d.Src.Armor}]");
    }

    public void ArmorLoseProcedure(StageEntity tgt, int value)
    {
        tgt.Armor -= value;
        tgt.LostArmorRecord += value;
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

    public void DispelProcedure(StringBuilder seq, StageEntity entity, int stack, bool targetingFriendly)
    {
        List<Buff> buffs = entity.Buffs.FilterObj(b => b.Friendly == targetingFriendly && b.Dispellable).ToList();
        buffs.Do(b =>
        {
            b.Stack -= stack;
            seq.Append($"{b.GetName()}.Stack 驱散后变成 {b.Stack}");
        });
    }

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

    public StageReport Report;

    public void Enter()
    {
        // 文字战报，录像战报
        _hero = new StageHero(RunManager.Instance.Hero);
        _enemy = new StageEnemy(RunManager.Instance.Enemy);

        CanvasManager.Instance.StageCanvas.Refresh();

        Report = new(sb: new StringBuilder());

        Simulate();

        // config the scene
        StageCanvas.Instance.SetHeroHealth(_hero.Hp);
        StageCanvas.Instance.SetHeroArmor(0);
        StageCanvas.Instance.SetEnemyHealth(_enemy.Hp);
        StageCanvas.Instance.SetEnemyArmor(0);

        RunManager.Instance.Report = Report;

        if (!RunManager.Instance.IsStream)
        {
            AppManager.Pop();
            return;
        }

        Report.Play();
    }

    public static (int, int) SimulateBrief()
    {
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance._hero = new StageHero(RunManager.Instance.Hero);
        Instance._enemy = new StageEnemy(RunManager.Instance.Enemy);

        Instance.Report = new(sb: new StringBuilder());

        Instance.Simulate();

        return (Instance._hero.Hp, Instance._enemy.Hp);
    }

    private void Simulate()
    {
        bool heroTurn = true;
        _hero._p = -1;
        _enemy._p = -1;

        // register

        _hero.StartStage();
        _enemy.StartStage();

        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            if (heroTurn)
            {
                Report.Append($"--------第{i}回合, 玩家行动--------\n");
                _hero.Turn();
            }
            else
            {
                Report.Append($"--------第{i}回合, 敌人行动--------\n");
                _enemy.Turn();
            }

            Report.Append($"玩家 {_hero.Hp}[{_hero.Armor}] Buff:");
            foreach (Buff b in _hero.Buffs)
                Report.Append($"  {b.GetName()}*{b.Stack}");
            Report.Append("\n");
            Report.Append($"敌人 {_enemy.Hp}[{_enemy.Armor}] Buff:");
            foreach (Buff b in _enemy.Buffs)
                Report.Append($"  {b.GetName()}*{b.Stack}");
            Report.Append("\n");

            if (TryCommit(heroTurn))
                return;

            heroTurn = !heroTurn;
        }

        ForceCommit();
    }

    public static bool[] ManaSimulate()
    {
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance._hero = new StageHero(RunManager.Instance.Hero);
        Instance._enemy = new StageEnemy(new RunEnemy(1000000)); // 工具人

        Instance.Report = new();

        return Instance.InnerManaSimulate();
    }

    private bool[] InnerManaSimulate()
    {
        bool[] manaShortageBrief = new bool[RunManager.WaiGongLimit];
        bool stopWriting = false;

        void WriteManaShortage(int p)
        {
            if (stopWriting)
                return;
            manaShortageBrief[p + RunManager.WaiGongStartFromJingJie[RunManager.Instance.JingJie]] = true;
        }

        void StopWriting()
        {
            stopWriting = true;
        }

        _hero._p = -1;

        _hero.StartStage();

        _hero.ManaShortageEvent += WriteManaShortage;
        _hero.EndRoundEvent += StopWriting;

        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            _hero.Turn();
            if (stopWriting)
                break;
        }

        _hero.ManaShortageEvent -= WriteManaShortage;
        _hero.EndRoundEvent -= StopWriting;

        return manaShortageBrief;
    }

    public bool TryCommit(bool heroTurn)
    {
        if (heroTurn)
        {
            if (_hero.Hp <= 0)
            {
                _commitWin = false;
                Report.Append($"玩家失败\n");
                return true;
            }

            if (_enemy.Hp <= 0)
            {
                _commitWin = true;
                Report.Append($"玩家胜利\n");
                return true;
            }
        }
        else
        {
            if (_enemy.Hp <= 0)
            {
                _commitWin = true;
                Report.Append($"玩家胜利\n");
                return true;
            }
            if (_hero.Hp <= 0)
            {
                _commitWin = false;
                Report.Append($"玩家失败\n");
                return true;
            }
        }

        return false;
    }

    public void ForceCommit()
    {
        if (_hero.Hp >= _enemy.Hp)
        {
            _commitWin = true;
            Report.Append($"玩家胜利\n");
            return;
        }
        _commitWin = false;
        Report.Append($"玩家失败\n");
    }

    private bool _commitWin;

    public void Exit()
    {
        BattlePanelDescriptor battlePanelDescriptor = RunManager.Instance.TryGetCurrentNode()?.CurrentPanel as BattlePanelDescriptor;
        if (battlePanelDescriptor == null)
            return;

        battlePanelDescriptor.ReceiveSignal(new BattleResultSignal(_commitWin ? BattleResultSignal.BattleResultState.Win : BattleResultSignal.BattleResultState.Lose));
    }
}
