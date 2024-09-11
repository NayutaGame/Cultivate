
using System.Collections.Generic;
using CLLibrary;

public class FormationCategory : Category<FormationGroupEntry>
{
    public FormationCategory()
    {
        AddRange(new List<FormationGroupEntry>()
        {
            new(id: "金灵阵",
                order: 0,
                contributorPred: s => s.GetWuXing() == WuXing.Jin,
                progressDescription: "携带越多金牌越强大",
                progressEvaluator: (e, d) => d.WuXingCounts[WuXing.Jin] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                11,
                        trivia:                                                     null,
                        rewardDescription:                                          "\n开局及每轮：获得1暴击" +
                                                                                    "\n击伤时：施加6减甲" +
                                                                                    "\n开局效果额外触发一次" +
                                                                                    "死亡不会停止战斗",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;
                                
                                await f.Owner.GainBuffProcedure("暴击");
                                await f.Owner.GainBuffProcedure("轮暴击");
                                await f.Owner.GainBuffProcedure("诸行无常", 6);
                                await f.Owner.GainBuffProcedure("人间无戈");
                            }),
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_START_STAGE_CAST, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StartStageCastDetails d = (StartStageCastDetails)stageEventDetails;

                                if (f.Owner != d.Caster)
                                    return;

                                d.Times += 1;
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1暴击" +
                                                                                    "\n击伤时：施加6减甲" +
                                                                                    "\n开局效果额外触发一次",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("暴击");
                                await f.Owner.GainBuffProcedure("轮暴击");
                                await f.Owner.GainBuffProcedure("诸行无常", 6);
                            }),
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_START_STAGE_CAST, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StartStageCastDetails d = (StartStageCastDetails)stageEventDetails;

                                if (f.Owner != d.Caster)
                                    return;

