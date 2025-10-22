
using System.Collections.Generic;

public class MapNodeCategory : Category<MapNodeEntry>
{
    public MapNodeCategory()
    {
        AddRange(new List<MapNodeEntry>()
        {
            new(id:                       "MapNode0001",
                name:                     "凌云峰",
                scripts:                  new List<RoomScript>()
                {
                    // int baseGoldReward = RoomDefinition.GetGoldRewardFromLadder(ladder);
                    // DialogCell A = new DialogCell(
                    //         titleText: "存钱",
                    //         detailedText: $"获得了{baseGoldReward}金钱")
                    //     .SetReward(Reward.FromGold(baseGoldReward));
                    // return A;
                    
                    // new WriteBuffer("ladder"),
                    // new ReadConstant("GoldRewardFromLadder"),
                    // new BuildString("获得了{0}金钱"),
                    // new DeclareVariable("detailedText"),
                    //
                    // new WriteBuffer("detailedText"),
                    // new WriteBuffer("存钱"),
                    // new DeclareDialogCell("A", "buffer", "buffer"),
                    // new DialogCellSetReward("A", ...),
                }),
            new(id:                       "MapNode0002",
                name:                     "逍遥海",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0003",
                name:                     "天机阁",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0004",
                name:                     "长明殿",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0005",
                name:                     "环岳岭",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0006",
                name:                     "易宝斋",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0007",
                name:                     "剑池",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0008",
                name:                     "风雨楼",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0009",
                name:                     "百草堂",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0010",
                name:                     "星宫",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0011",
                name:                     "大椿树",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0012",
                name:                     "蓬莱阁",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0013",
                name:                     "桃花林",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0014",
                name:                     "明心庐",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
            new(id:                       "MapNode0015",
                name:                     "坠星湖",
                scripts:                  new List<RoomScript>()
                {
                    
                }),
        });
    }
}
