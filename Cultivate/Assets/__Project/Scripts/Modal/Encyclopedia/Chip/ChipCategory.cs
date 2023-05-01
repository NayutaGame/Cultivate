using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using CLLibrary;
using DG.Tweening;

public class ChipCategory : Category<ChipEntry>
{
    public ChipCategory()
    {
        List = new List<ChipEntry>()
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
            // new WaigongEntry("真金印", "提升吸收到金系灵气的概率"),
            // new WaigongEntry("生水印", "提升吸收到水系灵气的概率"),
            // new WaigongEntry("回春印", "提升吸收到木系灵气的概率"),
            // new WaigongEntry("聚火印", "提升吸收到火系灵气的概率"),
            // new WaigongEntry("玄土印", "提升吸收到土系灵气的概率"),
            // new WaigongEntry("紫微印", "吸收一点水系灵气，【生】额外再吸收一点水系灵气"),
            // new WaigongEntry("善水印", "吸收一点木系灵气，【生】额外再吸收一点木系灵气"),
            // new WaigongEntry("上清印", "吸收一点火系灵气，【生】额外再吸收一点火系灵气"),
            // new WaigongEntry("火铃印", "吸收一点土系灵气，【生】额外再吸收一点土系灵气"),
            // new WaigongEntry("三山印", "吸收一点金系灵气，【生】额外再吸收一点金系灵气"),
            // new WaigongEntry("丹阳印", "获得【焰】*2"),
            // new WaigongEntry("灵光印", "在本回合下一次受到伤害时，获得3点金系灵气"),
            // new WaigongEntry("炙火印", "在本回合下一次受到技能伤害时，使对手获得【灼烧】*2"),
            // new WaigongEntry("璇水印", "在本回合下一次受到技能伤害时，恢复3点生命值。"),
            // new WaigongEntry("罡水印", "下回合造成的水系技能伤害+2"),
            // new WaigongEntry("春丝印", "下一次造成的木系技能伤害+5"),
            // new WaigongEntry("灵体印", "获得【护罩】*6；本回合释放金系技能时，将额外消耗一点金系灵气，并获得【护罩】*1"),
            // new WaigongEntry("分金印", "本场战斗中每次触发五行【连击】后，敌方获得【易伤】*1。若上一次释放的是土系技能，则立即触发此效果。"),
            // new WaigongEntry("驱寒印", "本场战斗中每吸收一点灵气，移除自身1层【霜冻】。只能使用一次。"),
            // new WaigongEntry("业火印", "本场战斗中每次触发五行【连击】后，敌方获得【灼烧】*1。若上一次释放的是木系技能，则立即触发此效果。"),
            // new WaigongEntry("灵藤印", "本场战斗中每次触发五行【连击】后，敌方获得【缠绕】*1。若上一次释放的是水系技能，则立即触发此效果。"),
            // new WaigongEntry("怒水印", "本场战斗中每次触发五行【连击】后，获得【疗】*1。若上一次释放的是金系技能，则立即触发此效果。"),
            // new WaigongEntry("回风印", "直到下回合开始前，每次受到伤害后获得【蓄力】*1"),
            // new WaigongEntry("神皇印", "若使用相同的灵气释放，则下回合开始时，自身【蓄势】层数翻倍。否则，本回合【护罩】抵挡的伤害等量转化为【蓄势】。"),
            // new WaigongEntry("覆体印", "消散所有金系灵气，每消散一点获得【减伤】*1"),

            new WaiGongEntry("聚气术", JingJie.LianQi, "灵气+1",
                execute: (caster, waiGong, recursive) =>
                    caster.BuffSelfProcedure("灵气")),

            new ChipEntry("拆除", JingJie.LianQi, "拆除", null,
                canPlug: (tile, runChip) => tile.AcquiredRunChip != null && tile.AcquiredRunChip.Chip._entry.CanUnplug(tile.AcquiredRunChip),
                plug: (tile, runChip) => tile.AcquiredRunChip.Chip._entry.Unplug(tile.AcquiredRunChip),
                canUnplug: acquiredRunChip => false,
                unplug: acquiredRunChip => { }),












            // new WaiGongEntry("木30", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗10生命\n{18 + 4 * dj}攻\n每1闪避，多{4 + 2 * dj}攻"), WuXing.Mu, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int value = (4 + 2 * waiGong.Dj) * caster.GetStackOfBuff("闪避");
            //         caster.AttackProcedure(18 + 4 * waiGong.Dj + value);
            //     }),
            //
            // new WaiGongEntry("木31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"每{4 - dj}格挡，消耗1点，闪避+1"), WuXing.Mu,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int value = caster.GetStackOfBuff("格挡") / (4 - waiGong.Dj);
            //         caster.TryConsumeBuff("格挡", value);
            //         caster.BuffSelfProcedure("闪避", value);
            //     }),
            //
            // new WaiGongEntry("雷鸣", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗\n二动\n力量+{1 + dj}\n{3 + dj}攻"), WuXing.Mu, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         waiGong.Consumed = true;
            //         caster.Swift = true;
            //         caster.BuffSelfProcedure("力量", 1 + waiGong.Dj);
            //         caster.AttackProcedure(3 + waiGong.Dj);
            //     }),
            //
            // new WaiGongEntry("金22", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{8 + 2 * dj}攻\n相邻牌都非攻击：翻倍\n充沛：翻倍"), WuXing.Jin, manaCost: 2, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int d = waiGong.NoAttackAdjacents ? 2 : 1;
            //         d *= caster.TryConsumeMana(2) ? 2 : 1;
            //         caster.AttackProcedure((8 + 3 * waiGong.Dj) * d);
            //     }),
            //
            // new WaiGongEntry("水23", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"每{6 - dj}锋锐，消耗1点，格挡+1"), WuXing.Shui, 1,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int value = caster.GetStackOfBuff("锋锐") / (6 - waiGong.Dj);
            //         caster.TryConsumeBuff("锋锐", value);
            //         caster.BuffSelfProcedure("格挡", value);
            //     }),
            //
            // new WaiGongEntry("木20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+2\n每有{6 - dj}点格挡，力量+1"), WuXing.Mu, type: WaiGongType.LingQi,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.BuffSelfProcedure("灵气", 2);
            //         int value = caster.GetStackOfBuff("格挡") / (6 - waiGong.Dj);
            //         caster.BuffSelfProcedure("力量", value);
            //     }),
            //
            // new WaiGongEntry("火31", new CLLibrary.Range(3, 5), new ChipDescription((l, j,, dj p) => $"消耗所有生命转护甲"), WuXing.Huo, new ManaCost((l, j,, dj p) => 4 - 2 * (j - 3)),
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int value = caster.Hp;
            //         StageManager.Instance.AttackProcedure(caster, caster, value);
            //         caster.ArmorGainSelfProcedure(value);
            //     }),
            //
            // new WaiGongEntry("金31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗一半护甲，施加等量减甲"), WuXing.Jin, new ManaCost((l, j, dj, p) => 6 - j),
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int value = caster.Armor / 2;
            //         caster.ArmorLoseSelfProcedure(value);
            //         caster.ArmorLoseOppoProcedure(value);
            //     }),
            //
            // new WaiGongEntry("水24", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+{2 + dj}\n满血：翻倍"), WuXing.Shui, type: WaiGongType.LingQi,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int d = caster.IsFullHp ? 2 : 1;
            //         caster.BuffSelfProcedure("灵气", (2 + waiGong.Dj) * d);
            //     }),
            //
            // new WaiGongEntry("熟能生巧", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"使用消耗不少于2的牌，下一次攻击带有吸血"), WuXing.Mu, manaCost: 2, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //     }),
            //
            // new WaiGongEntry("夜幕", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"本轮格挡+{3 + dj}"), WuXing.Mu, manaCost: 1,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.BuffSelfProcedure("轮格挡", 3);
            //     }),
            //
            // new WaiGongEntry("朝孔雀", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"力量+1\n{3 + 3 * dj}攻\n消耗1格挡：翻倍"), WuXing.Mu, manaCost: 1, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int d = caster.TryConsumeBuff("格挡") ? 2 : 1;
            //         caster.BuffSelfProcedure("力量", d);
            //         caster.AttackProcedure((3 + 3 * waiGong.Dj) * d);
            //     }),
            //
            // new WaiGongEntry("贪狼", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"5攻x{3 + dj}\n不屈+2"), WuXing.Huo, 2, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(5, 3 + waiGong.Dj);
            //         caster.BuffSelfProcedure("不屈", 2);
            //     }),
            //
            // new WaiGongEntry("飞龙在天", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{18 + 3 * dj}攻\n击伤：施加{2 + dj}缠绕"), WuXing.Shui, 4, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(18 + 3 * waiGong.Dj,
            //             damaged: d => caster.BuffOppoProcedure("缠绕", 2 + waiGong.Dj));
            //     }),
            //
            // new WaiGongEntry("廉贞", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"对敌方造成伤害时，至少为{4 + 3 * dj}"), WuXing.Huo,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //     }),
            //
            // new WaiGongEntry("腐朽", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{(dj >= 1 ? "二动\n" : "")}敌方减甲大于生命：斩杀"), WuXing.Jin,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         if (waiGong.Dj >= 1)
            //             caster.Swift = true;
            //
            //         if (-caster.Opponent().Armor >= caster.Opponent().Hp)
            //             caster.Opponent().Hp = 0;
            //     }),
            //
            // new WaiGongEntry("水41", new CLLibrary.Range(4, 5), "消耗\n本场战斗中，治疗被代替，每有10点，格挡+1\n格挡效果翻倍", WuXing.Shui,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         waiGong.Consumed = true;
            //         caster.BuffSelfProcedure("强化格挡");
            //     }),
            //
            // new WaiGongEntry("水42", new CLLibrary.Range(4, 5), "消耗\n本场战斗中，被治疗时，如果实际治疗>=20，二动", WuXing.Shui,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         waiGong.Consumed = true;
            //         caster.BuffSelfProcedure("治疗转二动");
            //     }),
            //
            // new WaiGongEntry("土41", new CLLibrary.Range(4, 5), "消耗\n累计获得200护甲：永久暴击", WuXing.Tu,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //     }),
            //
            // new WaiGongEntry("土42", new CLLibrary.Range(4, 5), new ChipDescription((l, j, dj, p) => $"持续4次，护甲减少时，加回来"), WuXing.Tu,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //     }),

            new WaiGongEntry("金00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"{3 + dj}攻\n施加{3 + dj}减甲"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(3 + waiGong.Dj);
                    await caster.ArmorLoseOppoProcedure(3 + waiGong.Dj);
                }),

