
using System.Collections.Generic;

public class SpriteCategory : Category<SpriteEntry>
{
    public SpriteCategory()
    {
        AddRange(new List<SpriteEntry>()
        {
            new("Sprite0001", "缺失卡面插画", "Images/MissingSprite"),
            
            // JingJie Icons
            new("Sprite0002", "JingJie练气", "Images/JingJieIcons/LianQi"),
            new("Sprite0003", "JingJie筑基", "Images/JingJieIcons/ZhuJi"),
            new("Sprite0004", "JingJie金丹", "Images/JingJieIcons/JinDan"),
            new("Sprite0005", "JingJie元婴", "Images/JingJieIcons/YuanYing"),
            new("Sprite0006", "JingJie化神", "Images/JingJieIcons/HuaShen"),
            new("Sprite0007", "JingJie返虚", "Images/JingJieIcons/FanXu"),
            
            // BuffIcons
            new("Sprite0008", "Buff不存在", "Images/BuffIcons/不存在"),
            new("Sprite0009", "Buff缺失插画", "Images/BuffIcons/缺失插画"),
            
            // NodeIcons
            new("Sprite0068", "奇遇", "Images/NodeIcons/Adventure"),
            new("Sprite0069", "战斗", "Images/NodeIcons/Battle"),
            new("Sprite0070", "Boss", "Images/NodeIcons/Boss"),
            new("Sprite0071", "事件", "Images/NodeIcons/Event"),
            new("Sprite0072", "金钱", "Images/NodeIcons/JinQian"),
            new("Sprite0073", "人参", "Images/NodeIcons/RenShen"),
            new("Sprite0074", "人参果", "Images/NodeIcons/RenShenGuo"),
            new("Sprite0075", "商店", "Images/NodeIcons/Shop"),
            new("Sprite0076", "算卦", "Images/NodeIcons/SuanGua"),
            new("Sprite0077", "温泉", "Images/NodeIcons/WenQuan"),
            new("Sprite0078", "悟道", "Images/NodeIcons/WuDao"),
            new("Sprite0079", "修炼", "Images/NodeIcons/XiuLian"),
            new("Sprite0080", "以物易物", "Images/NodeIcons/YiWuYiWu"),
            
            // RunConfigTabCharacters
            new("Sprite06_001", "RunConfigTab缺失", "Images/RunConfigTabs/缺失"),
            new("Sprite06_002", "RunConfigTab徐福", "Images/RunConfigTabs/徐福"),
            new("Sprite06_003", "RunConfigTab子非鱼", "Images/RunConfigTabs/子非鱼"),
            new("Sprite06_004", "RunConfigTab子非燕", "Images/RunConfigTabs/子非燕"),
            new("Sprite06_005", "RunConfigTab彼此卿", "Images/RunConfigTabs/彼此卿"),
            new("Sprite06_006", "RunConfigTab风雨晴", "Images/RunConfigTabs/风雨晴"),
            
            // RunConfigIconCharacters
            new("Sprite07_001", "RunConfigIcon缺失", "Images/RunConfigIcons/缺失"),
            new("Sprite07_002", "RunConfigIcon徐福", "Images/RunConfigIcons/徐福"),
            new("Sprite07_003", "RunConfigIcon子非鱼", "Images/RunConfigIcons/子非鱼"),
            new("Sprite07_004", "RunConfigIcon子非燕", "Images/RunConfigIcons/子非燕"),
            new("Sprite07_005", "RunConfigIcon彼此卿", "Images/RunConfigIcons/彼此卿"),
            new("Sprite07_006", "RunConfigIcon风雨晴", "Images/RunConfigIcons/风雨晴"),
            
            // CharacterIcons
            new("Sprite0085", $"CharacterIcon缺失", "Images/CharacterIcons/风雨晴"),
            new("Sprite0086", $"CharacterIconSelect缺失", "Images/CharacterIcons/风雨晴Select"),
            new("Sprite0087", $"CharacterIcon徐福", "Images/CharacterIcons/徐福"),
            new("Sprite0088", $"CharacterIconSelect徐福", "Images/CharacterIcons/徐福Select"),
            new("Sprite0089", $"CharacterIcon子非鱼", "Images/CharacterIcons/子非鱼"),
            new("Sprite0090", $"CharacterIconSelect子非鱼", "Images/CharacterIcons/子非鱼Select"),
            new("Sprite0091", $"CharacterIcon子非燕", "Images/CharacterIcons/子非燕"),
            new("Sprite0092", $"CharacterIconSelect子非燕", "Images/CharacterIcons/子非燕Select"),
            new("Sprite0093", $"CharacterIcon彼此卿", "Images/CharacterIcons/彼此卿"),
            new("Sprite0094", $"CharacterIconSelect彼此卿", "Images/CharacterIcons/彼此卿Select"),
            new("Sprite0095", $"CharacterIcon风雨晴", "Images/CharacterIcons/风雨晴"),
            new("Sprite0096", $"CharacterIconSelect风雨晴", "Images/CharacterIcons/风雨晴Select"),

            // 卡牌
            new("Sprite08_001", "SkillCard缺失插画", "Images/SkillCardIllustrations/缺失插画"),
            new("Sprite08_002", "SkillBar缺失插画", "Images/SkillBarIllustrations/缺失插画"),
            
            // 合成
            new("Sprite0113", "无法合成", "Images/MergeIllustrations/Invalid"),
            new("Sprite0114", "可以合成", "Images/MergeIllustrations/Valid"),
            new("Sprite0115", "金合成", "Images/MergeIllustrations/Jin"),
            new("Sprite0116", "水合成", "Images/MergeIllustrations/Shui"),
            new("Sprite0117", "木合成", "Images/MergeIllustrations/Mu"),
            new("Sprite0118", "火合成", "Images/MergeIllustrations/Huo"),
            new("Sprite0119", "土合成", "Images/MergeIllustrations/Tu"),
            
            // 背景
            new("Sprite0120", "练气背景", "Images/StageBackgrounds/01"),
            new("Sprite0121", "筑基背景", "Images/StageBackgrounds/02"),
            new("Sprite0122", "金丹背景", "Images/StageBackgrounds/03"),
            new("Sprite0123", "元婴背景", "Images/StageBackgrounds/04"),
            new("Sprite0124", "化神背景", "Images/StageBackgrounds/05"),
            
            // 阵法
            new("Sprite0125", "未激活阵法背景", "Images/Formation/Backgrounds/Inactive"),
            new("Sprite0126", "练气阵法背景", "Images/Formation/Backgrounds/LianQi"),
            new("Sprite0127", "筑基阵法背景", "Images/Formation/Backgrounds/ZhuJi"),
            new("Sprite0128", "金丹阵法背景", "Images/Formation/Backgrounds/JinDan"),
            new("Sprite0129", "元婴阵法背景", "Images/Formation/Backgrounds/YuanYing"),
            new("Sprite0130", "化神阵法背景", "Images/Formation/Backgrounds/HuaShen"),
            
            new("Sprite0131", "普通阵法发光", "Images/Formation/Glow/Normal"),
            new("Sprite0132", "接近激活阵法发光", "Images/Formation/Glow/NearExcited"),
            
            new("Sprite0133", "金灵阵", "Images/Formation/Icons/金灵阵"),
            new("Sprite0134", "水灵阵", "Images/Formation/Icons/水灵阵"),
            new("Sprite0135", "木灵阵", "Images/Formation/Icons/木灵阵"),
            new("Sprite0136", "火灵阵", "Images/Formation/Icons/火灵阵"),
            new("Sprite0137", "土灵阵", "Images/Formation/Icons/土灵阵"),
            new("Sprite0138", "攻击阵", "Images/Formation/Icons/攻击阵"),
            new("Sprite0139", "防御阵", "Images/Formation/Icons/防御阵"),
            new("Sprite0140", "灵气阵", "Images/Formation/Icons/灵气阵"),
            new("Sprite0141", "气血阵", "Images/Formation/Icons/气血阵"),
            new("Sprite0142", "燃命阵", "Images/Formation/Icons/燃命阵"),
            
            // TagIcons
            new("Sprite14_001", "Tag金", "Images/TagIcons/金"),
            new("Sprite14_002", "Tag水", "Images/TagIcons/水"),
            new("Sprite14_003", "Tag木", "Images/TagIcons/木"),
            new("Sprite14_004", "Tag火", "Images/TagIcons/火"),
            new("Sprite14_005", "Tag土", "Images/TagIcons/土"),
            new("Sprite14_006", "Tag无色", "Images/TagIcons/无色"),
            new("Sprite14_007", "Tag攻击", "Images/TagIcons/攻击"),
            new("Sprite14_008", "Tag防御", "Images/TagIcons/防御"),
            new("Sprite14_009", "Tag灵气", "Images/TagIcons/灵气"),
            new("Sprite14_010", "Tag气血", "Images/TagIcons/气血"),
            new("Sprite14_011", "Tag二动", "Images/TagIcons/二动"),
            new("Sprite14_012", "Tag自指", "Images/TagIcons/自指"),
            new("Sprite14_013", "Tag升华", "Images/TagIcons/升华"),
            new("Sprite14_014", "Tag一次性", "Images/TagIcons/一次性"),
            
            // WuXingDeco
            new("Sprite15_001", "WuXingDeco金", "Images/WuXingDecoIcons/WuXingDeco金"),
            new("Sprite15_002", "WuXingDeco水", "Images/WuXingDecoIcons/WuXingDeco水"),
            new("Sprite15_003", "WuXingDeco木", "Images/WuXingDecoIcons/WuXingDeco木"),
            new("Sprite15_004", "WuXingDeco火", "Images/WuXingDecoIcons/WuXingDeco火"),
            new("Sprite15_005", "WuXingDeco土", "Images/WuXingDecoIcons/WuXingDeco土"),
            new("Sprite15_006", "WuXingDeco无色", "Images/WuXingDecoIcons/WuXingDeco无色"),
            
            // RoomIcons
            new("Sprite0152", "AdventureRoomIcon", "Images/RoomIcons/AdventureRoomIcon"),
            new("Sprite0153", "AscensionRoomIcon", "Images/RoomIcons/AscensionRoomIcon"),
            new("Sprite0154", "BossRoomIcon", "Images/RoomIcons/BossRoomIcon"),
            new("Sprite0155", "EliteRoomIcon", "Images/RoomIcons/EliteRoomIcon"),
            new("Sprite0156", "UnderlingRoomIcon", "Images/RoomIcons/UnderlingRoomIcon"),
            new("Sprite0157", "RestRoomIcon", "Images/RoomIcons/RestRoomIcon"),
            new("Sprite0158", "ShopRoomIcon", "Images/RoomIcons/ShopRoomIcon"),
            new("Sprite0159", "EncounterRoomIcon", "Images/RoomIcons/EncounterRoomIcon"),
            
            // ShopIllustrations
            new("Sprite0160", "以物易物", "Images/ShopIllustrations/以物易物"),
            new("Sprite0161", "收藏家", "Images/ShopIllustrations/收藏家"),
            new("Sprite0162", "毕业季", "Images/ShopIllustrations/毕业季"),
            new("Sprite0163", "盲盒", "Images/ShopIllustrations/盲盒"),
            new("Sprite0164", "黑市", "Images/ShopIllustrations/黑市"),
            
            // FaceIcons
            new("Sprite0165", "Afraid", "Images/FaceIcons/Afraid"),
            new("Sprite0166", "Smirk", "Images/FaceIcons/Smirk"),
            
            // ArmorIcons
            new("Sprite0167", "ArmorIcon", "Images/ArmorIcons/ArmorIcon"),
            new("Sprite0168", "FragileIcon", "Images/ArmorIcons/FragileIcon"),
            
            // Packs
            new("Sprite0169", "Pack丹兵道", "Images/PackIllustrations/丹兵道"),
            new("Sprite0170", "Pack化哉", "Images/PackIllustrations/化哉"),
            new("Sprite0171", "Pack大椿功", "Images/PackIllustrations/大椿功"),
            new("Sprite0172", "Pack大焚天秘乘", "Images/PackIllustrations/大焚天秘乘"),
            new("Sprite0173", "Pack大音希声", "Images/PackIllustrations/大音希声"),
            new("Sprite0174", "Pack天河引气录", "Images/PackIllustrations/天河引气录"),
            new("Sprite0175", "Pack归鸿十二步", "Images/PackIllustrations/归鸿十二步"),
            new("Sprite0176", "Pack御虚诀", "Images/PackIllustrations/御虚诀"),
            new("Sprite0177", "Pack无常路引", "Images/PackIllustrations/无常路引"),
            new("Sprite0178", "Pack游龙遁", "Images/PackIllustrations/游龙遁"),
            new("Sprite0179", "Pack锻体四则", "Images/PackIllustrations/锻体四则"),
            new("Sprite0180", "Pack须弥妙法", "Images/PackIllustrations/须弥妙法"),
            
            // Unlock Icons
            new("Sprite0181", "UnlockIcon徐福专精", "Images/UnlockIcons/徐福专精"),
            new("Sprite0182", "UnlockIcon剑心通明", "Images/UnlockIcons/剑心通明"),
            new("Sprite0183", "UnlockIcon五彩缤纷", "Images/UnlockIcons/五彩缤纷"),
            new("Sprite0184", "UnlockIcon灵气灌顶", "Images/UnlockIcons/灵气灌顶"),
            new("Sprite0185", "UnlockIcon不染凡尘", "Images/UnlockIcons/不染凡尘"),
            new("Sprite0186", "UnlockIcon道基初成", "Images/UnlockIcons/道基初成"),
            new("Sprite0187", "UnlockIcon丹火正旺", "Images/UnlockIcons/丹火正旺"),
            new("Sprite0188", "UnlockIcon子非鱼专精", "Images/UnlockIcons/子非鱼专精"),
            new("Sprite0189", "UnlockIcon风驰电掣", "Images/UnlockIcons/风驰电掣"),
            new("Sprite0190", "UnlockIcon五行化生", "Images/UnlockIcons/五行化生"),
            new("Sprite0191", "UnlockIcon国士无双", "Images/UnlockIcons/国士无双"),
            new("Sprite0192", "UnlockIcon顺风顺水", "Images/UnlockIcons/顺风顺水"),
            new("Sprite0193", "UnlockIcon杀伐果断", "Images/UnlockIcons/杀伐果断"),
            new("Sprite0194", "UnlockIcon斡旋造化", "Images/UnlockIcons/斡旋造化"),
            new("Sprite0195", "UnlockIcon子非燕专精", "Images/UnlockIcons/子非燕专精"),
            new("Sprite0196", "UnlockIcon破而后立", "Images/UnlockIcons/破而后立"),
            new("Sprite0197", "UnlockIcon千机百变", "Images/UnlockIcons/千机百变"),
            new("Sprite0198", "UnlockIcon一日筑基", "Images/UnlockIcons/一日筑基"),
            new("Sprite0199", "UnlockIcon乾坤大挪移", "Images/UnlockIcons/乾坤大挪移"),
            new("Sprite0200", "UnlockIcon财运亨通", "Images/UnlockIcons/财运亨通"),
            new("Sprite0201", "UnlockIcon九莲宝灯", "Images/UnlockIcons/九莲宝灯"),
            new("Sprite0202", "UnlockIcon彼此卿专精", "Images/UnlockIcons/彼此卿专精"),
            new("Sprite0203", "UnlockIcon雷劫余韵", "Images/UnlockIcons/雷劫余韵"),
            new("Sprite0204", "UnlockIcon妙手空空", "Images/UnlockIcons/妙手空空"),
            new("Sprite0205", "UnlockIcon精打细算", "Images/UnlockIcons/精打细算"),
            new("Sprite0206", "UnlockIcon返璞归真", "Images/UnlockIcons/返璞归真"),
            new("Sprite0207", "UnlockIcon身强体壮", "Images/UnlockIcons/身强体壮"),
            new("Sprite0208", "UnlockIcon斗转星移", "Images/UnlockIcons/斗转星移"),
            new("Sprite0209", "UnlockIcon风雨晴专精", "Images/UnlockIcons/风雨晴专精"),
            new("Sprite0210", "UnlockIcon逆天改命", "Images/UnlockIcons/逆天改命"),
            new("Sprite0211", "UnlockIcon缘法天成", "Images/UnlockIcons/缘法天成"),
            new("Sprite0212", "UnlockIcon真元澎湃", "Images/UnlockIcons/真元澎湃"),
            new("Sprite0213", "UnlockIcon逍遥游", "Images/UnlockIcons/逍遥游"),
            new("Sprite0214", "UnlockIcon金碧辉煌", "Images/UnlockIcons/金碧辉煌"),
            new("Sprite0215", "UnlockIcon七星连珠", "Images/UnlockIcons/七星连珠"),
            new("Sprite0216", "UnlockIcon百炼成钢", "Images/UnlockIcons/百炼成钢"),
            new("Sprite0217", "UnlockIcon剑气冲霄", "Images/UnlockIcons/剑气冲霄"),
            new("Sprite0218", "UnlockIcon气贯长虹", "Images/UnlockIcons/气贯长虹"),
            new("Sprite0219", "UnlockIcon生生不息", "Images/UnlockIcons/生生不息"),
            new("Sprite0220", "UnlockIcon融会贯通", "Images/UnlockIcons/融会贯通"),
            new("Sprite0221", "UnlockIcon凌波微步", "Images/UnlockIcons/凌波微步"),
            new("Sprite0222", "UnlockIcon倾国倾城", "Images/UnlockIcons/倾国倾城"),
            new("Sprite0223", "UnlockIcon无欲则刚", "Images/UnlockIcons/无欲则刚"),
            new("Sprite0224", "UnlockIcon巍然矗立", "Images/UnlockIcons/巍然矗立"),
            new("Sprite0225", "UnlockIcon一锤定音", "Images/UnlockIcons/一锤定音"),
            new("Sprite0226", "UnlockIcon腾云驾雾", "Images/UnlockIcons/腾云驾雾"),
            new("Sprite0227", "UnlockIcon流转达人", "Images/UnlockIcons/流转达人"),
            new("Sprite0228", "UnlockIcon初窥门径", "Images/UnlockIcons/初窥门径"),
            new("Sprite0229", "UnlockIcon略有小成", "Images/UnlockIcons/略有小成"),
            new("Sprite0230", "UnlockIcon渐入佳境", "Images/UnlockIcons/渐入佳境"),
            new("Sprite0231", "UnlockIcon出神入化", "Images/UnlockIcons/出神入化"),
            
            // PackConstraints
            new("Sprite0232", "PackConstraints金", "Images/PackConstraintIllustrations/Jin"),
            new("Sprite0233", "PackConstraints水", "Images/PackConstraintIllustrations/Shui"),
            new("Sprite0234", "PackConstraints木", "Images/PackConstraintIllustrations/Mu"),
            new("Sprite0235", "PackConstraints火", "Images/PackConstraintIllustrations/Huo"),
            new("Sprite0236", "PackConstraints土", "Images/PackConstraintIllustrations/Tu"),
            new("Sprite0237", "PackConstraints任意", "Images/PackConstraintIllustrations/Any"),
            
            // RunResultIllustrations
            new("Sprite0238", "RunResultIllustrationWin", "Images/RunResultIllustrations/Win"),
            new("Sprite0239", "RunResultIllustrationLose", "Images/RunResultIllustrations/Lose"),
            
            // Event
            new("Sprite0240", "Event缺失插画", "Images/EventIllustrations/缺失插画"),
            new("Sprite0241", "Event不存在的事件", "Images/EventIllustrations/不存在的事件"),
            new("Sprite0242", "Event丢尺子", "Images/EventIllustrations/丢尺子"),
            new("Sprite0243", "Event仙人下棋", "Images/EventIllustrations/仙人下棋"),
            new("Sprite0244", "Event仙岛玉液酒", "Images/EventIllustrations/仙岛玉液酒"),
            new("Sprite0245", "Event全等合成", "Images/EventIllustrations/全等合成"),
            new("Sprite0246", "Event出门", "Images/EventIllustrations/出门"),
            new("Sprite0247", "Event分子打印机", "Images/EventIllustrations/分子打印机"),
            new("Sprite0248", "Event境界突破", "Images/EventIllustrations/境界突破"),
            new("Sprite0249", "Event夏虫语冰", "Images/EventIllustrations/夏虫语冰"),
            new("Sprite0250", "Event天机阁", "Images/EventIllustrations/天机阁"),
            new("Sprite0251", "Event天津四", "Images/EventIllustrations/天津四"),
            new("Sprite0252", "Event天界树", "Images/EventIllustrations/天界树"),
            new("Sprite0253", "Event守株待兔", "Images/EventIllustrations/守株待兔"),
            new("Sprite0254", "Event山木", "Images/EventIllustrations/山木"),
            new("Sprite0255", "Event愿望单", "Images/EventIllustrations/愿望单"),
            new("Sprite0256", "Event我已膨胀", "Images/EventIllustrations/我已膨胀"),
            new("Sprite0257", "Event曹操三笑", "Images/EventIllustrations/曹操三笑"),
            new("Sprite0258", "Event检测仪", "Images/EventIllustrations/检测仪"),
            new("Sprite0259", "Event神灯精灵", "Images/EventIllustrations/神灯精灵"),
            new("Sprite0260", "Event解梦师", "Images/EventIllustrations/解梦师"),
            new("Sprite0261", "Event论无穷", "Images/EventIllustrations/论无穷"),
            new("Sprite0262", "Event连抽五张", "Images/EventIllustrations/连抽五张"),
            new("Sprite0263", "Event重新尝试教学", "Images/EventIllustrations/重新尝试教学"),
            new("Sprite0264", "Event鸡肉面", "Images/EventIllustrations/鸡肉面"),
            // 目前以下两个没有用到
            // 全等合成
            // 重新尝试教学
        });
    }

    public SpriteEntry MissingSkillCardIllustration() => FromName("SkillCard缺失插画");
    public SpriteEntry MissingSkillBarIllustration() => FromName("SkillBar缺失插画");
    public SpriteEntry ErrorBuffIcon() => FromName("Buff不存在");
    public SpriteEntry MissingBuffIcon() => FromName("Buff缺失插画");
    public SpriteEntry MissingEventIllustration() => FromName("Event缺失插画");
}
