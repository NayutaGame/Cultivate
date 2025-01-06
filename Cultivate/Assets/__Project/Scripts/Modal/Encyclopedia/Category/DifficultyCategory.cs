
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DifficultyCategory : Category<DifficultyEntry>
{
    public DifficultyCategory()
    {
        AddRange(new List<DifficultyEntry>()
        {
            new("0", order: 0, description: "基础的游戏体验"),
            new("1", order: 1, description: "可以到达元婴境界",
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("2", order: 2, description: "可以开始使用阵法",  inheritedDifficultyNames: new string[] { "1" },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("3", order: 3, description: "可以到达化神境界",  inheritedDifficultyNames: new string[] { "2", "1" },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("4", order: 4, description: "解锁流转规则",  inheritedDifficultyNames: new string[] { "3", "2", "1" },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("5", order: 5, description: "敌人获得1格挡",  inheritedDifficultyNames: new string[] { "4", "3", "2", "1" },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("6", order: 6, description: "跨越境界时不再提供升级",  inheritedDifficultyNames: new string[] { "5", "4", "3", "2", "1" },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("7", order: 7, description: "敌人获得1免疫",  inheritedDifficultyNames: new string[] { "6", "5", "4", "3", "2", "1" },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("8", order: 8, description: "主角气血上限-10%",  inheritedDifficultyNames: new string[] { "7", "6", "5", "4", "3", "2", "1" },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("9", order: 9, description: "敌人获得先手", inheritedDifficultyNames: new string[] { "8", "7", "6", "5", "4", "3", "2", "1" },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("10", order: 10, description: "萌新都玩这个难度", inheritedDifficultyNames: new string[] { "9", "8", "7", "6", "5", "4", "3", "2", "1" },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
        });
    }

    public void Init()
    {
        List.Do(entry => entry.CalcAdditionalDifficulties());
    }
}
