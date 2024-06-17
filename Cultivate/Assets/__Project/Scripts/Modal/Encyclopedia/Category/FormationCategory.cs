
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
                        requirement:                                                9,
                        trivia:                                                     null,
                        rewardDescription:                                          "20锋锐觉醒：死亡不会导致Stage结算\n奇偶同时激活两个效果\n击伤时：施加减甲，每携带1金，层数+1",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                await f.Owner.GainBuffProcedure("待激活的人间无戈");
                                await f.Owner.GainBuffProcedure("齐物论");
                                
                                int n = f.Owner.CountSuch(stageSkill => stageSkill.Entry.WuXing == WuXing.Jin);
                                await f.Owner.GainBuffProcedure("诸行无常", n);
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "20锋锐觉醒：死亡不会导致Stage结算\n击伤时：施加减甲，每携带1金，层数+1",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                await f.Owner.GainBuffProcedure("齐物论");
                                
                                int n = f.Owner.CountSuch(stageSkill => stageSkill.Entry.WuXing == WuXing.Jin);
                                await f.Owner.GainBuffProcedure("诸行无常", n);
                            }),
                        }),
                    // new FormationEntry(
                    //     jingJie:                                                    JingJie.JinDan,
                    //     requirement:                                                5,
                    //     trivia:                                                     null,
                    //     rewardDescription:                                          "击伤时：施加减甲，每携带1金，层数+1",
                    //     eventDescriptors: new StageEventDescriptor[]
                    //     {
                    //         new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                    //         {
                    //             Formation f = (Formation)listener;
                    //             StageDetails d = (StageDetails)stageEventDetails;
                    //             
                    //             int n = f.Owner.CountSuch(stageSkill => stageSkill.Entry.WuXing == WuXing.Jin);
                    //             await f.Owner.GainBuffProcedure("诸行无常", n);
                    //         }),
                    //     }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.LianQi,
                        requirement:                                                1,
                        trivia:                                                     null,
                        rewardDescription:                                          "击伤时：施加减甲，每携带1金，层数+1",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                int n = f.Owner.CountSuch(stageSkill => stageSkill.Entry.WuXing == WuXing.Jin);
                                await f.Owner.GainBuffProcedure("诸行无常", n);
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
                        rewardDescription:                                          "20格挡觉醒：连续行动八次，回合结束死亡\n元婴水灵阵效果\n所有攻击具有吸血，回合内未攻击：遭受1跳回合",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                await f.Owner.GainBuffProcedure("待激活的摩诃钵特摩");
                                UnityEngine.Debug.Log("待实现：元婴水灵阵效果");
                                await f.Owner.GainBuffProcedure("幻月狂乱");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "元婴水灵阵效果\n所有攻击具有吸血，回合内未攻击：遭受1跳回合",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                UnityEngine.Debug.Log("待实现：元婴水灵阵效果");
                                await f.Owner.GainBuffProcedure("幻月狂乱");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "所有攻击具有吸血，回合内未攻击：遭受1跳回合",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;

                                await f.Owner.GainBuffProcedure("幻月狂乱");
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
                        rewardDescription:                                          "20力量觉醒：攻击具有穿透\n元婴木灵阵效果\n金丹木灵阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                await f.Owner.GainBuffProcedure("待激活的通透世界");
                                UnityEngine.Debug.Log("待实现：元婴木灵阵效果");
                                UnityEngine.Debug.Log("待实现：金丹木灵阵效果");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "元婴木灵阵效果\n金丹木灵阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：元婴木灵阵效果");
                                UnityEngine.Debug.Log("待实现：金丹木灵阵效果");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "金丹木灵阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：金丹木灵阵效果");
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
                        rewardDescription:                                          "化神火灵阵效果\n元婴火灵阵效果\n金丹火灵阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：化神火灵阵效果");
                                UnityEngine.Debug.Log("待实现：元婴火灵阵效果");
                                UnityEngine.Debug.Log("待实现：金丹火灵阵效果");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "元婴火灵阵效果\n金丹火灵阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：元婴火灵阵效果");
                                UnityEngine.Debug.Log("待实现：金丹火灵阵效果");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "金丹火灵阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：金丹火灵阵效果");
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
                        rewardDescription:                                          "化神土灵阵效果\n元婴土灵阵效果\n金丹土灵阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：化神土灵阵效果");
                                UnityEngine.Debug.Log("待实现：元婴土灵阵效果");
                                UnityEngine.Debug.Log("待实现：金丹土灵阵效果");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                7,
                        trivia:                                                     null,
                        rewardDescription:                                          "元婴土灵阵效果\n金丹土灵阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：元婴土灵阵效果");
                                UnityEngine.Debug.Log("待实现：金丹土灵阵效果");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                5,
                        trivia:                                                     null,
                        rewardDescription:                                          "金丹土灵阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：金丹土灵阵效果");
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
                    d.WuXingOrder.Do(wuXing => score += d.WuXingCounts[wuXing].ClampUpper(2));
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
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：化神混元阵效果");
                            }),
                        }),
                }),

            new(id: "攻击阵",
                order: 0,
                contributorPred: s => s.GetSkillTypeComposite().Contains(SkillType.Attack),
                progressDescription: "携带越多攻击牌越强大",
                progressEvaluator: (e, d) => d.AttackCount + d.Proficiency,
                formationEntries: new[]
                {
                    new FormationEntry(
                        jingJie:                                                    JingJie.HuaShen,
                        requirement:                                                8,
                        trivia:                                                     null,
                        rewardDescription:                                          "化神攻击阵效果\n元婴攻击阵效果\n金丹攻击阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：化神攻击阵效果");
                                UnityEngine.Debug.Log("待实现：元婴攻击阵效果");
                                UnityEngine.Debug.Log("待实现：金丹攻击阵效果");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.YuanYing,
                        requirement:                                                6,
                        trivia:                                                     null,
                        rewardDescription:                                          "元婴攻击阵效果\n金丹攻击阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：元婴攻击阵效果");
                                UnityEngine.Debug.Log("待实现：金丹攻击阵效果");
                            }),
                        }),
                    new FormationEntry(
                        jingJie:                                                    JingJie.JinDan,
                        requirement:                                                4,
                        trivia:                                                     null,
                        rewardDescription:                                          "金丹攻击阵效果",
                        eventDescriptors: new StageEventDescriptor[]
                        {
                            new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
                            {
                                Formation f = (Formation)listener;
                                StageDetails d = (StageDetails)stageEventDetails;
                                
                                UnityEngine.Debug.Log("待实现：金丹攻击阵效果");
                            }),
                        }),
                }),

            // new("护甲阵", order: 0, conditionDescription: "携带越多护甲牌越强大",
            //     progressEvaluator: (e, d) => d.AttackCount + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.HuaShen,
            //             requirement:                                                8,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "化神护甲阵效果\n元婴护甲阵效果\n金丹护甲阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     
            //                     UnityEngine.Debug.Log("待实现：化神护甲阵效果");
            //                     UnityEngine.Debug.Log("待实现：元婴护甲阵效果");
            //                     UnityEngine.Debug.Log("待实现：金丹护甲阵效果");
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.YuanYing,
            //             requirement:                                                6,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "元婴护甲阵效果\n金丹护甲阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     
            //                     UnityEngine.Debug.Log("待实现：元婴护甲阵效果");
            //                     UnityEngine.Debug.Log("待实现：金丹护甲阵效果");
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.JinDan,
            //             requirement:                                                4,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "金丹护甲阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     
            //                     UnityEngine.Debug.Log("待实现：金丹护甲阵效果");
            //                 }),
            //             }),
            //     }),

            // new("灵气阵", order: 0, conditionDescription: "携带越多灵气牌越强大",
            //     progressEvaluator: (e, d) => d.AttackCount + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.HuaShen,
            //             requirement:                                                8,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "化神灵气阵效果\n元婴灵气阵效果\n金丹灵气阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     
            //                     UnityEngine.Debug.Log("待实现：化神灵气阵效果");
            //                     UnityEngine.Debug.Log("待实现：元婴灵气阵效果");
            //                     UnityEngine.Debug.Log("待实现：金丹灵气阵效果");
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.YuanYing,
            //             requirement:                                                6,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "元婴灵气阵效果\n金丹灵气阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     
            //                     UnityEngine.Debug.Log("待实现：元婴灵气阵效果");
            //                     UnityEngine.Debug.Log("待实现：金丹灵气阵效果");
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.JinDan,
            //             requirement:                                                4,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "金丹灵气阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     
            //                     UnityEngine.Debug.Log("待实现：金丹灵气阵效果");
            //                 }),
            //             }),
            //     }),
            //
            // new("治疗阵", order: 0, conditionDescription: "携带越多治疗牌越强大",
            //     progressEvaluator: (e, d) => d.AttackCount + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.HuaShen,
            //             requirement:                                                8,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "化神治疗阵效果\n元婴治疗阵效果\n金丹治疗阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     
            //                     UnityEngine.Debug.Log("待实现：化神治疗阵效果");
            //                     UnityEngine.Debug.Log("待实现：元婴治疗阵效果");
            //                     UnityEngine.Debug.Log("待实现：金丹治疗阵效果");
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.YuanYing,
            //             requirement:                                                6,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "元婴治疗阵效果\n金丹治疗阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     
            //                     UnityEngine.Debug.Log("待实现：元婴治疗阵效果");
            //                     UnityEngine.Debug.Log("待实现：金丹治疗阵效果");
            //                 }),
            //             }),
            //         new FormationEntry(
            //             jingJie:                                                    JingJie.JinDan,
            //             requirement:                                                4,
            //             trivia:                                                     null,
            //             rewardDescription:                                          "金丹治疗阵效果",
            //             eventDescriptors: new StageEventDescriptor[]
            //             {
            //                 new(StageEventDict.STAGE_FORMATION, StageEventDict.WIL_STAGE, 0, async (listener, stageEventDetails) =>
            //                 {
            //                     Formation f = (Formation)listener;
            //                     StageDetails d = (StageDetails)stageEventDetails;
            //                     
            //                     UnityEngine.Debug.Log("待实现：金丹治疗阵效果");
            //                 }),
            //             }),
            //     }),

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
            // new("八卦奇门阵", order: -1, conditionDescription: "非消耗牌的数量越多越好",
            //     progressEvaluator: (e, d)
            //         => d.NonExhaustedCount + d.Proficiency,
            //     formationEntries: new[]
            //     {
            //         new FormationEntry(JingJie.HuaShen, rewardDescription: "对方使用消耗牌后，自己也使用2次", trivia: null, requirement: 12,
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
            //         new FormationEntry(JingJie.YuanYing, rewardDescription: "对方使用消耗牌后，自己也使用1次", trivia: null, requirement: 10,
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
