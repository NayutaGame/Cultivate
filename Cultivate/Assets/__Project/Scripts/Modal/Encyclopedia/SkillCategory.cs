using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using CLLibrary;
using DG.Tweening;

public class SkillCategory : Category<SkillEntry>
{
    public SkillCategory()
    {
        List = new List<SkillEntry>()
        {
            // new XinfaEntry("龙象吞海决", "水系心法"),
            // new XinfaEntry("魔焰决", "火系心法"),
            // new XinfaEntry("明王决", "金系心法"),
            // new XinfaEntry("玄清天衍录", "通用心法"),
            // new XinfaEntry("大帝轮华经", "通用心法"),
            // new XinfaEntry("自在极意式", "通用心法"),
            // new XinfaEntry("逍遥游", "通用心法"),
            // new NeigongEntry("水雾决", "内功"),
            // new NeigongEntry("冰心决", "内功"),
            // new NeigongEntry("飞云劲", "内功"),
            // new NeigongEntry("春草决", "内功"),

            // new("真金印", "提升吸收到金系灵气的概率"),
            // new("紫微印", "吸收一点水系灵气，【生】额外再吸收一点水系灵气"),
            // new("分金印", "本场战斗中每次触发五行【连击】后，敌方获得【易伤】*1。若上一次释放的是土系技能，则立即触发此效果。"),
            // new("灵光印", "在本回合下一次受到伤害时，获得3点金系灵气"),
            // new("灵体印", "获得【护罩】*6；本回合释放金系技能时，将额外消耗一点金系灵气，并获得【护罩】*1"),
            // new("覆体印", "消散所有金系灵气，每消散一点获得【减伤】*1"),

            // new("生水印", "提升吸收到水系灵气的概率"),
            // new("善水印", "吸收一点木系灵气，【生】额外再吸收一点木系灵气"),
            // new("怒水印", "本场战斗中每次触发五行【连击】后，获得【疗】*1。若上一次释放的是金系技能，则立即触发此效果。"),
            // new("璇水印", "在本回合下一次受到技能伤害时，恢复3点生命值。"),
            // new("罡水印", "下回合造成的水系技能伤害+2"),

            // new("回春印", "提升吸收到木系灵气的概率"),
            // new("上清印", "吸收一点火系灵气，【生】额外再吸收一点火系灵气"),
            // new("灵藤印", "本场战斗中每次触发五行【连击】后，敌方获得【缠绕】*1。若上一次释放的是水系技能，则立即触发此效果。"),
            // new("春丝印", "下一次造成的木系技能伤害+5"),

            // new("聚火印", "提升吸收到火系灵气的概率"),
            // new("火铃印", "吸收一点土系灵气，【生】额外再吸收一点土系灵气"),
            // new("业火印", "本场战斗中每次触发五行【连击】后，敌方获得【灼烧】*1。若上一次释放的是木系技能，则立即触发此效果。"),
            // new("丹阳印", "获得【焰】*2"),
            // new("炙火印", "在本回合下一次受到技能伤害时，使对手获得【灼烧】*2"),

            // new("玄土印", "提升吸收到土系灵气的概率"),
            // new("三山印", "吸收一点金系灵气，【生】额外再吸收一点金系灵气"),

            // new("驱寒印", "本场战斗中每吸收一点灵气，移除自身1层【霜冻】。只能使用一次。"),
            // new("回风印", "直到下回合开始前，每次受到伤害后获得【蓄力】*1"),
            // new("神皇印", "若使用相同的灵气释放，则下回合开始时，自身【蓄势】层数翻倍。否则，本回合【护罩】抵挡的伤害等量转化为【蓄势】。"),

            new("不存在的技能", JingJie.LianQi, "不存在的技能"),

            new("聚气术", JingJie.LianQi, "灵气+1",
                execute: (entity, skill, recursive) =>
                    entity.BuffSelfProcedure("灵气")),

            // new("木30", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗10生命\n{18 + 4 * dj}攻\n每1闪避，多{4 + 2 * dj}攻"), WuXing.Mu, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int value = (4 + 2 * waiGong.Dj) * caster.GetStackOfBuff("闪避");
            //         caster.AttackProcedure(18 + 4 * waiGong.Dj + value);
            //     }),
            //
            // new("木31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"每{4 - dj}格挡，消耗1点，闪避+1"), WuXing.Mu,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int value = caster.GetStackOfBuff("格挡") / (4 - waiGong.Dj);
            //         caster.TryConsumeBuff("格挡", value);
            //         caster.BuffSelfProcedure("闪避", value);
            //     }),
            //
            // new("雷鸣", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗\n二动\n力量+{1 + dj}\n{3 + dj}攻"), WuXing.Mu, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         await waiGong.ConsumeProcedure();
            //         caster.Swift = true;
            //         caster.BuffSelfProcedure("力量", 1 + waiGong.Dj);
            //         caster.AttackProcedure(3 + waiGong.Dj);
            //     }),
            //
            // new("金22", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{8 + 2 * dj}攻\n相邻牌都非攻击：翻倍\n充沛：翻倍"), WuXing.Jin, manaCost: 2, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int d = waiGong.NoAttackAdjacents ? 2 : 1;
            //         d *= caster.TryConsumeMana(2) ? 2 : 1;
            //         caster.AttackProcedure((8 + 3 * waiGong.Dj) * d);
            //     }),
            //
            // new("水23", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"每{6 - dj}锋锐，消耗1点，格挡+1"), WuXing.Shui, 1,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int value = caster.GetStackOfBuff("锋锐") / (6 - waiGong.Dj);
            //         caster.TryConsumeBuff("锋锐", value);
            //         caster.BuffSelfProcedure("格挡", value);
            //     }),
            //
            // new("木20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+2\n每有{6 - dj}点格挡，力量+1"), WuXing.Mu, type: WaiGongType.LingQi,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.BuffSelfProcedure("灵气", 2);
            //         int value = caster.GetStackOfBuff("格挡") / (6 - waiGong.Dj);
            //         caster.BuffSelfProcedure("力量", value);
            //     }),
            //
            // new("火31", new CLLibrary.Range(3, 5), new ChipDescription((l, j,, dj p) => $"消耗所有生命转护甲"), WuXing.Huo, new ManaCost((l, j,, dj p) => 4 - 2 * (j - 3)),
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int value = caster.Hp;
            //         StageManager.Instance.AttackProcedure(caster, caster, value);
            //         caster.ArmorGainSelfProcedure(value);
            //     }),
            //
            // new("金31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗一半护甲，施加等量减甲"), WuXing.Jin, new ManaCost((j, dj) => 6 - j),
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int value = caster.Armor / 2;
            //         caster.ArmorLoseSelfProcedure(value);
            //         caster.ArmorLoseOppoProcedure(value);
            //     }),
            //
            // new("水24", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+{2 + dj}\n满血：翻倍"), WuXing.Shui, type: WaiGongType.LingQi,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int d = caster.IsFullHp ? 2 : 1;
            //         caster.BuffSelfProcedure("灵气", (2 + waiGong.Dj) * d);
            //     }),
            //
            // new("熟能生巧", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"使用消耗不少于2的牌，下一次攻击带有吸血"), WuXing.Mu, manaCost: 2, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //     }),
            //
            // new("夜幕", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"本轮格挡+{3 + dj}"), WuXing.Mu, manaCost: 1,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.BuffSelfProcedure("轮格挡", 3);
            //     }),
            //
            // new("朝孔雀", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"力量+1\n{3 + 3 * dj}攻\n消耗1格挡：翻倍"), WuXing.Mu, manaCost: 1, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int d = caster.TryConsumeBuff("格挡") ? 2 : 1;
            //         caster.BuffSelfProcedure("力量", d);
            //         caster.AttackProcedure((3 + 3 * waiGong.Dj) * d);
            //     }),
            //
            // new("贪狼", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"5攻x{3 + dj}\n不屈+2"), WuXing.Huo, 2, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(5, 3 + waiGong.Dj);
            //         caster.BuffSelfProcedure("不屈", 2);
            //     }),
            //
            // new("飞龙在天", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{18 + 3 * dj}攻\n击伤：施加{2 + dj}缠绕"), WuXing.Shui, 4, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(18 + 3 * waiGong.Dj,
            //             damaged: d => caster.BuffOppoProcedure("缠绕", 2 + waiGong.Dj));
            //     }),
            //
            // new("廉贞", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"对敌方造成伤害时，至少为{4 + 3 * dj}"), WuXing.Huo,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //     }),
            //
            // new("腐朽", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{(dj >= 1 ? "二动\n" : "")}敌方减甲大于生命：斩杀"), WuXing.Jin,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         if (waiGong.Dj >= 1)
            //             caster.Swift = true;
            //
            //         if (-caster.Opponent().Armor >= caster.Opponent().Hp)
            //             caster.Opponent().Hp = 0;
            //     }),
            //
            // new("水41", new CLLibrary.Range(4, 5), "消耗\n本场战斗中，治疗被代替，每有10点，格挡+1\n格挡效果翻倍", WuXing.Shui,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         await waiGong.ConsumeProcedure();
            //         caster.BuffSelfProcedure("强化格挡");
            //     }),
            //
            // new("水42", new CLLibrary.Range(4, 5), "消耗\n本场战斗中，被治疗时，如果实际治疗>=20，二动", WuXing.Shui,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         await waiGong.ConsumeProcedure();
            //         caster.BuffSelfProcedure("治疗转二动");
            //     }),
            //
            // new("土41", new CLLibrary.Range(4, 5), "消耗\n累计获得200护甲：永久暴击", WuXing.Tu,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //     }),
            //
            // new("土42", new CLLibrary.Range(4, 5), new ChipDescription((l, j, dj, p) => $"持续4次，护甲减少时，加回来"), WuXing.Tu,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //     }),
            //
            // new("土20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{10 + 4 * dj}攻\n每1灼烧，多{2 + dj}攻"), WuXing.Tu, 1, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(10 + 4 * waiGong.Dj + (2 + waiGong.Dj) * caster.GetStackOfBuff("灼烧"), wuXing: waiGong.Entry.WuXing);
            //     }),
            //
            // new("木剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{16 + 4 * dj}攻\n架势：暴击"), WuXing.Tu, 0, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(16 + 4 * waiGong.Dj, crit: waiGong.JiaShi, wuXing: waiGong.Entry.WuXing);
            //     }),
            //
            // new("土31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"1攻 每失去过{8 - j}点护甲，多1攻"), WuXing.Tu, 0, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(1 + (8 - caster.LostArmorRecord / waiGong.GetJingJie()), crit: true, wuXing: waiGong.Entry.WuXing);
            //     }),
            //
            // new("土32", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"护甲翻倍"), WuXing.Tu, new ManaCost((j, dj) => 12 - 2 * j),
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.ArmorGainSelfProcedure(caster.Armor);
            //     }),
            //
            // new("软剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{4 + dj * 4}攻\n击伤：二动"), WuXing.Tu, 1, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(4 + 4 * waiGong.Dj, wuXing: waiGong.Entry.WuXing,
            //             damaged: d => caster.Swift = true);
            //     }),

            new("乘风", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{5 + dj}攻\n若有锋锐：{3 + dj}攻"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int add = caster.GetStackOfBuff("锋锐") > 0 ? 3 + waiGong.Dj : 0;
                    await caster.AttackProcedure(5 + waiGong.Dj + add, wuXing: waiGong.Entry.WuXing);
                }),

            new("微风", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"护甲+{3 + dj}\n锋锐+1\n初次：翻倍"), WuXing.Jin,
                execute: async (caster, waiGong, recursive) =>
                {
                    int mul = waiGong.IsFirstTime ? 2 : 1;
                    await caster.ArmorGainSelfProcedure((3 + waiGong.Dj) * mul);
                    await caster.BuffSelfProcedure("锋锐", mul);
                }),

