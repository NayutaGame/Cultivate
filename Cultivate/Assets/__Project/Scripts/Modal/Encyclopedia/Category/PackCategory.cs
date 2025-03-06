
using System.Collections.Generic;
using UnityEngine;

public class PackCategory : Category<PackEntry>
{
    public PackCategory()
    {
        AddRange(new List<PackEntry>()
        {
            new(id:                  "0001",
                name:                "无常路引",
                wuXing:              WuXing.Jin,
                description:         "以迅疾的剑势破开敌人防御，每一击都直指要害。剑招连绵不绝，令敌人难以喘息。",
                trivia:              "无常剑法讲究'心无常处即是常'，剑随心动，无迹可循。",
                cardNames:           new string[] { "金刃", "起势", "敛息", "流云", "刃雨", "盘旋", "白刃", "山风", "无妄" },
                startCardNames:      new string[] { "起势", "金刃", "敛息" }
                ),
            
            new(id:                  "0002",
                name:                "大音希声",
                wuXing:              WuXing.Jin,
                description:         "一击必杀的极致剑术。以凌厉的剑气撕裂空间，在瞬息之间决定胜负。",
                trivia:              "大音希声，大象无形。此剑法追求剑道极致，一剑既出，天地俱寂。",
                cardNames:           new string[] { "寻猎", "秋露白", "天地同寿", "醉意", "摇曳", "刺穴", "袖里乾坤", "一莲托生", "闪击" },
                startCardNames:      new string[] { "秋露白", "寻猎", "天地同寿" }
                ),
            
            new(id:                  "0003",
                name:                "天河引气录",
                wuXing:              WuXing.Shui,
                description:         "引导天地元气，化为己用。可汇聚灵气疗伤，亦可凝聚剑气伤敌。",
                trivia:              "相传天河剑仙观天河之势，悟天地元气流转之理，创此神功。修习者常年观星引气，终有望羽化登仙。",
                cardNames:           new string[] { "恋花", "吐纳", "止水", "调和", "大鱼", "彩虹", "海啸", "飞鸿踏雪", "一梦如是" },
                startCardNames:      new string[] { "吐纳", "恋花", "止水" }
                ),
            
            new(id:                  "0004",
                name:                "御虚诀",
                wuXing:              WuXing.Shui,
                description:         "以幻化万千的身法迷惑敌人，令其陷入重重幻境。敌人越是挣扎，越是难以脱身。",
                trivia:              "此功原是蜃楼幻境之法，后经高人改良，化虚为实，幻境可伤人。",
                cardNames:           new string[] { "空幻", "激流", "潮汐", "踏浪", "写意", "气吞山河", "吞天", "瑞雪", "镜花水月" },
                startCardNames:      new string[] { "激流", "踏浪", "空幻" }
                ),
            
            new(id:                  "0005",
                name:                "大椿功",
                wuXing:              WuXing.Mu,
                description:         "如同参天大树般稳固持久的心法。每次运功都会增强功力，蕴养己身。",
                trivia:              "大椿者，以八千岁为春，八千岁为秋。此功讲究厚积薄发，修习者需有大毅力。",
                cardNames:           new string[] { "小松", "入木三分", "明神", "回春", "落英", "钟声", "一心一剑", "梅开二度", "一叶知秋" },
                startCardNames:      new string[] { "小松", "明神", "回春" }
                ),
            
            new(id:                  "0006",
                name:                "游龙遁",
                wuXing:              WuXing.Mu,
                description:         "灵动如龙的身法，可随心所欲地穿梭于敌阵之中。攻守之势，瞬息万变。",
                trivia:              "此功原是观龙游天时所创，讲究顺应自然，随风而动。",
                cardNames:           new string[] { "潜龙在渊", "见龙在田", "生机", "清泉", "飞龙在天", "回响", "亢龙有悔", "鹤回翔", "一念无量劫" },
                startCardNames:      new string[] { "潜龙在渊", "见龙在田", "生机" }
                ),
            
            new(id:                  "0007",
                name:                "归鸿十二步",
                wuXing:              WuXing.Huo,
                description:         "集攻防于一体的剑舞。每一式都蕴含多重变化，可攻可守，令敌人难以应对。",
                trivia:              "剑法灵感来自归鸿展翅，十二式浑然一体，如同天成。",
                cardNames:           new string[] { "云袖", "正念", "剑王行", "战意", "天衣无缝", "窑土", "九射", "登宝塔", "净天地" },
                startCardNames:      new string[] { "云袖", "正念", "战意" }
                ),
            
            new(id:                  "0008",
                name:                "大焚天秘乘",
                wuXing:              WuXing.Huo,
                description:         "以燃烧气血为代价，爆发出惊人的威力。使用者与敌人同归于尽的觉悟越强，威力越大。",
                trivia:              "此功原是佛门明王法，后流落江湖，习者多有走火入魔之危。",
                cardNames:           new string[] { "轰天", "怒瞳", "明镜", "舍生", "不动明王诀", "浴火", "晚霞", "观众生", "常夏" },
                startCardNames:      new string[] { "轰天", "怒瞳", "明镜" }
                ),
            
            new(id:                  "0009",
                name:                "须弥妙法",
                wuXing:              WuXing.Tu,
                description:         "以厚重的护体真气抵御伤害。越是危险的处境，防御越是坚不可摧。",
                trivia:              "须弥山不动，此功亦如是。传说创功之人曾以此功硬接天雷而不伤。",
                cardNames:           new string[] { "寸劲", "滑步", "八极拳", "活步", "震脚", "箭疾步", "崩山掌", "磐石", "须弥结界" },
                startCardNames:      new string[] { "寸劲", "滑步", "八极拳" }
                ),
            
            new(id:                  "0010",
                name:                "锻体四则",
                wuXing:              WuXing.Tu,
                description:         "以刚猛的力道摧毁一切。修炼者需苦练筋骨，方能发挥此功真正威力。",
                trivia:              "四则：炼筋、锻骨、淬体、养气。此功重在基础，却也最难练至大成。",
                cardNames:           new string[] { "固元", "锻骨", "守势", "隼击", "锻髓", "塑魂", "天人五衰", "养生", "阿修罗" },
                startCardNames:      new string[] { "固元", "守势", "隼击" }
                ),
            
            new(id:                  "0011",
                name:                "丹兵道",
                wuXing:              null,
                description:         "以丹药之力增强战力。药效来得快，去得也快，使用时需要权衡时机。",
                trivia:              "丹兵之道，讲究以丹养气，以气养兵。丹药之效，全在一瞬。",
                cardNames:           new string[] { "硬化蛊", "雷火弹", "养气丹", "天雷丸", "同心蛊", "七彩蛊", "破境丹", "小雷劫", "补天丹" }
                ),
            
            new(id:                  "0012",
                name:                "化哉",
                wuXing:              null,
                description:         "善于利用五行相生相克之理。随着战斗推进，威力会越来越强。",
                trivia:              "化哉者，化天地之造化。此功讲究顺应五行，借天地之力。",
                cardNames:           new string[] { "缭乱", "流霰", "蜕变", "燎原", "百草集", "停云", "常仪", "凝水", "羲和" }
                ),
        });
    }

    // public override PackEntry DefaultEntry() => this["0000"];
}
