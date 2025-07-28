
using System.Collections.Generic;

public class SpriteCategory : Category<SpriteEntry>
{
    public SpriteCategory()
    {
        AddRange(new List<SpriteEntry>()
        {
            new("缺失卡面插画", "Images/MissingSprite"),
            
            // JingJie Icons
            new("JingJie练气", "Images/JingJieIcons/练气"),
            new("JingJie筑基", "Images/JingJieIcons/筑基"),
            new("JingJie金丹", "Images/JingJieIcons/金丹"),
            new("JingJie元婴", "Images/JingJieIcons/元婴"),
            new("JingJie化神", "Images/JingJieIcons/化神"),
            new("JingJie返虚", "Images/JingJieIcons/返虚"),
            
            // BuffIcons
            new("Buff不存在", "Images/BuffIcons/不存在"),
            new("Buff缺失插画", "Images/BuffIcons/缺失插画"),
            
            new("Buff抱朴", "Images/BuffIcons/抱朴"),
            new("Buff暴击", "Images/BuffIcons/暴击"),
            new("Buff不堪一击", "Images/BuffIcons/不堪一击"),
            new("Buff缠绕", "Images/BuffIcons/缠绕"),
            new("Buff穿透", "Images/BuffIcons/穿透"),
            new("Buff多重", "Images/BuffIcons/多重"),
            new("Buff二动", "Images/BuffIcons/二动"),
            new("Buff锋锐", "Images/BuffIcons/锋锐"),
            new("Buff腐朽", "Images/BuffIcons/腐朽"),
            new("Buff格挡", "Images/BuffIcons/格挡"),
            new("Buff观众生", "Images/BuffIcons/观众生"),
            new("Buff架势", "Images/BuffIcons/架势"),
            new("Buff坚毅", "Images/BuffIcons/坚毅"),
            new("Buff禁止二动", "Images/BuffIcons/禁止二动"),
            new("Buff禁止行动", "Images/BuffIcons/禁止行动"),
            new("Buff禁止治疗", "Images/BuffIcons/禁止治疗"),
            new("Buff力量", "Images/BuffIcons/力量"),
            new("Buff敛息", "Images/BuffIcons/敛息"),
            new("Buff灵气", "Images/BuffIcons/灵气"),
            new("Buff灵气返还", "Images/BuffIcons/灵气返还"),
            new("Buff轮暴击", "Images/BuffIcons/轮暴击"),
            new("Buff轮穿透", "Images/BuffIcons/轮穿透"),
            new("Buff轮吸血", "Images/BuffIcons/轮吸血"),
            new("Buff免费", "Images/BuffIcons/免费"),
            new("Buff内伤", "Images/BuffIcons/内伤"),
            new("Buff升华", "Images/BuffIcons/升华"),
            new("Buff最后一张牌升华", "Images/BuffIcons/最后一张牌升华"),
            new("Buff清心", "Images/BuffIcons/清心"),
            new("Buff软弱", "Images/BuffIcons/软弱"),
            new("Buff闪避", "Images/BuffIcons/闪避"),
            new("Buff太虚", "Images/BuffIcons/太虚"),
            new("Buff跳行动", "Images/BuffIcons/跳行动"),
            new("Buff跳卡牌", "Images/BuffIcons/跳卡牌"),
            new("Buff跳走步", "Images/BuffIcons/跳走步"),
            new("Buff禁止攻击", "Images/BuffIcons/禁止攻击"),
            new("Buff禁止聚灵", "Images/BuffIcons/禁止聚灵"),
            new("Buff吸血", "Images/BuffIcons/吸血"),
            new("Buff心斋", "Images/BuffIcons/心斋"),
            new("Buff延迟攻", "Images/BuffIcons/延迟攻"),
            new("Buff延迟护甲", "Images/BuffIcons/延迟护甲"),
            new("Buff滞气", "Images/BuffIcons/滞气"),
            new("Buff钟声", "Images/BuffIcons/钟声"),
            new("Buff灼烧", "Images/BuffIcons/灼烧"),
            
            new("Buff淬体", "Images/BuffIcons/淬体"),
            new("Buff锻体", "Images/BuffIcons/锻体"),
            new("Buff飞龙在天", "Images/BuffIcons/飞龙在天"),
            new("Buff凤凰涅槃", "Images/BuffIcons/凤凰涅槃"),
            new("Buff护甲返还", "Images/BuffIcons/护甲返还"),
            new("Buff击伤赋予护甲", "Images/BuffIcons/击伤赋予护甲"),
            new("Buff摩诃钵特摩", "Images/BuffIcons/摩诃钵特摩"),
            new("Buff人间无戈", "Images/BuffIcons/人间无戈"),
            new("Buff盛开", "Images/BuffIcons/盛开"),
            new("Buff天人合一", "Images/BuffIcons/天人合一"),
            new("Buff天衣无缝", "Images/BuffIcons/天衣无缝"),
            new("Buff通透世界", "Images/BuffIcons/通透世界"),
            new("Buff玄武吐息法", "Images/BuffIcons/玄武吐息法"),
            new("Buff一梦如是", "Images/BuffIcons/一梦如是"),
            new("Buff诸行无常", "Images/BuffIcons/诸行无常"),
            
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
            new("CharacterPortrait缺失立绘", "Images/Characters/缺失立绘"),
            new("徐福", "Images/Characters/01"),
            new("子非鱼", "Images/Characters/02"),
            new("子非燕", "Images/Characters/03"),
            new("风雨晴", "Images/Characters/04"),
            new("梦乃遥", "Images/Characters/05"),
            
            // CharacterIcons
            new($"CharacterIcon徐福", "Images/CharacterIcons/徐福"),
            new($"CharacterIconSelect徐福", "Images/CharacterIcons/徐福Select"),
            new($"CharacterIcon子非鱼", "Images/CharacterIcons/子非鱼"),
            new($"CharacterIconSelect子非鱼", "Images/CharacterIcons/子非鱼Select"),
            new($"CharacterIcon子非燕", "Images/CharacterIcons/子非燕"),
            new($"CharacterIconSelect子非燕", "Images/CharacterIcons/子非燕Select"),
            new($"CharacterIcon彼此卿", "Images/CharacterIcons/彼此卿"),
            new($"CharacterIconSelect彼此卿", "Images/CharacterIcons/彼此卿Select"),
            new($"CharacterIcon风雨晴", "Images/CharacterIcons/风雨晴"),
            new($"CharacterIconSelect风雨晴", "Images/CharacterIcons/风雨晴Select"),
            
            // Entities
            new("噬金甲", "Images/Monsters/01"),
            new("墨蛟", "Images/Monsters/02"),
            new("渊虾", "Images/Monsters/03"),
            new("九尾狐", "Images/Monsters/04"),
            new("推山兽", "Images/Monsters/05"),
            new("白泽", "Images/Monsters/06"),
            new("鲲", "Images/Monsters/07"),
            new("毕方", "Images/Monsters/08"),
            new("火蟾", "Images/Monsters/09"),
            new("麒麟", "Images/Monsters/10"),
            new("凌霄大圣", "Images/Monsters/鹿仙人"),
            new("龟仙人", "Images/Monsters/鹤仙人"),
            new("鹤仙人", "Images/Monsters/鹿仙人"),
            new("鹿仙人", "Images/Monsters/鹤仙人"),
            new("土行大圣", "Images/Monsters/鹿仙人"),

            // 卡牌
            new("Skill缺失插画",                              "Images/CardIllustrations/缺失插画"),
            
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
            
            // TagIcons
            new("Tag金", "Images/TagIcons/金"),
            new("Tag水", "Images/TagIcons/水"),
            new("Tag木", "Images/TagIcons/木"),
            new("Tag火", "Images/TagIcons/火"),
            new("Tag土", "Images/TagIcons/土"),
            new("Tag攻击", "Images/TagIcons/攻击"),
            new("Tag防御", "Images/TagIcons/防御"),
            new("Tag灵气", "Images/TagIcons/灵气"),
            new("Tag气血", "Images/TagIcons/气血"),
            
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
            
            // FaceIcons
            new("Afraid", "Images/FaceIcons/Afraid"),
            new("Smirk", "Images/FaceIcons/Smirk"),
            
            // ArmorIcons
            new("ArmorIcon", "Images/ArmorIcons/ArmorIcon"),
            new("FragileIcon", "Images/ArmorIcons/FragileIcon"),
            
            // Packs
            new("Pack丹兵道", "Images/PackIllustrations/丹兵道"),
            new("Pack化哉", "Images/PackIllustrations/化哉"),
            new("Pack大椿功", "Images/PackIllustrations/大椿功"),
            new("Pack大焚天秘乘", "Images/PackIllustrations/大焚天秘乘"),
            new("Pack大音希声", "Images/PackIllustrations/大音希声"),
            new("Pack天河引气录", "Images/PackIllustrations/天河引气录"),
            new("Pack归鸿十二步", "Images/PackIllustrations/归鸿十二步"),
            new("Pack御虚诀", "Images/PackIllustrations/御虚诀"),
            new("Pack无常路引", "Images/PackIllustrations/无常路引"),
            new("Pack游龙遁", "Images/PackIllustrations/游龙遁"),
            new("Pack锻体四则", "Images/PackIllustrations/锻体四则"),
            new("Pack须弥妙法", "Images/PackIllustrations/须弥妙法"),
            
            // Unlock Icons
            new("UnlockIcon徐福专精", "Images/UnlockIcons/徐福专精"),
            new("UnlockIcon剑心通明", "Images/UnlockIcons/剑心通明"),
            new("UnlockIcon五彩缤纷", "Images/UnlockIcons/五彩缤纷"),
            new("UnlockIcon灵气灌顶", "Images/UnlockIcons/灵气灌顶"),
            new("UnlockIcon不染凡尘", "Images/UnlockIcons/不染凡尘"),
            new("UnlockIcon道基初成", "Images/UnlockIcons/道基初成"),
            new("UnlockIcon丹火正旺", "Images/UnlockIcons/丹火正旺"),
            new("UnlockIcon子非鱼专精", "Images/UnlockIcons/子非鱼专精"),
            new("UnlockIcon风驰电掣", "Images/UnlockIcons/风驰电掣"),
            new("UnlockIcon五行化生", "Images/UnlockIcons/五行化生"),
            new("UnlockIcon国士无双", "Images/UnlockIcons/国士无双"),
            new("UnlockIcon顺风顺水", "Images/UnlockIcons/顺风顺水"),
            new("UnlockIcon杀伐果断", "Images/UnlockIcons/杀伐果断"),
            new("UnlockIcon斡旋造化", "Images/UnlockIcons/斡旋造化"),
            new("UnlockIcon子非燕专精", "Images/UnlockIcons/子非燕专精"),
            new("UnlockIcon破而后立", "Images/UnlockIcons/破而后立"),
            new("UnlockIcon千机百变", "Images/UnlockIcons/千机百变"),
            new("UnlockIcon一日筑基", "Images/UnlockIcons/一日筑基"),
            new("UnlockIcon乾坤大挪移", "Images/UnlockIcons/乾坤大挪移"),
            new("UnlockIcon财运亨通", "Images/UnlockIcons/财运亨通"),
            new("UnlockIcon九莲宝灯", "Images/UnlockIcons/九莲宝灯"),
            new("UnlockIcon彼此卿专精", "Images/UnlockIcons/彼此卿专精"),
            new("UnlockIcon雷劫余韵", "Images/UnlockIcons/雷劫余韵"),
            new("UnlockIcon妙手空空", "Images/UnlockIcons/妙手空空"),
            new("UnlockIcon精打细算", "Images/UnlockIcons/精打细算"),
            new("UnlockIcon返璞归真", "Images/UnlockIcons/返璞归真"),
            new("UnlockIcon身强体壮", "Images/UnlockIcons/身强体壮"),
            new("UnlockIcon斗转星移", "Images/UnlockIcons/斗转星移"),
            new("UnlockIcon风雨晴专精", "Images/UnlockIcons/风雨晴专精"),
            new("UnlockIcon逆天改命", "Images/UnlockIcons/逆天改命"),
            new("UnlockIcon缘法天成", "Images/UnlockIcons/缘法天成"),
            new("UnlockIcon真元澎湃", "Images/UnlockIcons/真元澎湃"),
            new("UnlockIcon逍遥游", "Images/UnlockIcons/逍遥游"),
            new("UnlockIcon金碧辉煌", "Images/UnlockIcons/金碧辉煌"),
            new("UnlockIcon七星连珠", "Images/UnlockIcons/七星连珠"),
            new("UnlockIcon百炼成钢", "Images/UnlockIcons/百炼成钢"),
            new("UnlockIcon剑气冲霄", "Images/UnlockIcons/剑气冲霄"),
            new("UnlockIcon气贯长虹", "Images/UnlockIcons/气贯长虹"),
            new("UnlockIcon生生不息", "Images/UnlockIcons/生生不息"),
            new("UnlockIcon融会贯通", "Images/UnlockIcons/融会贯通"),
            new("UnlockIcon凌波微步", "Images/UnlockIcons/凌波微步"),
            new("UnlockIcon倾国倾城", "Images/UnlockIcons/倾国倾城"),
            new("UnlockIcon无欲则刚", "Images/UnlockIcons/无欲则刚"),
            new("UnlockIcon巍然矗立", "Images/UnlockIcons/巍然矗立"),
            new("UnlockIcon一锤定音", "Images/UnlockIcons/一锤定音"),
            new("UnlockIcon腾云驾雾", "Images/UnlockIcons/腾云驾雾"),
            new("UnlockIcon流转达人", "Images/UnlockIcons/流转达人"),
            new("UnlockIcon初窥门径", "Images/UnlockIcons/初窥门径"),
            new("UnlockIcon略有小成", "Images/UnlockIcons/略有小成"),
            new("UnlockIcon渐入佳境", "Images/UnlockIcons/渐入佳境"),
            new("UnlockIcon出神入化", "Images/UnlockIcons/出神入化"),
            
            // PackConstraints
            new("PackConstraints金", "Images/PackConstraintIllustrations/Jin"),
            new("PackConstraints水", "Images/PackConstraintIllustrations/Shui"),
            new("PackConstraints木", "Images/PackConstraintIllustrations/Mu"),
            new("PackConstraints火", "Images/PackConstraintIllustrations/Huo"),
            new("PackConstraints土", "Images/PackConstraintIllustrations/Tu"),
            new("PackConstraints任意", "Images/PackConstraintIllustrations/Any"),
            
            // RunResultIllustrations
            new("RunResultIllustrationWin", "Images/RunResultIllustrations/Win"),
            new("RunResultIllustrationLose", "Images/RunResultIllustrations/Lose"),
            
            // Event
            new("Event缺失插画", "Images/EventIllustrations/缺失插画"),
            new("Event不存在的事件", "Images/EventIllustrations/不存在的事件"),
            new("Event丢尺子", "Images/EventIllustrations/丢尺子"),
            new("Event仙人下棋", "Images/EventIllustrations/仙人下棋"),
            new("Event仙岛玉液酒", "Images/EventIllustrations/仙岛玉液酒"),
            new("Event全等合成", "Images/EventIllustrations/全等合成"),
            new("Event出门", "Images/EventIllustrations/出门"),
            new("Event分子打印机", "Images/EventIllustrations/分子打印机"),
            new("Event境界突破", "Images/EventIllustrations/境界突破"),
            new("Event夏虫语冰", "Images/EventIllustrations/夏虫语冰"),
            new("Event天机阁", "Images/EventIllustrations/天机阁"),
            new("Event天津四", "Images/EventIllustrations/天津四"),
            new("Event天界树", "Images/EventIllustrations/天界树"),
            new("Event守株待兔", "Images/EventIllustrations/守株待兔"),
            new("Event山木", "Images/EventIllustrations/山木"),
            new("Event愿望单", "Images/EventIllustrations/愿望单"),
            new("Event我已膨胀", "Images/EventIllustrations/我已膨胀"),
            new("Event曹操三笑", "Images/EventIllustrations/曹操三笑"),
            new("Event检测仪", "Images/EventIllustrations/检测仪"),
            new("Event神灯精灵", "Images/EventIllustrations/神灯精灵"),
            new("Event解梦师", "Images/EventIllustrations/解梦师"),
            new("Event论无穷", "Images/EventIllustrations/论无穷"),
            new("Event连抽五张", "Images/EventIllustrations/连抽五张"),
            new("Event重新尝试教学", "Images/EventIllustrations/重新尝试教学"),
            new("Event鸡肉面", "Images/EventIllustrations/鸡肉面"),
            // 目前以下两个没有用到
            // 全等合成
            // 重新尝试教学
        });
    }

    public SpriteEntry MissingSkillIllustration() => this["Skill缺失插画"];
    public SpriteEntry ErrorBuffIcon() => this["Buff不存在"];
    public SpriteEntry MissingBuffIcon() => this["Buff缺失插画"];
    public SpriteEntry MissingCharacterPortrait() => this["CharacterPortrait缺失立绘"];
    public SpriteEntry MissingEventIllustration() => this["Event缺失插画"];
}
