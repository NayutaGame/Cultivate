
using System.Collections.Generic;
using System.Linq;
using CLLibrary;

public class FormationCategory : Category<FormationGroupEntry>
{
    public FormationCategory()
    {
        AddRange(new List<FormationGroupEntry>()
        {
            new("一业常置阵", order: 1, formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有一种五行，不少于11张", "战斗开始时，使用前两位的卡", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 11;
                        int score = d.WuXingCounts[d.WuXingOrder[0]];
                        return score >= requirement;
                    },
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
                new FormationEntry(JingJie.YuanYing, "有一种五行，不少于9张", "战斗开始时，使用第一位的卡", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 9;
                        int score = d.WuXingCounts[d.WuXingOrder[0]];
                        return score >= requirement;
                    },
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

            new("二律无相阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有两个五行，都不少于6张", "主动消耗锋锐\\格挡\\闪避\\力量\\灼烧时，返还2点", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 12;
                        int score = d.WuXingCounts[d.WuXingOrder[0]].ClampUpper(6) +
                                    d.WuXingCounts[d.WuXingOrder[1]].ClampUpper(6);
                        return score >= requirement;
                    },
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
                new FormationEntry(JingJie.YuanYing, "有两个五行，都不少于5张", "主动消耗锋锐\\格挡\\闪避\\力量\\灼烧时，返还1点", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 10;
                        int score = d.WuXingCounts[d.WuXingOrder[0]].ClampUpper(5) +
                                    d.WuXingCounts[d.WuXingOrder[1]].ClampUpper(5);
                        return score >= requirement;
                    },
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

            new("三才流转阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有三个五行，都不少于4张", "获得锋锐\\格挡\\闪避\\力量\\灼烧时，额外2点", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 12;
                        int score = d.WuXingCounts[d.WuXingOrder[0]].ClampUpper(4) +
                                    d.WuXingCounts[d.WuXingOrder[1]].ClampUpper(4) +
                                    d.WuXingCounts[d.WuXingOrder[2]].ClampUpper(4);
                        return score >= requirement;
                    },
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
                new FormationEntry(JingJie.YuanYing, "有三个五行，都不少于3张", "获得锋锐\\格挡\\闪避\\力量\\灼烧时，额外1点", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 9;
                        int score = d.WuXingCounts[d.WuXingOrder[0]].ClampUpper(3) +
                                    d.WuXingCounts[d.WuXingOrder[1]].ClampUpper(3) +
                                    d.WuXingCounts[d.WuXingOrder[2]].ClampUpper(3);
                        return score >= requirement;
                    },
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

            new("四元禁法阵", order: -3, formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有四个五行，都不少于3张", "双方所有阵法失效", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 12;
                        int score = d.WuXingCounts[d.WuXingOrder[0]].ClampUpper(3) +
                                    d.WuXingCounts[d.WuXingOrder[1]].ClampUpper(3) +
                                    d.WuXingCounts[d.WuXingOrder[2]].ClampUpper(3) +
                                    d.WuXingCounts[d.WuXingOrder[3]].ClampUpper(3);
                        return score >= requirement;
                    },
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

            new("五行颠倒阵", order: -2, formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有五种不同五行，都不少于2张", "复制对方的所有阵法，所有条件算作已激活", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 10;
                        int score = 0;
                        d.WuXingOrder.Do(wuXing => score += d.WuXingCounts[wuXing].ClampUpper(2));
                        return score >= requirement;
                    },
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

            new("六爻化劫阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "无二动牌，角色境界不低于化神", "第二轮开始时，双方重置生命上限，回100%血", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 0;
                        int score = -d.SwiftCount;
                        return entity.GetJingJie() >= JingJie.HuaShen &&
                               score >= requirement;
                    },
                    eventDescriptors: new StageEventDescriptor[]
                    {
                        new(StageEventDict.STAGE_FORMATION, StageEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            GainFormationDetails d = (GainFormationDetails)stageEventDetails;

                            await f.Owner.BuffSelfProcedure("六爻化劫", 100);
                        }),
                    }),
                new FormationEntry(JingJie.YuanYing, "无二动牌，角色境界不低于元婴", "第二轮开始时，双方重置生命上限，回30%血", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 0;
                        int score = -d.SwiftCount;
                        return entity.GetJingJie() >= JingJie.YuanYing &&
                               score >= requirement;
                    },
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

            new("七曜移星阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有至少6张带有二动的牌", "轮开始时，对方遭受1跳回合\n战斗开始时，对方遭受2跳回合", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 6;
                        int score = d.SwiftCount;
                        return score >= requirement;
                    },
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
                new FormationEntry(JingJie.YuanYing, "有至少5张带有二动的牌", "战斗开始时，对方遭受2跳回合", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 5;
                        int score = d.SwiftCount;
                        return score >= requirement;
                    },
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
                new FormationEntry(JingJie.JinDan, "有至少4张带有二动的牌", "战斗开始时，对方遭受1跳回合", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 4;
                        int score = d.SwiftCount;
                        return score >= requirement;
                    },
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

            new("八卦奇门阵", order: -1, formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "无消耗牌，角色境界不低于化神", "对方使用消耗牌后，自己也使用2次", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 0;
                        int score = -d.ExhaustedCount;
                        return entity.GetJingJie() >= JingJie.HuaShen &&
                               score >= requirement;
                    },
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
                new FormationEntry(JingJie.YuanYing, "无消耗牌，角色境界不低于元婴", "对方使用消耗牌后，自己也使用1次", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 0;
                        int score = -d.ExhaustedCount;
                        return entity.GetJingJie() >= JingJie.YuanYing &&
                               score >= requirement;
                    },
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

            new("九宫迷踪阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "不少于9张非攻击牌", "战斗开始护甲+30，每回合护甲+3", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 9;
                        int score = d.NonAttackCount;
                        return score >= requirement;
                    },
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
                new FormationEntry(JingJie.YuanYing, "不少于7张非攻击牌", "战斗开始护甲+20，每回合护甲+2", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 7;
                        int score = d.NonAttackCount;
                        return score >= requirement;
                    },
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
                new FormationEntry(JingJie.JinDan, "不少于5张非攻击牌", "战斗开始护甲+10，每回合护甲+1", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 5;
                        int score = d.NonAttackCount;
                        return score >= requirement;
                    },
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

            new("千界聚灵阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "原始费用消耗超过20", "战斗开始时，灵气+7", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 20;
                        int score = d.TotalCostCount;
                        return score >= requirement;
                    },
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
                new FormationEntry(JingJie.YuanYing, "原始费用消耗超过16", "战斗开始时，灵气+5", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 16;
                        int score = d.TotalCostCount;
                        return score >= requirement;
                    },
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
                new FormationEntry(JingJie.JinDan, "原始费用消耗超过12", "战斗开始时，灵气+3", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 12;
                        int score = d.TotalCostCount;
                        return score >= requirement;
                    },
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

            new("万剑归宗阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "连续7张攻击牌", "战斗开始时，力量+3", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 7;
                        int score = d.HighestConsecutiveAttackCount;
                        return score >= requirement;
                    },
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
                new FormationEntry(JingJie.YuanYing, "连续6张攻击牌", "战斗开始时，力量+2", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 6;
                        int score = d.HighestConsecutiveAttackCount;
                        return score >= requirement;
                    },
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
                new FormationEntry(JingJie.JinDan, "连续5张攻击牌", "战斗开始时，力量+1", trivia: null,
                    canActivate: (entity, d) =>
                    {
                        int requirement = 5;
                        int score = d.HighestConsecutiveAttackCount;
                        return score >= requirement;
                    },
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
