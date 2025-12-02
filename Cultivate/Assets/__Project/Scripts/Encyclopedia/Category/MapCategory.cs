
using System.Collections.Generic;

public class MapCategory : Category<MapEntry>
{
    public MapCategory()
    {
        AddRange(new List<MapEntry>()
        {
            new(id: "Map0001",
                name: "测试",
                envJingJie: JingJie.JinDan,
                slotCount: 8,
                gold: 23,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "出门"),
                        new DirectRoomDefinition(0, "忘忧堂"),
                        new DirectRoomDefinition(0, "教学10"),
                        new DirectRoomDefinition(0, "天界树"),
                    },
                }),
            
            new(id: "Map0002",
                name: "标准",
                envJingJie: JingJie.LianQi,
                slotCount: 3,
                gold: 3,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "漫画", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("1")),
                        new DirectRoomDefinition(0, "教学1", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("1")),
                        new DirectRoomDefinition(0, "教学2", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("1")),
                        new DirectRoomDefinition(0, "教学3", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("1")),
                        new DirectRoomDefinition(0, "教学4", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("1")),
                        new DirectRoomDefinition(0, "教学5", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("1")),
                        
                        new DirectRoomDefinition(0, "教学8", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("3")
                            && env.GetRunConfig().GetDifficulty() == 2),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "出门", pred: (profile, env) =>
                            profile.DifficultyIsUnlocked("1")),
                        new DirectRoomDefinition(0, "序章出门", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("1")),
                        
                        new BattleRoomDefinition(0, 3, 4),
                        new AdventureRoomDefinition(0),
                        // new RestRoomDescriptor(0),
                        new BattleRoomDefinition(1, 4, 5),
                        new AscensionRoomDefinition(0),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(2, "教学6", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("1")),
                        
                        new DirectRoomDefinition(2, "教学10", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("5")
                            && env.GetRunConfig().GetDifficulty() == 4),
                        
                        new BattleRoomDefinition(2, 5, 6),
                        new AdventureRoomDefinition(2),
                        new ShopRoomDefinition(2),
                        new BattleRoomDefinition(3, 6, 7),
                        new AdventureRoomDefinition(3),
                        // new RestRoomDescriptor(3),
                        new BattleRoomDefinition(4, 7, 8),
                        new AscensionRoomDefinition(4),
                    },
                    new RoomDefinition[]
                    {
                        // 教学13 同境界合成
                        new BattleRoomDefinition(5, 8, 8),
                        new AdventureRoomDefinition(5),
                        new ShopRoomDefinition(5),
                        new BattleRoomDefinition(5, 8, 9),
                        new AdventureRoomDefinition(5),
                        new BattleRoomDefinition(6, 9, 9),
                        new AdventureRoomDefinition(6),
                        // new RestRoomDescriptor(6),
                        new BattleRoomDefinition(7, 9, 10),
                        new AscensionRoomDefinition(7),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(8, "教学7", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("2")
                            && env.GetRunConfig().GetDifficulty() == 1),
                        
                        new DirectRoomDefinition(8, "教学9", pred: (profile, env) =>
                            !profile.DifficultyIsUnlocked("3")
                            && env.GetRunConfig().GetDifficulty() == 2),
                        
                        new BattleRoomDefinition(8, 10, 10),
                        new AdventureRoomDefinition(8),
                        new BattleRoomDefinition(8, 10, 11),
                        new AdventureRoomDefinition(8),
                        new ShopRoomDefinition(8),
                        new BattleRoomDefinition(9, 11, 11),
                        new AdventureRoomDefinition(9),
                        new BattleRoomDefinition(9, 11, 12),
                        new AdventureRoomDefinition(9),
                        // new RestRoomDescriptor(9),
                        new BattleRoomDefinition(10, 12, 12),
                        new AscensionRoomDefinition(10),
                    },
                    new RoomDefinition[]
                    {
                        new BattleRoomDefinition(11, 12, 12),
                        new AdventureRoomDefinition(11),
                        new BattleRoomDefinition(11, 12, 12),
                        new AdventureRoomDefinition(11),
                        new BattleRoomDefinition(11, 12, 12),
                        // new RestRoomDescriptor(11),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        // new RestRoomDescriptor(12),
                        new ShopRoomDefinition(12),
                        new BattleRoomDefinition(13, 12, 12),
                        // new AscensionRoomDefinition(13),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(13, "斩断尘缘"),
                        new DirectRoomDefinition(13, "无名泉水"),
                        new DirectRoomDefinition(13, "气血商店"),
                        new DirectRoomDefinition(13, "命元商店"),
                        new DirectRoomDefinition(13, "镜中世界"),
                        new DirectRoomDefinition(13, "空荡回廊"),
                        new DirectRoomDefinition(13, "返虚战斗"),
                        // new DirectRoomDefinition(13, "返虚三战斗"),
                    },
                }),
            
            new(id: "Map0003",
                name: "序章",
                envJingJie: JingJie.LianQi,
                slotCount: 3,
                gold: 3,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "漫画"),
                        new DirectRoomDefinition(0, "教学1"),
                        new DirectRoomDefinition(0, "教学2"),
                        new DirectRoomDefinition(0, "教学3"),
                        new DirectRoomDefinition(0, "教学4"),
                        new DirectRoomDefinition(0, "教学5"),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "序章出门"),
                        
                        new BattleRoomDefinition(0, 3, 4),
                        new AdventureRoomDefinition(0),
                        // new RestRoomDescriptor(0),
                        new BattleRoomDefinition(1, 4, 5),
                        new AscensionRoomDefinition(0),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(2, "教学6"),
                        
                        new BattleRoomDefinition(2, 5, 6),
                        new AdventureRoomDefinition(2),
                        new ShopRoomDefinition(2),
                        new BattleRoomDefinition(3, 6, 7),
                        new AdventureRoomDefinition(3),
                        // new RestRoomDescriptor(3),
                        new BattleRoomDefinition(4, 7, 8),
                        new AscensionRoomDefinition(4),
                    },
                    new RoomDefinition[]
                    {
                        // 教学13 同境界合成
                        
                        new BattleRoomDefinition(5, 8, 8),
                        new AdventureRoomDefinition(5),
                        new ShopRoomDefinition(5),
                        new BattleRoomDefinition(5, 8, 9),
                        new AdventureRoomDefinition(5),
                        new BattleRoomDefinition(6, 9, 9),
                        new AdventureRoomDefinition(6),
                        // new RestRoomDescriptor(6),
                        new BattleRoomDefinition(7, 9, 10),
                        new AscensionRoomDefinition(7),
                    },
                    new RoomDefinition[]
                    {
                        new BattleRoomDefinition(8, 10, 10),
                        new AdventureRoomDefinition(8),
                        new BattleRoomDefinition(8, 10, 11),
                        new AdventureRoomDefinition(8),
                        new ShopRoomDefinition(8),
                        new BattleRoomDefinition(9, 11, 11),
                        new AdventureRoomDefinition(9),
                        new BattleRoomDefinition(9, 11, 12),
                        new AdventureRoomDefinition(9),
                        // new RestRoomDescriptor(9),
                        new BattleRoomDefinition(10, 12, 12),
                        new AscensionRoomDefinition(10),
                    },
                    new RoomDefinition[]
                    {
                        new BattleRoomDefinition(11, 12, 12),
                        new AdventureRoomDefinition(11),
                        new BattleRoomDefinition(11, 12, 12),
                        new AdventureRoomDefinition(11),
                        new BattleRoomDefinition(11, 12, 12),
                        // new RestRoomDescriptor(11),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        // new RestRoomDescriptor(12),
                        new ShopRoomDefinition(12),
                        new BattleRoomDefinition(13, 12, 12),
                        new SuccessRoomDefinition(13),
                    },
                }),
            
            new(id: "Map0019",
                name: "教学10",
                envJingJie: JingJie.YuanYing,
                slotCount: 8,
                gold: 53,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(2, "教学10"),
                        // new DirectRoomDefinition(2, "教学10", pred: (profile, env) =>
                        //     !profile.DifficultyIsUnlocked("5")
                        //     && env.GetRunConfig().GetDifficulty() == 4),
                    },
                }),
            
            new(id: "Map0020",
                name: "返虚测试",
                envJingJie: JingJie.HuaShen,
                slotCount: 12,
                gold: 289,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new BattleRoomDefinition(13, 12, 12),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(13, "斩断尘缘"),
                        new DirectRoomDefinition(13, "无名泉水"),
                        new DirectRoomDefinition(13, "气血商店"),
                        new DirectRoomDefinition(13, "命元商店"),
                        new DirectRoomDefinition(13, "镜中世界"),
                        new DirectRoomDefinition(13, "空荡回廊"),
                        // new DirectRoomDefinition(13, "返虚战斗"),
                        new DirectRoomDefinition(13, "返虚三战斗"),
                    },
                }),
        });
    }

    // public override MapEntry DefaultEntry() => this["0000"];
}
