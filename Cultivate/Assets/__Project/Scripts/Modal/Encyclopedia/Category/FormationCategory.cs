
using System.Collections.Generic;
using CLLibrary;

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
                        rewardDescription:                                          "2张：开局及每轮：获得1暴击" +
                                                                                    "\n4张：击伤时：施加6减甲" +
                                                                                    "\n6张：开局效果额外触发一次" +
                                                                                    "\n9张：死亡不会停止战斗",
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
                        rewardDescription:                                          "2张：开局及每轮：获得1暴击" +
                        "\n4张：击伤时：施加6减甲" +
                        "\n6张：开局效果额外触发一次" +
                        "\n9张：死亡不会停止战斗".ApplyInactive(),
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
                        rewardDescription:                                          "2张：开局及每轮：获得1暴击" +
                        "\n4张：击伤时：施加6减甲" +
                        "\n6张：开局效果额外触发一次".ApplyInactive() +
                        "\n9张：死亡不会停止战斗".ApplyInactive(),
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
                        rewardDescription:                                          "2张：开局及每轮：获得1暴击" +
                        "\n4张：击伤时：施加6减甲".ApplyInactive() +
                        "\n6张：开局效果额外触发一次".ApplyInactive() +
                        "\n9张：死亡不会停止战斗".ApplyInactive(),
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
                    new FormationEntry(
                        jingJie:                                                    JingJie.LianQi,
                        requirement:                                                0,
                        trivia:                                                     null,
                        rewardDescription:                                          "2张：开局及每轮：获得1暴击".ApplyInactive() +
                        "\n4张：击伤时：施加6减甲".ApplyInactive() +
                        "\n6张：开局效果额外触发一次".ApplyInactive() +
                        "\n9张：死亡不会停止战斗".ApplyInactive()),
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
                        rewardDescription:                                          "2张：开局：获得1二动" +
                                                                                    "\n4张：开局及每轮：获得1吸血" +
                                                                                    "\n6张：所有耗蓝-1" +
                                                                                    "\n9张：八动，如果受伤则死亡",
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
                        rewardDescription:                                          "2张：开局：获得1二动" +
                        "\n4张：开局及每轮：获得1吸血" +
                        "\n6张：所有耗蓝-1" +
                        "\n9张：八动，如果受伤则死亡".ApplyInactive(),
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
                        rewardDescription:                                          "2张：开局：获得1二动" +
                        "\n4张：开局及每轮：获得1吸血" +
                        "\n6张：所有耗蓝-1".ApplyInactive() +
                        "\n9张：八动，如果受伤则死亡".ApplyInactive(),
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
                        rewardDescription:                                          "2张：开局：获得1二动" +
                        "\n4张：开局及每轮：获得1吸血".ApplyInactive() +
                        "\n6张：所有耗蓝-1".ApplyInactive() +
                        "\n9张：八动，如果受伤则死亡".ApplyInactive(),
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
                    new FormationEntry(
                        jingJie:                                                    JingJie.LianQi,
                        requirement:                                                0,
                        trivia:                                                     null,
                        rewardDescription:                                          "2张：开局：获得1二动".ApplyInactive() +
                        "\n4张：开局及每轮：获得1吸血".ApplyInactive() +
                        "\n6张：所有耗蓝-1".ApplyInactive() +
                        "\n9张：八动，如果受伤则死亡".ApplyInactive()),
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
                        rewardDescription:                                          "2张：第一张牌使用两次" +
                                                                                    "\n4张：开局及每轮：获得1穿透" +
                                                                                    "\n6张：所有牌算作使用过一次" +
                                                                                    "\n9张：永久穿透和集中",
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
                        rewardDescription:                                          "2张：第一张牌使用两次" +
                        "\n4张：开局及每轮：获得1穿透" +
                        "\n6张：所有牌算作使用过一次" +
                        "\n9张：永久穿透和集中".ApplyInactive(),
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
                        rewardDescription:                                          "2张：第一张牌使用两次" +
                        "\n4张：开局及每轮：获得1穿透" +
                        "\n6张：所有牌算作使用过一次".ApplyInactive() +
                        "\n9张：永久穿透和集中".ApplyInactive(),
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
                        rewardDescription:                                          "2张：第一张牌使用两次" +
                        "\n4张：开局及每轮：获得1穿透".ApplyInactive() +
                        "\n6张：所有牌算作使用过一次".ApplyInactive() +
                        "\n9张：永久穿透和集中".ApplyInactive(),
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
                    new FormationEntry(
                        jingJie:                                                    JingJie.LianQi,
                        requirement:                                                0,
                        trivia:                                                     null,
                        rewardDescription:                                          "2张：第一张牌使用两次".ApplyInactive() +
                        "\n4张：开局及每轮：获得1穿透".ApplyInactive() +
                        "\n6张：所有牌算作使用过一次".ApplyInactive() +
                        "\n9张：永久穿透和集中".ApplyInactive()),
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
                        rewardDescription:                                          "2张：使用最后第一张牌时，使其升华" +
                                                                                    "\n4张：燃命时：获得1灼烧" +
                                                                                    "\n6张：暂缺" +
                                                                                    "\n9张：每轮气血恢复至上限",
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("最后一张牌升华");
                                await f.Owner.GainBuffProcedure("淬体");
                                await f.Owner.GainBuffProcedure("凤凰涅槃");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                6,
                        trivia:                                                     null,
                        rewardDescription:                                          "2张：使用最后第一张牌时，使其升华" +
                        "\n4张：燃命时：获得1灼烧" +
                        "\n6张：暂缺" +
                        "\n9张：每轮气血恢复至上限".ApplyInactive(),
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("最后一张牌升华");
                                await f.Owner.GainBuffProcedure("淬体");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                4,
                        trivia:                                                     null,
                        rewardDescription:                                          "2张：使用最后第一张牌时，使其升华" +
                        "\n4张：燃命时：获得1灼烧" +
                        "\n6张：暂缺".ApplyInactive() +
                        "\n9张：每轮气血恢复至上限".ApplyInactive(),
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("最后一张牌升华");
                                await f.Owner.GainBuffProcedure("淬体");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.ZhuJi,
                        requirement:                                                2,
                        trivia:                                                     null,
                        rewardDescription:                                          "2张：使用最后第一张牌时，使其升华" +
                        "\n4张：燃命时：获得1灼烧".ApplyInactive() +
                        "\n6张：暂缺".ApplyInactive() +
                        "\n9张：每轮气血恢复至上限".ApplyInactive(),
                        closures: new StageClosure[]
                        {
                            new(StageClosureDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                if (f.Owner != d.Owner) return;

                                await f.Owner.GainBuffProcedure("最后一张牌升华");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.LianQi,
                        requirement:                                                0,
                        trivia:                                                     null,
                        rewardDescription:                                          "2张：使用最后第一张牌时，使其升华".ApplyInactive() +
                        "\n4张：燃命时：获得1灼烧".ApplyInactive() +
                        "\n6张：暂缺".ApplyInactive() +
                        "\n9张：每轮气血恢复至上限".ApplyInactive()),
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
                        rewardDescription:                                          "2张：架势消耗-1" +
                                                                                    "\n4张：开局：获得25护甲" +
                                                                                    "\n6张：开局准备好架势" +
                                                                                    "\n9张：天人合一（未设计）",
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
                        rewardDescription:                                          "2张：架势消耗-1" +
                        "\n4张：开局：获得25护甲" +
                        "\n6张：开局准备好架势" +
                        "\n9张：天人合一（未设计）".ApplyInactive(),
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
                        rewardDescription:                                          "2张：架势消耗-1" +
                        "\n4张：开局：获得25护甲" +
                        "\n6张：开局准备好架势".ApplyInactive() +
                        "\n9张：天人合一（未设计）".ApplyInactive(),
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
                        rewardDescription:                                          "2张：架势消耗-1" +
                        "\n4张：开局：获得25护甲".ApplyInactive() +
                        "\n6张：开局准备好架势".ApplyInactive() +
                        "\n9张：天人合一（未设计）".ApplyInactive(),
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
                    new FormationEntry(
                        jingJie:                                                    JingJie.LianQi,
                        requirement:                                                0,
                        trivia:                                                     null,
                        rewardDescription:                                          "2张：架势消耗-1".ApplyInactive() +
                        "\n4张：开局：获得25护甲".ApplyInactive() +
                        "\n6张：开局准备好架势".ApplyInactive() +
                        "\n9张：天人合一（未设计）".ApplyInactive()),
                }),
            
            // new(id: "攻击阵",
            //     order: 0,
            //     contributorPred: s => s.GetSkillTypeComposite().Contains(SkillType.Attack),
            //     progressDescription: "携带越多攻击牌越强大",
            //     progressEvaluator: (e, d) => d.TypeCounts[SkillType.Attack._index] + d.Proficiency,
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
                        rewardDescription:                                          "3张：开局：获得4灵气" +
                                                                                    "\n5张：每回合：获得1灵气" +
                                                                                    "\n7张：获得灵气时：每1，回复2气血",
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
                        rewardDescription:                                          "3张：开局：获得4灵气" +
                        "\n5张：每回合：获得1灵气" +
                        "\n7张：获得灵气时：每1，回复2气血".ApplyInactive(),
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
                        rewardDescription:                                          "3张：开局：获得4灵气" +
                        "\n5张：每回合：获得1灵气".ApplyInactive() +
                        "\n7张：获得灵气时：每1，回复2气血".ApplyInactive(),
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
                    new FormationEntry(
                        jingJie:                                                    JingJie.LianQi,
                        requirement:                                                0,
                        trivia:                                                     null,
                        rewardDescription:                                          "3张：开局：获得4灵气".ApplyInactive() +
                        "\n5张：每回合：获得1灵气".ApplyInactive() +
                        "\n7张：获得灵气时：每1，回复2气血".ApplyInactive()),
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
                        rewardDescription:                                          "3张：开局：气血及上限变为1.2倍" +
                                                                                    "\n5张：第二轮：气血回复到上限" +
                                                                                    "\n7张：受到治疗时：力量+1",
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
                        rewardDescription:                                          "3张：开局：气血及上限变为1.2倍" +
                        "\n5张：第二轮：气血回复到上限" +
                        "\n7张：受到治疗时：力量+1".ApplyInactive(),
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
                        rewardDescription:                                          "3张：开局：气血及上限变为1.2倍" +
                        "\n5张：第二轮：气血回复到上限".ApplyInactive() +
                        "\n7张：受到治疗时：力量+1".ApplyInactive(),
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
                    new FormationEntry(
                        jingJie:                                                    JingJie.LianQi,
                        requirement:                                                0,
                        trivia:                                                     null,
                        rewardDescription:                                          "3张：开局：气血及上限变为1.2倍".ApplyInactive() +
                        "\n5张：第二轮：气血回复到上限".ApplyInactive() +
                        "\n7张：受到治疗时：力量+1".ApplyInactive()),
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

    public void Init()
    {
        List.Do(entry => entry.GenerateAnnotations());
    }
}
