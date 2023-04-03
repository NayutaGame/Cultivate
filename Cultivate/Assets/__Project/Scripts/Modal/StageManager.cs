using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using Sequence = DG.Tweening.Sequence;

public class StageManager : Singleton<StageManager>, GDictionary
{
    private readonly int MAX_ACTION_COUNT = 128;

    /// <summary>
    /// 发起一次攻击行为，会结算目标的护甲
    /// </summary>
    /// <param name="src">攻击者</param>
    /// <param name="tgt">受攻击者</param>
    /// <param name="value">攻击数值</param>
    /// <param name="times">攻击次数</param>
    /// <param name="lifeSteal">是否吸血</param>
    /// <param name="pierce">是否穿透</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="damaged">如果造成伤害时候的额外行为</param>
    /// <param name="undamaged">如果未造成伤害的额外行为</param>
    public void AttackProcedure(StageEntity src, StageEntity tgt, int value, int times = 1, bool lifeSteal = false, bool pierce = false, bool recursive = true,
        Action<DamageDetails> damaged = null, Action<DamageDetails> undamaged = null)
        => AttackProcedure(new AttackDetails(src, tgt, value, lifeSteal, pierce, false, recursive, damaged, undamaged), times);
    public void AttackProcedure(AttackDetails attackDetails, int times)
    {
        // 结算连击

        for (int i = 0; i < times; i++)
        {
            AttackDetails d = attackDetails.Clone();

            StageEntity src = d.Src;
            StageEntity tgt = d.Tgt;

            src.Attack(d);
            tgt.Attacked(d);
            if (d.Cancel)
            {
                Report.Append($"    攻击被取消");
                continue;
            }

            if (!d.Pierce && d.Evade) // 提取事件 target.Evade(attackDetails);
            {
                Report.Append($"    攻击被闪避");
                continue;
            }

            if (d.Value == 0)
            {
                Report.Append($"    攻击为0");
                continue;
            }

            if (!d.Pierce && tgt.Armor >= 0) // 结算护甲
            {
                int negate = Mathf.Min(d.Value, tgt.Armor);
                d.Value -= negate;
                ArmorLoseProcedure(d.Tgt, negate);

                if (d.Value == 0) // undamage?.Invoke();
                {
                    Report.Append($"    攻击被格挡");
                    continue;
                }
            }
            else if (tgt.Armor < 0) // 结算破甲
            {
                d.Value += -tgt.Armor;
                tgt.Armor = 0;
            }

            // 伤害Procedure
            DamageDetails damageDetails = DamageProcedure(d.Src, d.Tgt, d.Value, damaged: d.Damaged, undamaged: d.Undamaged);

            // 结算吸血
            if (!damageDetails.Cancel)
            {
                if (d.LifeSteal)
                    HealProcedure(src, src, damageDetails.Value);
            }

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
    public DamageDetails DamageProcedure(StageEntity src, StageEntity tgt, int value, bool recursive = true,
        Action<DamageDetails> damaged = null, Action<DamageDetails> undamaged = null)
        => DamageProcedure(new DamageDetails(src, tgt, value, recursive, damaged, undamaged));
    public DamageDetails DamageProcedure(DamageDetails d)
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        // 如果击伤
        // damaged?.Invoke(damageDetails);

        src.Damage(d);
        tgt.Damaged(d);

        if (d.Cancel)
        {
            d.Undamaged?.Invoke(d);
            return d;
        }

        tgt.Hp -= d.Value;

        if (tgt.Hp <= 0)
        {
            bool activeLastStand = tgt.GetStackOfBuff("激活的不屈") > 0;
            if (activeLastStand)
            {
                tgt.Hp = 1;
            }
            else
            {
                bool lastStand = tgt.TryConsumeBuff("不屈");
                if (lastStand)
                {
                    BuffProcedure(tgt, tgt, "激活的不屈");
                    tgt.Hp = 1;
                }
            }
        }

        if (d.Value == 0)
        {
            d.Undamaged?.Invoke(d);
            return d;
        }
        else
        {
            d.Damaged?.Invoke(d);
            return d;
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
        if (tgt.Armor < 0)
        {
            tgt.Armor -= value;
            return;
        }

        tgt.LostArmorRecord += Mathf.Min(tgt.Armor, value);
        tgt.Armor -= value;
    }

    public void DispelProcedure(StageEntity entity, int stack, bool targetingFriendly)
    {
        List<Buff> buffs = entity.Buffs.FilterObj(b => b.Friendly == targetingFriendly && b.Dispellable).ToList();
        buffs.Do(b =>
        {
            b.Stack -= stack;
            Report.Append($"{b.GetName()}.Stack 驱散后变成 {b.Stack}");
        });
    }

    public StageEntity[] _entities;
    [NonSerialized] public EntitySlot[] _slots;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
    public static T Get<T>(IndexPath indexPath)
    {
        object curr = Instance;
        foreach (string key in indexPath.Values)
        {
            if (int.TryParse(key, out int i))
            {
                IList l = curr as IList;
                if (l.Count <= i)
                    return default;
                curr = l[i];
            }
            else
            {
                curr = (curr as GDictionary).GetAccessors()[key]();
            }
        }

        if (curr is T ret)
            return ret;
        else
            return default;
    }

    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new()
        {
            { "Home",                  () => _entities[0] },
            { "Away",                  () => _entities[1] },
        };
    }

    // public int GetHeroNeiGongCount() => _hero._neiGongList.Length;
    // public int GetHeroWaiGongCount() => _hero._waiGongList.Length;
    // public int GetEnemyNeiGongCount() => _enemy._neiGongList.Length;
    // public int GetEnemyWaiGongCount() => _enemy._waiGongList.Length;

    public int GetHeroBuffCount() => _entities[0].GetBuffCount();
    public int GetEnemyBuffCount() => _entities[1].GetBuffCount();

    public StageReport Report;

    public void Enter()
    {
        // 文字战报，录像战报
        _entities = new StageEntity[]
        {
            new StageHero(RunManager.Instance.Hero, 0),
            new StageEnemy(RunManager.Instance.Enemy, 1),
        };

        CanvasManager.Instance.StageCanvas.Refresh();

        Report = new(sb: new StringBuilder());

        Simulate();

        // config the scene
        StageCanvas.Instance.SetHeroHealth(_entities[0].Hp);
        StageCanvas.Instance.SetHeroArmor(0);
        StageCanvas.Instance.SetEnemyHealth(_entities[1].Hp);
        StageCanvas.Instance.SetEnemyArmor(0);

        Report.HomeLeftHp = _entities[0].Hp;
        Report.AwayLeftHp = _entities[1].Hp;
        Report.HomeVictory = _commitWin;
        Report.MingYuanPenalty = _commitWin ? 0 : 1;
        RunManager.Instance.Report = Report;

        if (!RunManager.Instance.IsStream)
        {
            AppManager.Pop();
            return;
        }

        Report.Play();
    }

    public static StageReport SimulateBrief(StageEntity[] entities)
    {
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance._entities = entities;

        Instance.Report = new(sb: new StringBuilder());

        Instance.Simulate();

        Instance.Report.HomeLeftHp = Instance._entities[0].Hp;
        Instance.Report.AwayLeftHp = Instance._entities[1].Hp;
        Instance.Report.HomeVictory = Instance._commitWin;
        Instance.Report.MingYuanPenalty = Instance._commitWin ? 0 : 1;

        return Instance.Report;
    }

    private void Simulate()
    {
        int whosTurn = 0;

        _entities.Do(e =>
        {
            e._p = -1;
            e.StartStage();
        });

        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            StageEntity actor = _entities[whosTurn];

            Report.Append($"--------第{i}回合, {actor.GetName()}行动--------\n");
            actor.Turn();

            _entities.Do(e =>
            {
                Report.Append($"{e.GetName()} {e.Hp}[{e.Armor}] Buff:");
                foreach (Buff b in e.Buffs)
                    Report.Append($"  {b.GetName()}*{b.Stack}");
                Report.Append("\n");
            });

            if (TryCommit(whosTurn))
                return;

            whosTurn = 1 - whosTurn;
        }

        ForceCommit();
    }

