
using System.Collections.Generic;

public class DifficultyCategory : Category<DifficultyEntry>
{
    public DifficultyCategory()
    {
        AddRange(new List<DifficultyEntry>()
        {
            new("-1", description: "敌人生命上限减少30%，不会受到命元惩罚"),
            new("0", description: "没有变化"),
            new("1", description: "敌人生命上限增加10%"),
            new("2", description: "跨越境界时回复的命元-1"),
            new("3", description: "敌人获得1力量"),
            new("4", description: "商店的价格+50%"),
            new("5", description: "敌人获得1格挡"),
            new("6", description: "跨越境界时不再提供升级"),
            new("7", description: "敌人获得1免疫"),
            new("8", description: "主角生命上限-10%"),
            new("9", description: "敌人获得先手"),
            new("10", description: "最终Boss需要击败两次"),
        });
    }
}
