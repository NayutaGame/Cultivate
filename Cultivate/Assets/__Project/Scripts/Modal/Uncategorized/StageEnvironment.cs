
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class StageEnvironment : GDictionary, CLEventListener
{
    private static readonly int MAX_ACTION_COUNT = 120;

    public async Task FormationProcedure(StageEntity owner, FormationEntry formation, bool recursive = true)
        => await FormationProcedure(new FormationDetails(owner, formation, recursive));
    public async Task FormationProcedure(FormationDetails d)
    {
        await _eventDict.SendEvent(CLEventDict.FORMATION_WILL_ADD, d);
        if (d.Cancel) return;

        await d.Owner.AddFormation(new GainFormationDetails(d._formation));

        // if (_report.UseTween)
        //     await _report.PlayTween(new BuffTweenDescriptor(d));

        _report.Append($"    {d._formation.GetName()} is set");

        if (d.Cancel) return;
        await _eventDict.SendEvent(CLEventDict.FORMATION_DID_ADD, d);
    }

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
    public async Task AttackProcedure(StageEntity src, StageEntity tgt, int value, WuXing? wuXing = null, int times = 1,
        bool lifeSteal = false, bool pierce = false, bool crit = false, bool recursive = true,
        Func<DamageDetails, Task> damaged = null, Func<DamageDetails, Task> undamaged = null)
        => await AttackProcedure(new AttackDetails(src, tgt, value, wuXing, lifeSteal, pierce, crit, false, recursive, damaged, undamaged), times);
    public async Task AttackProcedure(AttackDetails attackDetails, int times)
    {
        if (await attackDetails.Src.TryConsumeProcedure("追击")) // 结算连击/追击
            times += 1;

        for (int i = 0; i < times; i++)
        {
            AttackDetails d = attackDetails.Clone();

            StageEntity src = d.Src;
            StageEntity tgt = d.Tgt;

            await _eventDict.SendEvent(CLEventDict.WILL_ATTACK, d);

            if (d.Cancel)
            {
                _report.Append($"    攻击被取消");
                continue;
            }

            if (!d.Pierce && d.Evade)
            {
                await _eventDict.SendEvent(CLEventDict.DID_EVADE, d);
                _report.Append($"    攻击被闪避");

                if (d.Undamaged != null)
                    await d.Undamaged(new DamageDetails(d.Src, d.Tgt, 0));
                await _eventDict.SendEvent(CLEventDict.DID_ATTACK, d);
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
                _report.Append($"    攻击为0");

                if (d.Undamaged != null)
                    await d.Undamaged(new DamageDetails(d.Src, d.Tgt, 0));
                await _eventDict.SendEvent(CLEventDict.DID_ATTACK, d);
                continue;
            }

            if (d.Crit)
                d.Value *= 2;

            DamageDetails damageDetails = new DamageDetails(d.Src, d.Tgt, d.Value, damaged: d.Damaged, undamaged: d.Undamaged);
            await DamageProcedure(damageDetails);

            if (_report.UseTween)
                await _report.PlayTween(new AttackTweenDescriptor(d));
            _report.Append($"    敌方生命[护甲]变成了${tgt.Hp}[{tgt.Armor}]");

            if (!damageDetails.Cancel)
            {
                if (d.LifeSteal)
                    await HealProcedure(src, src, damageDetails.Value);
            }

            await _eventDict.SendEvent(CLEventDict.DID_ATTACK, d);
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
    public async Task DamageProcedure(StageEntity src, StageEntity tgt, int value, bool recursive = true,
        Func<DamageDetails, Task> damaged = null, Func<DamageDetails, Task> undamaged = null)
        => await DamageProcedure(new DamageDetails(src, tgt, value, recursive, damaged, undamaged));
    public async Task DamageProcedure(DamageDetails d)
    {
        await _eventDict.SendEvent(CLEventDict.WILL_DAMAGE, d);

        if (d.Cancel || d.Value == 0)
        {
            if (d.Undamaged != null)
                await d.Undamaged(d);
            return;
        }

        await LoseHealthProcedure(d.Tgt, d.Value);
        if (d.Damaged != null)
            await d.Damaged(d);

        await _eventDict.SendEvent(CLEventDict.DID_DAMAGE, d);
    }

    public async Task LoseHealthProcedure(StageEntity owner, int value)
        => await LoseHealthProcedure(new LoseHealthDetails(owner, value));
    public async Task LoseHealthProcedure(LoseHealthDetails d)
    {
        await _eventDict.SendEvent(CLEventDict.WILL_LOSE_HEALTH, d);
        if (d.Cancel)
            return;

        d.Owner.Hp -= d.Value;

        await _eventDict.SendEvent(CLEventDict.DID_LOSE_HEALTH, d);
    }

    public async Task HealProcedure(StageEntity src, StageEntity tgt, int value)
        => await HealProcedure(new HealDetails(src, tgt, value));
    public async Task HealProcedure(HealDetails d)
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        await _eventDict.SendEvent(CLEventDict.WILL_HEAL, d);

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

        if (_report.UseTween)
        {
            await _report.PlayTween(new HealTweenDescriptor(d));
        }
        _report.Append($"    生命变成了${tgt.Hp}");

        await _eventDict.SendEvent(CLEventDict.DID_HEAL, d);
    }

    public async Task BuffProcedure(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await BuffProcedure(new BuffDetails(src, tgt, buffEntry, stack, recursive));
    public async Task BuffProcedure(BuffDetails d)
    {
        await _eventDict.SendEvent(CLEventDict.WILL_BUFF, d);
        if (d.Cancel) return;

        Buff same = d.Tgt.FindBuff(d._buffEntry);

        int oldStack = same?.Stack ?? 0;

        if (same != null && d._buffEntry.BuffStackRule != BuffStackRule.Individual)
        {
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

            if (_report.UseTween)
            {
                await _report.PlayTween(new BuffTweenDescriptor(d));
            }
            _report.Append($"    {d._buffEntry.Name}: {oldStack} -> {same.Stack}");
        }
        else
        {
            await d.Tgt.AddBuff(new GainBuffDetails(d._buffEntry, d._stack));

            if (_report.UseTween)
            {
                await _report.PlayTween(new BuffTweenDescriptor(d));
            }
            _report.Append($"    {d._buffEntry.Name}: 0 -> {d._stack}");
        }

        await _eventDict.SendEvent(CLEventDict.DID_BUFF, d);
    }

    public async Task ArmorGainProcedure(StageEntity src, StageEntity tgt, int value)
        => await ArmorGainProcedure(new ArmorGainDetails(src, tgt, value));
    public async Task ArmorGainProcedure(ArmorGainDetails d)
    {
        await _eventDict.SendEvent(CLEventDict.ARMOR_WILL_GAIN, d);

        if (d.Cancel)
            return;

        d.Tgt.Armor += d.Value;
        _report.Append($"    护甲变成了[{d.Tgt.Armor}]");

        await _eventDict.SendEvent(CLEventDict.ARMOR_DID_GAIN, d);
    }

    public async Task ArmorLoseProcedure(StageEntity src, StageEntity tgt, int value)
        => await ArmorLoseProcedure(new ArmorLoseDetails(src, tgt, value));
    public async Task ArmorLoseProcedure(ArmorLoseDetails d)
    {
        await _eventDict.SendEvent(CLEventDict.ARMOR_WILL_LOSE, d);

        if (d.Cancel)
            return;

        if (d.Tgt.Armor >= 0)
            d.Tgt.LostArmorRecord += Mathf.Min(d.Tgt.Armor, d.Value);

        d.Tgt.Armor -= d.Value;
        _report.Append($"    护甲变成了[{d.Tgt.Armor}]");

        await _eventDict.SendEvent(CLEventDict.ARMOR_DID_LOSE, d);
    }

    public async Task DispelProcedure(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool friendly = true, bool recursive = true)
        => await DispelProcedure(new DispelDetails(src, tgt, buffEntry, stack, friendly, recursive));
    public async Task DispelProcedure(DispelDetails d)
    {
        await _eventDict.SendEvent(CLEventDict.WILL_DISPEL, d);

        if (d.Cancel)
            return;

        Buff b = d.Tgt.FindBuff(d._buffEntry);
        if (b != null)
            await b.SetStack(Mathf.Max(0, b.Stack - d._stack));

        await _eventDict.SendEvent(CLEventDict.DID_DISPEL, d);
    }

    public async Task ManaShortageProcedure(StageEntity owner, int position, StageSkill skill)
        => await ManaShortageProcedure(new ManaShortageDetails(owner, position, skill));
    public async Task ManaShortageProcedure(ManaShortageDetails d)
    {
        await _eventDict.SendEvent(CLEventDict.WILL_MANA_SHORTAGE, d);

        if (d.Cancel)
            return;

        await _eventDict.SendEvent(CLEventDict.DID_MANA_SHORTAGE, d);
    }

    public async Task ExhaustProcedure(StageEntity owner, StageSkill skill)
        => await ExhaustProcedure(new ExhaustDetails(owner, skill));
    public async Task ExhaustProcedure(ExhaustDetails d)
    {
        if (d.Skill.Exhausted)
            return;

        await _eventDict.SendEvent(CLEventDict.WILL_EXHAUST, d);

        d.Skill.Exhausted = true;

        await _eventDict.SendEvent(CLEventDict.DID_EXHAUST, d);
    }

    private StageEntity[] _entities;
    public StageEntity[] Entities => _entities;

    private StageReport _report;
    public StageReport Report => _report;

    public CLEventDict _eventDict;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public StageEnvironment(RunEntity home, RunEntity away, bool useTween = false, bool useTimeline = false, bool useSb = false)
    {
        _accessors = new()
        {
            { "Home",                  () => _entities[0] },
            { "Away",                  () => _entities[1] },
            { "Report",                () => _report },
        };

        _eventDict = new();

        _entities = new StageEntity[]
        {
            new(this, home, 0),
            new(this, away, 1),
        };

        _report = new(useTween, useTimeline, useSb);
    }

    public int GetHeroBuffCount() => _entities[0].GetBuffCount();
    public int GetEnemyBuffCount() => _entities[1].GetBuffCount();

    public void WriteResult()
    {
        _report.HomeLeftHp = _entities[0].Hp;
        _report.AwayLeftHp = _entities[1].Hp;
        _report.DMingYuan = _report.HomeVictory ? 0 : 1;
    }

    public void WriteEffect()
    {
        _entities[0].WriteEffect();
        RunManager.Instance.Battle.Hero.TryExhaust();
    }

    public async Task Simulate()
    {
        await MingYuanPenaltyProcedure();
        await FormationProcedure();
        await StartStageProcedure();
        await TurnProcedure();
        await EndStageProcedure();
    }

    private async Task MingYuanPenaltyProcedure()
    {
        await _entities[0].MingYuan.MingYuanPenaltyProcedure(_entities[0]);
        await _entities[1].MingYuan.MingYuanPenaltyProcedure(_entities[1]);
    }

    private async Task FormationProcedure()
    {
        List<FormationDetails> details = new List<FormationDetails>();

        foreach (var e in _entities)
        foreach (var f in e.RunFormations())
            details.Add(new FormationDetails(e, f));

        details.Sort((lhs, rhs) => lhs._formation.GetOrder() - rhs._formation.GetOrder());

        foreach (var d in details)
        {
            await FormationProcedure(d);
        }
    }

    private async Task StartStageProcedure()
    {
        foreach (var e in _entities)
        {
            e._p = -1;
            await _eventDict.SendEvent(CLEventDict.START_STAGE, new StageDetails(e));
        }
    }

    private async Task TurnProcedure()
    {
        int whosTurn = 0;
        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            StageEntity actor = _entities[whosTurn];

            _report.Append($"--------第{i}回合, {actor.GetName()}行动--------\n");
            await actor.Turn();

            _entities.Do(e =>
            {
                _report.Append($"{e.GetName()} {e.Hp}[{e.Armor}] Buff:");
                foreach (Buff b in e.Buffs)
                    _report.Append($"  {b.GetName()}*{b.Stack}");
                _report.Append("\n");
            });

            if (TryCommit(whosTurn))
                return;

            whosTurn = 1 - whosTurn;
        }
    }

    private async Task EndStageProcedure()
    {
        await _eventDict.SendEvent(CLEventDict.END_STAGE, new StageDetails(_entities[1]));
        await _eventDict.SendEvent(CLEventDict.END_STAGE, new StageDetails(_entities[0]));
        ForceCommit();
    }

    private void ForceCommit()
    {
        _report.HomeVictory = _entities[0].Hp >= _entities[1].Hp;
    }

    public async Task<bool[]> InnerManaSimulate()
    {
        bool[] manaShortageBrief = new bool[RunManager.SkillLimit];
        bool stopWriting = false;

        CLEventDescriptor writeManaShortageEventDescriptor = new CLEventDescriptor(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_MANA_SHORTAGE, 0,
            async (listener, d) =>
            {
                if (stopWriting)
                    return;
                ManaShortageDetails manaShortageDetails = (ManaShortageDetails)d;
                manaShortageBrief[manaShortageDetails.Position + RunManager.SkillStartFromJingJie[_entities[0].RunEntity.GetJingJie()]] = true;
            });

        CLEventDescriptor stopWriteEventDescriptor = new CLEventDescriptor(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.END_ROUND, 0,
            async (listener, d) => stopWriting = true);

        StageEntity hero = _entities[0];

        hero._p = -1;

        await FormationProcedure();
        await MingYuanPenaltyProcedure();
        await _eventDict.SendEvent(CLEventDict.START_STAGE, new StageDetails(hero));

        _eventDict.Register(this, writeManaShortageEventDescriptor);
        _eventDict.Register(this, stopWriteEventDescriptor);

        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            await hero.Turn();
            if (stopWriting)
                break;
        }

        _eventDict.Unregister(this, writeManaShortageEventDescriptor);
        _eventDict.Unregister(this, stopWriteEventDescriptor);

        return manaShortageBrief;
    }

    private bool TryCommit(int whosTurn)
    {
        if (whosTurn == 0)
        {
            if (_entities[whosTurn].Hp <= 0)
            {
                _report.HomeVictory = false;
                return true;
            }

            if (_entities[1 - whosTurn].Hp <= 0)
            {
                _report.HomeVictory = true;
                return true;
            }
        }
        else
        {
            if (_entities[whosTurn].Hp <= 0)
            {
                _report.HomeVictory = true;
                return true;
            }
            if (_entities[1 - whosTurn].Hp <= 0)
            {
                _report.HomeVictory = false;
                return true;
            }
        }

        return false;
    }
}
