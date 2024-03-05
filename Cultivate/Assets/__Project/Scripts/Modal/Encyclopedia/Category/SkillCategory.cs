
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;
using Range = CLLibrary.Range;

public class SkillCategory : Category<SkillEntry>
{
    public SkillCategory()
    {
        AddRange(new List<SkillEntry>()
        {
            new("不存在的技能", JingJie.LianQi, "不存在的技能", withinPool: false),

            new("聚气术", JingJie.LianQi, "灵气+1", withinPool: false,
                execute: (entity, skill, recursive) =>
                    entity.BuffSelfProcedure("灵气")),

            // new("木30", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗10生命\n{18 + 4 * dj}攻\n每1闪避，多{4 + 2 * dj}攻"), WuXing.Mu, type: skillType.Attack,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         int value = (4 + 2 * skill.Dj) * caster.GetStackOfBuff("闪避");
            //         caster.AttackProcedure(18 + 4 * skill.Dj + value);
            //     }),
            //
            // new("木31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"每{4 - dj}格挡，消耗1点，闪避+1"), WuXing.Mu,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         int value = caster.GetStackOfBuff("格挡") / (4 - skill.Dj);
            //         caster.TryConsumeBuff("格挡", value);
            //         caster.BuffSelfProcedure("闪避", value);
            //     }),
            //
            // new("雷鸣", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗\n二动\n力量+{1 + dj}\n{3 + dj}攻"), WuXing.Mu, type: skillType.Attack,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         await skill.ConsumeProcedure();
            //         caster.Swift = true;
            //         caster.BuffSelfProcedure("力量", 1 + skill.Dj);
            //         caster.AttackProcedure(3 + skill.Dj);
            //     }),
            //
            // new("金22", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{8 + 2 * dj}攻\n相邻牌都非攻击：翻倍\n充沛：翻倍"), WuXing.Jin, manaCost: 2, type: skillType.Attack,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         int d = skill.NoAttackAdjacents ? 2 : 1;
            //         d *= caster.TryConsumeMana(2) ? 2 : 1;
            //         caster.AttackProcedure((8 + 3 * skill.Dj) * d);
            //     }),
            //
            // new("水23", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"每{6 - dj}锋锐，消耗1点，格挡+1"), WuXing.Shui, 1,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         int value = caster.GetStackOfBuff("锋锐") / (6 - skill.Dj);
            //         caster.TryConsumeBuff("锋锐", value);
            //         caster.BuffSelfProcedure("格挡", value);
            //     }),
            //
            // new("木20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+2\n每有{6 - dj}点格挡，力量+1"), WuXing.Mu, type: skillType.LingQi,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         caster.BuffSelfProcedure("灵气", 2);
            //         int value = caster.GetStackOfBuff("格挡") / (6 - skill.Dj);
            //         caster.BuffSelfProcedure("力量", value);
            //     }),
            //
            // new("火31", new CLLibrary.Range(3, 5), new ChipDescription((l, j,, dj p) => $"消耗所有生命转护甲"), WuXing.Huo, new ManaCost((l, j,, dj p) => 4 - 2 * (j - 3)),
            //     execute: (caster, skill, recursive) =>
            //     {
            //         int value = caster.Hp;
            //         StageManager.Instance.AttackProcedure(caster, caster, value);
            //         caster.ArmorGainSelfProcedure(value);
            //     }),
            //
            // new("金31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗一半护甲，施加等量减甲"), WuXing.Jin, new ManaCost((j, dj) => 6 - j),
            //     execute: (caster, skill, recursive) =>
            //     {
            //         int value = caster.Armor / 2;
            //         caster.ArmorLoseSelfProcedure(value);
            //         caster.ArmorLoseOppoProcedure(value);
            //     }),
            //
            // new("水24", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+{2 + dj}\n满血：翻倍"), WuXing.Shui, type: skillType.LingQi,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         int d = caster.IsFullHp ? 2 : 1;
            //         caster.BuffSelfProcedure("灵气", (2 + skill.Dj) * d);
            //     }),
            //
            // new("熟能生巧", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"使用消耗不少于2的牌，下一次攻击带有吸血"), WuXing.Mu, manaCost: 2, type: skillType.Attack,
            //     execute: (caster, skill, recursive) =>
            //     {
            //     }),
            //
            // new("夜幕", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"本轮格挡+{3 + dj}"), WuXing.Mu, manaCost: 1,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         caster.BuffSelfProcedure("轮格挡", 3);
            //     }),
            //
            // new("朝孔雀", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"力量+1\n{3 + 3 * dj}攻\n消耗1格挡：翻倍"), WuXing.Mu, manaCost: 1, type: skillType.Attack,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         int d = caster.TryConsumeBuff("格挡") ? 2 : 1;
            //         caster.BuffSelfProcedure("力量", d);
            //         caster.AttackProcedure((3 + 3 * skill.Dj) * d);
            //     }),
            //
            // new("贪狼", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"5攻x{3 + dj}\n不屈+2"), WuXing.Huo, 2, type: skillType.Attack,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         caster.AttackProcedure(5, 3 + skill.Dj);
            //         caster.BuffSelfProcedure("不屈", 2);
            //     }),
            //
            // new("飞龙在天", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{18 + 3 * dj}攻\n击伤：施加{2 + dj}缠绕"), WuXing.Shui, 4, type: skillType.Attack,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         caster.AttackProcedure(18 + 3 * skill.Dj,
            //             damaged: d => caster.BuffOppoProcedure("缠绕", 2 + skill.Dj));
            //     }),
            //
            // new("廉贞", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"对敌方造成伤害时，至少为{4 + 3 * dj}"), WuXing.Huo,
            //     execute: (caster, skill, recursive) =>
            //     {
            //     }),
            //
            // new("腐朽", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{(dj >= 1 ? "二动\n" : "")}敌方减甲大于生命：斩杀"), WuXing.Jin,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         if (skill.Dj >= 1)
            //             caster.Swift = true;
            //
            //         if (-caster.Opponent().Armor >= caster.Opponent().Hp)
            //             caster.Opponent().Hp = 0;
            //     }),
            //
            // new("水41", new CLLibrary.Range(4, 5), "消耗\n本场战斗中治疗被代替，受到治疗时，每有10点，格挡+1\n格挡效果翻倍", WuXing.Shui,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         await skill.ConsumeProcedure();
            //         caster.BuffSelfProcedure("强化格挡");
            //     }),
            //
            // new("水42", new CLLibrary.Range(4, 5), "消耗\n被治疗时，如果实际治疗>=20，二动", WuXing.Shui,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         await skill.ConsumeProcedure();
            //         caster.BuffSelfProcedure("治疗转二动");
            //     }),
            //
            // new("土41", new CLLibrary.Range(4, 5), "消耗\n累计获得200护甲：永久暴击", WuXing.Tu,
            //     execute: (caster, skill, recursive) =>
            //     {
            //     }),
            //
            // new("土42", new CLLibrary.Range(4, 5), new ChipDescription((l, j, dj, p) => $"持续4次，护甲减少时，加回来"), WuXing.Tu,
            //     execute: (caster, skill, recursive) =>
            //     {
            //     }),
            //
            // new("木剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{16 + 4 * dj}攻\n架势：暴击"), WuXing.Tu, 0, type: skillType.Attack,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         caster.AttackProcedure(16 + 4 * skill.Dj, crit: skill.JiaShi, wuXing: skill.Entry.WuXing);
            //     }),
            //
            // new("土31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"1攻 每失去过{8 - j}点护甲，多1攻"), WuXing.Tu, 0, type: skillType.Attack,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         caster.AttackProcedure(1 + (8 - caster.LostArmorRecord / skill.GetJingJie()), crit: true, wuXing: skill.Entry.WuXing);
            //     }),
            //
            // new("土32", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"护甲翻倍"), WuXing.Tu, new ManaCost((j, dj) => 12 - 2 * j),
            //     execute: (caster, skill, recursive) =>
            //     {
            //         caster.ArmorGainSelfProcedure(caster.Armor);
            //     }),
            //
            // new("软剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{4 + dj * 4}攻\n击伤：二动"), WuXing.Tu, 1, type: skillType.Attack,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         caster.AttackProcedure(4 + 4 * skill.Dj, wuXing: skill.Entry.WuXing,
            //             damaged: d => caster.Swift = true);
            //     }),

            new("乘风", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{5 + dj}攻\n若有锋锐：{3 + dj}攻"),
                wuXing: WuXing.Jin, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.GetStackOfBuff("锋锐") > 0 || await caster.IsFocused();
                    int add = cond ? 3 + skill.Dj : 0;
                    await caster.AttackProcedure(5 + skill.Dj + add, wuXing: skill.Entry.WuXing);
                }),

            new("微风", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"护甲+{2 + dj}\n初次：锋锐+{3 + 2 * dj}"),
                wuXing: WuXing.Jin,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(2 + skill.Dj);
                    bool cond = skill.IsFirstTime || await caster.IsFocused();
                    if (cond)
                        await caster.BuffSelfProcedure("锋锐", 3 + 2 * skill.Dj);
                }),

