
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DifficultyCategory : Category<DifficultyEntry>
{
    public DifficultyCategory()
    {
        AddRange(new List<DifficultyEntry>()
        {
            new("0", order: 0, description: "没有变化"),
            new("1", order: 1, description: "敌人气血上限增加10%",
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("2", order: 2, description: "跨越境界时回复的命元-1",  inheritedDifficultyNames: new string[] { "1" },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("3", order: 3, description: "敌人获得1力量",  inheritedDifficultyNames: new string[] { "2", "1" },
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("4", order: 4, description: "商店的价格+50%",  inheritedDifficultyNames: new string[] { "3", "2", "1" },
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
            new("10", order: 10, description: "最终Boss需要击败两次", inheritedDifficultyNames: new string[] { "9", "8", "7", "6", "5", "4", "3", "2", "1" },
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
