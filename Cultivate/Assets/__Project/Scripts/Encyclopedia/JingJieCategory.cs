
using System.Collections.Generic;

public class JingJieCategory : Category<JingJie>
{
    public JingJieCategory()
    {
        AddRange(new List<JingJie>()
        {
            new(id:                     "JingJie0001",
                name:                   "练气",
                index:                  0,
                colorName:              "灰",
                audioName:              "BGMLianQi",
                rawDescription:         "初入修仙之路，体内开始凝聚灵气，为筑基打下基础。"),
            new(id:                     "JingJie0002",
                name:                   "筑基",
                index:                  1,
                colorName:              "绿",
                audioName:              "BGMZhuJi",
                rawDescription:         "灵气充盈，根基稳固，正式踏上修仙大道。"),
            new(id:                     "JingJie0003",
                name:                   "金丹",
                index:                  2,
                colorName:              "蓝",
                audioName:              "BGMJinDan",
                rawDescription:         "凝结金丹，法力大增，可御空飞行，寿元延长。"),
            new(id:                     "JingJie0004",
                name:                   "元婴",
                index:                  3,
                colorName:              "紫",
                audioName:              "BGMYuanYing",
                rawDescription:         "元婴出窍，神识外放，可探查千里，实力飞跃。"),
            new(id:                     "JingJie0005",
                name:                   "化神",
                index:                  4,
                colorName:              "金",
                audioName:              "BGMHuaShen",
                rawDescription:         "化神境界，神通广大，可移山填海，寿元千年。"),
            new(id:                     "JingJie0006",
                name:                   "返虚",
                index:                  5,
                colorName:              "红",
                audioName:              "BGMHuaShen",
                rawDescription:         "返虚境界，超脱凡尘，接近仙人，实力通天彻地。"),
        });
    }
}
