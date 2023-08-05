
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class StageEnvironment : GDictionary
{
    private static readonly int MAX_ACTION_COUNT = 120;

    public async Task FormationProcedure(StageEntity owner, FormationEntry formation, bool recursive = true)
        => await FormationProcedure(new FormationDetails(owner, formation, recursive));
    public async Task FormationProcedure(FormationDetails d)
    {
        await _eventDict.FireEvent(CLEventDict.FORMATION_WILL_ADD, d);
        if (d.Cancel) return;

        await d.Owner.AddFormation(new GainFormationDetails(d._formation));

        // if (_report.UseTween)
        //     await _report.PlayTween(new BuffTweenDescriptor(d));

        _report.Append($"    {d._formation.GetName()} is set");

        if (d.Cancel) return;
        await _eventDict.FireEvent(CLEventDict.FORMATION_DID_ADD, d);
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

            await _eventDict.FireEvent(CLEventDict.WILL_ATTACK, d);

            if (d.Cancel)
            {
                _report.Append($"    攻击被取消");
                continue;
            }

            if (!d.Pierce && d.Evade)
            {
                await _eventDict.FireEvent(CLEventDict.DID_EVADE, d);
                _report.Append($"    攻击被闪避");
                continue;
            }

            if (d.Value == 0)
            {
                _report.Append($"    攻击为0");
                await _eventDict.FireEvent(CLEventDict.DID_ATTACK, d);
                continue;
            }

            if (!d.Pierce && tgt.Armor >= 0) // 结算护甲
            {
                int negate = Mathf.Min(d.Value, tgt.Armor);
                d.Value -= negate;
                await ArmorLoseProcedure(d.Src, d.Tgt, negate);

                if (d.Value == 0) // undamage?.Invoke();
                {
                    _report.Append($"    攻击被格挡");
                    await _eventDict.FireEvent(CLEventDict.DID_ATTACK, d);
                    continue;
                }
            }
            else if (tgt.Armor < 0) // 结算破甲
            {
                d.Value += -tgt.Armor;
                tgt.Armor = 0;
            }

            // 结算暴击
            if (d.Crit)
                d.Value *= 2;

            // 伤害Procedure
            DamageDetails damageDetails = await DamageProcedure(d.Src, d.Tgt, d.Value, damaged: d.Damaged, undamaged: d.Undamaged);

            if (_report.UseTween)
            {
                await _report.PlayTween(new AttackTweenDescriptor(d));
            }
            _report.Append($"    敌方生命[护甲]变成了${tgt.Hp}[{tgt.Armor}]");

            // 结算吸血
            if (!damageDetails.Cancel)
            {
                if (d.LifeSteal)
                    await HealProcedure(src, src, damageDetails.Value);
            }

            await _eventDict.FireEvent(CLEventDict.DID_ATTACK, d);
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
    public async Task<DamageDetails> DamageProcedure(StageEntity src, StageEntity tgt, int value, bool recursive = true,
        Func<DamageDetails, Task> damaged = null, Func<DamageDetails, Task> undamaged = null)
        => await DamageProcedure(new DamageDetails(src, tgt, value, recursive, damaged, undamaged));
    public async Task<DamageDetails> DamageProcedure(DamageDetails d)
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        await _eventDict.FireEvent(CLEventDict.WILL_DAMAGE, d);

        if (d.Cancel)
        {
            d.Undamaged?.Invoke(d);
            return d;
        }

        tgt.Hp -= d.Value;

        if (d.Value == 0)
        {
            d.Undamaged?.Invoke(d);
            return d;
        }

        d.Damaged?.Invoke(d);

        await _eventDict.FireEvent(CLEventDict.DID_DAMAGE, d);

        return d;
    }

    public async Task HealProcedure(StageEntity src, StageEntity tgt, int value)
        => await HealProcedure(new HealDetails(src, tgt, value));
    public async Task HealProcedure(HealDetails d)
    {
        StageEntity src = d.Src;
        StageEntity tgt = d.Tgt;

        await _eventDict.FireEvent(CLEventDict.WILL_HEAL, d);

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

        await _eventDict.FireEvent(CLEventDict.DID_HEAL, d);
    }

    public async Task BuffProcedure(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool recursive = true)
        => await BuffProcedure(new BuffDetails(src, tgt, buffEntry, stack, recursive));
    public async Task BuffProcedure(BuffDetails d)
    {
        await _eventDict.FireEvent(CLEventDict.WILL_BUFF, d);
        if (d.Cancel) return;

        Buff same = d.Tgt.FindBuff(d._buffEntry);

        int oldStack = same?.Stack ?? 0;

        if (same != null && d._buffEntry.BuffStackRule != BuffStackRule.Individual)
        {
            switch (d._buffEntry.BuffStackRule)
            {
                case BuffStackRule.Wasted:
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

        await _eventDict.FireEvent(CLEventDict.DID_BUFF, d);
    }

    public async Task ArmorGainProcedure(StageEntity src, StageEntity tgt, int value)
        => await ArmorGainProcedure(new ArmorGainDetails(src, tgt, value));
    public async Task ArmorGainProcedure(ArmorGainDetails d)
    {
        await _eventDict.FireEvent(CLEventDict.ARMOR_WILL_GAIN, d);

        if (d.Cancel)
            return;

        d.Tgt.Armor += d.Value;
        _report.Append($"    护甲变成了[{d.Tgt.Armor}]");

        await _eventDict.FireEvent(CLEventDict.ARMOR_DID_GAIN, d);
    }

    public async Task ArmorLoseProcedure(StageEntity src, StageEntity tgt, int value)
        => await ArmorLoseProcedure(new ArmorLoseDetails(src, tgt, value));
    public async Task ArmorLoseProcedure(ArmorLoseDetails d)
    {
        await _eventDict.FireEvent(CLEventDict.ARMOR_WILL_LOSE, d);

        if (d.Cancel)
            return;

        if (d.Tgt.Armor >= 0)
            d.Tgt.LostArmorRecord += Mathf.Min(d.Tgt.Armor, d.Value);

        d.Tgt.Armor -= d.Value;
        _report.Append($"    护甲变成了[{d.Tgt.Armor}]");

        await _eventDict.FireEvent(CLEventDict.ARMOR_DID_LOSE, d);
    }

    public async Task DispelProcedure(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool friendly = true, bool recursive = true)
        => await DispelProcedure(new DispelDetails(src, tgt, buffEntry, stack, friendly, recursive));
    public async Task DispelProcedure(DispelDetails d)
    {
        await _eventDict.FireEvent(CLEventDict.WILL_DISPEL, d);

        if (d.Cancel)
            return;

        Buff b = d.Src.FindBuff(d._buffEntry);
        await b.SetStack(Mathf.Max(0, b.Stack - d._stack));

        await _eventDict.FireEvent(CLEventDict.DID_DISPEL, d);
    }

    public async Task ExhaustProcedure(StageEntity owner, StageSkill skill, bool forRun)
        => await ExhaustProcedure(new ExhaustDetails(owner, skill, forRun));
    public async Task ExhaustProcedure(ExhaustDetails d)
    {
        if (d.ForRun)
            d.Skill.RunExhausted = true;

        if (d.Skill.Exhausted)
            return;

        await _eventDict.FireEvent(CLEventDict.WILL_EXHAUST, d);

        d.Skill.Exhausted = true;

        await _eventDict.FireEvent(CLEventDict.DID_EXHAUST, d);
    }

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    private StageEntity[] _entities;
    public StageEntity[] Entities => _entities;
    private StageReport _report;
    public StageReport Report => _report;

    public CLEventDict _eventDict;

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
        _report.MingYuanPenalty = _report.HomeVictory ? 0 : 1;
    }

    public void WriteEffect()
    {
        _entities[0].WriteEffect();
    }

    public async Task Simulate()
    {
        int whosTurn = 0;

        await StartFormation();

        foreach (var e in _entities)
        {
            e._p = -1;
            await _eventDict.FireEvent(CLEventDict.START_STAGE, new StageDetails(e));
        }

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

        await _eventDict.FireEvent(CLEventDict.END_STAGE, new StageDetails(_entities[1]));
        await _eventDict.FireEvent(CLEventDict.END_STAGE, new StageDetails(_entities[0]));
        ForceCommit();
    }

    private async Task StartFormation()
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

    public async Task<bool[]> InnerManaSimulate()
    {
        bool[] manaShortageBrief = new bool[RunManager.SkillLimit];
        bool stopWriting = false;

        async Task WriteManaShortage(int p)
        {
            if (stopWriting)
                return;
            manaShortageBrief[p + RunManager.SkillStartFromJingJie[_entities[0].RunEntity.GetJingJie()]] = true;
        }

        async Task StopWriting(StageEventDetails d)
        {
            stopWriting = true;
        }

        StageEntity hero = _entities[0];

        hero._p = -1;

        await StartFormation();
        await _eventDict.FireEvent(CLEventDict.START_STAGE, new StageDetails(hero));

        hero.ManaShortageEvent += WriteManaShortage;
        _eventDict.AddCallback(CLEventDict.END_ROUND, 0, StopWriting);

        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            await hero.Turn();
            if (stopWriting)
                break;
        }

        hero.ManaShortageEvent -= WriteManaShortage;
        _eventDict.RemoveCallback(CLEventDict.END_ROUND, StopWriting);

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

    private void ForceCommit()
    {
        if (_entities[0].Hp >= _entities[1].Hp)
        {
            _report.HomeVictory = true;
            return;
        }

        _report.HomeVictory = false;
    }
}
