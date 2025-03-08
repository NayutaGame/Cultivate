
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DifficultyCategory : Category<DifficultyEntry>
{
    public DifficultyCategory()
    {
        AddRange(new List<DifficultyEntry>()
        {
            new("0", order: 0, description: "基础的游戏体验", finalJingJie: JingJie.JinDan,
                homeAllowFormation: false, awayAllowFormation: false, allowRotate: false),
            new("1", order: 1, description: "可以到达元婴境界", finalJingJie: JingJie.YuanYing,
                homeAllowFormation: false, awayAllowFormation: false, allowRotate: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("2", order: 2, description: "可以开始使用阵法", inheritedDifficultyNames: new string[] { "1" }, finalJingJie: JingJie.YuanYing,
                homeAllowFormation: true, awayAllowFormation: false, allowRotate: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("3", order: 3, description: "可以到达化神境界", inheritedDifficultyNames: new string[] { "2", "1" },
                homeAllowFormation: true, awayAllowFormation: false, allowRotate: false,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("4", order: 4, description: "解锁流转规则", inheritedDifficultyNames: new string[] { "3", "2", "1" },
                homeAllowFormation: true, awayAllowFormation: false, allowRotate: true,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("5", order: 5, description: "敌人也可以使用阵法", inheritedDifficultyNames: new string[] { "4", "3", "2", "1" },
                homeAllowFormation: true, awayAllowFormation: true, allowRotate: true,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("6", order: 6, description: "难度6", inheritedDifficultyNames: new string[] { "5", "4", "3", "2", "1" },
                homeAllowFormation: true, awayAllowFormation: true, allowRotate: true,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("7", order: 7, description: "难度7", inheritedDifficultyNames: new string[] { "6", "5", "4", "3", "2", "1" },
                homeAllowFormation: true, awayAllowFormation: true, allowRotate: true,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("8", order: 8, description: "敌人获得先手", inheritedDifficultyNames: new string[] { "7", "6", "5", "4", "3", "2", "1" },
                homeAllowFormation: true, awayAllowFormation: true, allowRotate: true,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("9", order: 9, description: "最终Boss需要击败两次", inheritedDifficultyNames: new string[] { "8", "7", "6", "5", "4", "3", "2", "1" },
                homeAllowFormation: true, awayAllowFormation: true, allowRotate: true,
                stageClosures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("10", order: 10, description: "萌新都玩这个难度", inheritedDifficultyNames: new string[] { "9", "8", "7", "6", "5", "4", "3", "2", "1" },
                homeAllowFormation: true, awayAllowFormation: true, allowRotate: true,
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

    public DifficultyEntry GetNext(DifficultyEntry entry)
    {
        int index = List.IndexOf(entry);
        if (index == -1 || index == List.Count - 1)
            return null;
        return List[index + 1];
    }
}
