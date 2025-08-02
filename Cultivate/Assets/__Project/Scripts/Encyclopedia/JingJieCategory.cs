
using System.Collections.Generic;

public class JingJieCategory : Category<JingJie>
{
    public JingJieCategory()
    {
        AddRange(new List<JingJie>()
        {
            new("JingJie0000", "练气", 0, "灰", "BGMLianQi", "初入修仙之路，体内开始凝聚灵气，为筑基打下基础。"),
            new("JingJie0000", "筑基", 1, "绿", "BGMZhuJi", "灵气充盈，根基稳固，正式踏上修仙大道。"),
            new("JingJie0000", "金丹", 2, "蓝", "BGMJinDan", "凝结金丹，法力大增，可御空飞行，寿元延长。"),
            new("JingJie0000", "元婴", 3, "紫", "BGMYuanYing", "元婴出窍，神识外放，可探查千里，实力飞跃。"),
            new("JingJie0000", "化神", 4, "金", "BGMHuaShen", "化神境界，神通广大，可移山填海，寿元千年。"),
            new("JingJie0000", "返虚", 5, "红", "BGMHuaShen", "返虚境界，超脱凡尘，接近仙人，实力通天彻地。"),
        });
    }
}
