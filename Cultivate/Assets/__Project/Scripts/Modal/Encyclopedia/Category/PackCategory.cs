
using System.Collections.Generic;

public class PackCategory : Category<PackEntry>
{
    public PackCategory()
    {
        AddRange(new List<PackEntry>()
        {
            new(id:                  "0001",
                name:                "金破甲",
                wuXing:              WuXing.Jin,
                cards:               new SkillEntry[] { "金刃", "起势", "流云", "敛息", "匕首雨", "盘旋", "白刃", "山风", "闪击" }
                ),
            
            new(id:                  "0002",
                name:                "金暴击",
                wuXing:              WuXing.Jin,
                cards:               new SkillEntry[] { "寻猎", "醉意", "秋露白", "刺穴", "摇曳", "天地同寿", "无妄", "袖里乾坤", "一莲托生" }
                ),
            
            new(id:                  "0003",
                name:                "水幻术",
                wuXing:              WuXing.Shui,
                cards:               new SkillEntry[] { "恋花", "吐纳", "止水", "甘露", "大鱼", "气吞山河", "吞天", "飞鸿踏雪", "玄武吐息法" }
                ),
            
            new(id:                  "0004",
                name:                "水灵气",
                wuXing:              WuXing.Shui,
                cards:               new SkillEntry[] { "空幻", "激流", "潮汐", "踏浪", "写意", "瑞雪", "一梦如是", "彩虹", "镜花水月" }
                ),
            
            new(id:                  "0005",
                name:                "木成长",
                wuXing:              WuXing.Mu,
                cards:               new SkillEntry[] { "若竹", "清泉", "缭乱", "回春", "小松", "钟声", "入木三分", "梅开二度", "一叶知秋" }
                ),
            
            new(id:                  "0006",
                name:                "木化龙",
                wuXing:              WuXing.Mu,
                cards:               new SkillEntry[] { "潜龙在渊", "明神", "见龙在田", "彼岸花", "飞龙在天", "回响", "亢龙有悔", "鹤回翔", "一念无量劫" }
                ),
            
            new(id:                  "0007",
                name:                "火剑舞",
                wuXing:              WuXing.Huo,
                cards:               new SkillEntry[] { "云袖", "正念", "剑王行", "浴火", "天衣无缝", "怒瞳", "九射", "登宝塔", "净天地" }
                ),
            
            new(id:                  "0008",
                name:                "火明王",
                wuXing:              WuXing.Huo,
                cards:               new SkillEntry[] { "轰天", "明镜", "不动明王诀", "舍生", "战意", "燎原", "晚霞", "观众生", "常夏" }
                ),
            
            new(id:                  "0009",
                name:                "土护甲",
                wuXing:              WuXing.Tu,
                cards:               new SkillEntry[] { "寸劲", "滑步", "八极拳", "蜕变", "震脚", "箭疾步", "崩山掌", "崩山掌", "一诺五岳" }
                ),
            
            new(id:                  "0010",
                name:                "土高攻",
                wuXing:              WuXing.Tu,
                cards:               new SkillEntry[] { "活步", "活步", "一力降十会", "一力降十会", "连环腿", "龟息", "边腿", "金刚不坏", "一诺五岳" }
                ),
        });
    }

    // public override PackEntry DefaultEntry() => this["0000"];
}
