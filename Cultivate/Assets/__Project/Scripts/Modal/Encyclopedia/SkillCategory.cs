
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using CLLibrary;

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

            // 机关牌

            // 筑基
            new("醒神香", JingJie.ZhuJi, "灵气+4", // 香
                skillTypeCollection: SkillTypeCollection.LingQi, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 4);
                }),

            new("飞镖", JingJie.ZhuJi, "12攻", // 刃
                skillTypeCollection: SkillTypeCollection.Attack, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(12);
                }),

            new("铁匣", JingJie.ZhuJi, "护甲+12", // 匣
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 4);
                }),

            new("滑索", JingJie.ZhuJi, "三动 消耗", // 轮
                skillTypeCollection: SkillTypeCollection.ErDong | SkillTypeCollection.XiaoHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    caster.UltraSwift = true;
                    await skill.ExhaustProcedure();
                }),

            // 元婴
            new("还魂香", JingJie.YuanYing, "灵气+8", // 香香
                skillTypeCollection: SkillTypeCollection.LingQi, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 8);
                }),

            new("净魂刀", JingJie.YuanYing, "10攻 击伤：灵气+1，对手灵气-1", // 香刃
                skillTypeCollection: SkillTypeCollection.Attack | SkillTypeCollection.LingQi, withinPool: false,
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
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    int add = caster.GetStackOfBuff("灵气");
                    await caster.ArmorGainSelfProcedure(8 + add);
                }),

            new("能量饮料", JingJie.YuanYing, "下1次灵气减少时，加回", // 香轮
                skillTypeCollection: SkillTypeCollection.LingQi, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气回收");
                }),

            new("炎铳", JingJie.YuanYing, "25攻", // 刃刃
                skillTypeCollection: SkillTypeCollection.Attack, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(25);
                }),

            new("机关人偶", JingJie.YuanYing, "10攻 护甲+12", // 刃匣
                skillTypeCollection: SkillTypeCollection.Attack, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(12);
                    await caster.AttackProcedure(10);
                }),

            new("铁陀螺", JingJie.YuanYing, "2攻x6", // 刃轮
                skillTypeCollection: SkillTypeCollection.Attack, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2, times: 6);
                }),

            new("防壁", JingJie.YuanYing, "护甲+20\n自动护甲+2", // 匣匣
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(20);
                    await caster.BuffSelfProcedure("自动护甲", 2);
                }),

            new("不倒翁", JingJie.YuanYing, "下2次护甲减少时，加回", // 匣轮
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("护甲回收", 2);
                }),

            new("助推器", JingJie.YuanYing, "二动 双发", // 轮轮
                skillTypeCollection: SkillTypeCollection.ErDong, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    caster.Swift = true;
                    await caster.BuffSelfProcedure("双发");
                }),

            // 返虚
            new("反应堆", JingJie.FanXu, "消耗\n生命上限设为1，无法收到治疗，永久双发+1", // 香香香
                skillTypeCollection: SkillTypeCollection.XiaoHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    caster.MaxHp = 1;
                    caster.Hp = 1;
                    await caster.BuffSelfProcedure("禁止治疗");
                    await caster.BuffSelfProcedure("永久双发");
                }),

            new("烟花", JingJie.FanXu, "消耗所有灵气，每1，力量+1", // 香香刃
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    int stack = caster.GetStackOfBuff("灵气");
                    await caster.TryConsumeProcedure("灵气", stack);
                    await caster.BuffSelfProcedure("力量", stack);
                }),

            new("长明灯", JingJie.FanXu, "消耗\n本场战斗中，获得灵气时：每1，生命+3", // 香香匣
                skillTypeCollection: SkillTypeCollection.XiaoHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("长明灯", 3);
                }),

            new("大往生香", JingJie.FanXu, "消耗\n永久免费+1", // 香香轮
                skillTypeCollection: SkillTypeCollection.XiaoHao | SkillTypeCollection.LingQi, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("永久免费");
                }),

            new("地府通讯器", JingJie.FanXu, "失去一半生命，每8，灵气+1", // 轮香刃
                skillTypeCollection: SkillTypeCollection.LingQi, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    int gain = caster.Hp / 16;
                    caster.Hp -= gain * 8;
                    await caster.BuffSelfProcedure("灵气", gain);
                }),

            new("无人机阵列", JingJie.FanXu, "消耗\n永久穿透+1", // 刃刃刃
                skillTypeCollection: SkillTypeCollection.XiaoHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("永久穿透");
                }),

            new("弩炮", JingJie.FanXu, "50攻 吸血", // 刃刃香
                skillTypeCollection: SkillTypeCollection.Attack, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(50, lifeSteal: true);
                }),

            new("尖刺陷阱", JingJie.FanXu, "下次受到攻击时，对对方施加等量减甲", // 刃刃匣
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("尖刺陷阱");
                }),

            new("暴雨梨花针", JingJie.FanXu, "1攻x10", // 刃刃轮
                skillTypeCollection: SkillTypeCollection.Attack, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1, times: 10);
                }),

            new("炼丹炉", JingJie.FanXu, "消耗\n本场战斗中，每回合：力量+1", // 香刃匣
                skillTypeCollection: SkillTypeCollection.XiaoHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("回合力量");
                }),

            new("浮空艇", JingJie.FanXu, "消耗\n本场战斗中，回合被跳过后，该回合无法受到伤害\n遭受12跳回合", // 匣匣匣
                skillTypeCollection: SkillTypeCollection.XiaoHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("浮空艇");
                    await caster.BuffSelfProcedure("跳回合", 12);
                }),

            new("动量中和器", JingJie.FanXu, "格挡+10", // 匣匣香
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("格挡", 10);
                }),

            new("机关伞", JingJie.FanXu, "灼烧+8", // 匣匣刃
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灼烧", 8);
                }),

            new("一轮马", JingJie.FanXu, "闪避+6", // 匣匣轮
                withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("闪避", 6);
                }),

            new("外骨骼", JingJie.FanXu, "消耗\n本场战斗中，每次攻击时，护甲+3", // 刃匣轮
                skillTypeCollection: SkillTypeCollection.XiaoHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("外骨骼", 3);
                }),

            new("永动机", JingJie.FanXu, "消耗\n力量+8 灵气+8\n8回合后死亡", // 轮轮轮
                skillTypeCollection: SkillTypeCollection.XiaoHao | SkillTypeCollection.LingQi, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("力量", 8);
                    await caster.BuffSelfProcedure("灵气", 8);
                    await caster.BuffSelfProcedure("永动机", 8);
                }),

            new("火箭靴", JingJie.FanXu, "消耗\n本场战斗中，使用灵气牌时，获得二动", // 轮轮香
                skillTypeCollection: SkillTypeCollection.XiaoHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("火箭靴");
                }),

            new("定龙桩", JingJie.FanXu, "消耗\n本场战斗中，对方二动时，如果没有暴击，获得1", // 轮轮刃
                skillTypeCollection: SkillTypeCollection.XiaoHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("定龙桩");
                }),

            new("飞行器", JingJie.FanXu, "消耗\n本场战斗中，成功闪避时，如果对方没有跳回合，施加1", // 轮轮匣
                skillTypeCollection: SkillTypeCollection.XiaoHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("飞行器");
                }),

            new("时光机", JingJie.FanXu, "消耗\n本场战斗中，使用一张牌前，升级", // 匣轮香
                skillTypeCollection: SkillTypeCollection.XiaoHao, withinPool: false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("时光机");
                }),

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
            // new("水41", new CLLibrary.Range(4, 5), "消耗\n本场战斗中，治疗被代替，每有10点，格挡+1\n格挡效果翻倍", WuXing.Shui,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         await skill.ConsumeProcedure();
            //         caster.BuffSelfProcedure("强化格挡");
            //     }),
            //
            // new("水42", new CLLibrary.Range(4, 5), "消耗\n本场战斗中，被治疗时，如果实际治疗>=20，二动", WuXing.Shui,
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
            // new("土20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{10 + 4 * dj}攻\n每1灼烧，多{2 + dj}攻"), WuXing.Tu, 1, type: skillType.Attack,
            //     execute: (caster, skill, recursive) =>
            //     {
            //         caster.AttackProcedure(10 + 4 * skill.Dj + (2 + skill.Dj) * caster.GetStackOfBuff("灼烧"), wuXing: skill.Entry.WuXing);
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
                WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.GetStackOfBuff("锋锐") > 0 || await caster.IsFocused();
                    int add = cond ? 3 + skill.Dj : 0;
                    await caster.AttackProcedure(5 + skill.Dj + add, wuXing: skill.Entry.WuXing);
                }),

            new("微风", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"护甲+{3 + dj}\n锋锐+1\n初次：翻倍"),
                WuXing.Jin,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = skill.IsFirstTime || await caster.IsFocused();
                    int mul = cond ? 2 : 1;
                    await caster.ArmorGainSelfProcedure((3 + skill.Dj) * mul);
                    await caster.BuffSelfProcedure("锋锐", mul);
                }),

            new("金刃", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{3 + dj}攻\n施加{3 + dj}减甲"),
                WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.ArmorLoseOppoProcedure(3 + skill.Dj);
                }),

            new("贪狼", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"奇偶：{5 + 2 * dj}攻/护甲+{5 + 2 * dj}"),
                WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    int value = 5 + 2 * skill.Dj;
                    if (skill.IsOdd || await caster.IsFocused())
                        await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
                    if (skill.IsEven || await caster.IsFocused())
                        await caster.ArmorGainSelfProcedure(value);
                }),

            new("起", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{4 + dj}攻\n锋锐+{2 + dj}\n初次：翻倍"),
                WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = skill.IsFirstTime || await caster.IsFocused();
                    int mul = cond ? 2 : 1;
                    await caster.AttackProcedure((4 + skill.Dj) * mul, wuXing: skill.Entry.WuXing);
                    await caster.BuffSelfProcedure("锋锐", (2 + skill.Dj) * mul);
                }),

            new("金光罩", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"护甲+{4 + 2 * dj}\n施加{4 + 2 * dj}减甲"),
                WuXing.Jin,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(4 + 2 * skill.Dj);
                    await caster.ArmorLoseOppoProcedure(4 + 2 * skill.Dj);
                }),

            new("竹剑", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{6 + 2 * dj}攻\n敌方有减甲：多1次"),
                WuXing.Jin, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().Armor < 0 || await caster.IsFocused();
                    int times = cond ? 2 : 1;
                    await caster.AttackProcedure(6 + 2 * skill.Dj, times: times, wuXing: skill.Entry.WuXing);
                }),

            new("廉贞", new CLLibrary.Range(1, 5),
                new SkillDescription((j, dj) => $"奇偶：施加{8 + 2 * dj}减甲\n/护甲+{16 + 2 * dj}"), WuXing.Jin, manaCost: 1,
                execute: async (caster, skill, recursive) =>
                {
                    if (skill.IsOdd || await caster.IsFocused())
                        await caster.ArmorLoseOppoProcedure(8 + 2 * skill.Dj);
                    if (skill.IsEven || await caster.IsFocused())
                        await caster.ArmorGainSelfProcedure(16 + 2 * skill.Dj);
                }),

            new("承", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"锋锐+{2 + dj}\n{9 + dj}攻\n每1锋锐，多1攻"),
                WuXing.Jin, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("锋锐", 2 + skill.Dj);
                    await caster.AttackProcedure(9 + caster.GetStackOfBuff("锋锐"), wuXing: skill.Entry.WuXing);
                }),

            new("无常已至", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"消耗\n本场战斗中\n造成伤害时：施加{5 + 2 * dj}减甲，不高于伤害值"), WuXing.Jin, 4, skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("无常已至", 5 + 2 * skill.Dj);
                }),

            new("菊剑", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{4 + 4 * dj}攻\n敌方有减甲：二动"),
                WuXing.Jin, skillTypeCollection: SkillTypeCollection.Attack | SkillTypeCollection.ErDong,
                execute: async (caster, skill, recursive) =>
                {
                    caster.Swift |= caster.Opponent().Armor < 0;
                    await caster.AttackProcedure(4 + 4 * skill.Dj, wuXing: skill.Entry.WuXing);
                    caster.Swift |= caster.Opponent().Armor < 0;

                    if (!caster.Swift)
                        caster.Swift |= await caster.IsFocused();
                }),

            new("刺穴", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n灵气+{6 + 2 * dj}"), WuXing.Jin,
                skillTypeCollection: SkillTypeCollection.LingQi | SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("灵气", 6 + 2 * skill.Dj);
                }),

            new("转", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"护甲+6\n每{6 - dj}护甲，锋锐+1"), WuXing.Jin,
                manaCost: 1,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(6);
                    int value = caster.Armor / (6 - skill.Dj);
                    await caster.BuffSelfProcedure("锋锐", value);
                }),

            new("武曲", new CLLibrary.Range(3, 5),
                new SkillDescription((j, dj) => $"奇偶：施加{12 + 2 * dj}减甲/护甲+{12 + 2 * dj}\n消耗1锋锐：多{8 + 2 * dj}"),
                WuXing.Jin, manaCost: 1,
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

            new("敛息", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"下{1 + dj}次造成伤害转减甲"), WuXing.Jin,
                manaCost: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("敛息", 1 + skill.Dj);
                }),

            new("破军", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"奇偶：对方灵气-{2 + dj}\n/灵气+{4 + 2 * dj}"),
                WuXing.Jin, skillTypeCollection: SkillTypeCollection.LingQi,
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

            new("合", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"奇偶：每1锋锐，施加1减甲/护甲+1"), WuXing.Jin, 1,
                execute: async (caster, skill, recursive) =>
                {
                    int value = caster.GetStackOfBuff("锋锐");
                    if (skill.IsOdd || await caster.IsFocused())
                        await caster.ArmorLoseOppoProcedure(value);
                    if (skill.IsEven || await caster.IsFocused())
                        await caster.ArmorGainSelfProcedure(value);
                }),

            new("少阴", new CLLibrary.Range(4, 5), "消耗\n施加减甲：额外+3\n消耗少阳：额外层数", WuXing.Jin,
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    int value = caster.GetStackOfBuff("少阳") + 3;
                    await caster.TryRemoveBuff("少阳");
                    await caster.BuffSelfProcedure("少阴", value);
                }),

            new("梅剑", new CLLibrary.Range(4, 5), "5攻x2\n敌方有减甲：暴击", WuXing.Jin,
                skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().Armor < 0 || await caster.IsFocused();
                    await caster.AttackProcedure(5, crit: cond, wuXing: skill.Entry.WuXing);
                    await caster.AttackProcedure(5, crit: cond || caster.Opponent().Armor < 0 || await caster.IsFocused(), wuXing: skill.Entry.WuXing);
                }),

            new("森罗万象", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"消耗\n本场战斗中：奇偶同时激活两个效果"), WuXing.Jin,
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("森罗万象");
                }),

            new("恋花", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{4 + 2 * dj}攻 吸血"), WuXing.Shui,
                skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(4 + 2 * skill.Dj, lifeSteal: true, wuXing: skill.Entry.WuXing);
                }),

            new("冰弹", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{3 + 2 * dj}攻\n格挡+1"), WuXing.Shui,
                manaCost: 2, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.BuffSelfProcedure("格挡");
                }),

            new("满招损", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{5 + dj}攻\n对方有灵气：{3 + dj}攻"),
                WuXing.Shui, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().GetStackOfBuff("灵气") > 0 || await caster.IsFocused();
                    int add = cond ? 3 + skill.Dj : 0;
                    await caster.AttackProcedure(5 + skill.Dj + add, wuXing: skill.Entry.WuXing);
                }),

            new("清泉", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{2 + dj}"), WuXing.Shui,
                skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 2 + skill.Dj);
                }),

            new("归意", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{10 + 2 * dj}攻\n终结：吸血"), WuXing.Shui,
                manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10 + 2 * skill.Dj, lifeSteal: skill.IsEnd || await caster.IsFocused(),
                        wuXing: skill.Entry.WuXing);
                }),

            new("吐纳", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"灵气+{3 + dj}\n生命上限+{8 + 4 * dj}"),
                WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 3 + skill.Dj);
                    // await Procedure
                    caster.MaxHp += 8 + 4 * skill.Dj;
                }),

            new("冰雨", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{10 + 2 * dj}攻\n击伤：格挡+1"),
                WuXing.Shui, manaCost: 2, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10 + 2 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: d => caster.BuffSelfProcedure("格挡"));
                }),

            new("勤能补拙", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"护甲+{10 + 4 * dj}\n初次：遭受1跳回合"),
                WuXing.Shui,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(10 + 4 * skill.Dj);

                    if (!skill.IsFirstTime || await caster.IsFocused())
                    {

                    }
                    else
                        await caster.BuffSelfProcedure("跳回合");
                }),

            new("庄周梦蝶", new CLLibrary.Range(2, 5), "消耗\n永久吸血，直到使用非攻击牌", WuXing.Shui,
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                manaCost: new ManaCost((j, dj) => 3 - dj),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("永久吸血");
                }),

            new("秋水", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{12 + 2 * dj}攻\n消耗1锋锐：吸血\n充沛：翻倍"),
                WuXing.Shui, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool useFocus = !(caster.GetMana() > 0 && caster.GetStackOfBuff("锋锐") > 0);
                    bool focus = useFocus && await caster.IsFocused();
                    int d = focus || await caster.TryConsumeProcedure("灵气") ? 2 : 1;
                    await caster.AttackProcedure((12 + 2 * skill.Dj) * d, lifeSteal: focus || await caster.TryConsumeProcedure("锋锐"),
                        wuXing: skill.Entry.WuXing);
                }),

            new("玄冰刺", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"{16 + 8 * dj}攻\n每造成{8 - dj}点伤害，格挡+1"), WuXing.Shui, 4,
                skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(16 + 8 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: d => caster.BuffSelfProcedure("格挡", d.Value / (8 - skill.Dj)));
                }),

            new("腾跃", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"{6 + 3 * dj}攻\n二动\n第1 ~ {1 + dj}次：遭受1跳卡牌"), WuXing.Shui,
                skillTypeCollection: SkillTypeCollection.Attack | SkillTypeCollection.ErDong,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(6 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                    caster.Swift = true;
                    if (skill.RunUsedTimes <= skill.Dj)
                        await caster.BuffSelfProcedure("跳卡牌");
                }),

            // new("治疗转灵气", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"消耗\n本场战斗中\n受到治疗时：灵气+{1 + dj}"), WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
            //     execute: async (caster, skill, recursive) =>
            //     {
            //         await caster.BuffSelfProcedure("治疗转灵气", 1 + skill.Dj);
            //     }),

            new("透骨严寒", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n消耗所有灵气，每3：格挡+1"),
                WuXing.Shui, skillTypeCollection: SkillTypeCollection.LingQi,
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

            new("观棋烂柯", new CLLibrary.Range(3, 5), "施加1跳回合", WuXing.Shui, manaCost: new ManaCost((j, dj) => 1 - dj),
                execute: async (caster, skill, recursive) =>
                    await caster.BuffOppoProcedure("跳回合")),

            new("激流", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"生命+{5 + 5 * dj}\n下一次使用牌时二动"),
                WuXing.Shui, manaCost: new ManaCost((j, dj) => 1 - dj),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.HealProcedure(5 + 5 * skill.Dj);
                    await caster.BuffSelfProcedure("二动");
                }),

            new("气吞山河", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"将灵气补至本局最大值+{1 + dj}"), WuXing.Shui,
                skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    int space = caster.HighestManaRecord - caster.GetMana() + 1 + skill.Dj;
                    await caster.BuffSelfProcedure("灵气", space);
                }),

            new("吞天", new CLLibrary.Range(4, 5), "消耗\n10攻 暴击 吸血", WuXing.Shui, manaCost: 2,
                skillTypeCollection: SkillTypeCollection.Attack | SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.AttackProcedure(10, crit: true, lifeSteal: true, wuXing: skill.Entry.WuXing);
                }),

            new("玄武吐息法", new CLLibrary.Range(4, 5), "消耗\n本场战斗中：治疗可以穿上限", WuXing.Shui, manaCost: 2,
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("玄武吐息法");
                }),

            new("千里神行符", new CLLibrary.Range(4, 5), "消耗\n灵气+4\n二动\n", WuXing.Shui,
                skillTypeCollection: SkillTypeCollection.LingQi | SkillTypeCollection.XiaoHao | SkillTypeCollection.ErDong,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("灵气", 4);
                    caster.Swift = true;
                }),

            new("不动明王咒", new CLLibrary.Range(4, 5), "消耗\n格挡翻倍\n本场战斗中：无法二动", WuXing.Shui, manaCost: 3,
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("格挡", caster.GetStackOfBuff("格挡"));
                    await caster.BuffSelfProcedure("永久缠绕");
                }),

            new("奔腾", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"二动\n充沛：三动"), WuXing.Shui,
                skillTypeCollection: SkillTypeCollection.ErDong,
                manaCost: 1,
                execute: async (caster, skill, recursive) =>
                {
                    caster.Swift = true;
                    if (await caster.TryConsumeProcedure("灵气") || await caster.IsFocused())
                        caster.UltraSwift = true;
                }),

            new("若竹", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n穿透+1"), WuXing.Mu,
                skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + skill.Dj);
                    await caster.BuffSelfProcedure("穿透", 1);
                }),

            new("突刺", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{6 + 2 * dj}攻 穿透"), WuXing.Mu,
                manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(6 + 2 * skill.Dj, pierce: true, wuXing: skill.Entry.WuXing);
                }),

            new("花舞", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"护甲+{2 + 2 * dj}\n力量+1"), WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(2 + 2 * skill.Dj);
                    await caster.BuffSelfProcedure("力量");
                }),

            new("治愈", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n生命+{3 + dj}"),
                WuXing.Mu, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + skill.Dj);
                    await caster.HealProcedure(3 + skill.Dj);
                }),

            new("潜龙在渊", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"生命+{6 + 4 * dj}\n初次：闪避+1"),
                WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.HealProcedure(6 + 4 * skill.Dj);
                    if (skill.IsFirstTime || await caster.IsFocused())
                        await caster.BuffSelfProcedure("闪避");
                }),

            new("早春", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"力量+1\n{6 + dj}攻\n初次：翻倍"), WuXing.Mu,
                manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = skill.IsFirstTime || await caster.IsFocused();
                    int mul = cond ? 2 : 1;
                    await caster.BuffSelfProcedure("力量", mul);
                    await caster.AttackProcedure((6 + skill.Dj) * mul, wuXing: skill.Entry.WuXing);
                }),

            new("身骑白马", new CLLibrary.Range(1, 5),
                new SkillDescription((j, dj) => $"第{3 + dj}+次使用：{(6 + 2 * dj) * (6 + 2 * dj)}攻 穿透"), WuXing.Mu,
                manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    if (skill.StageUsedTimes < 2 + skill.Dj)
                        return;
                    await caster.AttackProcedure((6 + 2 * skill.Dj) * (6 + 2 * skill.Dj), pierce: true,
                        wuXing: skill.Entry.WuXing);
                }),

            new("回马枪", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"下次受攻击时：{12 + 4 * dj}攻"), WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("回马枪", 12 + 4 * skill.Dj);
                }),

            new("千年笋", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{15 + 3 * dj}攻\n消耗1格挡：穿透"),
                WuXing.Mu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(15 + 3 * skill.Dj, wuXing: skill.Entry.WuXing,
                        pierce: await caster.TryConsumeProcedure("格挡") || await caster.IsFocused());
                }),

            new("见龙在田", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"闪避+{1 + dj / 2}\n如果没有闪避：闪避+{1 + (dj + 1) / 2}"), WuXing.Mu,
                manaCost: 2,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.GetStackOfBuff("闪避") == 0 || await caster.IsFocused();
                    int add = cond ? 1 + (skill.Dj + 1) / 2 : 0;
                    await caster.BuffSelfProcedure("闪避", 1 + skill.Dj / 2 + add);
                }),

            new("回春印", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"双方护甲+{10 + 3 * dj}\n双方生命+{10 + 3 * dj}"), WuXing.Mu, manaCost: 1,
                execute: async (caster, skill, recursive) =>
                {
                    int value = 10 + 3 * skill.Dj;
                    await caster.ArmorGainSelfProcedure(value);
                    await caster.ArmorGainOppoProcedure(value);
                    await caster.HealProcedure(value);
                    await caster.Opponent().HealProcedure(value);
                }),

            new("一虚一实", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"8攻\n受到{3 + dj}倍力量影响\n未造成伤害：治疗等量数值"), WuXing.Mu, manaCost: 2,
                skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    int add = caster.GetStackOfBuff("力量") * (2 + skill.Dj);
                    int value = 8 + add;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
                        undamaged: d => caster.HealProcedure(value));
                }),

            new("飞龙在天", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"消耗\n每轮：闪避补至{1 + dj}"), WuXing.Mu,
                manaCost: 2, skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("自动闪避", 1 + skill.Dj);
                }),

            new("凝神", new CLLibrary.Range(3, 5),
                new SkillDescription((j, dj) => $"护甲+{5 + 5 * dj}\n下{1 + dj}次受到治疗：护甲+治疗量"), WuXing.Mu, manaCost: 2,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(5 + 5 * skill.Dj);
                    await caster.BuffSelfProcedure("凝神", 1 + skill.Dj);
                }),

            new("摩利支天咒", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"力量+{4 + 5 * dj}\n遭受{1 + dj}跳回合"),
                WuXing.Mu, manaCost: 1,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("力量", 4 + 5 * skill.Dj);
                    await caster.BuffSelfProcedure("跳回合", 1 + skill.Dj);
                }),

            new("双发", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"下{1 + dj}张牌使用两次"), WuXing.Mu,
                manaCost: 1,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("双发", 1 + skill.Dj);
                }),

            new("心斋", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"消耗\n本场战斗中：所有耗蓝-1"), WuXing.Mu,
                manaCost: new ManaCost((j, dj) => 2 - dj), skillTypeCollection: SkillTypeCollection.LingQi | SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("心斋");
                }),

            new("亢龙有悔", new CLLibrary.Range(4, 5), "消耗\n10攻x3 穿透\n闪避+3 力量+3\n消耗3灵气：无需消耗", WuXing.Mu,
                skillTypeCollection: SkillTypeCollection.Attack | SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, times: 3, pierce: true, wuXing: skill.Entry.WuXing);
                    await caster.BuffSelfProcedure("闪避", 3);
                    await caster.BuffSelfProcedure("力量", 3);
                    bool cond = await caster.TryConsumeProcedure("灵气", 3) || await caster.IsFocused();
                    if (!cond)
                        await skill.ExhaustProcedure();
                }),

            new("盛开", new CLLibrary.Range(4, 5), "消耗\n本场战斗中：\n受到治疗时：力量+1", WuXing.Mu,
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("盛开");
                }),

            // new("通透世界", new CLLibrary.Range(4, 5), "消耗\n本场战斗中：攻击具有穿透", WuXing.Mu, manaCost: 1,
            //     execute: async (caster, skill, recursive) =>
            //     {
            //         await skill.ConsumeProcedure();
            //         await caster.BuffSelfProcedure("通透世界");
            //     }),

            new("回响", new CLLibrary.Range(4, 5), "使用第一张牌", WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    if (!recursive)
                        return;
                    await caster._skills[0].Execute(caster, false);
                }),

            new("鹤回翔", new CLLibrary.Range(4, 5), "消耗\n反转出牌顺序", WuXing.Mu,
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    if (caster.Forward)
                        await caster.BuffSelfProcedure("鹤回翔");
                    else
                        await caster.TryRemoveBuff("鹤回翔");
                }),

            new("火墙", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{2 + dj}攻x2\n护甲+{3 + dj}"),
                WuXing.Huo, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2 + skill.Dj, wuXing: skill.Entry.WuXing, 2);
                    await caster.ArmorGainSelfProcedure(3 + skill.Dj);
                }),

            new("化焰", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{4 + 2 * dj}攻\n灼烧+1"), WuXing.Huo,
                manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(4 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.BuffSelfProcedure("灼烧");
                }),

            new("吐焰", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"消耗{2 + dj}生命\n{8 + 3 * dj}攻"),
                WuXing.Huo, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.DamageSelfProcedure(2 + skill.Dj);
                    await caster.AttackProcedure(8 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                }),

            new("燃命", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"消耗{3 + dj}生命\n{2 + 3 * dj}攻\n灵气+3"),
                WuXing.Huo, skillTypeCollection: SkillTypeCollection.LingQi | SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.DamageSelfProcedure(3 + skill.Dj);
                    await caster.AttackProcedure(2 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.BuffSelfProcedure("灵气", 3);
                }),

            new("火蛇", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"2攻x{3 + dj}"), WuXing.Huo, 1,
                skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2, times: 3 + skill.Dj, wuXing: skill.Entry.WuXing);
                }),

            new("聚火", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"消耗\n灼烧+{2 + dj}"), WuXing.Huo,
                manaCost: 1, skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("灼烧", 2 + skill.Dj);
                }),

            new("一切皆苦", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"消耗\n唯一灵气牌（无法使用集中）：每回合灵气+1"), WuXing.Huo,
                new ManaCost((j, dj) => 3 - dj), skillTypeCollection: SkillTypeCollection.LingQi | SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    if (skill.NoOtherLingQi)
                        await caster.BuffSelfProcedure("自动灵气");
                }),

            new("常夏", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{4 + dj}攻\n每相邻1张火，多{4 + dj}攻"),
                WuXing.Huo, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    int mul = 1;
                    mul += skill.Prev(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    mul += skill.Next(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    await caster.AttackProcedure((4 + skill.Dj) * mul, wuXing: skill.Entry.WuXing);
                }),

            new("天衣无缝", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n若无攻击牌（无法使用集中），每回合：{6 + 2 * dj}攻"),
                WuXing.Huo, manaCost: 4, skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    if (skill.NoOtherAttack)
                        await caster.BuffSelfProcedure("天衣无缝", 6 + 2 * skill.Dj);
                }),

            new("化劲", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗5生命\n灼烧+{2 + dj}\n消耗1力量：翻倍"),
                WuXing.Huo,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.DamageSelfProcedure(5);
                    bool cond = await caster.TryConsumeProcedure("力量") || await caster.IsFocused();
                    int d = cond ? 2 : 1;
                    await caster.BuffSelfProcedure("灼烧", (2 + skill.Dj) * d);
                }),

            new("业火", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n本场战斗中\n消耗牌时：使用2次"), WuXing.Huo,
                manaCost: new ManaCost((j, dj) => 4 - dj), skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("业火");
                }),

            new("淬体", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n本场战斗中\n消耗生命时：灼烧+1"), WuXing.Huo,
                manaCost: new ManaCost((j, dj) => 5 - dj), skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("淬体");
                }),

            new("燃灯留烬", new CLLibrary.Range(3, 5),
                new SkillDescription((j, dj) => $"护甲+{6 + 2 * dj}\n每1被消耗卡：多{6 + 2 * dj}"), WuXing.Huo, manaCost: 1,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure((caster.ExhaustedCount + 1) * (6 + 2 * skill.Dj));
                }),

            new("抱元守一", new CLLibrary.Range(3, 5),
                new SkillDescription((j, dj) => $"消耗\n每回合：消耗{3 + 3 * dj}生命，护甲+{3 + 3 * dj}"), WuXing.Huo,
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("抱元守一", 3 + 3 * skill.Dj);
                }),

            new("天女散花", new CLLibrary.Range(4, 5), "1攻 本局对战中，每获得过1闪避，多攻击1次", WuXing.Huo,
                skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1, times: caster.GainedEvadeRecord + 1, wuXing: skill.Entry.WuXing);
                }),

            new("凤凰涅槃", new CLLibrary.Range(4, 5), "消耗\n累计获得20灼烧激活\n每轮：生命回满", WuXing.Huo,
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("待激活的凤凰涅槃");
                }),

            new("净天地", new CLLibrary.Range(4, 5), "下1张非攻击卡不消耗灵气，使用之后消耗", WuXing.Huo,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("净天地");
                }),

            new("落石", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{12 + 4 * dj}攻"), WuXing.Tu,
                manaCost: 2, SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(12 + 4 * skill.Dj, wuXing: skill.Entry.WuXing);
                }),

            new("流沙", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"{3 + dj}攻\n灵气+{1 + dj}"), WuXing.Tu,
                skillTypeCollection: SkillTypeCollection.LingQi | SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + skill.Dj);
                    await caster.BuffSelfProcedure("灵气", 1 + skill.Dj);
                }),

            new("土墙", new CLLibrary.Range(0, 5), new SkillDescription((j, dj) => $"灵气+{1 + dj}\n护甲+{3 + dj}"),
                WuXing.Tu, skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 1 + skill.Dj);
                    await caster.ArmorGainSelfProcedure(3 + skill.Dj);
                }),

            new("地龙", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{7 + 2 * dj}攻\n击伤：护甲+{7 + 2 * dj}"),
                WuXing.Tu, 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    int value = 7 + 2 * skill.Dj;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
                        damaged: d => caster.ArmorGainSelfProcedure(value));
                }),

            new("利剑", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"{6 + 4 * dj}攻\n击伤：对方减少1灵气"),
                WuXing.Tu, manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(6 + 4 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: async d => await d.Tgt.TryConsumeProcedure("灵气"));
                }),

            new("铁骨", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"消耗\n自动护甲+{1 + dj}"), WuXing.Tu,
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("自动护甲", 1 + skill.Dj);
                }),

            new("巩固", new CLLibrary.Range(1, 5), new SkillDescription((j, dj) => $"灵气+{2 + dj}\n每1灵气，护甲+1"), WuXing.Tu,
                skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 2 + skill.Dj);
                    await caster.ArmorGainSelfProcedure(caster.GetMana());
                }),

            new("软剑", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"{9 + 4 * dj}攻\n击伤：施加1缠绕"), WuXing.Tu,
                manaCost: 1, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(9 + 4 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: async d => await caster.BuffOppoProcedure("缠绕"));
                }),

            new("一力降十会", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"6攻\n唯一攻击牌（无法使用集中）：{6 + dj}倍"), WuXing.Tu,
                manaCost: 3, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    int d = skill.NoOtherAttack ? (6 + skill.Dj) : 1;
                    await caster.AttackProcedure(6 * d, wuXing: skill.Entry.WuXing);
                }),

            new("磐石剑阵", new CLLibrary.Range(2, 5),
                new SkillDescription((j, dj) => $"护甲+{20 + 6 * dj}\n遭受1跳回合\n架势：无需跳回合"), WuXing.Tu, 0,
                skillTypeCollection: SkillTypeCollection.JianZhen,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.ArmorGainSelfProcedure(20 + 6 * skill.Dj);
                    if (skill.JiaShi || await caster.IsFocused())
                    {

                    }
                    else
                        await caster.BuffSelfProcedure("跳回合");
                }),

            new("少阳", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"消耗\n获得护甲：额外+{3 + 2 * dj}"),
                WuXing.Tu, skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("少阳", 3 + 2 * skill.Dj);
                }),

            new("高速剑阵", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"灵气+{4 + dj}\n架势：二动"), WuXing.Tu,
                skillTypeCollection: SkillTypeCollection.JianZhen | SkillTypeCollection.LingQi | SkillTypeCollection.ErDong,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("灵气", 4 + skill.Dj);
                    caster.Swift |= skill.JiaShi || await caster.IsFocused();
                }),

            new("收刀", new CLLibrary.Range(2, 5), new SkillDescription((j, dj) => $"下回合护甲+{8 + 4 * dj}\n上张牌激活架势"),
                WuXing.Tu,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("延迟护甲", 8 + 4 * skill.Dj);
                }),

            new("重剑", new CLLibrary.Range(3, 5),
                new SkillDescription((j, dj) => $"{22 + 8 * dj}攻\n击伤：护甲+击伤值\n遭受2跳回合\n架势：无需跳回合"), WuXing.Tu, 2,
                skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(22 + 8 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: d => caster.ArmorGainSelfProcedure(d.Value));
                    if (skill.JiaShi || await caster.IsFocused())
                    {

                    }
                    else
                        await caster.BuffSelfProcedure("跳回合", 2);
                }),

            new("金刚剑阵", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"1攻 每有一点护甲多1攻"), WuXing.Tu,
                new ManaCost((j, dj) => 3 - 2 * dj),
                skillTypeCollection: SkillTypeCollection.Attack | SkillTypeCollection.JianZhen,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1 + Mathf.Max(0, caster.Armor), wuXing: skill.Entry.WuXing);
                }),

            new("铁布衫", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"护甲+{10 + 4 * dj}\n若无护甲：翻倍"),
                WuXing.Tu,
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.Armor <= 0 || await caster.IsFocused();
                    int d = cond ? 2 : 1;
                    await caster.ArmorGainSelfProcedure((10 + 4 * skill.Dj) * d);
                }),

            new("拔刀", new CLLibrary.Range(3, 5), new SkillDescription((j, dj) => $"{10 + 5 * dj}攻\n下张牌激活架势"), WuXing.Tu,
                skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10 + 5 * skill.Dj, wuXing: skill.Entry.WuXing);
                }),

            new("天人合一", new CLLibrary.Range(3, 5), "消耗\n激活所有架势", WuXing.Tu, new ManaCost((j, dj) => 5 - 2 * dj),
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.BuffSelfProcedure("天人合一");
                }),

            new("木剑", new CLLibrary.Range(4, 5), new SkillDescription((j, dj) => $"18攻\n架势：暴击"), WuXing.Tu, 1,
                skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(18, wuXing: skill.Entry.WuXing, crit: skill.JiaShi || await caster.IsFocused());
                }),

            new("金钟罩", new CLLibrary.Range(4, 5), "消耗\n护甲+20\n充沛：翻倍", WuXing.Tu, manaCost: 1,
                skillTypeCollection: SkillTypeCollection.XiaoHao,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    bool cond = await caster.TryConsumeProcedure("灵气") || await caster.IsFocused();
                    int d = cond ? 2 : 1;
                    await caster.ArmorGainSelfProcedure(20 * d);
                }),

            new("汇聚", new CLLibrary.Range(4, 5), "灵气+3\n如果有（无法使用集中）：锋锐，格挡，闪避，力量，灼烧，则层数+2", WuXing.Tu,
                skillTypeCollection: SkillTypeCollection.LingQi,
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
            new("凶水：三步", 5, "10攻 击伤：对方剩余生命每有2点，施加1减甲", WuXing.Jin,
                skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            int value = d.Tgt.Hp / 2;
                            await d.Src.ArmorLoseOppoProcedure(value);
                        });
                }),

            new("缠枝：周天结", 5, "消耗6格挡\n消耗\n本场战斗中，灵气消耗后加回", WuXing.Shui,
                skillTypeCollection: SkillTypeCollection.LingQi,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),

            new("烬焰：须菩提", 5, "下一张牌使用之后消耗，第六次使用时消耗", WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),

            new("轰炎：焚天", 5, "消耗，本场战斗中，自己的所有攻击具有穿透", WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),

            new("狂火：钟声", 5, "消耗，永久三动，三回合后死亡", WuXing.Mu,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),

            new("霸王鼎：离别", 5, "消耗，永久不屈", WuXing.Huo,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),

            new("庚金：万千辉", 5, "无效化敌人下一次攻击，并且反击", WuXing.Tu, skillTypeCollection: SkillTypeCollection.Attack,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.BuffSelfProcedure("看破");
                }),

            new("返虚土", 5, "消耗", WuXing.Tu,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                }),
        });
    }

    public void Init()
    {
        List.Do(entry => entry.Generate());
    }

    public override SkillEntry Default() => this["不存在的技能"];
}