    public static bool[] ManaSimulate()
    {
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance._entities = new StageEntity[]
        {
            new StageHero(RunManager.Instance.Hero, 0),
            new StageEnemy(new RunEnemy(1000000), 1), // 工具人
        };

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

        StageEntity hero = _entities[0];

        hero._p = -1;

        hero.StartStage();

        hero.ManaShortageEvent += WriteManaShortage;
        hero.EndRoundEvent += StopWriting;

        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            hero.Turn();
            if (stopWriting)
                break;
        }

        hero.ManaShortageEvent -= WriteManaShortage;
        hero.EndRoundEvent -= StopWriting;

        return manaShortageBrief;
    }

    public bool TryCommit(int whosTurn)
    {
        if (whosTurn == 0)
        {
            if (_entities[whosTurn].Hp <= 0)
            {
                _commitWin = false;
                Report.Append($"玩家失败\n");
                return true;
            }

            if (_entities[1 - whosTurn].Hp <= 0)
            {
                _commitWin = true;
                Report.Append($"玩家胜利\n");
                return true;
            }
        }
        else
        {
            if (_entities[whosTurn].Hp <= 0)
            {
                _commitWin = true;
                Report.Append($"玩家胜利\n");
                return true;
            }
            if (_entities[1 - whosTurn].Hp <= 0)
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
        if (_entities[0].Hp >= _entities[1].Hp)
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
