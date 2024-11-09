
using System.Collections.Generic;

public class MapCategory : Category<MapEntry>
{
    public MapCategory()
    {
        AddRange(new List<MapEntry>()
        {
            new(id: "标准",
                envJingJie: JingJie.LianQi,
                slotCount: 3,
                gold: 3,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new RoomDescriptor[][]
                {
                    new RoomDescriptor[]
                    {
                        new DirectRoomDescriptor(0, "休息"),
                        new DirectRoomDescriptor(0, "出门"),
                        new BattleRoomDescriptor(0, 3, 4),
                        new AdventureRoomDescriptor(0),
                        // new RestRoomDescriptor(0),
                        new BattleRoomDescriptor(1, 4, 5),
                        new AscensionRoomDescriptor(0),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(2, 5, 6),
                        new AdventureRoomDescriptor(2),
                        new ShopRoomDescriptor(2),
                        new BattleRoomDescriptor(3, 6, 7),
                        new AdventureRoomDescriptor(3),
                        // new RestRoomDescriptor(3),
                        new BattleRoomDescriptor(4, 7, 8),
                        new AscensionRoomDescriptor(4),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(5, 8, 8),
                        new AdventureRoomDescriptor(5),
                        new ShopRoomDescriptor(5),
                        new BattleRoomDescriptor(5, 8, 9),
                        new AdventureRoomDescriptor(5),
                        new BattleRoomDescriptor(6, 9, 9),
                        new AdventureRoomDescriptor(6),
                        // new RestRoomDescriptor(6),
                        new BattleRoomDescriptor(7, 9, 10),
                        new AscensionRoomDescriptor(7),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(8, 10, 10),
                        new AdventureRoomDescriptor(8),
                        new BattleRoomDescriptor(8, 10, 11),
                        new AdventureRoomDescriptor(8),
                        new ShopRoomDescriptor(8),
                        new BattleRoomDescriptor(9, 11, 11),
                        new AdventureRoomDescriptor(9),
                        new BattleRoomDescriptor(9, 11, 12),
                        new AdventureRoomDescriptor(9),
                        // new RestRoomDescriptor(9),
                        new BattleRoomDescriptor(10, 12, 12),
                        new AscensionRoomDescriptor(10),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        // new RestRoomDescriptor(11),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        // new RestRoomDescriptor(12),
                        new ShopRoomDescriptor(12),
                        new BattleRoomDescriptor(13, 12, 12),
                        new SuccessRoomDescriptor(13),
                    },
                }),
            
            new(id: "序章",
                envJingJie: JingJie.LianQi,
                slotCount: 1,
                gold: 0,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new RoomDescriptor[][]
                {
                    new RoomDescriptor[]
                    {
                        new DirectRoomDescriptor(0, "漫画"),
                        new DirectRoomDescriptor(0, "教学1"),
                        new DirectRoomDescriptor(0, "教学2"),
                        new DirectRoomDescriptor(0, "教学3"),
                        new DirectRoomDescriptor(0, "教学4"),
                        new DirectRoomDescriptor(0, "教学5"),
                        new DirectRoomDescriptor(0, "教学6"),
                        new DirectRoomDescriptor(0, "教学7"),
                        new DirectRoomDescriptor(0, "教学8"),
                        new DirectRoomDescriptor(0, "教学9"),
                        new DirectRoomDescriptor(0, "教学10"),
                    },
                    new RoomDescriptor[]
                    {
                        new DirectRoomDescriptor(0, "出门"),
                        new BattleRoomDescriptor(0, 3, 4),
                        new AdventureRoomDescriptor(0),
                        // new RestRoomDescriptor(0),
                        new BattleRoomDescriptor(1, 4, 5),
                        new AscensionRoomDescriptor(0),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(2, 5, 6),
                        new AdventureRoomDescriptor(2),
                        new ShopRoomDescriptor(2),
                        new BattleRoomDescriptor(3, 6, 7),
                        new AdventureRoomDescriptor(3),
                        // new RestRoomDescriptor(3),
                        new BattleRoomDescriptor(4, 7, 8),
                        new AscensionRoomDescriptor(4),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(5, 8, 8),
                        new AdventureRoomDescriptor(5),
                        new ShopRoomDescriptor(5),
                        new BattleRoomDescriptor(5, 8, 9),
                        new AdventureRoomDescriptor(5),
                        new BattleRoomDescriptor(6, 9, 9),
                        new AdventureRoomDescriptor(6),
                        // new RestRoomDescriptor(6),
                        new BattleRoomDescriptor(7, 9, 10),
                        new AscensionRoomDescriptor(7),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(8, 10, 10),
                        new AdventureRoomDescriptor(8),
                        new BattleRoomDescriptor(8, 10, 11),
                        new AdventureRoomDescriptor(8),
                        new ShopRoomDescriptor(8),
                        new BattleRoomDescriptor(9, 11, 11),
                        new AdventureRoomDescriptor(9),
                        new BattleRoomDescriptor(9, 11, 12),
                        new AdventureRoomDescriptor(9),
                        // new RestRoomDescriptor(9),
                        new BattleRoomDescriptor(10, 12, 12),
                        new AscensionRoomDescriptor(10),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        // new RestRoomDescriptor(11),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        // new RestRoomDescriptor(12),
                        new ShopRoomDescriptor(12),
                        new BattleRoomDescriptor(13, 12, 12),
                        new SuccessRoomDescriptor(13),
                    },
                }),
            
            new(id: "发现",
                envJingJie: JingJie.LianQi,
                slotCount: 12,
                gold: 0,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new RoomDescriptor[][]
                {
                    new RoomDescriptor[]
                    {
                        // new DirectRoomDescriptor(0, "以物易物"),
                        new DirectRoomDescriptor(0, "发现一张牌"),
                        new DirectRoomDescriptor(0, "发现一张牌"),
                        new DirectRoomDescriptor(0, "发现一张牌"),
                        new DirectRoomDescriptor(0, "发现一张牌"),
                        new DirectRoomDescriptor(0, "发现一张牌"),
                        new DirectRoomDescriptor(0, "发现一张牌"),
                        new DirectRoomDescriptor(0, "发现一张牌"),
                        new AscensionRoomDescriptor(0),
                    },
                    new RoomDescriptor[]
                    {
                        new DirectRoomDescriptor(4, "发现一张牌"),
                        new DirectRoomDescriptor(4, "发现一张牌"),
                        new DirectRoomDescriptor(4, "发现一张牌"),
                        new DirectRoomDescriptor(4, "发现一张牌"),
                        new DirectRoomDescriptor(4, "发现一张牌"),
                        new DirectRoomDescriptor(4, "发现一张牌"),
                        new DirectRoomDescriptor(4, "发现一张牌"),
                        new AscensionRoomDescriptor(4),
                    },
                    new RoomDescriptor[]
                    {
                        new DirectRoomDescriptor(7, "发现一张牌"),
                        new DirectRoomDescriptor(7, "发现一张牌"),
                        new DirectRoomDescriptor(7, "发现一张牌"),
                        new DirectRoomDescriptor(7, "发现一张牌"),
                        new DirectRoomDescriptor(7, "发现一张牌"),
                        new DirectRoomDescriptor(7, "发现一张牌"),
                        new DirectRoomDescriptor(7, "发现一张牌"),
                        new AscensionRoomDescriptor(7),
                    },
                    new RoomDescriptor[]
                    {
                        new DirectRoomDescriptor(10, "发现一张牌"),
                        new DirectRoomDescriptor(10, "发现一张牌"),
                        new DirectRoomDescriptor(10, "发现一张牌"),
                        new DirectRoomDescriptor(10, "发现一张牌"),
                        new DirectRoomDescriptor(10, "发现一张牌"),
                        new DirectRoomDescriptor(10, "发现一张牌"),
                        new DirectRoomDescriptor(10, "发现一张牌"),
                        new AscensionRoomDescriptor(10),
                    },
                    new RoomDescriptor[]
                    {
                        new DirectRoomDescriptor(13, "发现一张牌"),
                        new DirectRoomDescriptor(13, "发现一张牌"),
                        new DirectRoomDescriptor(13, "发现一张牌"),
                        new DirectRoomDescriptor(13, "发现一张牌"),
                        new DirectRoomDescriptor(13, "发现一张牌"),
                        new DirectRoomDescriptor(13, "发现一张牌"),
                        new DirectRoomDescriptor(13, "发现一张牌"),
                        new SuccessRoomDescriptor(13),
                    },
                }),
            
            new(id: "筑基",
                envJingJie: JingJie.ZhuJi,
                slotCount: 5,
                gold: 5,
                skillJingJie: JingJie.LianQi,
                skillCount: 7,
                levels: new RoomDescriptor[][]
                {
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(2, 5, 6),
                        new AdventureRoomDescriptor(2),
                        new ShopRoomDescriptor(2),
                        new BattleRoomDescriptor(3, 6, 7),
                        new AdventureRoomDescriptor(3),
                        new RestRoomDescriptor(3),
                        new BattleRoomDescriptor(4, 7, 8),
                        new AscensionRoomDescriptor(4),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(5, 8, 8),
                        new AdventureRoomDescriptor(5),
                        new ShopRoomDescriptor(5),
                        new BattleRoomDescriptor(5, 8, 9),
                        new AdventureRoomDescriptor(5),
                        new BattleRoomDescriptor(6, 9, 9),
                        new AdventureRoomDescriptor(6),
                        new RestRoomDescriptor(6),
                        new BattleRoomDescriptor(7, 9, 10),
                        new AscensionRoomDescriptor(7),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(8, 10, 10),
                        new AdventureRoomDescriptor(8),
                        new BattleRoomDescriptor(8, 10, 11),
                        new AdventureRoomDescriptor(8),
                        new ShopRoomDescriptor(8),
                        new BattleRoomDescriptor(9, 11, 11),
                        new AdventureRoomDescriptor(9),
                        new BattleRoomDescriptor(9, 11, 12),
                        new AdventureRoomDescriptor(9),
                        new RestRoomDescriptor(9),
                        new BattleRoomDescriptor(10, 12, 12),
                        new AscensionRoomDescriptor(10),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        new RestRoomDescriptor(11),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        new RestRoomDescriptor(12),
                        new ShopRoomDescriptor(12),
                        new BattleRoomDescriptor(13, 12, 12),
                        new SuccessRoomDescriptor(13),
                    },
                }),
            
            new(id: "金丹",
                envJingJie: JingJie.JinDan,
                slotCount: 8,
                gold: 17,
                skillJingJie: JingJie.LianQi,
                skillCount: 13,
                levels: new RoomDescriptor[][]
                {
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(5, 8, 8),
                        new AdventureRoomDescriptor(5),
                        new ShopRoomDescriptor(5),
                        new BattleRoomDescriptor(5, 8, 9),
                        new AdventureRoomDescriptor(5),
                        new BattleRoomDescriptor(6, 9, 9),
                        new AdventureRoomDescriptor(6),
                        new RestRoomDescriptor(6),
                        new BattleRoomDescriptor(7, 9, 10),
                        new AscensionRoomDescriptor(7),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(8, 10, 10),
                        new AdventureRoomDescriptor(8),
                        new BattleRoomDescriptor(8, 10, 11),
                        new AdventureRoomDescriptor(8),
                        new ShopRoomDescriptor(8),
                        new BattleRoomDescriptor(9, 11, 11),
                        new AdventureRoomDescriptor(9),
                        new BattleRoomDescriptor(9, 11, 12),
                        new AdventureRoomDescriptor(9),
                        new RestRoomDescriptor(9),
                        new BattleRoomDescriptor(10, 12, 12),
                        new AscensionRoomDescriptor(10),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        new RestRoomDescriptor(11),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        new RestRoomDescriptor(12),
                        new ShopRoomDescriptor(12),
                        new BattleRoomDescriptor(13, 12, 12),
                        new SuccessRoomDescriptor(13),
                    },
                }),
            
            new(id: "元婴",
                envJingJie: JingJie.YuanYing,
                slotCount: 10,
                gold: 49,
                skillJingJie: JingJie.ZhuJi,
                skillCount: 15,
                levels: new RoomDescriptor[][]
                {
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(8, 10, 10),
                        new AdventureRoomDescriptor(8),
                        new BattleRoomDescriptor(8, 10, 11),
                        new AdventureRoomDescriptor(8),
                        new ShopRoomDescriptor(8),
                        new BattleRoomDescriptor(9, 11, 11),
                        new AdventureRoomDescriptor(9),
                        new BattleRoomDescriptor(9, 11, 12),
                        new AdventureRoomDescriptor(9),
                        new RestRoomDescriptor(9),
                        new BattleRoomDescriptor(10, 12, 12),
                        new AscensionRoomDescriptor(10),
                    },
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        new RestRoomDescriptor(11),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        new RestRoomDescriptor(12),
                        new ShopRoomDescriptor(12),
                        new BattleRoomDescriptor(13, 12, 12),
                        new SuccessRoomDescriptor(13),
                    },
                }),
            
            new(id: "化神",
                envJingJie: JingJie.HuaShen,
                slotCount: 12,
                gold: 129,
                skillJingJie: JingJie.JinDan,
                skillCount: 17,
                levels: new RoomDescriptor[][]
                {
                    new RoomDescriptor[]
                    {
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        new AdventureRoomDescriptor(11),
                        new BattleRoomDescriptor(11, 12, 12),
                        new RestRoomDescriptor(11),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        new AdventureRoomDescriptor(12),
                        new BattleRoomDescriptor(12, 12, 12),
                        new RestRoomDescriptor(12),
                        new ShopRoomDescriptor(12),
                        new BattleRoomDescriptor(13, 12, 12),
                        new SuccessRoomDescriptor(13),
                    },
                }),
            
            new(id: "化神决战",
                envJingJie: JingJie.HuaShen,
                slotCount: 12,
                gold: 289,
                skillJingJie: JingJie.JinDan,
                skillCount: 41,
                levels: new RoomDescriptor[][]
                {
                    new RoomDescriptor[]
                    {
                        new RestRoomDescriptor(12),
                        new ShopRoomDescriptor(12),
                        new BattleRoomDescriptor(13, 12, 12),
                        new SuccessRoomDescriptor(13),
                    },
                }),
            
            new(id: "测试",
                envJingJie: JingJie.LianQi,
                slotCount: 3,
                gold: 3,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new RoomDescriptor[][]
                {
                    new RoomDescriptor[]
                    {
                        new DirectRoomDescriptor(0, "连抽五张"),
                        //
                        new DirectRoomDescriptor(0, "教学1"),
                        new DirectRoomDescriptor(0, "教学2"),
                        new DirectRoomDescriptor(0, "教学3"),
                        new DirectRoomDescriptor(0, "教学4"),
                        new DirectRoomDescriptor(0, "教学5"),
                        new DirectRoomDescriptor(0, "教学6"),
                        new DirectRoomDescriptor(0, "教学7"),
                        new DirectRoomDescriptor(0, "教学8"),
                        new DirectRoomDescriptor(0, "教学9"),
                        new DirectRoomDescriptor(0, "教学10"),
                    },
                }),
        });
    }

    // public override MapEntry DefaultEntry() => this["0000"];
}
