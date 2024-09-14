
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
                skillCount: 5,
                levels: new StepDescriptor[][]
                {
                    new StepDescriptor[]
                    {
                        new AdventureStepDescriptor(0),
                        new BattleStepDescriptor(0, 3, 4),
                        new AdventureStepDescriptor(0),
                        new RestStepDescriptor(0),
                        new BattleStepDescriptor(1, 4, 5),
                        new AscensionStepDescriptor(0),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(2, 5, 6),
                        new AdventureStepDescriptor(2),
                        new ShopStepDescriptor(2),
                        new BattleStepDescriptor(3, 6, 7),
                        new AdventureStepDescriptor(3),
                        new RestStepDescriptor(3),
                        new BattleStepDescriptor(4, 7, 8),
                        new AscensionStepDescriptor(4),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(5, 8, 8),
                        new AdventureStepDescriptor(5),
                        new ShopStepDescriptor(5),
                        new BattleStepDescriptor(5, 8, 9),
                        new AdventureStepDescriptor(5),
                        new BattleStepDescriptor(6, 9, 9),
                        new AdventureStepDescriptor(6),
                        new RestStepDescriptor(6),
                        new BattleStepDescriptor(7, 9, 10),
                        new AscensionStepDescriptor(7),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(8, 10, 10),
                        new AdventureStepDescriptor(8),
                        new BattleStepDescriptor(8, 10, 11),
                        new AdventureStepDescriptor(8),
                        new ShopStepDescriptor(8),
                        new BattleStepDescriptor(9, 11, 11),
                        new AdventureStepDescriptor(9),
                        new BattleStepDescriptor(9, 11, 12),
                        new AdventureStepDescriptor(9),
                        new RestStepDescriptor(9),
                        new BattleStepDescriptor(10, 12, 12),
                        new AscensionStepDescriptor(10),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new RestStepDescriptor(11),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new RestStepDescriptor(12),
                        new ShopStepDescriptor(12),
                        new BattleStepDescriptor(13, 12, 12),
                        new SuccessStepDescriptor(13),
                    },
                }),
            
            new(id: "教程",
                envJingJie: JingJie.LianQi,
                slotCount: 1,
                gold: 0,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new StepDescriptor[][]
                {
                    new StepDescriptor[]
                    {
                        new DirectStepDescriptor(0, "初入蓬莱"),
                        new BattleStepDescriptor(0, 3, 4),
                        new AdventureStepDescriptor(0),
                        new RestStepDescriptor(0),
                        new BattleStepDescriptor(1, 4, 5),
                        new AscensionStepDescriptor(0),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(2, 5, 6),
                        new DirectStepDescriptor(2, "同境界合成教学"),
                        new ShopStepDescriptor(2),
                        new BattleStepDescriptor(3, 6, 7),
                        new AdventureStepDescriptor(3),
                        new RestStepDescriptor(3),
                        new BattleStepDescriptor(4, 7, 8),
                        new AscensionStepDescriptor(4),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(5, 8, 8),
                        new AdventureStepDescriptor(5),
                        new ShopStepDescriptor(5),
                        new BattleStepDescriptor(5, 8, 9),
                        new AdventureStepDescriptor(5),
                        new BattleStepDescriptor(6, 9, 9),
                        new AdventureStepDescriptor(6),
                        new RestStepDescriptor(6),
                        new BattleStepDescriptor(7, 9, 10),
                        new AscensionStepDescriptor(7),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(8, 10, 10),
                        new AdventureStepDescriptor(8),
                        new BattleStepDescriptor(8, 10, 11),
                        new AdventureStepDescriptor(8),
                        new ShopStepDescriptor(8),
                        new BattleStepDescriptor(9, 11, 11),
                        new AdventureStepDescriptor(9),
                        new BattleStepDescriptor(9, 11, 12),
                        new AdventureStepDescriptor(9),
                        new RestStepDescriptor(9),
                        new BattleStepDescriptor(10, 12, 12),
                        new AscensionStepDescriptor(10),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new RestStepDescriptor(11),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new RestStepDescriptor(12),
                        new ShopStepDescriptor(12),
                        new BattleStepDescriptor(13, 12, 12),
                        new SuccessStepDescriptor(13),
                    },
                }),
            
            new(id: "发现",
                envJingJie: JingJie.LianQi,
                slotCount: 12,
                gold: 0,
                skillJingJie: JingJie.LianQi,
                skillCount: 0,
                levels: new StepDescriptor[][]
                {
                    new StepDescriptor[]
                    {
                        new DirectStepDescriptor(0, "发现一张牌"),
                        new DirectStepDescriptor(0, "发现一张牌"),
                        new DirectStepDescriptor(0, "发现一张牌"),
                        new DirectStepDescriptor(0, "发现一张牌"),
                        new DirectStepDescriptor(0, "发现一张牌"),
                        new DirectStepDescriptor(0, "发现一张牌"),
                        new DirectStepDescriptor(0, "发现一张牌"),
                        new AscensionStepDescriptor(0),
                    },
                    new StepDescriptor[]
                    {
                        new DirectStepDescriptor(4, "发现一张牌"),
                        new DirectStepDescriptor(4, "发现一张牌"),
                        new DirectStepDescriptor(4, "发现一张牌"),
                        new DirectStepDescriptor(4, "发现一张牌"),
                        new DirectStepDescriptor(4, "发现一张牌"),
                        new DirectStepDescriptor(4, "发现一张牌"),
                        new DirectStepDescriptor(4, "发现一张牌"),
                        new AscensionStepDescriptor(4),
                    },
                    new StepDescriptor[]
                    {
                        new DirectStepDescriptor(7, "发现一张牌"),
                        new DirectStepDescriptor(7, "发现一张牌"),
                        new DirectStepDescriptor(7, "发现一张牌"),
                        new DirectStepDescriptor(7, "发现一张牌"),
                        new DirectStepDescriptor(7, "发现一张牌"),
                        new DirectStepDescriptor(7, "发现一张牌"),
                        new DirectStepDescriptor(7, "发现一张牌"),
                        new AscensionStepDescriptor(7),
                    },
                    new StepDescriptor[]
                    {
                        new DirectStepDescriptor(10, "发现一张牌"),
                        new DirectStepDescriptor(10, "发现一张牌"),
                        new DirectStepDescriptor(10, "发现一张牌"),
                        new DirectStepDescriptor(10, "发现一张牌"),
                        new DirectStepDescriptor(10, "发现一张牌"),
                        new DirectStepDescriptor(10, "发现一张牌"),
                        new DirectStepDescriptor(10, "发现一张牌"),
                        new AscensionStepDescriptor(10),
                    },
                    new StepDescriptor[]
                    {
                        new DirectStepDescriptor(13, "发现一张牌"),
                        new DirectStepDescriptor(13, "发现一张牌"),
                        new DirectStepDescriptor(13, "发现一张牌"),
                        new DirectStepDescriptor(13, "发现一张牌"),
                        new DirectStepDescriptor(13, "发现一张牌"),
                        new DirectStepDescriptor(13, "发现一张牌"),
                        new DirectStepDescriptor(13, "发现一张牌"),
                        new SuccessStepDescriptor(13),
                    },
                }),
            
            new(id: "筑基",
                envJingJie: JingJie.ZhuJi,
                slotCount: 5,
                gold: 5,
                skillJingJie: JingJie.LianQi,
                skillCount: 7,
                levels: new StepDescriptor[][]
                {
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(2, 5, 6),
                        new AdventureStepDescriptor(2),
                        new ShopStepDescriptor(2),
                        new BattleStepDescriptor(3, 6, 7),
                        new AdventureStepDescriptor(3),
                        new RestStepDescriptor(3),
                        new BattleStepDescriptor(4, 7, 8),
                        new AscensionStepDescriptor(4),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(5, 8, 8),
                        new AdventureStepDescriptor(5),
                        new ShopStepDescriptor(5),
                        new BattleStepDescriptor(5, 8, 9),
                        new AdventureStepDescriptor(5),
                        new BattleStepDescriptor(6, 9, 9),
                        new AdventureStepDescriptor(6),
                        new RestStepDescriptor(6),
                        new BattleStepDescriptor(7, 9, 10),
                        new AscensionStepDescriptor(7),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(8, 10, 10),
                        new AdventureStepDescriptor(8),
                        new BattleStepDescriptor(8, 10, 11),
                        new AdventureStepDescriptor(8),
                        new ShopStepDescriptor(8),
                        new BattleStepDescriptor(9, 11, 11),
                        new AdventureStepDescriptor(9),
                        new BattleStepDescriptor(9, 11, 12),
                        new AdventureStepDescriptor(9),
                        new RestStepDescriptor(9),
                        new BattleStepDescriptor(10, 12, 12),
                        new AscensionStepDescriptor(10),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new RestStepDescriptor(11),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new RestStepDescriptor(12),
                        new ShopStepDescriptor(12),
                        new BattleStepDescriptor(13, 12, 12),
                        new SuccessStepDescriptor(13),
                    },
                }),
            
            new(id: "金丹",
                envJingJie: JingJie.JinDan,
                slotCount: 8,
                gold: 17,
                skillJingJie: JingJie.LianQi,
                skillCount: 13,
                levels: new StepDescriptor[][]
                {
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(5, 8, 8),
                        new AdventureStepDescriptor(5),
                        new ShopStepDescriptor(5),
                        new BattleStepDescriptor(5, 8, 9),
                        new AdventureStepDescriptor(5),
                        new BattleStepDescriptor(6, 9, 9),
                        new AdventureStepDescriptor(6),
                        new RestStepDescriptor(6),
                        new BattleStepDescriptor(7, 9, 10),
                        new AscensionStepDescriptor(7),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(8, 10, 10),
                        new AdventureStepDescriptor(8),
                        new BattleStepDescriptor(8, 10, 11),
                        new AdventureStepDescriptor(8),
                        new ShopStepDescriptor(8),
                        new BattleStepDescriptor(9, 11, 11),
                        new AdventureStepDescriptor(9),
                        new BattleStepDescriptor(9, 11, 12),
                        new AdventureStepDescriptor(9),
                        new RestStepDescriptor(9),
                        new BattleStepDescriptor(10, 12, 12),
                        new AscensionStepDescriptor(10),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new RestStepDescriptor(11),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new RestStepDescriptor(12),
                        new ShopStepDescriptor(12),
                        new BattleStepDescriptor(13, 12, 12),
                        new SuccessStepDescriptor(13),
                    },
                }),
            
            new(id: "元婴",
                envJingJie: JingJie.YuanYing,
                slotCount: 10,
                gold: 49,
                skillJingJie: JingJie.ZhuJi,
                skillCount: 15,
                levels: new StepDescriptor[][]
                {
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(8, 10, 10),
                        new AdventureStepDescriptor(8),
                        new BattleStepDescriptor(8, 10, 11),
                        new AdventureStepDescriptor(8),
                        new ShopStepDescriptor(8),
                        new BattleStepDescriptor(9, 11, 11),
                        new AdventureStepDescriptor(9),
                        new BattleStepDescriptor(9, 11, 12),
                        new AdventureStepDescriptor(9),
                        new RestStepDescriptor(9),
                        new BattleStepDescriptor(10, 12, 12),
                        new AscensionStepDescriptor(10),
                    },
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new RestStepDescriptor(11),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new RestStepDescriptor(12),
                        new ShopStepDescriptor(12),
                        new BattleStepDescriptor(13, 12, 12),
                        new SuccessStepDescriptor(13),
                    },
                }),
            
            new(id: "化神",
                envJingJie: JingJie.HuaShen,
                slotCount: 12,
                gold: 129,
                skillJingJie: JingJie.JinDan,
                skillCount: 17,
                levels: new StepDescriptor[][]
                {
                    new StepDescriptor[]
                    {
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new AdventureStepDescriptor(11),
                        new BattleStepDescriptor(11, 12, 12),
                        new RestStepDescriptor(11),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new AdventureStepDescriptor(12),
                        new BattleStepDescriptor(12, 12, 12),
                        new RestStepDescriptor(12),
                        new ShopStepDescriptor(12),
                        new BattleStepDescriptor(13, 12, 12),
                        new SuccessStepDescriptor(13),
                    },
                }),
            
            new(id: "化神决战",
                envJingJie: JingJie.HuaShen,
                slotCount: 12,
                gold: 289,
                skillJingJie: JingJie.JinDan,
                skillCount: 41,
                levels: new StepDescriptor[][]
                {
                    new StepDescriptor[]
                    {
                        new RestStepDescriptor(12),
                        new ShopStepDescriptor(12),
                        new BattleStepDescriptor(13, 12, 12),
                        new SuccessStepDescriptor(13),
                    },
                }),
        });
    }

    // public override MapEntry DefaultEntry() => this["0000"];
}
