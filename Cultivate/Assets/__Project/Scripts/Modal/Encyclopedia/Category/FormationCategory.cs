
using System.Collections.Generic;
using System.Linq;
using CLLibrary;

public class FormationCategory : Category<FormationGroupEntry>
{
    public FormationCategory()
    {
        AddRange(new List<FormationGroupEntry>()
        {
            new("一业常置阵", order: 1, conditionDescription: "最多的五行越多越好", progressEvaluator: (e, d) => d.WuXingCounts[d.WuXingOrder[0]],
                formationEntries: new[]
                {
                    new FormationEntry(JingJie.HuaShen, rewardDescription: "战斗开始时，使用前两位的卡", trivia: null, requirement: 11,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                GainFormationDetails d = (GainFormationDetails)stageEventDetails;

                                await f.Owner._skills[0].ExecuteWithoutTween(f.Owner);
                                await f.Owner._skills[1].ExecuteWithoutTween(f.Owner);
                            }),
                        }),
                    new FormationEntry(JingJie.YuanYing, rewardDescription: "战斗开始时，使用第一位的卡", trivia: null, requirement: 9,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                GainFormationDetails d = (GainFormationDetails)stageEventDetails;

                                await f.Owner._skills[0].ExecuteWithoutTween(f.Owner);
                            }),
                        }),
                }),

            new("二律无相阵", order: 0, conditionDescription: "张数前二多的五行，各占卡组的一半", progressEvaluator:
                (e, d) =>
                    d.WuXingCounts[d.WuXingOrder[0]].ClampUpper(e.GetSlotLimit() / 2) +
                    d.WuXingCounts[d.WuXingOrder[1]].ClampUpper(e.GetSlotLimit() / 2),
                formationEntries: new[]
                {
                    new FormationEntry(JingJie.HuaShen, rewardDescription: "主动消耗锋锐\\格挡\\闪避\\力量\\灼烧时，返还2点", trivia: null, requirement: 12,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_DISPEL, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                DispelDetails d = (DispelDetails)stageEventDetails;

                                if (!(f.Owner == d.Src && f.Owner == d.Tgt)) return;
                                if (d._stack <= 0) return;

                                BuffEntry[] buffs = new BuffEntry[] { "锋锐", "格挡", "闪避", "力量", "灼烧" };
                                if (!buffs.Contains(d._buffEntry)) return;

                                await d.Tgt.BuffSelfProcedure(d._buffEntry, 2, recursive: false);
                            }),
                        }),
                    new FormationEntry(JingJie.YuanYing, rewardDescription: "主动消耗锋锐\\格挡\\闪避\\力量\\灼烧时，返还1点", trivia: null, requirement: 10,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_DISPEL, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                DispelDetails d = (DispelDetails)stageEventDetails;

                                if (!(f.Owner == d.Src && f.Owner == d.Tgt)) return;
                                if (d._stack <= 0) return;

                                BuffEntry[] buffs = new BuffEntry[] { "锋锐", "格挡", "闪避", "力量", "灼烧" };
                                if (!buffs.Contains(d._buffEntry)) return;

                                await d.Tgt.BuffSelfProcedure(d._buffEntry, recursive: false);
                            }),
                        }),
                }),

            new("三才流转阵", order: 0, conditionDescription: "张数前三多的五行，各占卡组的三分之一", progressEvaluator:
                (e, d) =>
                    d.WuXingCounts[d.WuXingOrder[0]].ClampUpper(e.GetSlotLimit() / 3) +
                    d.WuXingCounts[d.WuXingOrder[1]].ClampUpper(e.GetSlotLimit() / 3) +
                    d.WuXingCounts[d.WuXingOrder[2]].ClampUpper(e.GetSlotLimit() / 3),
                formationEntries: new[]
                {
                    new FormationEntry(JingJie.HuaShen, rewardDescription: "获得锋锐\\格挡\\闪避\\力量\\灼烧时，额外2点", trivia: null, requirement: 12,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_BUFF, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                BuffDetails d = (BuffDetails)stageEventDetails;

                                if (f.Owner != d.Tgt) return;
                                if (d._stack <= 0) return;

                                BuffEntry[] buffs = new BuffEntry[] { "锋锐", "格挡", "闪避", "力量", "灼烧" };
                                if (!buffs.Contains(d._buffEntry)) return;

                                d._stack += 2;
                            }),
                        }),
                    new FormationEntry(JingJie.YuanYing, rewardDescription: "获得锋锐\\格挡\\闪避\\力量\\灼烧时，额外1点", trivia: null, requirement: 9,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_BUFF, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                BuffDetails d = (BuffDetails)stageEventDetails;

                                if (f.Owner != d.Tgt) return;
                                if (d._stack <= 0) return;

                                BuffEntry[] buffs = new BuffEntry[] { "锋锐", "格挡", "闪避", "力量", "灼烧" };
                                if (!buffs.Contains(d._buffEntry)) return;

                                d._stack += 1;
                            }),
                        }),
                }),

            new("四元禁法阵", order: -3, conditionDescription: "有四种五行，每种有三张", progressEvaluator:
                (e, d) =>
                    d.WuXingCounts[d.WuXingOrder[0]].ClampUpper(3) +
                    d.WuXingCounts[d.WuXingOrder[1]].ClampUpper(3) +
                    d.WuXingCounts[d.WuXingOrder[2]].ClampUpper(3) +
                    d.WuXingCounts[d.WuXingOrder[3]].ClampUpper(3),
                formationEntries: new[]
                {
                    new FormationEntry(JingJie.HuaShen, rewardDescription: "双方所有阵法失效", trivia: null, requirement: 12,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.FORMATION_WILL_ADD, -3, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                FormationDetails d = (FormationDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                                if (d._formation.GetName() != "四元禁法阵")
                                    d.Cancel = true;
                            }),
                        }),
                }),

            new("五行颠倒阵", order: -2, conditionDescription: "有五种五行，每种有两张", progressEvaluator:
                (e, d) =>
                {
                    int score = 0;
                    d.WuXingOrder.Do(wuXing => score += d.WuXingCounts[wuXing].ClampUpper(2));
                    return score;
                },
                formationEntries: new[]
                {
                    new FormationEntry(JingJie.HuaShen, rewardDescription: "复制对方的所有阵法，所有条件算作已激活", trivia: null, requirement: 10,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                GainFormationDetails d = (GainFormationDetails)stageEventDetails;

                                for (int i = 0; i < f.Owner._skills.Length; i++)
                                {
                                    StageSkill skill = f.Owner._skills[i];
                                    if (skill.Entry.GetName() != "聚气术") continue;

                                    if (!(i < f.Owner.Opponent()._skills.Length)) continue;

                                    StageSkill opponentSkill = f.Owner.Opponent()._skills[i];
                                    f.Owner._skills[i] = StageSkill.FromSkillEntry(f.Owner, opponentSkill.Entry, opponentSkill.GetJingJie(), i);
                                }

                                await f.Owner.BuffSelfProcedure("永久集中");
                            }),
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.FORMATION_WILL_ADD, -2, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                FormationDetails d = (FormationDetails)stageEventDetails;
                                if (f.Owner == d.Owner) return;
                                if (!d._recursive) return;
                                if (d._formation.GetName() == "颠倒五行阵") return;

                                await f.Owner.FormationProcedure(d._formation, false);
                            }),
                        }),
                }),

            new("六爻化劫阵", order: 0, conditionDescription: "非二动牌的数量越多越好", progressEvaluator: (e, d) => d.NonSwiftCount,
                formationEntries: new[]
                {
                    new FormationEntry(JingJie.HuaShen, rewardDescription: "第二轮开始时，双方重置生命上限，回100%血", trivia: null, requirement: 12,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                GainFormationDetails d = (GainFormationDetails)stageEventDetails;

                                await f.Owner.BuffSelfProcedure("六爻化劫", 100);
                            }),
                        }),
                    new FormationEntry(JingJie.YuanYing, rewardDescription: "第二轮开始时，双方重置生命上限，回30%血", trivia: null, requirement: 10,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                GainFormationDetails d = (GainFormationDetails)stageEventDetails;

                                await f.Owner.BuffSelfProcedure("六爻化劫", 30);
                            }),
                        }),
                }),

            new("七曜移星阵", order: 0, conditionDescription: "二动牌的数量越多越好", progressEvaluator: (e, d) => d.SwiftCount,
                formationEntries: new[]
                {
                    new FormationEntry(JingJie.HuaShen, rewardDescription: "轮开始时，对方遭受1跳回合\n战斗开始时，对方遭受2跳回合", trivia: null, requirement: 6,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.BuffOppoProcedure("跳回合", 2);
                            }),
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_ROUND, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                RoundDetails d = (RoundDetails)stageEventDetails;

                                if (f.Owner == d.Owner)
                                    await f.Owner.BuffOppoProcedure("跳回合");
                            }),
                        }),
                    new FormationEntry(JingJie.YuanYing, rewardDescription: "战斗开始时，对方遭受2跳回合", trivia: null, requirement: 5,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.BuffOppoProcedure("跳回合", 2);
                            }),
                        }),
                    new FormationEntry(JingJie.JinDan, rewardDescription: "战斗开始时，对方遭受1跳回合", trivia: null, requirement: 4,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.BuffOppoProcedure("跳回合");
                            }),
                        }),
                }),

            new("八卦奇门阵", order: -1, conditionDescription: "非消耗牌的数量越多越好", progressEvaluator: (e, d) => d.NonExhaustedCount,
                formationEntries: new[]
                {
                    new FormationEntry(JingJie.HuaShen, rewardDescription: "对方使用消耗牌后，自己也使用2次", trivia: null, requirement: 12,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_EXHAUST, -1, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                ExhaustDetails d = (ExhaustDetails)stageEventDetails;
                                if (f.Owner != d.Owner.Opponent())
                                    return;
                                await d.Skill.Execute(f.Owner);
                                await d.Skill.Execute(f.Owner);
                            }),
                        }),
                    new FormationEntry(JingJie.YuanYing, rewardDescription: "对方使用消耗牌后，自己也使用1次", trivia: null, requirement: 10,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_EXHAUST, -1, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                ExhaustDetails d = (ExhaustDetails)stageEventDetails;
                                if (f.Owner != d.Owner.Opponent())
                                    return;
                                await d.Skill.Execute(f.Owner);
                            }),
                        }),
                }),

            new("九宫迷踪阵", order: 0, conditionDescription: "非攻击牌越多越好", progressEvaluator: (e, d) => d.NonAttackCount,
                formationEntries: new[]
                {
                    new FormationEntry(JingJie.HuaShen, rewardDescription: "战斗开始护甲+30，每回合护甲+3", trivia: null, requirement: 9,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.ArmorGainSelfProcedure(30);
                            }),
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                TurnDetails d = (TurnDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await d.Owner.ArmorGainSelfProcedure(3);
                            }),
                        }),
                    new FormationEntry(JingJie.YuanYing, rewardDescription: "战斗开始护甲+20，每回合护甲+2", trivia: null, requirement: 7,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.ArmorGainSelfProcedure(20);
                            }),
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                TurnDetails d = (TurnDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await d.Owner.ArmorGainSelfProcedure(2);
                            }),
                        }),
                    new FormationEntry(JingJie.JinDan, rewardDescription: "战斗开始护甲+10，每回合护甲+1", trivia: null, requirement: 5,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.ArmorGainSelfProcedure(10);
                            }),
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                TurnDetails d = (TurnDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await d.Owner.ArmorGainSelfProcedure(1);
                            }),
                        }),
                }),

            new("千界聚灵阵", order: 0, conditionDescription: "卡牌原始费用之和越高越好", progressEvaluator: (e, d) => d.TotalCostCount,
                formationEntries: new[]
                {
                    new FormationEntry(JingJie.HuaShen, rewardDescription: "战斗开始时，灵气+7", trivia: null, requirement: 20,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.BuffSelfProcedure("灵气", 7);
                            }),
                        }),
                    new FormationEntry(JingJie.YuanYing, rewardDescription: "战斗开始时，灵气+5", trivia: null, requirement: 16,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.BuffSelfProcedure("灵气", 5);
                            }),
                        }),
                    new FormationEntry(JingJie.JinDan, rewardDescription: "战斗开始时，灵气+3", trivia: null, requirement: 12,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.BuffSelfProcedure("灵气", 3);
                            }),
                        }),
                }),

            new("万剑归宗阵", order: 0, conditionDescription: "连续攻击牌越多越好", progressEvaluator: (e, d) => d.HighestConsecutiveAttackCount,
                formationEntries: new[]
                {
                    new FormationEntry(JingJie.HuaShen, rewardDescription: "战斗开始时，力量+3", trivia: null, requirement: 7,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.BuffSelfProcedure("力量", 3);
                            }),
                        }),
                    new FormationEntry(JingJie.YuanYing, rewardDescription: "战斗开始时，力量+2", trivia: null, requirement: 6,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.BuffSelfProcedure("力量", 2);
                            }),
                        }),
                    new FormationEntry(JingJie.JinDan, rewardDescription: "战斗开始时，力量+1", trivia: null, requirement: 5,
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.BuffSelfProcedure("力量");
                            }),
                        }),
                }),
        });
    }
}
