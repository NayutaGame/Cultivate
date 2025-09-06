
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DifficultyCategory : Category<DifficultyEntry>
{
    public DifficultyCategory()
    {
        AddRange(new List<DifficultyEntry>()
        {
            new(id: "Difficulty0001",
                name: "0",
                order: 0,
                description: "基础的游戏体验",
                finalJingJie: JingJie.JinDan,
                homeAllowFormation: false,
                awayAllowFormation: false,
                allowRotate: false,
                allowMutate: false,
                enemyInitiate: false,
                allowFanXuMerge: false,
                allowFanXuBoss: false,
                fanXuBossEncore: false),
            
            new(id: "Difficulty0002",
                name: "1",
                order: 1,
                description: "可以到达元婴境界",
                finalJingJie: JingJie.YuanYing,
                homeAllowFormation: false,
                awayAllowFormation: false,
                allowRotate: false,
                allowMutate: false,
                enemyInitiate: false,
                allowFanXuMerge: false,
                allowFanXuBoss: false,
                fanXuBossEncore: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            
            new(id: "Difficulty0003",
                name: "2",
                order: 2,
                description: "解锁阵法规则",
                inheritedDifficultyNames: new string[] { "1" },
                finalJingJie: JingJie.YuanYing,
                homeAllowFormation: true,
                awayAllowFormation: false,
                allowRotate: false,
                allowMutate: false,
                enemyInitiate: false,
                allowFanXuMerge: false,
                allowFanXuBoss: false,
                fanXuBossEncore: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            
            new(id: "Difficulty0004",
                name: "3",
                order: 3,
                description: "可以到达化神境界",
                inheritedDifficultyNames: new string[] { "2", "1" },
                finalJingJie: JingJie.HuaShen,
                homeAllowFormation: true,
                awayAllowFormation: false,
                allowRotate: false,
                allowMutate: false,
                enemyInitiate: false,
                allowFanXuMerge: false,
                allowFanXuBoss: false,
                fanXuBossEncore: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            
            new(id: "Difficulty0005",
                name: "4",
                order: 4,
                description: "解锁流转规则",
                inheritedDifficultyNames: new string[] { "3", "2", "1" },
                finalJingJie: JingJie.HuaShen,
                homeAllowFormation: true,
                awayAllowFormation: false,
                allowRotate: true,
                allowMutate: false,
                enemyInitiate: false,
                allowFanXuMerge: false,
                allowFanXuBoss: false,
                fanXuBossEncore: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            
            new(id: "Difficulty0006",
                name: "5",
                order: 5,
                description: "敌人也可以使用阵法",
                inheritedDifficultyNames: new string[] { "4", "3", "2", "1" },
                finalJingJie: JingJie.HuaShen,
                homeAllowFormation: true,
                awayAllowFormation: true,
                allowRotate: true,
                allowMutate: false,
                enemyInitiate: false,
                allowFanXuMerge: false,
                allowFanXuBoss: false,
                fanXuBossEncore: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            
            new(id: "Difficulty0007",
                name: "6",
                order: 6,
                description: "解锁墨染规则",
                inheritedDifficultyNames: new string[] { "5", "4", "3", "2", "1" },
                finalJingJie: JingJie.HuaShen,
                homeAllowFormation: true,
                awayAllowFormation: true,
                allowRotate: true,
                allowMutate: true,
                enemyInitiate: false,
                allowFanXuMerge: false,
                allowFanXuBoss: false,
                fanXuBossEncore: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            
            new(id: "Difficulty0008",
                name: "7",
                order: 7,
                description: "敌人获得先手",
                inheritedDifficultyNames: new string[] { "6", "5", "4", "3", "2", "1" },
                finalJingJie: JingJie.HuaShen,
                homeAllowFormation: true,
                awayAllowFormation: true,
                allowRotate: true,
                allowMutate: true,
                enemyInitiate: true,
                allowFanXuMerge: false,
                allowFanXuBoss: false,
                fanXuBossEncore: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            
            new(id: "Difficulty0009",
                name: "8",
                order: 8,
                description: "解锁返虚合成",
                inheritedDifficultyNames: new string[] { "7", "6", "5", "4", "3", "2", "1" },
                finalJingJie: JingJie.HuaShen,
                homeAllowFormation: true,
                awayAllowFormation: true,
                allowRotate: true,
                allowMutate: true,
                enemyInitiate: true,
                allowFanXuMerge: true,
                allowFanXuBoss: false,
                fanXuBossEncore: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            
            new(id: "Difficulty0010",
                name: "9",
                order: 9,
                description: "解锁返虚战斗",
                inheritedDifficultyNames: new string[] { "8", "7", "6", "5", "4", "3", "2", "1" },
                finalJingJie: JingJie.FanXu,
                homeAllowFormation: true,
                awayAllowFormation: true,
                allowRotate: true,
                allowMutate: true,
                enemyInitiate: true,
                allowFanXuMerge: true,
                allowFanXuBoss: true,
                fanXuBossEncore: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            
            new(id: "Difficulty0011",
                name: "10",
                order: 10,
                description: "返虚Boss需要击败两次",
                inheritedDifficultyNames: new string[] { "9", "8", "7", "6", "5", "4", "3", "2", "1" },
                finalJingJie: JingJie.FanXu,
                homeAllowFormation: true,
                awayAllowFormation: true,
                allowRotate: true,
                allowMutate: true,
                enemyInitiate: true,
                allowFanXuMerge: true,
                allowFanXuBoss: true,
                fanXuBossEncore: true,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, closure, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
        });
    }

    public DifficultyEntry GetNext(DifficultyEntry entry)
    {
        int index = List.IndexOf(entry);
        if (index == -1 || index == List.Count() - 1)
            return null;
        return List[index + 1];
    }
}
