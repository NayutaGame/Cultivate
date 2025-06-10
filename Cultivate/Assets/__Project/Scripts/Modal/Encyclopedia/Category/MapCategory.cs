
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
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "漫画", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "教学1", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "教学2", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "教学3", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "教学4", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "教学5", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        
                        new DirectRoomDefinition(0, "教学8", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("3").IsUnlocked()
                            && env.GetRunConfig().GetDifficulty() == 2),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "出门", pred: (profile, env) =>
                            profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "序章出门", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        
                        new BattleRoomDefinition(0, 3, 4),
                        new AdventureRoomDefinition(0),
                        // new RestRoomDescriptor(0),
                        new BattleRoomDefinition(1, 4, 5),
                        new AscensionRoomDefinition(0),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(2, "教学6", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        
                        new DirectRoomDefinition(2, "教学10", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("5").IsUnlocked()
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
            
            new(id: "序章",
                envJingJie: JingJie.LianQi,
                slotCount: 3,
                gold: 3,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
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
            
            new(id: "测试墨染",
                envJingJie: JingJie.LianQi,
                slotCount: 3,
                gold: 3,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                onStartRun: env =>
                {
                    env.PickSkillProcedure(SkillEntry.FromName("金刃"));
                    env.PickSkillProcedure(SkillEntry.FromName("起势"));
                    env.PickSkillProcedure(SkillEntry.FromName("暴击墨染"));
                    env.PickSkillProcedure(SkillEntry.FromName("吸血墨染"));
                    env.PickSkillProcedure(SkillEntry.FromName("穿透墨染"));
                    env.PickSkillProcedure(SkillEntry.FromName("二动墨染"));
                },
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "漫画", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "教学1", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "教学2", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "教学3", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "教学4", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "教学5", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        
                        new DirectRoomDefinition(0, "教学8", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("3").IsUnlocked()
                            && env.GetRunConfig().GetDifficulty() == 2),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "出门", pred: (profile, env) =>
                            profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        new DirectRoomDefinition(0, "序章出门", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        
                        new BattleRoomDefinition(0, 3, 4),
                        new AdventureRoomDefinition(0),
                        // new RestRoomDescriptor(0),
                        new BattleRoomDefinition(1, 4, 5),
                        new AscensionRoomDefinition(0),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(2, "教学6", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("1").IsUnlocked()),
                        
                        new DirectRoomDefinition(2, "教学10", pred: (profile, env) =>
                            !profile.DifficultyProfileList.Find("5").IsUnlocked()
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
            
            new(id: "发现",
                envJingJie: JingJie.LianQi,
                slotCount: 12,
                gold: 0,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "发现一张牌"),
                        new DirectRoomDefinition(0, "发现一张牌"),
                        new DirectRoomDefinition(0, "发现一张牌"),
                        new DirectRoomDefinition(0, "发现一张牌"),
                        new DirectRoomDefinition(0, "发现一张牌"),
                        new DirectRoomDefinition(0, "发现一张牌"),
                        new DirectRoomDefinition(0, "发现一张牌"),
                        new AscensionRoomDefinition(0),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(4, "发现一张牌"),
                        new DirectRoomDefinition(4, "发现一张牌"),
                        new DirectRoomDefinition(4, "发现一张牌"),
                        new DirectRoomDefinition(4, "发现一张牌"),
                        new DirectRoomDefinition(4, "发现一张牌"),
                        new DirectRoomDefinition(4, "发现一张牌"),
                        new DirectRoomDefinition(4, "发现一张牌"),
                        new AscensionRoomDefinition(4),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(7, "发现一张牌"),
                        new DirectRoomDefinition(7, "发现一张牌"),
                        new DirectRoomDefinition(7, "发现一张牌"),
                        new DirectRoomDefinition(7, "发现一张牌"),
                        new DirectRoomDefinition(7, "发现一张牌"),
                        new DirectRoomDefinition(7, "发现一张牌"),
                        new DirectRoomDefinition(7, "发现一张牌"),
                        new AscensionRoomDefinition(7),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(10, "发现一张牌"),
                        new DirectRoomDefinition(10, "发现一张牌"),
                        new DirectRoomDefinition(10, "发现一张牌"),
                        new DirectRoomDefinition(10, "发现一张牌"),
                        new DirectRoomDefinition(10, "发现一张牌"),
                        new DirectRoomDefinition(10, "发现一张牌"),
                        new DirectRoomDefinition(10, "发现一张牌"),
                        new AscensionRoomDefinition(10),
                    },
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(13, "发现一张牌"),
                        new DirectRoomDefinition(13, "发现一张牌"),
                        new DirectRoomDefinition(13, "发现一张牌"),
                        new DirectRoomDefinition(13, "发现一张牌"),
                        new DirectRoomDefinition(13, "发现一张牌"),
                        new DirectRoomDefinition(13, "发现一张牌"),
                        new DirectRoomDefinition(13, "发现一张牌"),
                        new SuccessRoomDefinition(13),
                    },
                }),
            
            new(id: "测试",
                envJingJie: JingJie.ZhuJi,
                slotCount: 5,
                gold: 0,
                skillJingJie: JingJie.LianQi,
                skillCount: 5,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "以物易物"),
                        new DirectRoomDefinition(0, "以物易物"),
                        new DirectRoomDefinition(0, "以物易物"),
                    },
                }),
            
            new(id: "结算测试",
                envJingJie: JingJie.LianQi,
                slotCount: 4,
                gold: 0,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "快速结算"),
                    },
                }),
            
            new(id: "动画测试",
                envJingJie: JingJie.LianQi,
                slotCount: 12,
                gold: 0,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(1, "动画测试"),
                        new AscensionRoomDefinition(0),
                    },
                }),
            
            new(id: "拖拽测试",
                envJingJie: JingJie.LianQi,
                slotCount: 12,
                gold: 0,
                skillJingJie: JingJie.LianQi,
                skillCount: 40,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new BattleRoomDefinition(1, 12, 12),
                        new AscensionRoomDefinition(0),
                    },
                }),
            
            new(id: "境界测试",
                envJingJie: JingJie.LianQi,
                slotCount: 3,
                gold: 3,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new AscensionRoomDefinition(1),
                        new AscensionRoomDefinition(4),
                        new AscensionRoomDefinition(7),
                        new AscensionRoomDefinition(10),
                        new AdventureRoomDefinition(12),
                    },
                }),
            
            new(id: "筑基",
                envJingJie: JingJie.ZhuJi,
                slotCount: 5,
                gold: 5,
                skillJingJie: JingJie.LianQi,
                skillCount: 7,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new BattleRoomDefinition(2, 5, 6),
                        new AdventureRoomDefinition(2),
                        new ShopRoomDefinition(2),
                        new BattleRoomDefinition(3, 6, 7),
                        new AdventureRoomDefinition(3),
                        new RestRoomDefinition(3),
                        new BattleRoomDefinition(4, 7, 8),
                        new AscensionRoomDefinition(4),
                    },
                    new RoomDefinition[]
                    {
                        new BattleRoomDefinition(5, 8, 8),
                        new AdventureRoomDefinition(5),
                        new ShopRoomDefinition(5),
                        new BattleRoomDefinition(5, 8, 9),
                        new AdventureRoomDefinition(5),
                        new BattleRoomDefinition(6, 9, 9),
                        new AdventureRoomDefinition(6),
                        new RestRoomDefinition(6),
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
                        new RestRoomDefinition(9),
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
                        new RestRoomDefinition(11),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        new RestRoomDefinition(12),
                        new ShopRoomDefinition(12),
                        new BattleRoomDefinition(13, 12, 12),
                        new SuccessRoomDefinition(13),
                    },
                }),
            
            new(id: "金丹",
                envJingJie: JingJie.JinDan,
                slotCount: 8,
                gold: 17,
                skillJingJie: JingJie.LianQi,
                skillCount: 13,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new BattleRoomDefinition(5, 8, 8),
                        new AdventureRoomDefinition(5),
                        new ShopRoomDefinition(5),
                        new BattleRoomDefinition(5, 8, 9),
                        new AdventureRoomDefinition(5),
                        new BattleRoomDefinition(6, 9, 9),
                        new AdventureRoomDefinition(6),
                        new RestRoomDefinition(6),
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
                        new RestRoomDefinition(9),
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
                        new RestRoomDefinition(11),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        new RestRoomDefinition(12),
                        new ShopRoomDefinition(12),
                        new BattleRoomDefinition(13, 12, 12),
                        new SuccessRoomDefinition(13),
                    },
                }),
            
            new(id: "元婴",
                envJingJie: JingJie.YuanYing,
                slotCount: 10,
                gold: 49,
                skillJingJie: JingJie.ZhuJi,
                skillCount: 15,
                levels: new RoomDefinition[][]
                {
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
                        new RestRoomDefinition(9),
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
                        new RestRoomDefinition(11),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        new RestRoomDefinition(12),
                        new ShopRoomDefinition(12),
                        new BattleRoomDefinition(13, 12, 12),
                        new SuccessRoomDefinition(13),
                    },
                }),
            
            new(id: "化神",
                envJingJie: JingJie.HuaShen,
                slotCount: 12,
                gold: 129,
                skillJingJie: JingJie.JinDan,
                skillCount: 17,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new BattleRoomDefinition(11, 12, 12),
                        new AdventureRoomDefinition(11),
                        new BattleRoomDefinition(11, 12, 12),
                        new AdventureRoomDefinition(11),
                        new BattleRoomDefinition(11, 12, 12),
                        new RestRoomDefinition(11),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        new AdventureRoomDefinition(12),
                        new BattleRoomDefinition(12, 12, 12),
                        new RestRoomDefinition(12),
                        new ShopRoomDefinition(12),
                        new BattleRoomDefinition(13, 12, 12),
                        new SuccessRoomDefinition(13),
                    },
                }),
            
            new(id: "化神决战",
                envJingJie: JingJie.HuaShen,
                slotCount: 12,
                gold: 289,
                skillJingJie: JingJie.JinDan,
                skillCount: 41,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new RestRoomDefinition(12),
                        new ShopRoomDefinition(12),
                        new BattleRoomDefinition(13, 12, 12),
                        new SuccessRoomDefinition(13),
                    },
                }),
            
            new(id: "排局1",
                envJingJie: JingJie.LianQi,
                slotCount: 3,
                gold: 3,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "排局1"),
                        new DirectRoomDefinition(0, "排局1"),
                        new DirectRoomDefinition(0, "排局1"),
                        new DirectRoomDefinition(0, "排局1"),
                        new DirectRoomDefinition(0, "排局1"),
                        new DirectRoomDefinition(0, "排局1"),
                        new DirectRoomDefinition(0, "排局1"),
                        new DirectRoomDefinition(0, "排局1"),
                        new DirectRoomDefinition(0, "排局1"),
                        new DirectRoomDefinition(0, "排局1"),
                        new DirectRoomDefinition(0, "排局1"),
                        new AscensionRoomDefinition(4),
                        new AscensionRoomDefinition(7),
                        new AscensionRoomDefinition(10),
                        new AdventureRoomDefinition(12),
                    },
                }),
            
            new(id: "排局2",
                envJingJie: JingJie.ZhuJi,
                slotCount: 5,
                gold: 12,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "排局2"),
                        new DirectRoomDefinition(0, "排局2"),
                        new DirectRoomDefinition(0, "排局2"),
                        new DirectRoomDefinition(0, "排局2"),
                        new DirectRoomDefinition(0, "排局2"),
                        new DirectRoomDefinition(0, "排局2"),
                        new DirectRoomDefinition(0, "排局2"),
                        new DirectRoomDefinition(0, "排局2"),
                        new DirectRoomDefinition(0, "排局2"),
                        new DirectRoomDefinition(0, "排局2"),
                        new DirectRoomDefinition(0, "排局2"),
                        new AscensionRoomDefinition(4),
                        new AscensionRoomDefinition(7),
                        new AscensionRoomDefinition(10),
                        new AdventureRoomDefinition(12),
                    },
                }),
            
            new(id: "排局3",
                envJingJie: JingJie.YuanYing,
                slotCount: 8,
                gold: 53,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new RoomDefinition[][]
                {
                    new RoomDefinition[]
                    {
                        new DirectRoomDefinition(0, "排局3抽牌"),
                        new DirectRoomDefinition(0, "排局3"),
                        new DirectRoomDefinition(0, "排局3"),
                        new DirectRoomDefinition(0, "排局3"),
                        new DirectRoomDefinition(0, "排局3"),
                        new DirectRoomDefinition(0, "排局3"),
                        new DirectRoomDefinition(0, "排局3"),
                        new DirectRoomDefinition(0, "排局3"),
                        new DirectRoomDefinition(0, "排局3"),
                        new DirectRoomDefinition(0, "排局3"),
                        new AscensionRoomDefinition(4),
                        new AscensionRoomDefinition(7),
                        new AscensionRoomDefinition(10),
                        new AdventureRoomDefinition(12),
                    },
                }),
        });
    }

    // public override MapEntry DefaultEntry() => this["0000"];
}
