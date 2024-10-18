
using System.Collections.Generic;

public class SpriteCategory : Category<SpriteEntry>
{
    public SpriteCategory()
    {
        AddRange(new List<SpriteEntry>()
        {
            // BuffIcons
            new("不存在的状态", "Images/BuffIcons/不存在"),
            new("缺失状态插画", "Images/BuffIcons/无图标"),
            new("二动", "Images/BuffIcons/二动"),
            new("力量", "Images/BuffIcons/力量"),
            new("吸血", "Images/BuffIcons/吸血"),
            new("暴击", "Images/BuffIcons/暴击"),
            new("格挡", "Images/BuffIcons/格挡"),
            new("永久暴击", "Images/BuffIcons/永久暴击"),
            new("灼烧", "Images/BuffIcons/灼烧"),
            new("穿透", "Images/BuffIcons/穿透"),
            new("锋锐", "Images/BuffIcons/锋锐"),
            new("闪避", "Images/BuffIcons/闪避"),
            
            // NodeIcons
            new("奇遇", "Images/NodeIcons/Adventure"),
            new("战斗", "Images/NodeIcons/Battle"),
            new("Boss", "Images/NodeIcons/Boss"),
            new("事件", "Images/NodeIcons/Event"),
            new("金钱", "Images/NodeIcons/JinQian"),
            new("人参", "Images/NodeIcons/RenShen"),
            new("人参果", "Images/NodeIcons/RenShenGuo"),
            new("商店", "Images/NodeIcons/Shop"),
            new("算卦", "Images/NodeIcons/SuanGua"),
            new("温泉", "Images/NodeIcons/WenQuan"),
            new("悟道", "Images/NodeIcons/WuDao"),
            new("修炼", "Images/NodeIcons/XiuLian"),
            new("以物易物", "Images/NodeIcons/YiWuYiWu"),
            
            // Characters
            new("徐福", "Images/Characters/01"),
            new("子非鱼", "Images/Characters/02"),
            new("子非燕", "Images/Characters/03"),
            new("风雨晴", "Images/Characters/04"),
            new("梦乃遥", "Images/Characters/05"),
            
            // Entities
            new("噬金甲", "Images/Bestiary/01"),
            new("墨蛟", "Images/Bestiary/02"),
            new("渊虾", "Images/Bestiary/03"),
            new("九尾狐", "Images/Bestiary/04"),
            new("推山兽", "Images/Bestiary/05"),
            new("白泽", "Images/Bestiary/06"),
            new("鲲", "Images/Bestiary/07"),
            new("毕方", "Images/Bestiary/08"),
            new("火蟾", "Images/Bestiary/09"),
            new("麒麟", "Images/Bestiary/10"),
            new("凌霄大圣", "Images/Bestiary/凌霄大圣"),
            new("龟仙人", "Images/Bestiary/龟仙人"),
            new("鹤仙人", "Images/Bestiary/鹤仙人"),
            new("鹿仙人", "Images/Bestiary/鹿仙人"),
            new("土行大圣", "Images/Bestiary/土行大圣"),

            // 卡牌
            new("缺失卡面插画", "Images/MissingSprite"),
            // new("聚气术", "Images/Cards/聚气术"),
            // new("冲撞", "Images/Cards/冲撞"),
            
            new("金刃",                                "Images/Cards/金刃"),
            new("寻猎",                                "Images/Cards/寻猎"),
            new("山风",                                "Images/Cards/山风"),
            new("起势",                                "Images/Cards/起势"),
            new("杀意",                                "Images/Cards/杀意"),
            new("刺穴",                                "Images/Cards/刺穴"),
            new("灵动",                                "Images/Cards/灵动"),
            new("凛冽",                                "Images/Cards/凛冽"),
            new("天地同寿",                              "Images/Cards/天地同寿"),
            new("无妄",                                "Images/Cards/无妄"),
            new("素弦",                                "Images/Cards/素弦"),
            new("袖里乾坤",                              "Images/Cards/袖里乾坤"),
            new("凝水",                                "Images/Cards/凝水"),
            new("一莲托生",                              "Images/Cards/一莲托生"),
            new("敛息",                                "Images/Cards/敛息"),
            new("停云",                                "Images/Cards/停云"),
            new("弹指",                                "Images/Cards/弹指"),
            new("恋花",                                "Images/Cards/恋花"),
            new("流霰",                                "Images/Cards/流霰"),
            new("吐纳",                                "Images/Cards/吐纳"),
            new("止水",                                "Images/Cards/止水"),
            new("空幻",                                "Images/Cards/空幻"),
            new("踏浪",                                "Images/Cards/踏浪"),
            new("写意",                                "Images/Cards/写意"),
            new("瑞雪",                                "Images/Cards/瑞雪"),
            new("无念无想",                              "Images/Cards/无念无想"),
            new("大鱼",                                "Images/Cards/大鱼"),
            new("苦寒",                                "Images/Cards/苦寒"),
            new("奔腾",                                "Images/Cards/奔腾"),
            new("气吞山河",                              "Images/Cards/气吞山河"),
            new("一梦如是",                              "Images/Cards/一梦如是"),
            new("吞天",                                "Images/Cards/吞天"),
            new("飞鸿踏雪",                              "Images/Cards/飞鸿踏雪"),
            new("镜花水月",                              "Images/Cards/镜花水月"),
            new("小松",                                "Images/Cards/小松"),
            new("潜龙在渊",                              "Images/Cards/潜龙在渊"),
            new("明神",                                "Images/Cards/明神"),
            new("水滴石穿",                              "Images/Cards/水滴石穿"),
            new("见龙在田",                              "Images/Cards/见龙在田"),
            new("回春",                                "Images/Cards/回春"),
            new("若竹",                                "Images/Cards/若竹"),
            new("入木三分",                              "Images/Cards/入木三分"),
            new("飞龙在天",                              "Images/Cards/飞龙在天"),
            new("彼岸花",                                "Images/Cards/彼岸花"),
            new("钟声",                                "Images/Cards/钟声"),
            new("弱昙",                                "Images/Cards/弱昙"),
            new("鹤回翔",                               "Images/Cards/鹤回翔"),
            new("落英",                                "Images/Cards/落英"),
            new("回响",                                "Images/Cards/回响"),
            new("一叶知秋",                              "Images/Cards/一叶知秋"),
            new("亢龙有悔",                              "Images/Cards/亢龙有悔"),
            new("刹那芳华",                              "Images/Cards/刹那芳华"),
            new("一念无量劫",                                 "Images/Cards/一念无量劫"),
            new("云袖",                                "Images/Cards/云袖"),
            new("轰天",                                "Images/Cards/轰天"),
            new("正念",                                "Images/Cards/正念"),
            new("拂晓",                                "Images/Cards/拂晓"),
            new("九射",                                "Images/Cards/九射"),
            new("不动明王诀",                                 "Images/Cards/不动明王诀"),
            new("浴火",                                "Images/Cards/浴火"),
            new("舍生",                                "Images/Cards/舍生"),
            new("天衣无缝",                              "Images/Cards/天衣无缝"),
            new("登宝塔",                               "Images/Cards/登宝塔"),
            new("燎原",                                "Images/Cards/燎原"),
            new("坐忘",                                "Images/Cards/坐忘"),
            new("剑王行",                               "Images/Cards/剑王行"),
            new("狂焰",                                "Images/Cards/狂焰"),
            new("观众生",                               "Images/Cards/观众生"),
            new("晚霞",                                "Images/Cards/晚霞"),
            new("一舞惊鸿",                              "Images/Cards/一舞惊鸿"),
            new("净天地",                               "Images/Cards/净天地"),
            new("窑土",                                "Images/Cards/窑土"),
            new("寸劲",                                "Images/Cards/寸劲"),
            new("八极拳",                               "Images/Cards/八极拳"),
            new("满招损",                               "Images/Cards/满招损"),
            new("流沙",                                "Images/Cards/流沙"),
            new("一力降十会",                                 "Images/Cards/一力降十会"),
            new("边腿",                                "Images/Cards/边腿"),
            new("连环腿",                               "Images/Cards/连环腿"),
            new("崩山掌",                               "Images/Cards/崩山掌"),
            new("玉骨",                                "Images/Cards/玉骨"),
            new("箭疾步",                               "Images/Cards/箭疾步"),
            new("震脚",                                "Images/Cards/震脚"),
            new("孤山",                                "Images/Cards/孤山"),
            new("龟息",                                "Images/Cards/龟息"),
            new("冰肌",                                "Images/Cards/冰肌"),
            new("一诺五岳",                              "Images/Cards/一诺五岳"),
            
            new("正域彼四方",                              "Images/Cards/正域彼四方"),
            new("射落金乌",                              "Images/Cards/射落金乌"),
            
            // 合成
            new("无法合成", "Images/MergeIllustrations/Invalid"),
            new("可以合成", "Images/MergeIllustrations/Valid"),
            new("金合成", "Images/MergeIllustrations/Jin"),
            new("水合成", "Images/MergeIllustrations/Shui"),
            new("木合成", "Images/MergeIllustrations/Mu"),
            new("火合成", "Images/MergeIllustrations/Huo"),
            new("土合成", "Images/MergeIllustrations/Tu"),
            
            // 背景
            new("练气背景", "Images/StageBackgrounds/01"),
            new("筑基背景", "Images/StageBackgrounds/02"),
            new("金丹背景", "Images/StageBackgrounds/03"),
            new("元婴背景", "Images/StageBackgrounds/04"),
            new("化神背景", "Images/StageBackgrounds/05"),
            
            // 阵法
            new("未激活阵法背景", "Images/Formation/Backgrounds/Inactive"),
            new("练气阵法背景", "Images/Formation/Backgrounds/LianQi"),
            new("筑基阵法背景", "Images/Formation/Backgrounds/ZhuJi"),
            new("金丹阵法背景", "Images/Formation/Backgrounds/JinDan"),
            new("元婴阵法背景", "Images/Formation/Backgrounds/YuanYing"),
            new("化神阵法背景", "Images/Formation/Backgrounds/HuaShen"),
            
            new("普通阵法发光", "Images/Formation/Glow/Normal"),
            new("接近激活阵法发光", "Images/Formation/Glow/NearExcited"),
            
            new("金灵阵", "Images/Formation/Icons/金灵阵"),
            new("水灵阵", "Images/Formation/Icons/水灵阵"),
            new("木灵阵", "Images/Formation/Icons/木灵阵"),
            new("火灵阵", "Images/Formation/Icons/火灵阵"),
            new("土灵阵", "Images/Formation/Icons/土灵阵"),
            new("攻击阵", "Images/Formation/Icons/攻击阵"),
            new("防御阵", "Images/Formation/Icons/防御阵"),
            new("灵气阵", "Images/Formation/Icons/灵气阵"),
            new("气血阵", "Images/Formation/Icons/气血阵"),
            new("燃命阵", "Images/Formation/Icons/燃命阵"),
            
            // RoomIcons
            new("AdventureRoomIcon", "Images/RoomIcons/AdventureRoomIcon"),
            new("AscensionRoomIcon", "Images/RoomIcons/AscensionRoomIcon"),
            new("BossRoomIcon", "Images/RoomIcons/BossRoomIcon"),
            new("EliteRoomIcon", "Images/RoomIcons/EliteRoomIcon"),
            new("UnderlingRoomIcon", "Images/RoomIcons/UnderlingRoomIcon"),
            new("RestRoomIcon", "Images/RoomIcons/RestRoomIcon"),
            new("ShopRoomIcon", "Images/RoomIcons/ShopRoomIcon"),
            new("EncounterRoomIcon", "Images/RoomIcons/EncounterRoomIcon"),
            
            // ShopIllustrations
            new("以物易物", "Images/ShopIllustrations/以物易物"),
            new("收藏家", "Images/ShopIllustrations/收藏家"),
            new("毕业季", "Images/ShopIllustrations/毕业季"),
            new("盲盒", "Images/ShopIllustrations/盲盒"),
            new("黑市", "Images/ShopIllustrations/黑市"),
        });
    }

    public SpriteEntry MissingSkillIllustration() => this["缺失卡面插画"];
    public SpriteEntry ErrorBuffIcon() => this["不存在的状态"];
    public SpriteEntry MissingBuffIcon() => this["缺失状态插画"];
}
