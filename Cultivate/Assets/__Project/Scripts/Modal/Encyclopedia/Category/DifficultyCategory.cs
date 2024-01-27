
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DifficultyCategory : Category<DifficultyEntry>
{
    public DifficultyCategory()
    {
        AddRange(new List<DifficultyEntry>()
        {
            new("-1", description: "敌人生命上限减少30%，不会受到命元惩罚",
                stageEventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("0", description: "没有变化"),
            new("1", description: "敌人生命上限增加10%",
                stageEventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("2", description: "跨越境界时回复的命元-1",  _additionalDifficultyNames: new string[] { "1" },
                stageEventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("3", description: "敌人获得1力量",  _additionalDifficultyNames: new string[] { "2", "1" },
                stageEventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("4", description: "商店的价格+50%",  _additionalDifficultyNames: new string[] { "3", "2", "1" },
                stageEventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("5", description: "敌人获得1格挡",  _additionalDifficultyNames: new string[] { "4", "3", "2", "1" },
                stageEventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("6", description: "跨越境界时不再提供升级",  _additionalDifficultyNames: new string[] { "5", "4", "3", "2", "1" },
                stageEventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("7", description: "敌人获得1免疫",  _additionalDifficultyNames: new string[] { "6", "5", "4", "3", "2", "1" },
                stageEventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("8", description: "主角生命上限-10%",  _additionalDifficultyNames: new string[] { "7", "6", "5", "4", "3", "2", "1" },
                stageEventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("9", description: "敌人获得先手", _additionalDifficultyNames: new string[] { "8", "7", "6", "5", "4", "3", "2", "1" },
                stageEventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, eventDetails) =>
                    {
                        StageEnvironment env = (StageEnvironment)listener;
                        StageDetails d = (StageDetails)eventDetails;
                    }),
                }),
            new("10", description: "最终Boss需要击败两次", _additionalDifficultyNames: new string[] { "9", "8", "7", "6", "5", "4", "3", "2", "1" },
                stageEventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STAGE, 0, async (listener, eventDetails) =>
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