            new("金刃", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{3 + dj}攻\n施加{3 + dj}减甲"),
                wuXing: WuXing.Jin, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.ArmorLoseOppoProcedure(3 + skill.Dj);
                }),

            new("贪狼", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"奇偶：{5 + 2 * dj}攻/护甲+{5 + 2 * dj}"),
                wuXing: WuXing.Jin, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    int value = 5 + 2 * skill.Dj;
                    if (skill.IsOdd || await caster.IsFocused())
                        await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
                    if (skill.IsEven || await caster.IsFocused())
                        await caster.ArmorGainSelfProcedure(value);
                }),

            new("起", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{3 + dj}攻\n锋锐+{3 + dj}"),
                wuXing: WuXing.Jin, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.BuffSelfProcedure("锋锐", 3 + skill.Dj);
                }),

            new("金光罩", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"护甲+{4 + 2 * dj}\n施加{4 + 2 * dj}减甲"),
                wuXing: WuXing.Jin,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(4 + 2 * skill.Dj);
                    await caster.ArmorLoseOppoProcedure(4 + 2 * skill.Dj);
                }),

            new("竹剑", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{6 + 2 * dj}攻\n敌方有减甲：多1次"),
                wuXing: WuXing.Jin, manaCostEvaluator: 1, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().Armor < 0 || await caster.IsFocused();
                    int times = cond ? 2 : 1;
                    await caster.AttackProcedure(6 + 2 * skill.Dj, times: times, wuXing: skill.Entry.WuXing);
                }),

            new("廉贞", new CLLibrary.Range(1, 5),
                new SkillDescription((j, dj) => $"奇偶：施加{8 + 2 * dj}减甲\n/护甲+{16 + 2 * dj}"), wuXing: WuXing.Jin, manaCostEvaluator: 1,
                execute: async (caster, skill, recursive) =>
                {
                    if (skill.IsOdd || await caster.IsFocused())
                        await caster.ArmorLoseOppoProcedure(8 + 2 * skill.Dj);
                    if (skill.IsEven || await caster.IsFocused())
                        await caster.ArmorGainSelfProcedure(16 + 2 * skill.Dj);
                }),

            new("承", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{14 + 6 * dj}攻\n每1锋锐，多1攻"),
                wuXing: WuXing.Jin, manaCostEvaluator: 1, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(14 + 6 * skill.Dj + caster.GetStackOfBuff("锋锐"), wuXing: skill.Entry.WuXing);
                }),

            new("无常已至", new CLLibrary.Range(1, 5),
                new SkillDescription((j, dj) => $"消耗\n造成伤害时：\n施加伤害值的减甲\n最多{3 + 2 * dj}"), wuXing: WuXing.Jin,
                manaCostEvaluator: 3, skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("无常已至", 3 + 2 * skill.Dj);
                }),

            new("菊剑", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{4 + 4 * dj}攻\n敌方有减甲：二动"),
                wuXing: WuXing.Jin, skillTypeComposite: SkillType.Attack | SkillType.ErDong,
                execute: async (caster, skill, recursive) =>
                {
                    caster.Swift |= caster.Opponent().Armor < 0;
                    await caster.AttackProcedure(4 + 4 * skill.Dj, wuXing: skill.Entry.WuXing);
                    caster.Swift |= caster.Opponent().Armor < 0;

                    if (!caster.Swift)
                        caster.Swift |= await caster.IsFocused();
                }),

            new("刺穴", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n灵气+{6 + 2 * dj}"), wuXing: WuXing.Jin,
                skillTypeComposite: SkillType.LingQi | SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("灵气", 6 + 2 * skill.Dj);
                }),

            new("转", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"护甲+6\n每{6 - dj}护甲，锋锐+1"), wuXing: WuXing.Jin,
                manaCostEvaluator: 1,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(6);
                    int value = caster.Armor / (6 - skill.Dj);
                    await caster.BuffSelfProcedure("锋锐", value);
                }),

            new("武曲", new CLLibrary.Range(3, 5),
                new SkillDescription((j, dj) => $"奇偶：施加{12 + 2 * dj}减甲/护甲+{12 + 2 * dj}\n消耗1锋锐：多{8 + 2 * dj}"),
                wuXing: WuXing.Jin, manaCostEvaluator: 1,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = await caster.IsFocused() || await caster.TryConsumeProcedure("锋锐");
                    int add = cond ? (8 + 2 * skill.Dj) : 0;
                    int value = 12 + 2 * skill.Dj + add;
                    if (skill.IsOdd)
                        await caster.ArmorLoseOppoProcedure(value);
                    if (skill.IsEven)
                        await caster.ArmorGainSelfProcedure(value);
                }),

            new("敛息", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"下{1 + dj}次造成伤害转减甲"), wuXing: WuXing.Jin,
                manaCostEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("敛息", 1 + skill.Dj);
                }),

            new("破军", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"奇偶：对方灵气-{2 + dj}\n/灵气+{4 + 2 * dj}"),
                wuXing: WuXing.Jin, skillTypeComposite: SkillType.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    if (skill.IsOdd || await caster.IsFocused())
                    {
                        Buff b = caster.Opponent().FindBuff("灵气");
                        if (b != null)
                            await b.SetDStack(2 + skill.Dj);
                    }

                    if (skill.IsEven || await caster.IsFocused())
                    {
                        await caster.BuffSelfProcedure("灵气", 4 + 2 * skill.Dj);
                    }
                }),

            new("合", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"奇偶：暴击/击伤：每造成5伤害，锋锐+1"), wuXing: WuXing.Jin, manaCostEvaluator: 1,
                execute: async (caster, skill, recursive) =>
                {
                    bool odd = skill.IsOdd || await caster.IsFocused();
                    bool even = skill.IsEven || await caster.IsFocused();

                    async Task<DamageDetails> Damaged(DamageDetails d)
                    {
                        await d.Src.BuffSelfProcedure("锋锐", d.Value / 5);
                        return d;
                    }

                    Func<DamageDetails, Task<DamageDetails>> damaged = even ? null : Damaged;

                    await caster.AttackProcedure(10, skill.Entry.WuXing, crit: odd, damaged: Damaged);
                }),

            new("少阴", new CLLibrary.Range(4, 5), "消耗\n施加减甲：额外+3\n消耗少阳：额外层数", wuXing: WuXing.Jin,
                skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    int value = caster.GetStackOfBuff("少阳") + 3;
                    await caster.TryRemoveBuff("少阳");
                    await caster.BuffSelfProcedure("少阴", value);
                }),

            new("梅剑", new CLLibrary.Range(4, 5), "5攻x2\n敌方有减甲：暴击", wuXing: WuXing.Jin,
                skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().Armor < 0 || await caster.IsFocused();
                    await caster.AttackProcedure(5, crit: cond, wuXing: skill.Entry.WuXing);
                    await caster.AttackProcedure(5, crit: cond || caster.Opponent().Armor < 0 || await caster.IsFocused(), wuXing: skill.Entry.WuXing);
                }),

            new("森罗万象", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"消耗\n奇偶同时激活两个效果"), wuXing: WuXing.Jin,
                skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("森罗万象");
                }),

            new("恋花", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{4 + 2 * dj}攻 吸血"), wuXing: WuXing.Shui,
                skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(4 + 2 * skill.Dj, lifeSteal: true, wuXing: skill.Entry.WuXing);
                }),

            new("冰弹", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{3 + 2 * dj}攻\n格挡+1"), wuXing: WuXing.Shui,
                manaCostEvaluator: 2, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.BuffSelfProcedure("格挡");
                }),

            new("满招损", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{5 + dj}攻\n对方有灵气：{3 + dj}攻"),
                wuXing: WuXing.Shui, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().GetStackOfBuff("灵气") > 0 || await caster.IsFocused();
                    int add = cond ? 3 + skill.Dj : 0;
                    await caster.AttackProcedure(5 + skill.Dj + add, wuXing: skill.Entry.WuXing);
                }),

            new("清泉", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{2 + dj}"), wuXing: WuXing.Shui,
                skillTypeComposite: SkillType.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 2 + skill.Dj);
                }),

            new("归意", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{10 + 2 * dj}攻\n终结：吸血"), wuXing: WuXing.Shui,
                manaCostEvaluator: 1, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10 + 2 * skill.Dj, lifeSteal: skill.IsEnd || await caster.IsFocused(),
                        wuXing: skill.Entry.WuXing);
                }),

            new("吐纳", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"灵气+{3 + dj}\n生命上限+{8 + 4 * dj}"),
                wuXing: WuXing.Shui, skillTypeComposite: SkillType.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 3 + skill.Dj);
                    // await Procedure
                    caster.MaxHp += 8 + 4 * skill.Dj;
                }),

            new("冰雨", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{10 + 2 * dj}攻\n击伤：格挡+1"),
                wuXing: WuXing.Shui, manaCostEvaluator: 2, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10 + 2 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: d => caster.BuffSelfProcedure("格挡"));
                }),

            new("勤能补拙", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"护甲+{10 + 4 * dj}\n初次：遭受1跳回合"),
                wuXing: WuXing.Shui,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(10 + 4 * skill.Dj);

                    if (!skill.IsFirstTime || await caster.IsFocused())
                    {

                    }
                    else
                        await caster.BuffSelfProcedure("跳回合");
                }),

            new("庄周梦蝶", new CLLibrary.Range(2, 5), "消耗\n永久吸血，直到使用非攻击牌", wuXing: WuXing.Shui,
                skillTypeComposite: SkillType.XiaoHao,
                manaCostEvaluator: new ManaCostEvaluator((j, dj, jiaShi) => 3 - dj),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("永久吸血");
                }),

            new("秋水", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{12 + 2 * dj}攻\n消耗1锋锐：吸血\n充沛：翻倍"),
                wuXing: WuXing.Shui, manaCostEvaluator: 1, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool useFocus = !(caster.GetMana() > 0 && caster.GetStackOfBuff("锋锐") > 0);
                    bool focus = useFocus && await caster.IsFocused();
                    int d = focus || await caster.TryConsumeProcedure("灵气") ? 2 : 1;
                    await caster.AttackProcedure((12 + 2 * skill.Dj) * d, lifeSteal: focus || await caster.TryConsumeProcedure("锋锐"),
                        wuXing: skill.Entry.WuXing);
                }),

            new("玄冰刺", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"{16 + 8 * dj}攻\n每造成{8 - dj}点伤害，格挡+1"), wuXing: WuXing.Shui, manaCostEvaluator: 4,
                skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(16 + 8 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: d => caster.BuffSelfProcedure("格挡", d.Value / (8 - skill.Dj)));
                }),

            new("腾跃", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"{6 + 3 * dj}攻\n二动\n第1 ~ {1 + dj}次：遭受1跳卡牌"), wuXing: WuXing.Shui,
                skillTypeComposite: SkillType.Attack | SkillType.ErDong,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(6 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                    caster.Swift = true;
                    if (skill.RunUsedTimes <= skill.Dj)
                        await caster.BuffSelfProcedure("跳卡牌");
                }),

            // new("治疗转灵气", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"消耗\n受到治疗时：灵气+{1 + dj}"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
            //     execute: async (caster, skill, recursive) =>
            //     {
            //         await caster.BuffSelfProcedure("治疗转灵气", 1 + skill.Dj);
            //     }),

            new("透骨严寒", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n消耗所有灵气，每3：格挡+1"),
                wuXing: WuXing.Shui, skillTypeComposite: SkillType.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + skill.Dj);
                    int value = caster.GetStackOfBuff("灵气") / 3;

                    if (value > 0)
                    {
                        await caster.DispelSelfProcedure("灵气", value * 3, true, true);
                        await caster.BuffSelfProcedure("格挡", value);
                    }
                }),

            new("观棋烂柯", new CLLibrary.Range(3, 5), "施加1跳回合", wuXing: WuXing.Shui, manaCostEvaluator: new ManaCostEvaluator((j, dj, jiaShi) => 1 - dj),
                execute: async (caster, skill, recursive) =>
                    await caster.BuffOppoProcedure("跳回合")),

            new("激流", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"生命+{5 + 5 * dj}\n下一次使用牌时二动"),
                wuXing: WuXing.Shui, manaCostEvaluator: new ManaCostEvaluator((j, dj, jiaShi) => 1 - dj),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.HealProcedure(5 + 5 * skill.Dj);
                    await caster.BuffSelfProcedure("二动");
                }),

            new("气吞山河", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"将灵气补至本局最大值+{1 + dj}"), wuXing: WuXing.Shui,
                skillTypeComposite: SkillType.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    int space = caster.HighestManaRecord - caster.GetMana() + 1 + skill.Dj;
                    await caster.BuffSelfProcedure("灵气", space);
                }),

            new("吞天", new CLLibrary.Range(4, 5), "消耗\n10攻 暴击 吸血", wuXing: WuXing.Shui, manaCostEvaluator: 2,
                skillTypeComposite: SkillType.Attack | SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.AttackProcedure(10, crit: true, lifeSteal: true, wuXing: skill.Entry.WuXing);
                }),

            new("玄武吐息法", new CLLibrary.Range(4, 5), "消耗\n治疗可以穿上限", wuXing: WuXing.Shui, manaCostEvaluator: 2,
                skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("玄武吐息法");
                }),

            new("千里神行符", new CLLibrary.Range(4, 5), "消耗\n灵气+4\n二动\n", wuXing: WuXing.Shui,
                skillTypeComposite: SkillType.LingQi | SkillType.XiaoHao | SkillType.ErDong,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("灵气", 4);
                    caster.Swift = true;
                }),

            new("不动明王咒", new CLLibrary.Range(4, 5), "消耗\n格挡翻倍\n无法二动", wuXing: WuXing.Shui, manaCostEvaluator: 3,
                skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("格挡", caster.GetStackOfBuff("格挡"));
                    await caster.BuffSelfProcedure("永久缠绕");
                }),

            new("奔腾", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"二动\n充沛：三动"), wuXing: WuXing.Shui,
                skillTypeComposite: SkillType.ErDong,
                manaCostEvaluator: 1,
                execute: async (caster, skill, recursive) =>
                {
                    caster.Swift = true;
                    if (await caster.TryConsumeProcedure("灵气") || await caster.IsFocused())
                        caster.UltraSwift = true;
                }),

            new("若竹", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n穿透+1"), wuXing: WuXing.Mu,
                skillTypeComposite: SkillType.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + skill.Dj);
                    await caster.BuffSelfProcedure("穿透", 1);
                }),

            new("突刺", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{6 + 2 * dj}攻 穿透"), wuXing: WuXing.Mu,
                manaCostEvaluator: 1, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(6 + 2 * skill.Dj, pierce: true, wuXing: skill.Entry.WuXing);
                }),

            new("花舞", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"力量+{1 + (dj / 2)}\n{2 + 2 * dj}攻"), wuXing: WuXing.Mu,
                skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("力量", 1 + skill.Dj / 2);
                    await caster.AttackProcedure(2 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                }),

            new("治愈", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n生命+{3 + dj}"),
                wuXing: WuXing.Mu, skillTypeComposite: SkillType.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + skill.Dj);
                    await caster.HealProcedure(3 + skill.Dj);
                }),

            new("潜龙在渊", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"生命+{6 + 4 * dj}\n初次：闪避+1"),
                wuXing: WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.HealProcedure(6 + 4 * skill.Dj);
                    if (skill.IsFirstTime || await caster.IsFocused())
                        await caster.BuffSelfProcedure("闪避");
                }),

            new("早春", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"力量+{1 + (dj / 2)}\n护甲+{6 + dj}\n初次：翻倍"), wuXing: WuXing.Mu,
                manaCostEvaluator: 1,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = skill.IsFirstTime || await caster.IsFocused();
                    int mul = cond ? 2 : 1;
                    await caster.BuffSelfProcedure("力量", (1 + (skill.Dj / 2)) * mul);
                    await caster.ArmorGainSelfProcedure((6 + skill.Dj) * mul);
                }),

            new("身骑白马", new CLLibrary.Range(1, 5),
                new SkillDescription((j, dj) => $"第{3 + dj}+次使用：{(6 + 2 * dj) * (6 + 2 * dj)}攻 穿透"), wuXing: WuXing.Mu,
                manaCostEvaluator: 1, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    if (skill.StageUsedTimes < 2 + skill.Dj)
                        return;
                    await caster.AttackProcedure((6 + 2 * skill.Dj) * (6 + 2 * skill.Dj), pierce: true,
                        wuXing: skill.Entry.WuXing);
                }),

            new("回马枪", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"下次受攻击时：{12 + 4 * dj}攻"), wuXing: WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("回马枪", 12 + 4 * skill.Dj);
                }),

            new("千年笋", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{15 + 3 * dj}攻\n消耗1格挡：穿透"),
                wuXing: WuXing.Mu, manaCostEvaluator: 1, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(15 + 3 * skill.Dj, wuXing: skill.Entry.WuXing,
                        pierce: await caster.TryConsumeProcedure("格挡") || await caster.IsFocused());
                }),

            new("见龙在田", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"闪避+{1 + dj / 2}\n如果没有闪避：闪避+{1 + (dj + 1) / 2}"), wuXing: WuXing.Mu,
                manaCostEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.GetStackOfBuff("闪避") == 0 || await caster.IsFocused();
                    int add = cond ? 1 + (skill.Dj + 1) / 2 : 0;
                    await caster.BuffSelfProcedure("闪避", 1 + skill.Dj / 2 + add);
                }),

            new("回春印", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"双方护甲+{10 + 3 * dj}\n双方生命+{10 + 3 * dj}"), wuXing: WuXing.Mu, manaCostEvaluator: 1,
                execute: async (caster, skill, recursive) =>
                {
                    int value = 10 + 3 * skill.Dj;
                    await caster.ArmorGainSelfProcedure(value);
                    await caster.ArmorGainOppoProcedure(value);
                    await caster.HealProcedure(value);
                    await caster.Opponent().HealProcedure(value);
                }),

            new("一虚一实", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"8攻\n受到{3 + dj}倍力量影响\n未造成伤害：治疗等量数值"), wuXing: WuXing.Mu, manaCostEvaluator: 2,
                skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    int add = caster.GetStackOfBuff("力量") * (2 + skill.Dj);
                    int value = 8 + add;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
                        undamaged: d => caster.HealProcedure(value));
                }),

            new("飞龙在天", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"消耗\n每轮：闪避补至{1 + dj}"), wuXing: WuXing.Mu,
                manaCostEvaluator: 2, skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("自动闪避", 1 + skill.Dj);
                }),

            new("凝神", new CLLibrary.Range(3, 5),
                new SkillDescription((j, dj) => $"护甲+{5 + 5 * dj}\n下{1 + dj}次受到治疗：护甲+治疗量"), wuXing: WuXing.Mu, manaCostEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(5 + 5 * skill.Dj);
                    await caster.BuffSelfProcedure("凝神", 1 + skill.Dj);
                }),

            new("摩利支天咒", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"吟唱{1 + dj}\n力量+{(2 + dj) * (2 + dj)}"),
                wuXing: WuXing.Mu, manaCostEvaluator: 1, channelTimeEvaluator: new ChannelTimeEvaluator((j, dj, jiaShi) => 1 + dj),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("力量", (2 + skill.Dj) * (2 + skill.Dj));
                }),

            new("双发", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"下{1 + dj}张牌使用两次"), wuXing: WuXing.Mu,
                manaCostEvaluator: 1,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("双发", 1 + skill.Dj);
                }),

            new("心斋", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"消耗\n所有耗蓝-1"), wuXing: WuXing.Mu,
                manaCostEvaluator: new ManaCostEvaluator((j, dj, jiaShi) => 2 - dj), skillTypeComposite: SkillType.LingQi | SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("心斋");
                }),

            new("亢龙有悔", new CLLibrary.Range(4, 5), "消耗\n10攻x3 穿透\n闪避+3 力量+3\n消耗3灵气：无需消耗", wuXing: WuXing.Mu,
                skillTypeComposite: SkillType.Attack | SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, times: 3, pierce: true, wuXing: skill.Entry.WuXing);
                    await caster.BuffSelfProcedure("闪避", 3);
                    await caster.BuffSelfProcedure("力量", 3);
                    bool cond = await caster.TryConsumeProcedure("灵气", 3) || await caster.IsFocused();
                    if (!cond)
                        await skill.ExhaustProcedure();
                }),

            new("盛开", new CLLibrary.Range(4, 5), "消耗\n受到治疗时：力量+1", wuXing: WuXing.Mu,
                skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("盛开");
                }),

            // new("通透世界", new CLLibrary.Range(4, 5), "消耗\n攻击具有穿透", WuXing.Mu, manaCost: 1,
            //     execute: async (caster, skill, recursive) =>
            //     {
            //         await skill.ConsumeProcedure();
            //         await caster.BuffSelfProcedure("通透世界");
            //     }),

            new("回响", new CLLibrary.Range(4, 5), "使用第一张牌", wuXing: WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    if (!recursive)
                        return;
                    await caster._skills[0].Execute(caster, false);
                }),

            new("鹤回翔", new CLLibrary.Range(4, 5), "消耗\n反转出牌顺序", wuXing: WuXing.Mu,
                skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    if (caster.Forward)
                        await caster.BuffSelfProcedure("鹤回翔");
                    else
                        await caster.TryRemoveBuff("鹤回翔");
                }),

            new("火墙", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{2 + dj}攻x2\n护甲+{3 + dj}"),
                wuXing: WuXing.Huo, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2 + skill.Dj, wuXing: skill.Entry.WuXing, 2);
                    await caster.ArmorGainSelfProcedure(3 + skill.Dj);
                }),

            new("化焰", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{4 + 2 * dj}攻\n灼烧+1"), wuXing: WuXing.Huo,
                manaCostEvaluator: 1, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(4 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.BuffSelfProcedure("灼烧");
                }),

            new("吐焰", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"消耗{2 + dj}生命\n{8 + 3 * dj}攻"),
                wuXing: WuXing.Huo, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.DamageSelfProcedure(2 + skill.Dj);
                    await caster.AttackProcedure(8 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                }),

            new("燃命", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"消耗{3 + dj}生命\n{2 + 3 * dj}攻\n灵气+3"),
                wuXing: WuXing.Huo, skillTypeComposite: SkillType.LingQi | SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.DamageSelfProcedure(3 + skill.Dj);
                    await caster.AttackProcedure(2 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.BuffSelfProcedure("灵气", 3);
                }),

            new("火蛇", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"2攻x{3 + dj}"), wuXing: WuXing.Huo, manaCostEvaluator:1,
                skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2, times: 3 + skill.Dj, wuXing: skill.Entry.WuXing);
                }),

            new("聚火", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"消耗\n灼烧+{2 + dj}"), wuXing: WuXing.Huo,
                manaCostEvaluator: 1, skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("灼烧", 2 + skill.Dj);
                }),

            new("一切皆苦", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"消耗\n唯一灵气牌（无法使用集中）：回合开始时：灵气+1"), wuXing: WuXing.Huo,
                manaCostEvaluator: new ManaCostEvaluator((j, dj, jiaShi) => 3 - dj), skillTypeComposite: SkillType.LingQi | SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    if (skill.NoOtherLingQi)
                        await caster.BuffSelfProcedure("自动灵气");
                }),

            new("常夏", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{4 + dj}攻\n每相邻1张火，多{4 + dj}攻"),
                wuXing: WuXing.Huo, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    int mul = 1;
                    mul += skill.Prev(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    mul += skill.Next(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    await caster.AttackProcedure((4 + skill.Dj) * mul, wuXing: skill.Entry.WuXing);
                }),

            new("天衣无缝", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n若无攻击牌（无法使用集中）：回合开始时：{6 + 2 * dj}攻"),
                wuXing: WuXing.Huo, manaCostEvaluator: 4, skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    if (skill.NoOtherAttack)
                        await caster.BuffSelfProcedure("天衣无缝", 6 + 2 * skill.Dj);
                }),

            new("化劲", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗5生命\n灼烧+{2 + dj}\n消耗1力量：翻倍"),
                wuXing: WuXing.Huo,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.DamageSelfProcedure(5);
                    bool cond = await caster.TryConsumeProcedure("力量") || await caster.IsFocused();
                    int d = cond ? 2 : 1;
                    await caster.BuffSelfProcedure("灼烧", (2 + skill.Dj) * d);
                }),

            new("业火", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n消耗牌时：使用2次"), wuXing: WuXing.Huo,
                manaCostEvaluator: new ManaCostEvaluator((j, dj, jiaShi) => 4 - dj), skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("业火");
                }),

            new("淬体", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n消耗生命时：灼烧+1"), wuXing: WuXing.Huo,
                manaCostEvaluator: new ManaCostEvaluator((j, dj, jiaShi) => 5 - dj), skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("淬体");
                }),

            new("燃灯留烬", new CLLibrary.Range(3, 5),
                new SkillDescription((j, dj) => $"护甲+{6 + 2 * dj}\n每1被消耗卡：多{6 + 2 * dj}"), wuXing: WuXing.Huo, manaCostEvaluator: 1,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure((caster.ExhaustedCount + 1) * (6 + 2 * skill.Dj));
                }),

            new("抱元守一", new CLLibrary.Range(3, 5),
                new SkillDescription((j, dj) => $"消耗\n回合开始时：消耗{3 + 3 * dj}生命，护甲+{3 + 3 * dj}"), wuXing: WuXing.Huo,
                skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("抱元守一", 3 + 3 * skill.Dj);
                }),

            new("天女散花", new CLLibrary.Range(4, 5), "1攻 本局对战中，每获得过1闪避，多攻击1次", wuXing: WuXing.Huo,
                skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1, times: caster.GainedEvadeRecord + 1, wuXing: skill.Entry.WuXing);
                }),

            new("凤凰涅槃", new CLLibrary.Range(4, 5), "消耗\n累计获得20灼烧激活\n每轮：生命回满", wuXing: WuXing.Huo,
                skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("待激活的凤凰涅槃");
                }),

            new("净天地", new CLLibrary.Range(4, 5), "下1张非攻击卡不消耗灵气，使用之后消耗", wuXing: WuXing.Huo,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("净天地");
                }),

            new("落石", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{12 + 4 * dj}攻"), wuXing: WuXing.Tu,
                manaCostEvaluator: 2, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(12 + 4 * skill.Dj, wuXing: skill.Entry.WuXing);
                }),

            new("流沙", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{3 + dj}攻\n灵气+{1 + dj}"), wuXing: WuXing.Tu,
                skillTypeComposite: SkillType.LingQi | SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + skill.Dj);
                    await caster.BuffSelfProcedure("灵气", 1 + skill.Dj);
                }),

            new("土墙", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n护甲+{3 + dj}"),
                wuXing: WuXing.Tu, skillTypeComposite: SkillType.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + skill.Dj);
                    await caster.ArmorGainSelfProcedure(3 + skill.Dj);
                }),

            new("地龙", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{7 + 2 * dj}攻\n击伤：护甲+{7 + 2 * dj}"),
                wuXing: WuXing.Tu, manaCostEvaluator: 1, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    int value = 7 + 2 * skill.Dj;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
                        damaged: d => caster.ArmorGainSelfProcedure(value));
                }),

            new("利剑", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{6 + 4 * dj}攻\n击伤：对方减少1灵气"),
                wuXing: WuXing.Tu, manaCostEvaluator: 1, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(6 + 4 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: async d => await d.Tgt.TryConsumeProcedure("灵气"));
                }),

            new("铁骨", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"消耗\n自动护甲+{1 + dj}"), wuXing: WuXing.Tu,
                skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("自动护甲", 1 + skill.Dj);
                }),

            new("巩固", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"灵气+{2 + 2 * dj}\n每2灵气，护甲+1"), wuXing: WuXing.Tu,
                skillTypeComposite: SkillType.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 2 + 2 * skill.Dj);
                    await caster.ArmorGainSelfProcedure(caster.GetMana() / 2);
                }),

            new("软剑", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{9 + 4 * dj}攻\n击伤：施加1缠绕"), wuXing: WuXing.Tu,
                manaCostEvaluator: 1, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(9 + 4 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: async d => await caster.BuffOppoProcedure("缠绕"));
                }),

            new("一力降十会", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"6攻\n唯一攻击牌（无法使用集中）：{6 + dj}倍"), wuXing: WuXing.Tu,
                manaCostEvaluator: 3, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    int d = skill.NoOtherAttack ? (6 + skill.Dj) : 1;
                    await caster.AttackProcedure(6 * d, wuXing: skill.Entry.WuXing);
                }),

            new("磐石剑阵", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"吟唱1\n护甲+{20 + 6 * dj}\n架势：无需吟唱"), wuXing: WuXing.Tu,
                channelTimeEvaluator: new ChannelTimeEvaluator((j, dj, jiaShi) => jiaShi ? 0 : 1),
                skillTypeComposite: SkillType.JianZhen,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(20 + 6 * skill.Dj);
                }),

            new("少阳", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n获得护甲：额外+{3 + 2 * dj}"),
                wuXing: WuXing.Tu, skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("少阳", 3 + 2 * skill.Dj);
                }),

            new("高速剑阵", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"灵气+{4 + dj}\n架势：二动"), wuXing: WuXing.Tu,
                skillTypeComposite: SkillType.JianZhen | SkillType.LingQi | SkillType.ErDong,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 4 + skill.Dj);
                    caster.Swift |= skill.JiaShi || await caster.IsFocused();
                }),

            new("收刀", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"下回合护甲+{8 + 4 * dj}\n上张牌激活架势"),
                wuXing: WuXing.Tu,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("延迟护甲", 8 + 4 * skill.Dj);
                }),

            new("重剑", new CLLibrary.Range(3, 5),
                new SkillDescription((j, dj) => $"吟唱2\n{22 + 8 * dj}攻\n击伤：护甲+击伤值\n架势：无需吟唱"), wuXing: WuXing.Tu,
                manaCostEvaluator: 2,
                channelTimeEvaluator: new ChannelTimeEvaluator((j, dj, jiaShi) => jiaShi ? 0 : 2),
                skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(22 + 8 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: d => caster.ArmorGainSelfProcedure(d.Value));
                }),

            new("金刚剑阵", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"1攻 每有一点护甲多1攻"), wuXing: WuXing.Tu,
                manaCostEvaluator: new ManaCostEvaluator((j, dj, jiaShi) => 3 - 2 * dj),
                skillTypeComposite: SkillType.Attack | SkillType.JianZhen,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1 + Mathf.Max(0, caster.Armor), wuXing: skill.Entry.WuXing);
                }),

            new("铁布衫", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"护甲+{10 + 4 * dj}\n若无护甲：翻倍"),
                wuXing: WuXing.Tu,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.Armor <= 0 || await caster.IsFocused();
                    int d = cond ? 2 : 1;
                    await caster.ArmorGainSelfProcedure((10 + 4 * skill.Dj) * d);
                }),

            new("拔刀", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"{10 + 5 * dj}攻\n下张牌激活架势"), wuXing: WuXing.Tu,
                skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10 + 5 * skill.Dj, wuXing: skill.Entry.WuXing);
                }),

            new("天人合一", new CLLibrary.Range(3, 5), "消耗\n激活所有架势", wuXing: WuXing.Tu, manaCostEvaluator: new ManaCostEvaluator((j, dj, jiaShi) => 5 - 2 * dj),
                skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("天人合一");
                }),

            new("木剑", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"18攻\n架势：暴击"), wuXing: WuXing.Tu, manaCostEvaluator: 1,
                skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(18, wuXing: skill.Entry.WuXing, crit: skill.JiaShi || await caster.IsFocused());
                }),

            new("金钟罩", new CLLibrary.Range(4, 5), "消耗\n护甲+20\n充沛：翻倍", wuXing: WuXing.Tu, manaCostEvaluator: 1,
                skillTypeComposite: SkillType.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    bool cond = await caster.TryConsumeProcedure("灵气") || await caster.IsFocused();
                    int d = cond ? 2 : 1;
                    await caster.ArmorGainSelfProcedure(20 * d);
                }),

            new("汇聚", new CLLibrary.Range(4, 5), "灵气+3\n如果有（无法使用集中）：锋锐，格挡，闪避，力量，灼烧，则层数+2", wuXing: WuXing.Tu,
                skillTypeComposite: SkillType.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 3);

                    BuffEntry[] buffs = new BuffEntry[] { "锋锐", "格挡", "闪避", "力量", "灼烧" };

                    foreach (BuffEntry b in buffs)
                    {
                        if (caster.GetStackOfBuff(b) > 0)
                            await caster.BuffSelfProcedure(b, 2);
                    }
                }),

            // new("无极剑阵", new CLLibrary.Range(4 , 5), "消耗\n使用之前3张剑阵牌", WuXing.Tu, 6, SkillTypeCollection.JianZhen,
            //     execute: async (caster, skill, recursive) =>
            //     {
            //         if (recursive == false)
            //             return;
            //
            //         await skill.ConsumeProcedure();
            //         await skill.Prevs(false)
            //             .FilterObj(wg => wg.GetskillType().Contains(SkillTypeCollection.JianZhen) && wg.GetName() != "无极剑阵")
            //             .FirstN(3)
            //             .Reverse()
            //             .Do(wg => wg.Execute(caster, false));
            //     }),

            // 莲花
            new("凶水：三步", 5, "10攻 击伤：对方剩余生命每有2点，施加1减甲", wuXing: WuXing.Jin,
                skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            int value = d.Tgt.Hp / 2;
                            await d.Src.ArmorLoseOppoProcedure(value);
                        });
                }),

            new("缠枝：周天结", 5, "消耗6格挡\n消耗\n灵气消耗后加回", wuXing: WuXing.Shui,
                skillTypeComposite: SkillType.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),

            new("烬焰：须菩提", 5, "下一张牌使用之后消耗，第六次使用时消耗", wuXing: WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),

            new("轰炎：焚天", 5, "消耗，自己的所有攻击具有穿透", wuXing: WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),

            new("狂火：钟声", 5, "消耗，永久三动，三回合后死亡", wuXing: WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),

            new("霸王鼎：离别", 5, "消耗，永久不屈", wuXing: WuXing.Huo,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),

            new("庚金：万千辉", 5, "无效化敌人下一次攻击，并且反击", wuXing: WuXing.Tu, skillTypeComposite: SkillType.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("看破");
                }),

            new("返虚土", 5, "消耗", wuXing: WuXing.Tu,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),

            #region 事件牌

            new("一念", new Range(1, 5), new SkillDescription((j, dj) => ($"消耗{8 - dj}生命\n") + (j <= JingJie.ZhuJi ? "二动" : "三动")),
                skillTypeComposite: SkillType.ErDong, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.LoseHealthProcedure(8 - skill.Dj);
                    if (skill.GetJingJie() <= JingJie.ZhuJi)
                        caster.Swift = true;
                    else
                        caster.UltraSwift = true;
                }),

            new("无量劫", new Range(1, 5), new SkillDescription((j, dj) => $"吟唱3\n治疗{18 + dj * 6}"),
                withinPool: false, channelTimeEvaluator: 3,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.HealProcedure(18 + skill.Dj * 6);
                }),

            new("百草集", new Range(3, 5), new SkillDescription((j, dj) => $"如果存在锋锐，格挡，力量，闪避，灼烧，层数+{1 + dj}"), manaCostEvaluator: 3,
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    BuffEntry[] buffEntries = new BuffEntry[] { "锋锐", "格挡", "力量", "闪避", "灼烧", };

                    foreach (BuffEntry buffEntry in buffEntries)
                    {
                        Buff b = caster.FindBuff(buffEntry);
                        if (b != null)
                            await caster.BuffSelfProcedure(b.GetEntry(), 1 + skill.Dj);
                    }
                }),

            new("遗憾", new Range(2, 5), new SkillDescription((j, dj) => $"对手失去{3 + dj}灵气"),
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.DispelOppoProcedure("灵气", 3 + skill.Dj, false);
                }),

            new("爱恋", new Range(2, 5), new SkillDescription((j, dj) => $"获得{2 + dj}集中"),
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("集中", 2 + skill.Dj, false);
                }),

            new("射金乌", new Range(2, 5), new SkillDescription((j, dj) => $"5攻x{4 + 2 * dj}"), manaCostEvaluator: 6,
                withinPool: false, skillTypeComposite: SkillType.Attack, wuXing: WuXing.Huo,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(5, times: 4 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                }),

            new("春雨", new Range(2, 5), new SkillDescription((j, dj) => $"双方下{1 + dj}次治疗时，效果变为1.5倍"),
                withinPool: false, wuXing: WuXing.Shui,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("春雨", 1 + skill.Dj);
                    await caster.BuffOppoProcedure("春雨", 1 + skill.Dj);
                }),

            new("枯木", new Range(2, 5), new SkillDescription((j, dj) => $"双方回合结束时，受到{2 + dj}减甲"),
                withinPool: false, wuXing: WuXing.Jin,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("枯木", 2 + skill.Dj);
                    await caster.BuffOppoProcedure("枯木", 2 + skill.Dj);
                }),

            #endregion

            #region 机关牌

            // 筑基
            new("醒神香", JingJie.ZhuJi, "灵气+4", // 香
                skillTypeComposite: SkillType.LingQi | SkillType.SunHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 4);
                }),

            new("飞镖", JingJie.ZhuJi, "12攻", // 刃
                skillTypeComposite: SkillType.Attack | SkillType.SunHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(12);
                }),

            new("铁匣", JingJie.ZhuJi, "护甲+12", // 匣
                skillTypeComposite: SkillType.SunHao,
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 4);
                }),

            new("滑索", JingJie.ZhuJi, "三动 消耗", // 轮
                skillTypeComposite: SkillType.ErDong | SkillType.XiaoHao | SkillType.SunHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    caster.UltraSwift = true;
                    await skill.ExhaustProcedure();
                }),

            // 元婴
            new("还魂香", JingJie.YuanYing, "灵气+8", // 香香
                skillTypeComposite: SkillType.LingQi | SkillType.SunHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 8);
                }),

            new("净魂刀", JingJie.YuanYing, "10攻\n击伤：灵气+1，对手灵气-1", // 香刃
                skillTypeComposite: SkillType.Attack | SkillType.LingQi | SkillType.SunHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10,
                        damaged: async d =>
                        {
                            await caster.BuffSelfProcedure("灵气");
                            await caster.Opponent().TryConsumeProcedure("灵气", friendly: false);
                        });
                }),

            new("防护罩", JingJie.YuanYing, "护甲+8\n每有1灵气，护甲+4", // 香匣
                skillTypeComposite: SkillType.SunHao,
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    int add = caster.GetStackOfBuff("灵气");
                    await caster.ArmorGainSelfProcedure(8 + add);
                }),

            new("能量饮料", JingJie.YuanYing, "下1次灵气减少时，加回", // 香轮
                skillTypeComposite: SkillType.LingQi | SkillType.SunHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气回收");
                }),

            new("炎铳", JingJie.YuanYing, "25攻", // 刃刃
                skillTypeComposite: SkillType.Attack | SkillType.SunHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(25);
                }),

            new("机关人偶", JingJie.YuanYing, "10攻\n护甲+12", // 刃匣
                skillTypeComposite: SkillType.Attack | SkillType.SunHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(12);
                    await caster.AttackProcedure(10);
                }),

            new("铁陀螺", JingJie.YuanYing, "2攻x6", // 刃轮
                skillTypeComposite: SkillType.Attack | SkillType.SunHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2, times: 6);
                }),

            new("防壁", JingJie.YuanYing, "护甲+20\n自动护甲+2", // 匣匣
                skillTypeComposite: SkillType.SunHao,
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(20);
                    await caster.BuffSelfProcedure("自动护甲", 2);
                }),

            new("不倒翁", JingJie.YuanYing, "下2次护甲减少时，加回", // 匣轮
                skillTypeComposite: SkillType.SunHao,
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("护甲回收", 2);
                }),

            new("助推器", JingJie.YuanYing, "二动 双发", // 轮轮
                skillTypeComposite: SkillType.ErDong | SkillType.SunHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    caster.Swift = true;
                    await caster.BuffSelfProcedure("双发");
                }),

            // 返虚
            new("反应堆", JingJie.FanXu, "消耗\n生命上限设为1，无法受到治疗，永久双发+1", // 香香香
                skillTypeComposite: SkillType.XiaoHao | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    caster.MaxHp = 1;
                    caster.Hp = 1;
                    await caster.BuffSelfProcedure("禁止治疗");
                    await caster.BuffSelfProcedure("永久双发");
                }),

            new("烟花", JingJie.FanXu, "消耗所有灵气，每1，力量+1", // 香香刃
                skillTypeComposite: SkillType.SunHao,
                withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    int stack = caster.GetStackOfBuff("灵气");
                    await caster.TryConsumeProcedure("灵气", stack);
                    await caster.BuffSelfProcedure("力量", stack);
                }),

            new("长明灯", JingJie.FanXu, "消耗\n获得灵气时：每1，生命+3", // 香香匣
                skillTypeComposite: SkillType.XiaoHao | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("长明灯", 3);
                }),

            new("大往生香", JingJie.FanXu, "消耗\n永久免费+1", // 香香轮
                skillTypeComposite: SkillType.XiaoHao | SkillType.LingQi | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("永久免费");
                }),

            new("地府通讯器", JingJie.FanXu, "失去一半生命，每8，灵气+1", // 轮香刃，缺少匣
                skillTypeComposite: SkillType.LingQi | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    int gain = caster.Hp / 16;
                    caster.Hp -= gain * 8;
                    await caster.BuffSelfProcedure("灵气", gain);
                }),

            new("无人机阵列", JingJie.FanXu, "消耗\n永久穿透+1", // 刃刃刃
                skillTypeComposite: SkillType.XiaoHao | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("永久穿透");
                }),

            new("弩炮", JingJie.FanXu, "50攻 吸血", // 刃刃香
                skillTypeComposite: SkillType.Attack | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(50, lifeSteal: true);
                }),

            new("尖刺陷阱", JingJie.FanXu, "下次受到攻击时，对对方施加等量减甲", // 刃刃匣
                skillTypeComposite: SkillType.SunHao,
                withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("尖刺陷阱");
                }),

            new("暴雨梨花针", JingJie.FanXu, "1攻x10", // 刃刃轮
                skillTypeComposite: SkillType.Attack | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1, times: 10);
                }),

            new("炼丹炉", JingJie.FanXu, "消耗\n回合开始时：力量+1", // 香刃匣，缺少轮
                skillTypeComposite: SkillType.XiaoHao | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("回合力量");
                }),

            new("浮空艇", JingJie.FanXu, "消耗\n回合被跳过时，该回合无法受到伤害\n遭受12跳回合", // 匣匣匣
                skillTypeComposite: SkillType.XiaoHao | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("浮空艇");
                    await caster.BuffSelfProcedure("跳回合", 12);
                }),

            new("动量中和器", JingJie.FanXu, "格挡+10", // 匣匣香
                skillTypeComposite: SkillType.SunHao,
                withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("格挡", 10);
                }),

            new("机关伞", JingJie.FanXu, "灼烧+8", // 匣匣刃
                skillTypeComposite: SkillType.SunHao,
                withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灼烧", 8);
                }),

            new("一轮马", JingJie.FanXu, "闪避+6", // 匣匣轮
                skillTypeComposite: SkillType.SunHao,
                withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("闪避", 6);
                }),

            new("外骨骼", JingJie.FanXu, "消耗\n攻击时，护甲+3", // 刃匣轮，缺少香
                skillTypeComposite: SkillType.XiaoHao | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("外骨骼", 3);
                }),

            new("永动机", JingJie.FanXu, "消耗\n力量+8 灵气+8\n8回合后死亡", // 轮轮轮
                skillTypeComposite: SkillType.XiaoHao | SkillType.LingQi | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("力量", 8);
                    await caster.BuffSelfProcedure("灵气", 8);
                    await caster.BuffSelfProcedure("永动机", 8);
                }),

            new("火箭靴", JingJie.FanXu, "消耗\n使用灵气牌时，获得二动", // 轮轮香
                skillTypeComposite: SkillType.XiaoHao | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("火箭靴");
                }),

            new("定龙桩", JingJie.FanXu, "消耗\n对方二动时，如果没有暴击，获得1", // 轮轮刃
                skillTypeComposite: SkillType.XiaoHao | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("定龙桩");
                }),

            new("飞行器", JingJie.FanXu, "消耗\n成功闪避时，如果对方没有跳回合，施加1", // 轮轮匣
                skillTypeComposite: SkillType.XiaoHao | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("飞行器");
                }),

            new("时光机", JingJie.FanXu, "消耗\n使用一张牌前，升级", // 匣轮香，缺少刃
                skillTypeComposite: SkillType.XiaoHao | SkillType.SunHao, withinPool: false,
                channelTimeEvaluator: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("时光机");
                }),

            #endregion
        });
    }

    public void Init()
    {
        List.Do(entry => entry.Generate());
    }

    public override SkillEntry DefaultEntry() => this["不存在的技能"];
}