                                d.Times += 1;
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1暴击" +
                                                                                    "\n击伤时：施加6减甲",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("暴击");
                                await f.Owner.GainBuffProcedure("轮暴击");
                                await f.Owner.GainBuffProcedure("诸行无常", 6);
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1暴击",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("暴击");
                                await f.Owner.GainBuffProcedure("轮暴击");
                            }),
                        }),
                }),

            new(id: "水灵阵",
                order: 0,
                contributorPred: s => s.GetWuXing() == WuXing.Shui,
                progressDescription: "携带越多水牌越强大",
                progressEvaluator: (e, d) => d.WuXingCounts[WuXing.Shui] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                11,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1吸血" +
                                                                                    "\n开局：获得1二动" +
                                                                                    "\n所有耗蓝-1" +
                                                                                    "\n八动，如果受伤则死亡",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("吸血");
                                await f.Owner.GainBuffProcedure("轮吸血");
                                await f.Owner.GainBuffProcedure("二动");
                                await f.Owner.GainBuffProcedure("心斋");
                            }),
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                TurnDetails d = (TurnDetails)stageEventDetails;

                                if (f.Owner != d.Owner) return;
                                if (f.Owner.GetStackOfBuff("摩诃钵特摩") > 0)
                                    return;
                                
                                // b.PlayPingAnimation();
                                await f.Owner.GainBuffProcedure("摩诃钵特摩");
                                d.Owner.SetActionPoint(d.Owner.GetActionPoint() + 8);
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1吸血" +
                                                                                    "\n开局：获得1二动" +
                                                                                    "\n所有耗蓝-1",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("吸血");
                                await f.Owner.GainBuffProcedure("轮吸血");
                                await f.Owner.GainBuffProcedure("二动");
                                await f.Owner.GainBuffProcedure("心斋");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1吸血" +
                                                                                    "\n开局：获得1二动",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("吸血");
                                await f.Owner.GainBuffProcedure("轮吸血");
                                await f.Owner.GainBuffProcedure("二动");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1吸血",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("吸血");
                                await f.Owner.GainBuffProcedure("轮吸血");
                            }),
                        }),
                }),

            new(id: "木灵阵",
                order: 0,
                contributorPred: s => s.GetWuXing() == WuXing.Mu,
                progressDescription: "携带越多木牌越强大",
                progressEvaluator: (e, d) => d.WuXingCounts[WuXing.Mu] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                11,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1穿透" +
                                                                                    "\n第一张牌使用两次" +
                                                                                    "\n所有牌算作使用过一次" +
                                                                                    "\n永久穿透和集中",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("穿透");
                                await f.Owner.GainBuffProcedure("轮穿透");
                                await f.Owner.GainBuffProcedure("多重");
                                foreach (var s in f.Owner._skills)
                                    s.IncreaseCastedCount();
                                await f.Owner.GainBuffProcedure("通透世界");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1穿透" +
                                                                                    "\n第一张牌使用两次" +
                                                                                    "\n所有牌算作使用过一次",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("穿透");
                                await f.Owner.GainBuffProcedure("轮穿透");
                                await f.Owner.GainBuffProcedure("多重");
                                foreach (var s in f.Owner._skills)
                                    s.IncreaseCastedCount();
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1穿透" +
                                                                                    "\n第一张牌使用两次",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;
                                
                                await f.Owner.GainBuffProcedure("穿透");
                                await f.Owner.GainBuffProcedure("轮穿透");
                                await f.Owner.GainBuffProcedure("多重");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1穿透",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;
                                
                                await f.Owner.GainBuffProcedure("穿透");
                                await f.Owner.GainBuffProcedure("轮穿透");
                            }),
                        }),
                }),

            new(id: "火灵阵",
                order: 0,
                contributorPred: s => s.GetWuXing() == WuXing.Huo,
                progressDescription: "携带越多火牌越强大",
                progressEvaluator: (e, d) => d.WuXingCounts[WuXing.Huo] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                11,
                        trivia:                                                     null,
                        rewardDescription:                                          "使用第一张牌后，变成疲劳" +
                                                                                    "\n燃命时：获得1灼烧" +
                                                                                    "\n暂缺" +
                                                                                    "\n每轮生命恢复至上限",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("疲劳");
                                await f.Owner.GainBuffProcedure("淬体");
                                await f.Owner.GainBuffProcedure("凤凰涅槃");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "使用第一张牌后，变成疲劳" +
                                                                                    "\n燃命时：获得1灼烧" +
                                                                                    "\n暂缺",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("疲劳");
                                await f.Owner.GainBuffProcedure("淬体");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "使用第一张牌后，变成疲劳" +
                                                                                    "\n燃命时：获得1灼烧",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("疲劳");
                                await f.Owner.GainBuffProcedure("淬体");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "使用第一张牌后，变成疲劳",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("疲劳");
                            }),
                        }),
                }),

            new(id: "土灵阵",
                order: 0,
                contributorPred: s => s.GetWuXing() == WuXing.Tu,
                progressDescription: "携带越多土牌越强大",
                progressEvaluator: (e, d) => d.WuXingCounts[WuXing.Tu] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                11,
                        trivia:                                                     null,
                        rewardDescription:                                          "架势消耗-1" +
                                                                                    "\n开局：获得25护甲" +
                                                                                    "\n开局准备好架势" +
                                                                                    "\n天人合一（未设计）",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                    
                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("架势消耗减少");
                                await f.Owner.GainArmorProcedure(25, induced: false);
                                await f.Owner.GainBuffProcedure("架势", 2);
                                // await f.Owner.GainBuffProcedure("天人合一");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "架势消耗-1" +
                                                                                    "\n开局：获得25护甲" +
                                                                                    "\n开局准备好架势",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("架势消耗减少");
                                await f.Owner.GainArmorProcedure(25, induced: false);
                                await f.Owner.GainBuffProcedure("架势", 2);
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "架势消耗-1" +
                                                                                    "\n开局：获得25护甲",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("架势消耗减少");
                                await f.Owner.GainArmorProcedure(25, induced: false);
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "架势消耗-1",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("架势消耗减少");
                            }),
                        }),
                }),

            // new(id: "攻击阵",
            //     order: 0,
            //     contributorPred: s => s.GetSkillTypeComposite().Contains(SkillType.Attack),
            //     progressDescription: "携带越多攻击牌越强大",
            //     progressEvaluator: (e, d) => d.AttackCount + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.HuaShen,
            //             requirement:                                                10,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "化神攻击阵效果\n元婴攻击阵效果\n金丹攻击阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //
            //                     if (f.Owner != d.Owner)
            //                         return;
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.YuanYing,
            //             requirement:                                                8,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "元婴攻击阵效果\n金丹攻击阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //
            //                     if (f.Owner != d.Owner)
            //                         return;
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.JinDan,
            //             requirement:                                                6,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "金丹攻击阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //
            //                     if (f.Owner != d.Owner)
            //                         return;
            //                 }),
            //             }),
            //     }),
            //
            // new(id: "防御阵",
            //     order: 0,
            //     contributorPred: s => s.GetSkillTypeComposite().Contains(SkillType.Defend),
            //     progressDescription: "携带越多防御牌越强大",
            //     progressEvaluator: (e, d) => d.TypeCounts[SkillType.Defend._index] + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.HuaShen,
            //             requirement:                                                9,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "金丹防御阵效果" +
            //                                                                         "\n元婴防御阵效果" +
            //                                                                         "\n每回合：6攻",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //
            //                     if (f.Owner != d.Owner)
            //                         return;
            //                     
            //                     await f.Owner.GainBuffProcedure("天衣无缝", 6);
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.YuanYing,
            //             requirement:                                                7,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "金丹防御阵效果" +
            //                                                                         "\n元婴防御阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //
            //                     if (f.Owner != d.Owner)
            //                         return;
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.JinDan,
            //             requirement:                                                5,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "金丹防御阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //
            //                     if (f.Owner != d.Owner)
            //                         return;
            //                 }),
            //             }),
            //     }),
            
            new(id: "灵气阵",
                order: 0,
                contributorPred: s => s.GetSkillTypeComposite().Contains(SkillType.Mana),
                progressDescription: "携带越多灵气牌越强大",
                progressEvaluator: (e, d) => d.TypeCounts[SkillType.Mana._index] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：获得4灵气" +
                                                                                    "\n获得灵气时：每1，回复2生命" +
                                                                                    "\n每回合：获得1灵气",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
            
                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("灵气", 4);
                                await f.Owner.GainBuffProcedure("清心", 2);
                                await f.Owner.GainBuffProcedure("抱朴");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                4,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：获得4灵气" +
                                                                                    "\n获得灵气时：每1，回复2生命",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
            
                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("灵气", 4);
                                await f.Owner.GainBuffProcedure("清心", 2);
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                3,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：获得4灵气",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
            
                                if (f.Owner != d.Owner)
                                    return;

                                await f.Owner.GainBuffProcedure("灵气", 4);
                            }),
                        }),
                }),
            
            new(id: "治疗阵",
                order: 0,
                contributorPred: s => s.GetSkillTypeComposite().Contains(SkillType.Heal),
                progressDescription: "携带越多治疗牌越强大",
                progressEvaluator: (e, d) => d.TypeCounts[SkillType.Heal._index] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：生命上限+50" +
                                                                                    "\n受到治疗时：力量+1" +
                                                                                    "\n第二轮：生命回复到上限",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
            
                                if (f.Owner != d.Owner)
                                    return;

                                f.Owner.MaxHp += 50;
                                await f.Owner.GainBuffProcedure("盛开");
                                await f.Owner.GainBuffProcedure("太虚");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                4,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：生命上限+50" +
                                                                                    "\n受到治疗时：力量+1",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
            
                                if (f.Owner != d.Owner)
                                    return;

                                f.Owner.MaxHp += 50;
                                await f.Owner.GainBuffProcedure("盛开");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                3,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：生命上限+50",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
            
                                if (f.Owner != d.Owner)
                                    return;

                                f.Owner.MaxHp += 50;
                            }),
                        }),
                }),

            new(id: "混元阵",
                order: 0,
                contributorPred: s => s.GetWuXing() != null,
                progressDescription: "有五种五行，每种携带两张以激活",
                progressEvaluator: (e, d) =>
                {
                    int score = 0;
                    d.WuXingCounts.Do(count => score += count.ClampUpper(2));
                    return score + d.Proficiency;
                },
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                10,
                        trivia:                                                     null,
                        rewardDescription:                                          "化神混元阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                if (f.Owner != d.Owner)
                                    return;
                                
                                UnityEngine.Debug.Log("待实现：化神混元阵效果");
                            }),
                        }),
                }),

            // new("六爻化劫阵", order: 0, conditionDescription: "非二动牌的数量越多越好",
            //     progressEvaluator: (e, d)
            //         => d.NonSwiftCount + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(JingJie.HuaShen, rewardDescription: "第二轮开始时，双方重置生命上限，回100%血", trivia: null, requirement: 12,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_FORMATION, StageEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     GainFormationDetails d = (GainFormationDetails)stageEventDetails;
            //
            //                     await f.Owner.GainBuffProcedure("六爻化劫", 100);
            //                 }),
            //             }),
            //         new FormationEntry(JingJie.YuanYing, rewardDescription: "第二轮开始时，双方重置生命上限，回30%血", trivia: null, requirement: 10,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_FORMATION, StageEventDict.GAIN_FORMATION, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     GainFormationDetails d = (GainFormationDetails)stageEventDetails;
            //
            //                     await f.Owner.GainBuffProcedure("六爻化劫", 30);
            //                 }),
            //             }),
            //     }),
            //
            // new("七曜移星阵", order: 0, conditionDescription: "二动牌的数量越多越好",
            //     progressEvaluator: (e, d)
            //         => d.SwiftCount + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(JingJie.HuaShen, rewardDescription: "轮开始时，对方遭受1跳回合\n战斗开始时，对方遭受2跳回合", trivia: null, requirement: 6,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GiveBuffProcedure("跳回合", 2);
            //                 }),
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ROUND, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     RoundDetails d = (RoundDetails)stageEventDetails;
            //
            //                     if (f.Owner == d.Owner)
            //                         await f.Owner.GiveBuffProcedure("跳回合");
            //                 }),
            //             }),
            //         new FormationEntry(JingJie.YuanYing, rewardDescription: "战斗开始时，对方遭受2跳回合", trivia: null, requirement: 5,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GiveBuffProcedure("跳回合", 2);
            //                 }),
            //             }),
            //         new FormationEntry(JingJie.JinDan, rewardDescription: "战斗开始时，对方遭受1跳回合", trivia: null, requirement: 4,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GiveBuffProcedure("跳回合");
            //                 }),
            //             }),
            //     }),
            //
            // new("八卦奇门阵", order: -1, conditionDescription: "非疲劳牌的数量越多越好",
            //     progressEvaluator: (e, d)
            //         => d.NonExhaustedCount + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(JingJie.HuaShen, rewardDescription: "对方卡牌变成疲劳后，自己也使用2次", trivia: null, requirement: 12,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_EXHAUST, -1, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     ExhaustDetails d = (ExhaustDetails)stageEventDetails;
            //                     // if (f.Owner != d.Owner.Opponent())
            //                     //     return;
            //                     // await f.Owner.CastProcedure(d.Skill);
            //                     // await f.Owner.CastProcedure(d.Skill);
            //                 }),
            //             }),
            //         new FormationEntry(JingJie.YuanYing, rewardDescription: "对方卡牌变成疲劳后，自己也使用1次", trivia: null, requirement: 10,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_EXHAUST, -1, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     ExhaustDetails d = (ExhaustDetails)stageEventDetails;
            //                     // if (f.Owner != d.Owner.Opponent())
            //                     //     return;
            //                     // await f.Owner.CastProcedure(d.Skill);
            //                 }),
            //             }),
            //     }),
            //
            // new("九宫迷踪阵", order: 0, conditionDescription: "非攻击牌越多越好",
            //     progressEvaluator: (e, d)
            //         => d.NonAttackCount + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(JingJie.HuaShen, rewardDescription: "战斗开始护甲+30，每回合护甲+3", trivia: null, requirement: 9,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GainArmorProcedure(30, induced: false);
            //                     await f.Owner.GainBuffProcedure("柔韧", 3);
            //                 }),
            //                 // new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
            //                 // {
            //                 //     Formation f = (Formation)listener;
            //                 //     TurnDetails d = (TurnDetails)stageEventDetails;
            //                 //     if (f.Owner != d.Owner) return;
            //                 //
            //                 //     await d.Owner.GainArmorProcedure(3);
            //                 // }),
            //             }),
            //         new FormationEntry(JingJie.YuanYing, rewardDescription: "战斗开始护甲+20，每回合护甲+2", trivia: null, requirement: 7,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GainArmorProcedure(20, induced: false);
            //                     await f.Owner.GainBuffProcedure("柔韧", 2);
            //                 }),
            //                 // new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
            //                 // {
            //                 //     Formation f = (Formation)listener;
            //                 //     TurnDetails d = (TurnDetails)stageEventDetails;
            //                 //     if (f.Owner != d.Owner) return;
            //                 //
            //                 //     await d.Owner.GainArmorProcedure(2);
            //                 // }),
            //             }),
            //         new FormationEntry(JingJie.JinDan, rewardDescription: "战斗开始护甲+10，每回合护甲+1", trivia: null, requirement: 5,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GainArmorProcedure(10, induced: false);
            //                     await f.Owner.GainBuffProcedure("柔韧", 1);
            //                 }),
            //                 // new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
            //                 // {
            //                 //     Formation f = (Formation)listener;
            //                 //     TurnDetails d = (TurnDetails)stageEventDetails;
            //                 //     if (f.Owner != d.Owner) return;
            //                 //
            //                 //     await d.Owner.GainArmorProcedure(1);
            //                 // }),
            //             }),
            //     }),
            //
            // new("千界聚灵阵", order: 0, conditionDescription: "卡牌原始费用之和越高越好",
            //     progressEvaluator: (e, d)
            //         => d.TotalCostCount + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(JingJie.HuaShen, rewardDescription: "战斗开始时，灵气+7", trivia: null, requirement: 20,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GainBuffProcedure("灵气", 7);
            //                 }),
            //             }),
            //         new FormationEntry(JingJie.YuanYing, rewardDescription: "战斗开始时，灵气+5", trivia: null, requirement: 16,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GainBuffProcedure("灵气", 5);
            //                 }),
            //             }),
            //         new FormationEntry(JingJie.JinDan, rewardDescription: "战斗开始时，灵气+3", trivia: null, requirement: 12,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GainBuffProcedure("灵气", 3);
            //                 }),
            //             }),
            //     }),
            //
            // new("万剑归宗阵", order: 0, conditionDescription: "连续攻击牌越多越好",
            //     progressEvaluator: (e, d)
            //         => d.HighestConsecutiveAttackCount + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(JingJie.HuaShen, rewardDescription: "战斗开始时，力量+3", trivia: null, requirement: 8,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GainBuffProcedure("力量", 3);
            //                 }),
            //             }),
            //         new FormationEntry(JingJie.YuanYing, rewardDescription: "战斗开始时，力量+2", trivia: null, requirement: 7,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GainBuffProcedure("力量", 2);
            //                 }),
            //             }),
            //         new FormationEntry(JingJie.JinDan, rewardDescription: "战斗开始时，力量+1", trivia: null, requirement: 6,
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     await f.Owner.GainBuffProcedure("力量");
            //                 }),
            //             }),
            //     }),
        });
    }
}