            new WaiGongEntry("金01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"{5 + dj}攻\n敌方有减甲：多{3 + dj}攻"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int add = caster.Opponent().Armor >= 0 ? (3 + waiGong.Dj) : 0;
                    await caster.AttackProcedure(5 + waiGong.Dj + add);
                }),

            new WaiGongEntry("水00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"灵气+{2 + dj}"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 2 + waiGong.Dj);
                }),

            new WaiGongEntry("水01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"{4 + 2 * dj}攻 吸血"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(4 + 2 * waiGong.Dj, lifeSteal: true);
                }),

            new WaiGongEntry("木01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"{6 + 2 * dj}攻 穿透"), WuXing.Mu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(6 + 2 * waiGong.Dj, pierce: true);
                }),

            new WaiGongEntry("木02", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"灵气+{1 + dj}\n生命+{3 + 2 * dj}"), WuXing.Mu, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + waiGong.Dj);
                    await caster.HealProcedure(3 + 2 * waiGong.Dj);
                }),

            new WaiGongEntry("火00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"消耗{2 + dj}生命\n{8 + 3 * dj}攻"), WuXing.Huo, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.DamageSelfProcedure(2 + waiGong.Dj);
                    await caster.AttackProcedure(8 + 3 * waiGong.Dj);
                }),

            new WaiGongEntry("火01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"{3 + 2 * dj}攻\n灵气+{1 + dj}"), WuXing.Huo, skillTypeCollection: SkillTypeCollection.LingQi | SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(3 + 2 * waiGong.Dj);
                    await caster.BuffSelfProcedure("灵气", 1 + waiGong.Dj);
                }),

            new WaiGongEntry("土00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"{12 + 4 * dj}攻"), WuXing.Tu, new ManaCost((l, j, dj, p) => 2 + dj), SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(12 + 4 * waiGong.Dj);
                }),

            new WaiGongEntry("土01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"灵气+{1 + dj}\n护甲+{3 + 2 * dj}"), WuXing.Tu, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + waiGong.Dj);
                    await caster.ArmorGainSelfProcedure(3 + 2 * waiGong.Dj);
                }),

            new WaiGongEntry("刺穴", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"消耗\n灵气+{6 + 2 * dj}"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("灵气", 6 + 2 * waiGong.Dj);
                }),

            new WaiGongEntry("起", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"{4 + 2 * dj}攻\n锋锐+{2 + dj}\n初次：翻倍"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int mul = waiGong.IsFirstTime ? 2 : 1;
                    await caster.AttackProcedure((4 + 2 * waiGong.Dj) * mul);
                    await caster.BuffSelfProcedure("锋锐", (2 + waiGong.Dj) * mul);
                }),

            new WaiGongEntry("金10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"护甲+{4 + 2 * dj}\n施加{4 + 2 * dj}减甲"), WuXing.Jin,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(4 + 2 * waiGong.Dj);
                    await caster.ArmorLoseOppoProcedure(4 + 2 * waiGong.Dj);
                }),

            new WaiGongEntry("竹剑", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"{6 + 2 * dj}攻\n敌方有减甲：多1次"), WuXing.Jin, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int times = caster.Opponent().Armor < 0 ? 2 : 1;
                    await caster.AttackProcedure(6 + 2 * waiGong.Dj, times);
                }),

            new WaiGongEntry("吐纳", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"灵气+{3 + dj}\n生命上限+{8 + 4 * dj}"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 3 + waiGong.Dj);
                    // await Procedure
                    caster.MaxHp += 8 + 4 * waiGong.Dj;
                }),

            new WaiGongEntry("水10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"{10 + 2 * dj}攻\n击伤：格挡+1"), WuXing.Shui, manaCost: 2, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(10 + 2 * waiGong.Dj,
                        damaged: d => caster.BuffSelfProcedure("格挡"));
                }),

            new WaiGongEntry("水11", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"{10 + 2 * dj}攻\n终结：吸血"), WuXing.Shui, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(10 + 2 * waiGong.Dj, lifeSteal: waiGong.IsEnd);
                }),

            new WaiGongEntry("勤能补拙", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"护甲+{10 + 4 * dj}\n初次：遭受1跳回合"), WuXing.Shui,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(10 + 4 * waiGong.Dj);
                    if(waiGong.IsFirstTime)
                        await caster.BuffSelfProcedure("跳回合");
                }),

            new WaiGongEntry("身骑白马", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"第{3 + dj}+次使用：{(6 + 2 * dj) * (6 + 2 * dj)}攻 穿透"), WuXing.Mu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (waiGong.StageUsedTimes < 2 + waiGong.Dj)
                        return;
                    await caster.AttackProcedure((6 + 2 * waiGong.Dj) * (6 + 2 * waiGong.Dj));
                }),

            new WaiGongEntry("回马枪", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"下次受攻击时：{12 + 4 * dj}攻"), WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("回马枪", 12 + 4 * waiGong.Dj);
                }),

            new WaiGongEntry("早春", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"力量+1\n{6 + dj}攻\n初次：翻倍"), WuXing.Mu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int mul = waiGong.IsFirstTime ? 2 : 1;
                    await caster.BuffSelfProcedure("力量", mul);
                    await caster.AttackProcedure((6 + waiGong.Dj) * mul);
                }),

            new WaiGongEntry("潜龙在渊", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"生命+{10 + 4 * dj}\n初次：闪避+1"), WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.HealProcedure(10 + 4 * waiGong.Dj);
                    if (waiGong.IsFirstTime)
                        await caster.BuffSelfProcedure("闪避");
                }),

            new WaiGongEntry("一切皆苦", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"消耗\n唯一灵气牌：每回合灵气+1"), WuXing.Huo, new ManaCost((l, j, dj, p) => 3 - dj), skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    if (waiGong.NoOtherLingQi)
                        await caster.BuffSelfProcedure("自动灵气");
                }),

            new WaiGongEntry("火10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"2攻x{3 + dj}"), WuXing.Huo, 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(2, 3 + waiGong.Dj);
                }),

            new WaiGongEntry("火11", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"消耗\n灼烧+{2 + dj}"), WuXing.Huo, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("灼烧", 2 + waiGong.Dj);
                }),

            new WaiGongEntry("常夏", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"{4 + dj}攻\n每相邻1张火，多{4 + dj}攻"), WuXing.Huo, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int mul = 1;
                    mul += waiGong.Prev(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    mul += waiGong.Next(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    await caster.AttackProcedure((4 + waiGong.Dj) * mul);
                }),

            new WaiGongEntry("土11", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"消耗\n自动护甲+{1 + dj}"), WuXing.Tu,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("自动护甲", 1 + waiGong.Dj);
                }),

            new WaiGongEntry("巩固", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"灵气+{2 + dj}\n每1灵气，护甲+1"), WuXing.Tu, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 2 + waiGong.Dj);
                    await caster.ArmorGainSelfProcedure(caster.GetMana());
                }),

            new WaiGongEntry("土10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"{7 + 2 * dj}攻\n击伤：护甲+{7 + 2 * dj}"), WuXing.Tu, 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int value = 7 + 2 * waiGong.Dj;
                    await caster.AttackProcedure(value,
                        damaged: d => caster.ArmorGainSelfProcedure(value));
                }),

            new WaiGongEntry("利剑", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"{10 + 2 * dj}攻\n击伤：对方减少1灵气"), WuXing.Tu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(10 + 2 * waiGong.Dj,
                        damaged: async d => d.Tgt.TryConsumeMana());
                }),

            new WaiGongEntry("无常已至", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗\n本场战斗中，造成伤害：施加{5 + 2 * dj}减甲，不高于伤害值"), WuXing.Jin, 4,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("无常已至", 5 + 2 * waiGong.Dj);
                }),

            new WaiGongEntry("金20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"奇偶：施加{12 + 2 * dj}减甲/护甲+{12 + 2 * dj}\n消耗1锋锐：多{8 + 2 * dj}"), WuXing.Jin, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    int add = caster.TryConsumeBuff("锋锐") ? (8 + 2 * waiGong.Dj) : 0;
                    int value = 12 + 2 * waiGong.Dj + add;
                    if (waiGong.IsOdd)
                        await caster.ArmorLoseOppoProcedure(value);
                    if (waiGong.IsEven)
                        await caster.ArmorGainSelfProcedure(value);
                }),

            new WaiGongEntry("承", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"锋锐+{2 + dj}\n10攻 每1锋锐，多1攻"), WuXing.Jin, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("锋锐", 2 + waiGong.Dj);
                    await caster.AttackProcedure(10 + caster.GetStackOfBuff("锋锐"));
                }),

            new WaiGongEntry("菊剑", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{4 + 4 * dj}攻\n敌方有减甲：二动"), WuXing.Jin, 0, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    caster.Swift |= caster.Opponent().Armor < 0;
                    await caster.AttackProcedure(4 + 4 * waiGong.Dj);
                    caster.Swift |= caster.Opponent().Armor < 0;
                }),

            new WaiGongEntry("永久吸血", new CLLibrary.Range(2, 5), "消耗\n永久吸血，直到使用非攻击牌", WuXing.Shui, manaCost: new ManaCost((l, j, dj, p) => 3 - dj),
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("永久吸血");
                }),

            new WaiGongEntry("气吞山河", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"将灵气补至本局最大值+{1 + dj}"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    int space = caster.HighestManaRecord - caster.GetMana() + 1 + waiGong.Dj;
                    await caster.BuffSelfProcedure("灵气", space);
                }),

            new WaiGongEntry("水22", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{3 * j}攻\n二动\n初次：遭受1跳卡牌"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(3 * waiGong.GetJingJie());
                    caster.Swift = true;
                    if (waiGong.IsFirstTime)
                        await caster.BuffSelfProcedure("跳卡牌");
                }),

            new WaiGongEntry("水25", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{12 + 2 * dj}攻\n消耗1锋锐：吸血\n充沛：翻倍"), WuXing.Shui, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int d = caster.TryConsumeMana() ? 2 : 1;
                    await caster.AttackProcedure((12 + 2 * waiGong.Dj) * d, lifeSteal: caster.TryConsumeBuff("锋锐"));
                }),

            new WaiGongEntry("凝神", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"护甲+{5 + 5 * dj}\n下{1 + dj}次受到治疗：护甲+治疗量"), WuXing.Shui, manaCost: 2,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(5 + 5 * waiGong.Dj);
                    await caster.BuffSelfProcedure("凝神", 1 + waiGong.Dj);
                }),

            new WaiGongEntry("盛开", new CLLibrary.Range(2, 5), "消耗\n本场战斗中，受到治疗：力量+1", WuXing.Mu, manaCost: new ManaCost((l, j, dj, p) => 3 - dj),
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("盛开");
                }),

            new WaiGongEntry("一虚一实", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"8攻\n受到{3 + dj}倍力量影响\n未造成伤害：治疗等量数值"), WuXing.Mu, manaCost: 2, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int add = caster.GetStackOfBuff("力量") * (2 + waiGong.Dj);
                    int value = 8 + add;
                    await caster.AttackProcedure(value, undamaged:
                        d => caster.HealProcedure(value));
                }),

            new WaiGongEntry("见龙在田", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"闪避+{2 + dj}"), WuXing.Mu, manaCost: new ManaCost((l, j, dj, p) =>  3 + dj),
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("闪避", 2 + waiGong.Dj);
                }),

            new WaiGongEntry("天衣无缝", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗\n若无攻击牌，每回合：{6 + 2 * dj}攻"), WuXing.Huo,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    if (waiGong.NoOtherAttack)
                        await caster.BuffSelfProcedure("天衣无缝", 6 + 2 * waiGong.Dj);
                }),

            new WaiGongEntry("化劲", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗5生命\n灼烧+{2 + dj}\n消耗1力量：翻倍"), WuXing.Huo,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.DamageSelfProcedure(5);
                    int d = caster.TryConsumeBuff("力量") ? 2 : 1;
                    await caster.BuffSelfProcedure("灼烧", (2 + waiGong.Dj) * d);
                }),

            // new WaiGongEntry("土20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{10 + 4 * dj}攻\n每1灼烧，多{2 + dj}攻"), WuXing.Tu, 1, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(10 + 4 * waiGong.Dj + (2 + waiGong.Dj) * caster.GetStackOfBuff("灼烧"));
            //     }),

            new WaiGongEntry("收刀", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"下回合护甲+{8 + 4 * dj}\n上张牌激活架势"), WuXing.Tu,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("延迟护甲", 8 + 4 * waiGong.Dj);
                }),

            new WaiGongEntry("一力降十会", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"6攻\n唯一攻击牌：{6 + dj}倍"), WuXing.Tu, manaCost: 3, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    int d = waiGong.NoOtherAttack ? (6 + waiGong.Dj) : 1;
                    await caster.AttackProcedure(6 * d);
                }),

            new WaiGongEntry("高速剑阵", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+4\n架势：二动"), WuXing.Tu, 0, skillTypeCollection: SkillTypeCollection.JianZhen | SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 4);
                    caster.Swift |= waiGong.JiaShi;
                }),

            new WaiGongEntry("磐石剑阵", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"护甲+{20 + 6 * dj}\n遭受1跳回合\n架势：无需跳回合"), WuXing.Tu, 0, skillTypeCollection: SkillTypeCollection.JianZhen,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(20 + 6 * waiGong.Dj);
                    if (!waiGong.JiaShi)
                        await caster.BuffSelfProcedure("跳回合");
                }),

           new WaiGongEntry("兰剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"施加{4 + 4 * dj}减甲\n二动"), WuXing.Jin, 0,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorLoseOppoProcedure(4 + 4 * waiGong.Dj);
                    caster.Swift = true;
                }),

            new WaiGongEntry("敛息", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"下{1 + dj}次造成伤害转减甲"), WuXing.Jin,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("敛息", 1 + waiGong.Dj);
                }),

            new WaiGongEntry("转", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗\n消耗所有护甲，每消耗{4 - dj}点，锋锐+1"), WuXing.Jin, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    int value = caster.Armor / (4 - waiGong.Dj);
                    await caster.ArmorLoseSelfProcedure(caster.Armor);
                    await caster.BuffSelfProcedure("锋锐", value);
                }),

            new WaiGongEntry("金30", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"奇偶：\n消耗 施加{10 + 10 * dj}减甲\n二动 施加{5 + 5 * dj}减甲"), WuXing.Jin,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (waiGong.IsOdd)
                    {
                        waiGong.Consumed = true;
                        await caster.ArmorLoseOppoProcedure(10 + 10 * waiGong.Dj);
                    }
                    if (waiGong.IsEven)
                    {
                        caster.Swift = true;
                        await caster.ArmorLoseOppoProcedure(5 + 5 * waiGong.Dj);
                    }
                }),

            new WaiGongEntry("水30", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{16 + 8 * dj}攻\n每造成{8 - dj}点伤害，格挡+1"), WuXing.Shui, 4, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(16 + 8 * waiGong.Dj,
                        damaged: d => caster.BuffSelfProcedure("格挡", d.Value / (8 - waiGong.Dj)));
                }),

            new WaiGongEntry("奔腾", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"二动\n充沛：三动"), WuXing.Shui, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    caster.Swift = true;
                    if (caster.TryConsumeMana())
                        caster.UltraSwift = true;
                }),

            new WaiGongEntry("心斋", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"灵气+2\n下{1 + dj}次耗灵气免费"), WuXing.Mu, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 2);
                    await caster.BuffSelfProcedure("免费", 1 + waiGong.Dj);
                }),

            new WaiGongEntry("双发", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"下{1 + dj}张牌使用两次"), WuXing.Mu, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("双发", 1 + waiGong.Dj);
                }),

            new WaiGongEntry("千年笋", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{15 + 3 * dj}攻\n消耗1格挡：穿透"), WuXing.Mu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(15 + 5 * waiGong.Dj,
                        pierce: caster.TryConsumeBuff("格挡"));
                }),

            new WaiGongEntry("飞龙在天", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗\n每轮：闪避补至{1 + dj}"), WuXing.Mu, manaCost: 2,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("自动闪避", 1 + waiGong.Dj);
                }),

            new WaiGongEntry("燃灯留烬", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"护甲+{6 + 2 * dj}\n每1被消耗卡：多{6 + 2 * dj}"), WuXing.Huo, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure((caster.ConsumedCount + 1) * (6 + 2 * waiGong.Dj));
                }),

            new WaiGongEntry("抱元守一", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"每回合：消耗{3 + 3 * dj}生命，护甲+{3 + 3 * dj}"), WuXing.Huo,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("抱元守一", 3 + 3 * waiGong.Dj);
                }),

            // new WaiGongEntry("木剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{16 + 4 * dj}攻\n架势：暴击"), WuXing.Tu, 0, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(16 + 4 * waiGong.Dj, crit: waiGong.JiaShi);
            //     }),
            //
            // new WaiGongEntry("土31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"1攻 每失去过{8 - j}点护甲，多1攻"), WuXing.Tu, 0, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(1 + (8 - caster.LostArmorRecord / waiGong.GetJingJie()), crit: true);
            //     }),
            //
            // new WaiGongEntry("土32", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"护甲翻倍"), WuXing.Tu, new ManaCost((l, j, dj, p) => 12 - 2 * j),
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.ArmorGainSelfProcedure(caster.Armor);
            //     }),
            //
            // new WaiGongEntry("软剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{4 + dj * 4}攻\n击伤：二动"), WuXing.Tu, 1, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         caster.AttackProcedure(4 + 4 * waiGong.Dj,
            //             damaged: d => caster.Swift = true);
            //     }),

            new WaiGongEntry("少阳", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗\n获得护甲：额外+{3 + 2 * dj}"), WuXing.Tu,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("少阳", 3 + 2 * waiGong.Dj);
                }),

            new WaiGongEntry("金刚剑阵", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"1攻 每有一点护甲多1攻"), WuXing.Tu, new ManaCost((l, j, dj, p) => 3 - 2 * dj), skillTypeCollection: SkillTypeCollection.Attack | SkillTypeCollection.JianZhen,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(1 + Mathf.Max(0, caster.Armor));
                }),

            new WaiGongEntry("重剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{22 + 8 * dj}攻\n击伤：护甲+击伤值\n遭受2跳回合\n架势：无需跳回合"), WuXing.Tu, 2, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(22 + 8 * waiGong.Dj,
                        damaged: d => caster.ArmorGainSelfProcedure(d.Value));
                    if (!waiGong.JiaShi)
                        await caster.BuffSelfProcedure("跳回合", 2);
                }),

            new WaiGongEntry("拔刀", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{10 + 5 * dj}攻\n下张牌激活架势"), WuXing.Tu, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(10 + 5 * waiGong.Dj);
                }),

            new WaiGongEntry("森罗万象", new CLLibrary.Range(4, 5), new ChipDescription((l, j, dj, p) => $"消耗\n本场战斗中：奇偶同时激活两个效果"), WuXing.Jin,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("森罗万象");
                }),

            new WaiGongEntry("梅剑", new CLLibrary.Range(4, 5), "5攻x2\n敌方有减甲：暴击", WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(5, crit: caster.Opponent().Armor < 0);
                    await caster.AttackProcedure(5, crit: caster.Opponent().Armor < 0);
                }),

            new WaiGongEntry("少阴", new CLLibrary.Range(4, 5), "消耗\n施加减甲：额外+3\n消耗少阳：额外层数", WuXing.Jin,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    int value = caster.GetStackOfBuff("少阳") + 3;
                    caster.TryRemoveBuff("少阳");
                    await caster.BuffSelfProcedure("少阴", value);
                }),

            new WaiGongEntry("合", new CLLibrary.Range(4, 5), new ChipDescription((l, j, dj, p) => $"奇偶：每1锋锐，施加1减甲/护甲+1"), WuXing.Jin, 6,
                execute: async (caster, waiGong, recursive) =>
                {
                    int value = caster.GetStackOfBuff("锋锐");
                    if (waiGong.IsOdd)
                        await caster.ArmorLoseOppoProcedure(value);
                    if (waiGong.IsEven)
                        await caster.ArmorGainSelfProcedure(value);
                }),

            new WaiGongEntry("玄武吐息法", new CLLibrary.Range(4, 5), "消耗\n本局游戏中：治疗可以穿上限", WuXing.Shui, manaCost: 2,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("玄武吐息法");
                }),

            new WaiGongEntry("吞天", new CLLibrary.Range(4, 5), "消耗\n10攻 暴击 吸血", WuXing.Shui, manaCost: 2, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.AttackProcedure(10, crit: true, lifeSteal: true);
                }),

            new WaiGongEntry("观棋烂柯", new CLLibrary.Range(4, 5), "施加1跳回合", WuXing.Shui, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffOppoProcedure("跳回合");
                }),

            new WaiGongEntry("通透世界", new CLLibrary.Range(4, 5), "消耗\n本场战斗中：攻击具有穿透", WuXing.Mu, manaCost: 1,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("通透世界");
                }),

            new WaiGongEntry("回响", new CLLibrary.Range(4, 5), "使用第一张牌", WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (!recursive)
                        return;
                    await caster._waiGongList[0].Execute(caster, false);
                }),

            new WaiGongEntry("鹤回翔", new CLLibrary.Range(4, 5), "消耗\n反转出牌顺序", WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (caster.Forward)
                        await caster.BuffSelfProcedure("鹤回翔");
                    else
                        caster.TryRemoveBuff("鹤回翔");
                }),

            new WaiGongEntry("亢龙有悔", new CLLibrary.Range(4, 5), "10攻x3 穿透\n闪避+3\n每使用一次效果减弱", WuXing.Mu, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (waiGong.RunUsedTimes >= 3)
                        return;

                    await caster.AttackProcedure(10, 3 - waiGong.RunUsedTimes, pierce: true);
                    await caster.BuffSelfProcedure("闪避", 3 - waiGong.RunUsedTimes);
                }),

            new WaiGongEntry("净天地", new CLLibrary.Range(4, 5), "下1张非攻击卡不消耗灵气，使用之后消耗", WuXing.Huo,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("净天地");
                }),

            new WaiGongEntry("天女散花", new CLLibrary.Range(4, 5), "1攻 本局对战中，每获得过1闪避，多攻击1次", WuXing.Huo, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(1, caster.GainedEvadeRecord + 1);
                }),

            new WaiGongEntry("凤凰涅槃", new CLLibrary.Range(4, 5), "消耗\n累计获得20灼烧激活\n每轮：生命回满", WuXing.Huo,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.BuffSelfProcedure("待激活的凤凰涅槃");
                }),

            new WaiGongEntry("土40", new CLLibrary.Range(4, 5), "消耗\n护甲+50", WuXing.Tu,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    await caster.ArmorGainSelfProcedure(50);
                }),

            new WaiGongEntry("天人合一", new CLLibrary.Range(4, 5), "消耗\n激活所有架势", WuXing.Tu,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                }),

            new WaiGongEntry("无极剑阵", new CLLibrary.Range(4 , 5), "消耗\n使用之前3张剑阵牌", WuXing.Tu, 6, SkillTypeCollection.JianZhen,
                execute: async (caster, waiGong, recursive) =>
                {
                    if (recursive == false)
                        return;

                    waiGong.Consumed = true;
                    await waiGong.Prevs(false)
                        .FilterObj(wg => wg.GetWaiGongType().Contains(SkillTypeCollection.JianZhen) && wg.GetName() != "无极剑阵")
                        .FirstN(3)
                        .Reverse()
                        .Do(wg => wg.Execute(caster, false));
                }),

            // 莲花
            new WaiGongEntry("凶水：三步", 5, "10攻 击伤：对方剩余生命每有2点，施加1减甲", WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.AttackProcedure(10,
                        damaged: async d =>
                        {
                            int value = d.Tgt.Hp / 2;
                            await d.Src.ArmorLoseOppoProcedure(value);
                        });
                }),

            new WaiGongEntry("缠枝：周天结", 5, "消耗6格挡\n消耗\n本场战斗中，灵气消耗后加回", WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                }),

            new WaiGongEntry("烬焰：须菩提", 5, "下一张牌使用之后消耗，第六次使用时消耗", WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                }),

            new WaiGongEntry("轰炎：焚天", 5, "消耗，本场战斗中，自己的所有攻击具有穿透", WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                }),

            new WaiGongEntry("狂火：钟声", 5, "消耗，永久三动，三回合后死亡", WuXing.Mu,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                }),

            new WaiGongEntry("霸王鼎：离别", 5, "消耗，永久不屈", WuXing.Huo,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                }),

            new WaiGongEntry("庚金：万千辉", 5, "无效化敌人下一次攻击，并且反击", WuXing.Tu, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, waiGong, recursive) =>
                {
                    await caster.BuffSelfProcedure("看破");
                }),

            new WaiGongEntry("返虚土", 5, "消耗", WuXing.Tu,
                execute: async (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                }),
        };
    }
}
