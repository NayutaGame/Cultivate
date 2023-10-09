
using System.Collections.Generic;
using System.Linq;

public class FormationCategory : Category<FormationGroupEntry>
{
    public FormationCategory()
    {
        AddRange(new List<FormationGroupEntry>()
        {
            new("一业常置阵", order: 1, formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有且只有1种五行，不少于9张", "战斗开始时，使用前两位的卡",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 9;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        int countEQ0 = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] == 0);
                        return countGErequirement == 1 && countEQ0 == 4;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_FORMATION, CLEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            GainFormationDetails d = (GainFormationDetails)stageEventDetails;

                            await f.Owner._skills[0].ExecuteWithoutTween(f.Owner);
                            await f.Owner._skills[1].ExecuteWithoutTween(f.Owner);
                        }),
                    }),
                new FormationEntry(JingJie.YuanYing, "有且只有1种五行，不少于7张", "战斗开始时，使用第一位的卡",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 7;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        int countEQ0 = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] == 0);
                        return countGErequirement == 1 && countEQ0 == 4;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_FORMATION, CLEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            GainFormationDetails d = (GainFormationDetails)stageEventDetails;

                            await f.Owner._skills[0].ExecuteWithoutTween(f.Owner);
                        }),
                    }),
            }),

            new("二律无相阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有两个五行，都不少于6张", "主动消耗锋锐\\格挡\\闪避\\力量\\灼烧时，返还2点",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 6;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 2;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_DISPEL, 0, async (listener, stageEventDetails) =>
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
                new FormationEntry(JingJie.YuanYing, "有两个五行，都不少于5张", "主动消耗锋锐\\格挡\\闪避\\力量\\灼烧时，返还1点",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 5;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 2;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_DISPEL, 0, async (listener, stageEventDetails) =>
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
                new FormationEntry(JingJie.HuaShen, "有三个五行，都不少于4张", "获得锋锐\\格挡\\闪避\\力量\\灼烧时，额外2点",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 4;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 3;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_BUFF, 0, async (listener, stageEventDetails) =>
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
                new FormationEntry(JingJie.YuanYing, "有三个五行，都不少于3张", "获得锋锐\\格挡\\闪避\\力量\\灼烧时，额外1点",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 3;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 3;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_BUFF, 0, async (listener, stageEventDetails) =>
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
                new FormationEntry(JingJie.HuaShen, "有四个五行，都不少于3张", "双方所有阵法失效",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 3;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 4;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.FORMATION_WILL_ADD, -3, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            FormationDetails d = (FormationDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;
                            if (d._formation.GetName() != "四元禁法阵")
                                d.Cancel = true;
                        }),
                    }),
            }),

            new("颠倒五行阵", order: -2, formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有五种不同五行，都不少于2张", "复制对方的所有阵法，战斗开始时，空置位将复制对方同位置的卡，所有条件算作已激活",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 2;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 5;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_FORMATION, CLEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            GainFormationDetails d = (GainFormationDetails)stageEventDetails;

                            for (int i = 0; i < f.Owner._skills.Length; i++)
                            {
                                StageSkill skill = f.Owner._skills[i];
                                if (skill.Entry.GetName() != "聚气术") continue;

                                if (!(i < f.Owner.Opponent()._skills.Length)) continue;

                                StageSkill opponentSkill = f.Owner.Opponent()._skills[i];
                                f.Owner._skills[i] = new StageSkill(f.Owner, opponentSkill.Entry, opponentSkill.GetJingJie(), i);
                            }

                            await f.Owner.BuffSelfProcedure("永久集中");
                        }),
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.FORMATION_WILL_ADD, -2, async (listener, stageEventDetails) =>
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
                new FormationEntry(JingJie.HuaShen, "无二动牌，角色境界不低于化神", "第二轮开始时，双方重置生命上限，回100%血",
                    canActivate: (entity, args) =>
                    {
                        return args.SwiftCount == 0 && entity.GetJingJie() >= JingJie.HuaShen;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_FORMATION, CLEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            GainFormationDetails d = (GainFormationDetails)stageEventDetails;

                            await f.Owner.BuffSelfProcedure("六爻化劫", 100);
                        }),
                    }),
                new FormationEntry(JingJie.YuanYing, "无二动牌，角色境界不低于元婴", "第二轮开始时，双方重置生命上限，回30%血",
                    canActivate: (entity, args) =>
                    {
                        return args.SwiftCount == 0 && entity.GetJingJie() >= JingJie.YuanYing;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_FORMATION, CLEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            GainFormationDetails d = (GainFormationDetails)stageEventDetails;

                            await f.Owner.BuffSelfProcedure("六爻化劫", 30);
                        }),
                    }),
            }),

            new("七曜移星阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有至少6张带有二动的牌", "轮开始时，对方遭受1跳回合\n战斗开始时，对方遭受2跳回合",
                    canActivate: (entity, args) =>
                    {
                        return args.SwiftCount >= 6;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            StageDetails d = (StageDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;

                            await f.Owner.BuffOppoProcedure("跳回合", 2);
                        }),
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_ROUND, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            RoundDetails d = (RoundDetails)stageEventDetails;

                            if (f.Owner == d.Owner)
                                await f.Owner.BuffOppoProcedure("跳回合");
                        }),
                    }),
                new FormationEntry(JingJie.YuanYing, "有至少5张带有二动的牌", "战斗开始时，对方遭受2跳回合",
                    canActivate: (entity, args) =>
                    {
                        return args.SwiftCount >= 5;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            StageDetails d = (StageDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;

                            await f.Owner.BuffOppoProcedure("跳回合", 2);
                        }),
                    }),
                new FormationEntry(JingJie.JinDan, "有至少4张带有二动的牌", "战斗开始时，对方遭受1跳回合",
                    canActivate: (entity, args) =>
                    {
                        return args.SwiftCount >= 4;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
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
                new FormationEntry(JingJie.HuaShen, "无消耗牌，角色境界不低于化神", "对方使用消耗牌后，自己也使用2次",
                    canActivate: (entity, args) =>
                    {
                        return args.ExhaustedCount == 0 && entity.GetJingJie() >= JingJie.HuaShen;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_EXHAUST, -1, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            ExhaustDetails d = (ExhaustDetails)stageEventDetails;
                            if (f.Owner != d.Owner.Opponent())
                                return;
                            await d.Skill.Execute(f.Owner);
                            await d.Skill.Execute(f.Owner);
                        }),
                    }),
                new FormationEntry(JingJie.YuanYing, "无消耗牌，角色境界不低于元婴", "对方使用消耗牌后，自己也使用1次",
                    canActivate: (entity, args) =>
                    {
                        return args.ExhaustedCount == 0 && entity.GetJingJie() >= JingJie.YuanYing;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_EXHAUST, -1, async (listener, stageEventDetails) =>
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
                new FormationEntry(JingJie.HuaShen, "不少于9张非攻击牌", "战斗开始护甲+30，每回合护甲+3",
                    canActivate: (entity, args) =>
                    {
                        return args.NonAttackCount >= 9;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            StageDetails d = (StageDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;

                            await f.Owner.ArmorGainSelfProcedure(30);
                        }),
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            TurnDetails d = (TurnDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;

                            await d.Owner.ArmorGainSelfProcedure(3);
                        }),
                    }),
                new FormationEntry(JingJie.YuanYing, "不少于7张非攻击牌", "战斗开始护甲+20，每回合护甲+2",
                    canActivate: (entity, args) =>
                    {
                        return args.NonAttackCount >= 7;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            StageDetails d = (StageDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;

                            await f.Owner.ArmorGainSelfProcedure(20);
                        }),
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            TurnDetails d = (TurnDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;

                            await d.Owner.ArmorGainSelfProcedure(2);
                        }),
                    }),
                new FormationEntry(JingJie.JinDan, "不少于5张非攻击牌", "战斗开始护甲+10，每回合护甲+1",
                    canActivate: (entity, args) =>
                    {
                        return args.NonAttackCount >= 5;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            StageDetails d = (StageDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;

                            await f.Owner.ArmorGainSelfProcedure(10);
                        }),
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
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
                new FormationEntry(JingJie.HuaShen, "费用消耗超过20", "战斗开始时，灵气+7",
                    canActivate: (entity, args) =>
                    {
                        return args.TotalCostCount >= 20;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            StageDetails d = (StageDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;

                            await f.Owner.BuffSelfProcedure("灵气", 7);
                        }),
                    }),
                new FormationEntry(JingJie.YuanYing, "费用消耗超过16", "战斗开始时，灵气+5",
                    canActivate: (entity, args) =>
                    {
                        return args.TotalCostCount >= 16;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            StageDetails d = (StageDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;

                            await f.Owner.BuffSelfProcedure("灵气", 5);
                        }),
                    }),
                new FormationEntry(JingJie.JinDan, "费用消耗超过12", "战斗开始时，灵气+3",
                    canActivate: (entity, args) =>
                    {
                        return args.TotalCostCount >= 12;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
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
                new FormationEntry(JingJie.HuaShen, "连续7张攻击牌", "战斗开始时，力量+3",
                    canActivate: (entity, args) =>
                    {
                        return args.HighestConsecutiveAttackCount >= 7;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            StageDetails d = (StageDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;

                            await f.Owner.BuffSelfProcedure("力量", 3);
                        }),
                    }),
                new FormationEntry(JingJie.YuanYing, "连续6张攻击牌", "战斗开始时，力量+2",
                    canActivate: (entity, args) =>
                    {
                        return args.HighestConsecutiveAttackCount >= 6;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
                        {
                            Formation f = (Formation)listener;
                            StageDetails d = (StageDetails)stageEventDetails;
                            if (f.Owner != d.Owner) return;

                            await f.Owner.BuffSelfProcedure("力量", 2);
                        }),
                    }),
                new FormationEntry(JingJie.JinDan, "连续5张攻击牌", "战斗开始时，力量+1",
                    canActivate: (entity, args) =>
                    {
                        return args.HighestConsecutiveAttackCount >= 5;
                    },
                    eventDescriptors: new CLEventDescriptor[]
                    {
                        new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STAGE, 0, async (listener, stageEventDetails) =>
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