            new("金刃", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{3 + dj}攻\n施加{3 + dj}减甲"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(3 + waiGong.Dj, wuXing: waiGong.Entry.WuXing);
                    await caster.ArmorLoseOppoProcedure(3 + waiGong.Dj);
                }),

            new("贪狼", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"奇偶：{5 + 2 * dj}攻/护甲+{5 + 2 * dj}"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int value = 5 + 2 * waiGong.Dj;
                    if (waiGong.IsOdd)
                        await caster.AttackProcedure(value, wuXing: waiGong.Entry.WuXing);
                    if (waiGong.IsEven)
                        await caster.ArmorGainSelfProcedure(value);
                }),

            new("起", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{4 + dj}攻\n锋锐+{2 + dj}\n初次：翻倍"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int mul = waiGong.IsFirstTime ? 2 : 1;
                    await caster.AttackProcedure((4 + waiGong.Dj) * mul, wuXing: waiGong.Entry.WuXing);
                    await caster.BuffSelfProcedure("锋锐", (2 + waiGong.Dj) * mul);
                }),

            new("金光罩", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"护甲+{4 + 2 * dj}\n施加{4 + 2 * dj}减甲"), WuXing.Jin,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(4 + 2 * waiGong.Dj);
                    await caster.ArmorLoseOppoProcedure(4 + 2 * waiGong.Dj);
                }),

            new("竹剑", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{6 + 2 * dj}攻\n敌方有减甲：多1次"), WuXing.Jin, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int times = caster.Opponent().Armor < 0 ? 2 : 1;
                    await caster.AttackProcedure(6 + 2 * waiGong.Dj, times: times, wuXing: waiGong.Entry.WuXing);
                }),

            new("廉贞", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"奇偶：施加{8 + 2 * dj}减甲\n/护甲+{16 + 2 * dj}"), WuXing.Jin, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (waiGong.IsOdd)
                        await caster.ArmorLoseOppoProcedure(8 + 2 * waiGong.Dj);
                    if (waiGong.IsEven)
                        await caster.ArmorGainSelfProcedure(16 + 2 * waiGong.Dj);
                }),

            new("承", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"锋锐+{2 + dj}\n{9 + dj}攻\n每1锋锐，多1攻"), WuXing.Jin, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("锋锐", 2 + waiGong.Dj);
                    await caster.AttackProcedure(9 + caster.GetStackOfBuff("锋锐"), wuXing: waiGong.Entry.WuXing);
                }),

            new("无常已至", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n本场战斗中\n造成伤害时：施加{5 + 2 * dj}减甲，不高于伤害值"), WuXing.Jin, 4,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("无常已至", 5 + 2 * waiGong.Dj);
                }),

            new("菊剑", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{4 + 4 * dj}攻\n敌方有减甲：二动"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    caster.Swift |= caster.Opponent().Armor < 0;
                    await caster.AttackProcedure(4 + 4 * waiGong.Dj, wuXing: waiGong.Entry.WuXing);
                    caster.Swift |= caster.Opponent().Armor < 0;
                }),

            new("刺穴", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n灵气+{6 + 2 * dj}"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("灵气", 6 + 2 * waiGong.Dj);
                }),

            new("转", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"护甲+6\n每{6 - dj}护甲，锋锐+1"), WuXing.Jin, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(6);
                    int value = caster.Armor / (6 - waiGong.Dj);
                    await caster.BuffSelfProcedure("锋锐", value);
                }),

            new("武曲", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"奇偶：施加{12 + 2 * dj}减甲/护甲+{12 + 2 * dj}\n消耗1锋锐：多{8 + 2 * dj}"), WuXing.Jin, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    int add = caster.TryConsumeBuff("锋锐") ? (8 + 2 * waiGong.Dj) : 0;
                    int value = 12 + 2 * waiGong.Dj + add;
                    if (waiGong.IsOdd)
                        await caster.ArmorLoseOppoProcedure(value);
                    if (waiGong.IsEven)
                        await caster.ArmorGainSelfProcedure(value);
                }),

            new("敛息", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"下{1 + dj}次造成伤害转减甲"), WuXing.Jin, manaCost: 2,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("敛息", 1 + waiGong.Dj);
                }),

            new("破军", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"奇偶：对方灵气-{2 + dj}\n/灵气+{4 + 2 * dj}"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (waiGong.IsOdd)
                    {
                        Buff b = caster.Opponent().FindBuff("灵气");
                        if (b != null)
                            b.Stack -= 2 + waiGong.Dj;
                    }
                    if (waiGong.IsEven)
                    {
                        await caster.BuffSelfProcedure("灵气", 4 + 2 * waiGong.Dj);
                    }
                }),

            new("合", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"奇偶：每1锋锐，施加1减甲/护甲+1"), WuXing.Jin, 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    int value = caster.GetStackOfBuff("锋锐");
                    if (waiGong.IsOdd)
                        await caster.ArmorLoseOppoProcedure(value);
                    if (waiGong.IsEven)
                        await caster.ArmorGainSelfProcedure(value);
                }),

            new("少阴", new CLLibrary.Range(4, 5), "消耗\n施加减甲：额外+3\n消耗少阳：额外层数", WuXing.Jin,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    int value = caster.GetStackOfBuff("少阳") + 3;
                    caster.TryRemoveBuff("少阳");
                    await caster.BuffSelfProcedure("少阴", value);
                }),

            new("梅剑", new CLLibrary.Range(4, 5), "5攻x2\n敌方有减甲：暴击", WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(5, crit: caster.Opponent().Armor < 0, wuXing: waiGong.Entry.WuXing);
                    await caster.AttackProcedure(5, crit: caster.Opponent().Armor < 0, wuXing: waiGong.Entry.WuXing);
                }),

            new("森罗万象", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"消耗\n本场战斗中：奇偶同时激活两个效果"), WuXing.Jin,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("森罗万象");
                }),

            new("恋花", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{4 + 2 * dj}攻 吸血"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(4 + 2 * waiGong.Dj, lifeSteal: true, wuXing: waiGong.Entry.WuXing);
                }),

            new("冰弹", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{3 + 2 * dj}攻\n格挡+1"), WuXing.Shui, manaCost: 2, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(3 + 2 * waiGong.Dj, wuXing: waiGong.Entry.WuXing);
                    await caster.BuffSelfProcedure("格挡");
                }),

            new("满招损", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{5 + dj}攻\n对方有灵气：{3 + dj}攻"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int add = caster.Opponent().GetStackOfBuff("灵气") > 0 ? 3 + waiGong.Dj : 0;
                    await caster.AttackProcedure(5 + waiGong.Dj + add, wuXing: waiGong.Entry.WuXing);
                }),

            new("清泉", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{2 + dj}"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 2 + waiGong.Dj);
                }),

            new("归意", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{10 + 2 * dj}攻\n终结：吸血"), WuXing.Shui, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(10 + 2 * waiGong.Dj, lifeSteal: waiGong.IsEnd, wuXing: waiGong.Entry.WuXing);
                }),

            new("吐纳", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"灵气+{3 + dj}\n生命上限+{8 + 4 * dj}"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 3 + waiGong.Dj);
                    // await Procedure
                    caster.MaxHp += 8 + 4 * waiGong.Dj;
                }),

            new("冰雨", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{10 + 2 * dj}攻\n击伤：格挡+1"), WuXing.Shui, manaCost: 2, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(10 + 2 * waiGong.Dj, wuXing: waiGong.Entry.WuXing,
                        damaged: d => caster.BuffSelfProcedure("格挡"));
                }),

            new("勤能补拙", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"护甲+{10 + 4 * dj}\n初次：遭受1跳回合"), WuXing.Shui,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(10 + 4 * waiGong.Dj);
                    if(waiGong.IsFirstTime)
                        await caster.BuffSelfProcedure("跳回合");
                }),

            new("庄周梦蝶", new CLLibrary.Range(2, 5), "消耗\n永久吸血，直到使用非攻击牌", WuXing.Shui, manaCost: new ManaCost((j, dj) => 3 - dj),
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("永久吸血");
                }),

            new("秋水", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{12 + 2 * dj}攻\n消耗1锋锐：吸血\n充沛：翻倍"), WuXing.Shui, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int d = caster.TryConsumeMana() ? 2 : 1;
                    await caster.AttackProcedure((12 + 2 * waiGong.Dj) * d, lifeSteal: caster.TryConsumeBuff("锋锐"), wuXing: waiGong.Entry.WuXing);
                }),

            new("玄冰刺", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{16 + 8 * dj}攻\n每造成{8 - dj}点伤害，格挡+1"), WuXing.Shui, 4, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(16 + 8 * waiGong.Dj, wuXing: waiGong.Entry.WuXing,
                        damaged: d => caster.BuffSelfProcedure("格挡", d.Value / (8 - waiGong.Dj)));
                }),

            new("腾跃", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{6 + 3 * dj}攻\n二动\n第1 ~ {1 + dj}次：遭受1跳卡牌"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(6 + 3 * waiGong.Dj, wuXing: waiGong.Entry.WuXing);
                    caster.Swift = true;
                    if (waiGong.RunUsedTimes <= waiGong.Dj)
                        await caster.BuffSelfProcedure("跳卡牌");
                }),

            // new("治疗转灵气", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"消耗\n本场战斗中\n受到治疗时：灵气+{1 + dj}"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
            //     execute: async (caster, waiGong, recursive) =>
            //     {
            //         await caster.BuffSelfProcedure("治疗转灵气", 1 + waiGong.Dj);
            //     }),

            new("透骨严寒", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n消耗所有灵气，每3：格挡+1"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + waiGong.Dj);
                    int value = caster.GetStackOfBuff("灵气") / 3;

                    if (value > 0)
                    {
                        caster.TryRemoveBuff("灵气");
                        await caster.BuffSelfProcedure("格挡", value);
                    }
                }),

            new("观棋烂柯", new CLLibrary.Range(3, 5), "施加1跳回合", WuXing.Shui, manaCost: new ManaCost((j, dj) => 1 - dj),
                execute: async (caster, waiGong, recursive) =>
                    await caster.BuffOppoProcedure("跳回合")),

            new("激流", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"生命+{5 + 5 * dj}\n下一次使用牌时二动"), WuXing.Shui, manaCost: new ManaCost((j, dj) => 1 - dj),
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.HealProcedure(5 + 5 * waiGong.Dj);
                    await caster.BuffSelfProcedure("二动");
                }),

            new("气吞山河", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"将灵气补至本局最大值+{1 + dj}"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    int space = caster.HighestManaRecord - caster.GetMana() + 1 + waiGong.Dj;
                    await caster.BuffSelfProcedure("灵气", space);
                }),

            new("吞天", new CLLibrary.Range(4, 5), "消耗\n10攻 暴击 吸血", WuXing.Shui, manaCost: 2, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.AttackProcedure(10, crit: true, lifeSteal: true, wuXing: waiGong.Entry.WuXing);
                }),

            new("玄武吐息法", new CLLibrary.Range(4, 5), "消耗\n本场战斗中：治疗可以穿上限", WuXing.Shui, manaCost: 2,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("玄武吐息法");
                }),

            new("千里神行符", new CLLibrary.Range(4, 5), "消耗\n灵气+4\n二动\n", WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("灵气", 4);
                    caster.Swift = true;
                }),

            new("不动明王咒", new CLLibrary.Range(4, 5), "消耗\n格挡翻倍\n本场战斗中：无法二动", WuXing.Shui, manaCost: 3,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("格挡", caster.GetStackOfBuff("格挡"));
                    await caster.BuffSelfProcedure("不动明王咒");
                }),

            new("奔腾", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"二动\n充沛：三动"), WuXing.Shui, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    caster.Swift = true;
                    if (caster.TryConsumeMana())
                        caster.UltraSwift = true;
                }),

            new("若竹", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n穿透+1"), WuXing.Mu, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + waiGong.Dj);
                    await caster.BuffSelfProcedure("穿透", 1);
                }),

            new("突刺", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{6 + 2 * dj}攻 穿透"), WuXing.Mu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(6 + 2 * waiGong.Dj, pierce: true, wuXing: waiGong.Entry.WuXing);
                }),

            new("花舞", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"护甲+{2 + 2 * dj}\n力量+1"), WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(2 + 2 * waiGong.Dj);
                    await caster.BuffSelfProcedure("力量");
                }),

            new("治愈", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n生命+{3 + dj}"), WuXing.Mu, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + waiGong.Dj);
                    await caster.HealProcedure(3 + waiGong.Dj);
                }),

            new("潜龙在渊", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"生命+{6 + 4 * dj}\n初次：闪避+1"), WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.HealProcedure(6 + 4 * waiGong.Dj);
                    if (waiGong.IsFirstTime)
                        await caster.BuffSelfProcedure("闪避");
                }),

            new("早春", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"力量+1\n{6 + dj}攻\n初次：翻倍"), WuXing.Mu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int mul = waiGong.IsFirstTime ? 2 : 1;
                    await caster.BuffSelfProcedure("力量", mul);
                    await caster.AttackProcedure((6 + waiGong.Dj) * mul, wuXing: waiGong.Entry.WuXing);
                }),

            new("身骑白马", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"第{3 + dj}+次使用：{(6 + 2 * dj) * (6 + 2 * dj)}攻 穿透"), WuXing.Mu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (waiGong.StageUsedTimes < 2 + waiGong.Dj)
                        return;
                    await caster.AttackProcedure((6 + 2 * waiGong.Dj) * (6 + 2 * waiGong.Dj), pierce: true, wuXing: waiGong.Entry.WuXing);
                }),

            new("回马枪", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"下次受攻击时：{12 + 4 * dj}攻"), WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("回马枪", 12 + 4 * waiGong.Dj);
                }),

            new("千年笋", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{15 + 3 * dj}攻\n消耗1格挡：穿透"), WuXing.Mu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(15 + 3 * waiGong.Dj, wuXing: waiGong.Entry.WuXing,
                        pierce: caster.TryConsumeBuff("格挡"));
                }),

            new("见龙在田", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"闪避+{1 + dj / 2}\n如果没有闪避：闪避+{1 + (dj + 1) / 2}"), WuXing.Mu, manaCost: 2,
                execute: async (caster, waiGong, recursive) =>
                {
                    int add = caster.GetStackOfBuff("闪避") == 0 ? 1 + (waiGong.Dj + 1) / 2 : 0;
                    await caster.BuffSelfProcedure("闪避", 1 + waiGong.Dj / 2 + add);
                }),

            new("回春印", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"双方护甲+{10 + 3 * dj}\n双方生命+{10 + 3 * dj}"), WuXing.Mu, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    int value = 10 + 3 * waiGong.Dj;
                    await caster.ArmorGainSelfProcedure(value);
                    await caster.ArmorGainOppoProcedure(value);
                    await caster.HealProcedure(value);
                    await caster.Opponent().HealProcedure(value);
                }),

            new("一虚一实", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"8攻\n受到{3 + dj}倍力量影响\n未造成伤害：治疗等量数值"), WuXing.Mu, manaCost: 2, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int add = caster.GetStackOfBuff("力量") * (2 + waiGong.Dj);
                    int value = 8 + add;
                    await caster.AttackProcedure(value, wuXing: waiGong.Entry.WuXing,
                        undamaged: d => caster.HealProcedure(value));
                }),

            new("飞龙在天", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"消耗\n每轮：闪避补至{1 + dj}"), WuXing.Mu, manaCost: 2,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("自动闪避", 1 + waiGong.Dj);
                }),

            new("凝神", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"护甲+{5 + 5 * dj}\n下{1 + dj}次受到治疗：护甲+治疗量"), WuXing.Mu, manaCost: 2,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(5 + 5 * waiGong.Dj);
                    await caster.BuffSelfProcedure("凝神", 1 + waiGong.Dj);
                }),

            new("摩利支天咒", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"力量+{4 + 5 * dj}\n遭受{1 + dj}跳回合"), WuXing.Mu, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("力量", 4 + 5 * waiGong.Dj);
                    await caster.BuffSelfProcedure("跳回合", 1 + waiGong.Dj);
                }),

            new("双发", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"下{1 + dj}张牌使用两次"), WuXing.Mu, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("双发", 1 + waiGong.Dj);
                }),

            new("心斋", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"消耗\n本场战斗中：所有耗蓝-1"), WuXing.Mu, manaCost: new ManaCost((j, dj) => 2 - dj), skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("心斋");
                }),

            new("亢龙有悔", new CLLibrary.Range(4, 5), "10攻x3 穿透\n闪避+3\n每使用一次效果减弱", WuXing.Mu, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (waiGong.RunUsedTimes >= 3)
                        return;

                    await caster.AttackProcedure(10, times: 3 - waiGong.RunUsedTimes, pierce: true, wuXing: waiGong.Entry.WuXing);
                    await caster.BuffSelfProcedure("闪避", 3 - waiGong.RunUsedTimes);
                }),

            new("盛开", new CLLibrary.Range(4, 5), "消耗\n本场战斗中：\n受到治疗时：力量+1", WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("盛开");
                }),

            // new("通透世界", new CLLibrary.Range(4, 5), "消耗\n本场战斗中：攻击具有穿透", WuXing.Mu, manaCost: 1,
            //     execute: async (caster, waiGong, recursive) =>
            //     {
            //         await waiGong.ConsumeProcedure();
            //         await caster.BuffSelfProcedure("通透世界");
            //     }),

            new("回响", new CLLibrary.Range(4, 5), "使用第一张牌", WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (!recursive)
                        return;
                    await caster._waiGongList[0].Execute(caster, false);
                }),

            new("鹤回翔", new CLLibrary.Range(4, 5), "消耗\n反转出牌顺序", WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (caster.Forward)
                        await caster.BuffSelfProcedure("鹤回翔");
                    else
                        caster.TryRemoveBuff("鹤回翔");
                }),

            new("火墙", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{2 + dj}攻x2\n护甲+{3 + dj}"), WuXing.Huo, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(2 + waiGong.Dj, wuXing: waiGong.Entry.WuXing, 2);
                    await caster.ArmorGainSelfProcedure(3 + waiGong.Dj);
                }),

            new("化焰", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{4 + 2 * dj}攻\n灼烧+1"), WuXing.Huo, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(4 + 2 * waiGong.Dj, wuXing: waiGong.Entry.WuXing);
                    await caster.BuffSelfProcedure("灼烧");
                }),

            new("吐焰", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"消耗{2 + dj}生命\n{8 + 3 * dj}攻"), WuXing.Huo, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.DamageSelfProcedure(2 + waiGong.Dj);
                    await caster.AttackProcedure(8 + 3 * waiGong.Dj, wuXing: waiGong.Entry.WuXing);
                }),

            new("燃命", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"消耗{3 + dj}生命\n{2 + 3 * dj}攻\n灵气+3"), WuXing.Huo, skillTypeCollection: SkillTypeCollection.LingQi | SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.DamageSelfProcedure(3 + waiGong.Dj);
                    await caster.AttackProcedure(2 + 3 * waiGong.Dj, wuXing: waiGong.Entry.WuXing);
                    await caster.BuffSelfProcedure("灵气", 3);
                }),

            new("火蛇", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"2攻x{3 + dj}"), WuXing.Huo, 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(2, times: 3 + waiGong.Dj, wuXing: waiGong.Entry.WuXing);
                }),

            new("聚火", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"消耗\n灼烧+{2 + dj}"), WuXing.Huo, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("灼烧", 2 + waiGong.Dj);
                }),

            new("一切皆苦", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"消耗\n唯一灵气牌：每回合灵气+1"), WuXing.Huo, new ManaCost((j, dj) => 3 - dj), skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    if (waiGong.NoOtherLingQi)
                        await caster.BuffSelfProcedure("自动灵气");
                }),

            new("常夏", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{4 + dj}攻\n每相邻1张火，多{4 + dj}攻"), WuXing.Huo, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int mul = 1;
                    mul += waiGong.Prev(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    mul += waiGong.Next(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    await caster.AttackProcedure((4 + waiGong.Dj) * mul, wuXing: waiGong.Entry.WuXing);
                }),

            new("天衣无缝", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n若无攻击牌，每回合：{6 + 2 * dj}攻"), WuXing.Huo, manaCost: 4,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    if (waiGong.NoOtherAttack)
                        await caster.BuffSelfProcedure("天衣无缝", 6 + 2 * waiGong.Dj);
                }),

            new("化劲", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗5生命\n灼烧+{2 + dj}\n消耗1力量：翻倍"), WuXing.Huo,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.DamageSelfProcedure(5);
                    int d = caster.TryConsumeBuff("力量") ? 2 : 1;
                    await caster.BuffSelfProcedure("灼烧", (2 + waiGong.Dj) * d);
                }),

            new("业火", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n本场战斗中\n消耗牌时：使用2次"), WuXing.Huo, manaCost: new ManaCost((j, dj) => 4 - dj),
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("业火");
                }),

            new("淬体", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n本场战斗中\n消耗生命时：灼烧+1"), WuXing.Huo, manaCost: new ManaCost((j, dj) => 5 - dj),
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("淬体");
                }),

            new("燃灯留烬", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"护甲+{6 + 2 * dj}\n每1被消耗卡：多{6 + 2 * dj}"), WuXing.Huo, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure((caster.ConsumedCount + 1) * (6 + 2 * waiGong.Dj));
                }),

            new("抱元守一", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"每回合：消耗{3 + 3 * dj}生命，护甲+{3 + 3 * dj}"), WuXing.Huo,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("抱元守一", 3 + 3 * waiGong.Dj);
                }),

            new("天女散花", new CLLibrary.Range(4, 5), "1攻 本局对战中，每获得过1闪避，多攻击1次", WuXing.Huo, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(1, times: caster.GainedEvadeRecord + 1, wuXing: waiGong.Entry.WuXing);
                }),

            new("凤凰涅槃", new CLLibrary.Range(4, 5), "消耗\n累计获得20灼烧激活\n每轮：生命回满", WuXing.Huo,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("待激活的凤凰涅槃");
                }),

            new("净天地", new CLLibrary.Range(4, 5), "下1张非攻击卡不消耗灵气，使用之后消耗", WuXing.Huo,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("净天地");
                }),

            new("落石", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{12 + 4 * dj}攻"), WuXing.Tu, manaCost: 2, SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(12 + 4 * waiGong.Dj, wuXing: waiGong.Entry.WuXing);
                }),

            new("流沙", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{3 + dj}攻\n灵气+{1 + dj}"), WuXing.Tu, skillTypeCollection: SkillTypeCollection.LingQi | SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(3 + waiGong.Dj);
                    await caster.BuffSelfProcedure("灵气", 1 + waiGong.Dj);
                }),

            new("土墙", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n护甲+{3 + dj}"), WuXing.Tu, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + waiGong.Dj);
                    await caster.ArmorGainSelfProcedure(3 + waiGong.Dj);
                }),

            new("地龙", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{7 + 2 * dj}攻\n击伤：护甲+{7 + 2 * dj}"), WuXing.Tu, 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int value = 7 + 2 * waiGong.Dj;
                    await caster.AttackProcedure(value, wuXing: waiGong.Entry.WuXing,
                        damaged: d => caster.ArmorGainSelfProcedure(value));
                }),

            new("利剑", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{6 + 4 * dj}攻\n击伤：对方减少1灵气"), WuXing.Tu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(6 + 4 * waiGong.Dj, wuXing: waiGong.Entry.WuXing,
                        damaged: async d => d.Tgt.TryConsumeMana());
                }),

            new("铁骨", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"消耗\n自动护甲+{1 + dj}"), WuXing.Tu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("自动护甲", 1 + waiGong.Dj);
                }),

            new("巩固", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"灵气+{2 + dj}\n每1灵气，护甲+1"), WuXing.Tu, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 2 + waiGong.Dj);
                    await caster.ArmorGainSelfProcedure(caster.GetMana());
                }),

            new("软剑", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{9 + 4 * dj}攻\n击伤：施加1缠绕"), WuXing.Tu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(9 + 4 * waiGong.Dj, wuXing: waiGong.Entry.WuXing,
                        damaged: async d => caster.BuffOppoProcedure("缠绕"));
                }),

            new("一力降十会", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"6攻\n唯一攻击牌：{6 + dj}倍"), WuXing.Tu, manaCost: 3, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int d = waiGong.NoOtherAttack ? (6 + waiGong.Dj) : 1;
                    await caster.AttackProcedure(6 * d, wuXing: waiGong.Entry.WuXing);
                }),

            new("磐石剑阵", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"护甲+{20 + 6 * dj}\n遭受1跳回合\n架势：无需跳回合"), WuXing.Tu, 0, skillTypeCollection: SkillTypeCollection.JianZhen,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(20 + 6 * waiGong.Dj);
                    if (!waiGong.JiaShi)
                        await caster.BuffSelfProcedure("跳回合");
                }),

            new("少阳", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n获得护甲：额外+{3 + 2 * dj}"), WuXing.Tu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    await caster.BuffSelfProcedure("少阳", 3 + 2 * waiGong.Dj);
                }),

            new("高速剑阵", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"灵气+{4 + dj}\n架势：二动"), WuXing.Tu, skillTypeCollection: SkillTypeCollection.JianZhen | SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 4 + waiGong.Dj);
                    caster.Swift |= waiGong.JiaShi;
                }),

            new("收刀", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"下回合护甲+{8 + 4 * dj}\n上张牌激活架势"), WuXing.Tu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("延迟护甲", 8 + 4 * waiGong.Dj);
                }),

            new("重剑", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"{22 + 8 * dj}攻\n击伤：护甲+击伤值\n遭受2跳回合\n架势：无需跳回合"), WuXing.Tu, 2, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(22 + 8 * waiGong.Dj, wuXing: waiGong.Entry.WuXing,
                        damaged: d => caster.ArmorGainSelfProcedure(d.Value));
                    if (!waiGong.JiaShi)
                        await caster.BuffSelfProcedure("跳回合", 2);
                }),

            new("金刚剑阵", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"1攻 每有一点护甲多1攻"), WuXing.Tu, new ManaCost((j, dj) => 3 - 2 * dj), skillTypeCollection: SkillTypeCollection.Attack | SkillTypeCollection.JianZhen,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(1 + Mathf.Max(0, caster.Armor), wuXing: waiGong.Entry.WuXing);
                }),

            new("铁布衫", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"护甲+{10 + 4 * dj}\n若无护甲：翻倍"), WuXing.Tu,
                execute: async (caster, waiGong, recursive) =>
                {
                    int d = caster.Armor <= 0 ? 2 : 1;
                    await caster.ArmorGainSelfProcedure((10 + 4 * waiGong.Dj) * d);
                }),

            new("拔刀", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"{10 + 5 * dj}攻\n下张牌激活架势"), WuXing.Tu, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(10 + 5 * waiGong.Dj, wuXing: waiGong.Entry.WuXing);
                }),

            new("天人合一", new CLLibrary.Range(3, 5), "消耗\n激活所有架势", WuXing.Tu, new ManaCost((j, dj) => 5 - 2 * dj),
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                }),

            new("木剑", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"18攻\n架势：暴击"), WuXing.Tu, 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(18, wuXing: waiGong.Entry.WuXing, crit: waiGong.JiaShi);
                }),

            new("金钟罩", new CLLibrary.Range(4, 5), "消耗\n护甲+20\n充沛：翻倍", WuXing.Tu, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                    int d = caster.TryConsumeMana() ? 2 : 1;
                    await caster.ArmorGainSelfProcedure(20 * d);
                }),

            new("汇聚", new CLLibrary.Range(4, 5), "灵气+3\n如果有：锋锐，格挡，闪避，力量，灼烧，则层数+2", WuXing.Tu, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
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
            //     execute: async (caster, waiGong, recursive) =>
            //     {
            //         if (recursive == false)
            //             return;
            //
            //         await waiGong.ConsumeProcedure();
            //         await waiGong.Prevs(false)
            //             .FilterObj(wg => wg.GetWaiGongType().Contains(SkillTypeCollection.JianZhen) && wg.GetName() != "无极剑阵")
            //             .FirstN(3)
            //             .Reverse()
            //             .Do(wg => wg.Execute(caster, false));
            //     }),

            // 莲花
            new("凶水：三步", 5, "10攻 击伤：对方剩余生命每有2点，施加1减甲", WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: waiGong.Entry.WuXing,
                        damaged: async d =>
                        {
                            int value = d.Tgt.Hp / 2;
                            await d.Src.ArmorLoseOppoProcedure(value);
                        });
                }),

            new("缠枝：周天结", 5, "消耗6格挡\n消耗\n本场战斗中，灵气消耗后加回", WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                }),

            new("烬焰：须菩提", 5, "下一张牌使用之后消耗，第六次使用时消耗", WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                }),

            new("轰炎：焚天", 5, "消耗，本场战斗中，自己的所有攻击具有穿透", WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                }),

            new("狂火：钟声", 5, "消耗，永久三动，三回合后死亡", WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                }),

            new("霸王鼎：离别", 5, "消耗，永久不屈", WuXing.Huo,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                }),

            new("庚金：万千辉", 5, "无效化敌人下一次攻击，并且反击", WuXing.Tu, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("看破");
                }),

            new("返虚土", 5, "消耗", WuXing.Tu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await waiGong.ConsumeProcedure();
                }),
        };
    }

    public void Init()
    {
        List.Do(entry => entry.Generate());
    }

    public override SkillEntry Default() => this["不存在的技能"];
}
