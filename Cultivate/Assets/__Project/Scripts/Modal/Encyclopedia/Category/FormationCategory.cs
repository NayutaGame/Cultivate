
using System.Collections.Generic;

public class FormationCategory : Category<FormationGroupEntry>
{
    public FormationCategory()
    {
        AddRange(new List<FormationGroupEntry>()
        {
            // new(id: "未知阵",
            //     order: 0,
            //     contributorPred: s => s.GetWuXing() == WuXing.Huo,
            //     progressDescription: "携带越多火牌越强大",
            //     progressEvaluator: (e, d) => d.WuXingCounts[WuXing.Huo] + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.HuaShen,
            //             requirement:                                                9,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "",
            //             closures: new StageClosure[]
            //             {
            //                 new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.YuanYing,
            //             requirement:                                                6,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "",
            //             closures: new StageClosure[]
            //             {
            //                 new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.JinDan,
            //             requirement:                                                4,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "",
            //             closures: new StageClosure[]
            //             {
            //                 new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.ZhuJi,
            //             requirement:                                                2,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "",
            //             closures: new StageClosure[]
            //             {
            //                 new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //                 }),
            //             }),
            //     }),
            
            new(id: "金灵阵",
                order: 0,
                contributorPred: s => s.GetWuXing() == WuXing.Jin,
                progressDescription: "携带越多金牌越强大",
                progressEvaluator: (e, d) => d.WuXingCounts[WuXing.Jin] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1暴击" +
                                                                                    "\n击伤时：施加6减甲" +
                                                                                    "\n开局效果额外触发一次" +
                                                                                    "\n死亡不会停止战斗",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                                
                                await f.Owner.GainBuffProcedure("暴击");
                                await f.Owner.GainBuffProcedure("轮暴击");
                                await f.Owner.GainBuffProcedure("诸行无常", 6);
                                await f.Owner.GainBuffProcedure("人间无戈");
                            }),
                            new(StageClosureDict.WIL_START_STAGE_CAST, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StartStageCastDetails d = (StartStageCastDetails)stageEventDetails;
                                if (f.Owner != d.Caster) return;

                                d.Times += 1;
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                6,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1暴击" +
                                                                                    "\n击伤时：施加6减甲" +
                                                                                    "\n开局效果额外触发一次",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("暴击");
                                await f.Owner.GainBuffProcedure("轮暴击");
                                await f.Owner.GainBuffProcedure("诸行无常", 6);
                            }),
                            new(StageClosureDict.WIL_START_STAGE_CAST, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StartStageCastDetails d = (StartStageCastDetails)stageEventDetails;
                                if (f.Owner != d.Caster) return;

                                d.Times += 1;
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                4,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1暴击" +
                                                                                    "\n击伤时：施加6减甲",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("暴击");
                                await f.Owner.GainBuffProcedure("轮暴击");
                                await f.Owner.GainBuffProcedure("诸行无常", 6);
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                2,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局及每轮：获得1暴击",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

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
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：获得1二动" +
                                                                                    "\n开局及每轮：获得1吸血" +
                                                                                    "\n所有耗蓝-1" +
                                                                                    "\n八动，如果受伤则死亡",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("二动");
                                await f.Owner.GainBuffProcedure("吸血");
                                await f.Owner.GainBuffProcedure("轮吸血");
                                await f.Owner.GainBuffProcedure("心斋");
                            }),
                            new(StageClosureDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                TurnDetails d = (TurnDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                                
                                if (f.Owner.GetStackOfBuff("摩诃钵特摩") > 0) return;
                                
                                // b.PlayPingAnimation();
                                await f.Owner.GainBuffProcedure("摩诃钵特摩");
                                d.Owner.SetActionPoint(d.Owner.GetActionPoint() + 8);
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                6,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：获得1二动" +
                                                                                    "\n开局及每轮：获得1吸血" +
                                                                                    "\n所有耗蓝-1",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("二动");
                                await f.Owner.GainBuffProcedure("吸血");
                                await f.Owner.GainBuffProcedure("轮吸血");
                                await f.Owner.GainBuffProcedure("心斋");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                4,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：获得1二动" +
                                                                                    "\n开局及每轮：获得1吸血",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("二动");
                                await f.Owner.GainBuffProcedure("吸血");
                                await f.Owner.GainBuffProcedure("轮吸血");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                2,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：获得1二动",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("二动");
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
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "第一张牌使用两次" +
                                                                                    "\n开局及每轮：获得1穿透" +
                                                                                    "\n所有牌算作使用过一次" +
                                                                                    "\n永久穿透和集中",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("多重");
                                await f.Owner.GainBuffProcedure("穿透");
                                await f.Owner.GainBuffProcedure("轮穿透");
                                foreach (var s in f.Owner._skills)
                                    s.IncreaseCastedCount();
                                await f.Owner.GainBuffProcedure("通透世界");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                6,
                        trivia:                                                     null,
                        rewardDescription:                                          "第一张牌使用两次" +
                                                                                    "\n开局及每轮：获得1穿透" +
                                                                                    "\n所有牌算作使用过一次",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("多重");
                                await f.Owner.GainBuffProcedure("穿透");
                                await f.Owner.GainBuffProcedure("轮穿透");
                                foreach (var s in f.Owner._skills)
                                    s.IncreaseCastedCount();
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                4,
                        trivia:                                                     null,
                        rewardDescription:                                          "第一张牌使用两次" +
                                                                                    "\n开局及每轮：获得1穿透",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                                
                                await f.Owner.GainBuffProcedure("多重");
                                await f.Owner.GainBuffProcedure("穿透");
                                await f.Owner.GainBuffProcedure("轮穿透");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                2,
                        trivia:                                                     null,
                        rewardDescription:                                          "第一张牌使用两次",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                                
                                await f.Owner.GainBuffProcedure("多重");
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
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "使用第一张牌后，变成疲劳" +
                                                                                    "\n燃命时：获得1灼烧" +
                                                                                    "\n暂缺" +
                                                                                    "\n每轮气血恢复至上限",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("疲劳");
                                await f.Owner.GainBuffProcedure("淬体");
                                await f.Owner.GainBuffProcedure("凤凰涅槃");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                6,
                        trivia:                                                     null,
                        rewardDescription:                                          "使用第一张牌后，变成疲劳" +
                                                                                    "\n燃命时：获得1灼烧" +
                                                                                    "\n暂缺",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("疲劳");
                                await f.Owner.GainBuffProcedure("淬体");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                4,
                        trivia:                                                     null,
                        rewardDescription:                                          "使用第一张牌后，变成疲劳" +
                                                                                    "\n燃命时：获得1灼烧",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("疲劳");
                                await f.Owner.GainBuffProcedure("淬体");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                2,
                        trivia:                                                     null,
                        rewardDescription:                                          "使用第一张牌后，变成疲劳",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

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
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "架势消耗-1" +
                                                                                    "\n开局：获得25护甲" +
                                                                                    "\n开局准备好架势" +
                                                                                    "\n天人合一（未设计）",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("架势消耗减少");
                                await f.Owner.GainArmorProcedure(25);
                                await f.Owner.GainBuffProcedure("架势", 2);
                                // await f.Owner.GainBuffProcedure("天人合一");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                6,
                        trivia:                                                     null,
                        rewardDescription:                                          "架势消耗-1" +
                                                                                    "\n开局：获得25护甲" +
                                                                                    "\n开局准备好架势",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("架势消耗减少");
                                await f.Owner.GainArmorProcedure(25);
                                await f.Owner.GainBuffProcedure("架势", 2);
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                4,
                        trivia:                                                     null,
                        rewardDescription:                                          "架势消耗-1" +
                                                                                    "\n开局：获得25护甲",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("架势消耗减少");
                                await f.Owner.GainArmorProcedure(25);
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                2,
                        trivia:                                                     null,
                        rewardDescription:                                          "架势消耗-1",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("架势消耗减少");
                            }),
                        }),
                }),
            
            new(id: "攻击阵",
                order: 0,
                contributorPred: s => s.GetSkillTypeComposite().Contains(SkillType.Attack),
                progressDescription: "携带越多攻击牌越强大",
                progressEvaluator: (e, d) => d.TypeCounts[SkillType.Attack._index] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                6,
                        trivia:                                                     null,
                        rewardDescription:                                          "",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                4,
                        trivia:                                                     null,
                        rewardDescription:                                          "",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                2,
                        trivia:                                                     null,
                        rewardDescription:                                          "",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                            }),
                        }),
                }),
            
            new(id: "防御阵",
                order: 0,
                contributorPred: s => s.GetSkillTypeComposite().Contains(SkillType.Defend),
                progressDescription: "携带越多防御牌越强大",
                progressEvaluator: (e, d) => d.TypeCounts[SkillType.Defend._index] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                6,
                        trivia:                                                     null,
                        rewardDescription:                                          "",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                4,
                        trivia:                                                     null,
                        rewardDescription:                                          "",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                2,
                        trivia:                                                     null,
                        rewardDescription:                                          "",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                            }),
                        }),
                }),
            
            new(id: "灵气阵",
                order: 0,
                contributorPred: s => s.GetSkillTypeComposite().Contains(SkillType.Mana),
                progressDescription: "携带越多灵气牌越强大",
                progressEvaluator: (e, d) => d.TypeCounts[SkillType.Mana._index] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：获得4灵气" +
                                                                                    "\n每回合：获得1灵气" +
                                                                                    "\n获得灵气时：每1，回复2气血",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("灵气", 4);
                                await f.Owner.GainBuffProcedure("抱朴");
                                await f.Owner.GainBuffProcedure("清心", 2);
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：获得4灵气" +
                                                                                    "\n每回合：获得1灵气",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("灵气", 4);
                                await f.Owner.GainBuffProcedure("抱朴");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                3,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：获得4灵气",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("灵气", 4);
                            }),
                        }),
                }),
            
            new(id: "气血阵",
                order: 0,
                contributorPred: s => s.GetSkillTypeComposite().Contains(SkillType.Health),
                progressDescription: "携带越多气血牌越强大",
                progressEvaluator: (e, d) => d.TypeCounts[SkillType.Health._index] + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：气血及上限变为1.2倍" +
                                                                                    "\n第二轮：气血回复到上限" +
                                                                                    "\n受到治疗时：力量+1",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                int value = (int)(f.Owner.MaxHp * 0.2f);
                                f.Owner.MaxHp += value;
                                await f.Owner.HealProcedure(value);
                                await f.Owner.GainBuffProcedure("太虚");
                                await f.Owner.GainBuffProcedure("盛开");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：气血及上限变为1.2倍" +
                                                                                    "\n第二轮：气血回复到上限",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;
                                
                                int value = (int)(f.Owner.MaxHp * 0.2f);
                                f.Owner.MaxHp += value;
                                await f.Owner.HealProcedure(value);
                                await f.Owner.GainBuffProcedure("太虚");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                3,
                        trivia:                                                     null,
                        rewardDescription:                                          "开局：气血及上限变为1.2倍",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                int value = (int)(f.Owner.MaxHp * 0.2f);
                                f.Owner.MaxHp += value;
                                await f.Owner.HealProcedure(value);
                            }),
                        }),
                }),
            
            // new(id: "燃命阵",
            //     order: 0,
            //     contributorPred: s => s.GetCostDescription(),
            //     progressDescription: "携带越多气血牌越强大",
            //     progressEvaluator: (e, d) => d.TypeCounts[SkillType.Health._index] + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.HuaShen,
            //             requirement:                                                7,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "开局：气血及上限变为1.2倍" +
            //                                                                         "\n第二轮：气血回复到上限" +
            //                                                                         "\n受到治疗时：力量+1",
            //             closures: new StageClosure[]
            //             {
            //                 new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     int value = (int)(f.Owner.MaxHp * 0.2f);
            //                     f.Owner.MaxHp += value;
            //                     await f.Owner.HealProcedure(value, induced: false);
            //                     await f.Owner.GainBuffProcedure("太虚");
            //                     await f.Owner.GainBuffProcedure("盛开");
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.YuanYing,
            //             requirement:                                                5,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "开局：气血及上限变为1.2倍" +
            //                                                                         "\n第二轮：气血回复到上限",
            //             closures: new StageClosure[]
            //             {
            //                 new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //                     
            //                     int value = (int)(f.Owner.MaxHp * 0.2f);
            //                     f.Owner.MaxHp += value;
            //                     await f.Owner.HealProcedure(value, induced: false);
            //                     await f.Owner.GainBuffProcedure("太虚");
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.JinDan,
            //             requirement:                                                3,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "开局：气血及上限变为1.2倍",
            //             closures: new StageClosure[]
            //             {
            //                 new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     int value = (int)(f.Owner.MaxHp * 0.2f);
            //                     f.Owner.MaxHp += value;
            //                     await f.Owner.HealProcedure(value, induced: false);
            //                 }),
            //             }),
            //     }),
            //
            // new(id: "吟唱阵",
            //     order: 0,
            //     contributorPred: s => s.GetSkillTypeComposite().Contains(SkillType.Health),
            //     progressDescription: "携带越多气血牌越强大",
            //     progressEvaluator: (e, d) => d.TypeCounts[SkillType.Health._index] + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.HuaShen,
            //             requirement:                                                7,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "开局：气血及上限变为1.2倍" +
            //                                                                         "\n第二轮：气血回复到上限" +
            //                                                                         "\n受到治疗时：力量+1",
            //             closures: new StageClosure[]
            //             {
            //                 new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     int value = (int)(f.Owner.MaxHp * 0.2f);
            //                     f.Owner.MaxHp += value;
            //                     await f.Owner.HealProcedure(value, induced: false);
            //                     await f.Owner.GainBuffProcedure("太虚");
            //                     await f.Owner.GainBuffProcedure("盛开");
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.YuanYing,
            //             requirement:                                                5,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "开局：气血及上限变为1.2倍" +
            //                                                                         "\n第二轮：气血回复到上限",
            //             closures: new StageClosure[]
            //             {
            //                 new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //                     
            //                     int value = (int)(f.Owner.MaxHp * 0.2f);
            //                     f.Owner.MaxHp += value;
            //                     await f.Owner.HealProcedure(value, induced: false);
            //                     await f.Owner.GainBuffProcedure("太虚");
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.JinDan,
            //             requirement:                                                3,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "开局：气血及上限变为1.2倍",
            //             closures: new StageClosure[]
            //             {
            //                 new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     if (f.Owner != d.Owner) return;
            //
            //                     int value = (int)(f.Owner.MaxHp * 0.2f);
            //                     f.Owner.MaxHp += value;
            //                     await f.Owner.HealProcedure(value, induced: false);
            //                 }),
            //             }),
            //     }),
        });
    }
}
