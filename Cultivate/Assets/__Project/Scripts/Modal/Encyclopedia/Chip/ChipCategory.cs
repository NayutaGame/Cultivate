using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using UnityEngine;

public class ChipCategory : Category<ChipEntry>
{
    public ChipCategory()
    {
        List = new List<ChipEntry>()
        {
            /**********************************************************************************************************/
            /*********************************************** 百花缭乱 **************************************************/
            /**********************************************************************************************************/

            // new XinfaEntry("龙象吞海决", "水系心法"),
            // new XinfaEntry("魔焰决", "火系心法"),
            // new XinfaEntry("明王决", "金系心法"),
            // new XinfaEntry("玄清天衍录", "通用心法"),
            // new XinfaEntry("大帝轮华经", "通用心法"),
            // new XinfaEntry("自在极意式", "通用心法"),
            // new XinfaEntry("逍遥游", "通用心法"),
            // new NeigongEntry("水雾决", "内功"),
            // new NeigongEntry("冰心决", "内功"),
            // new NeigongEntry("飞云劲", "内功"),
            // new NeigongEntry("春草决", "内功"),
            // new WaigongEntry("真金印", "提升吸收到金系灵气的概率"),
            // new WaigongEntry("生水印", "提升吸收到水系灵气的概率"),
            // new WaigongEntry("回春印", "提升吸收到木系灵气的概率"),
            // new WaigongEntry("聚火印", "提升吸收到火系灵气的概率"),
            // new WaigongEntry("玄土印", "提升吸收到土系灵气的概率"),
            // new WaigongEntry("紫微印", "吸收一点水系灵气，【生】额外再吸收一点水系灵气"),
            // new WaigongEntry("善水印", "吸收一点木系灵气，【生】额外再吸收一点木系灵气"),
            // new WaigongEntry("上清印", "吸收一点火系灵气，【生】额外再吸收一点火系灵气"),
            // new WaigongEntry("火铃印", "吸收一点土系灵气，【生】额外再吸收一点土系灵气"),
            // new WaigongEntry("三山印", "吸收一点金系灵气，【生】额外再吸收一点金系灵气"),
            // new WaigongEntry("丹阳印", "获得【焰】*2"),
            // new WaigongEntry("灵光印", "在本回合下一次受到伤害时，获得3点金系灵气"),
            // new WaigongEntry("炙火印", "在本回合下一次受到技能伤害时，使对手获得【灼烧】*2"),
            // new WaigongEntry("璇水印", "在本回合下一次受到技能伤害时，恢复3点生命值。"),
            // new WaigongEntry("罡水印", "下回合造成的水系技能伤害+2"),
            // new WaigongEntry("春丝印", "下一次造成的木系技能伤害+5"),
            // new WaigongEntry("灵体印", "获得【护罩】*6；本回合释放金系技能时，将额外消耗一点金系灵气，并获得【护罩】*1"),
            // new WaigongEntry("分金印", "本场战斗中每次触发五行【连击】后，敌方获得【易伤】*1。若上一次释放的是土系技能，则立即触发此效果。"),
            // new WaigongEntry("驱寒印", "本场战斗中每吸收一点灵气，移除自身1层【霜冻】。只能使用一次。"),
            // new WaigongEntry("业火印", "本场战斗中每次触发五行【连击】后，敌方获得【灼烧】*1。若上一次释放的是木系技能，则立即触发此效果。"),
            // new WaigongEntry("灵藤印", "本场战斗中每次触发五行【连击】后，敌方获得【缠绕】*1。若上一次释放的是水系技能，则立即触发此效果。"),
            // new WaigongEntry("怒水印", "本场战斗中每次触发五行【连击】后，获得【疗】*1。若上一次释放的是金系技能，则立即触发此效果。"),
            // new WaigongEntry("回风印", "直到下回合开始前，每次受到伤害后获得【蓄力】*1"),
            // new WaigongEntry("神皇印", "若使用相同的灵气释放，则下回合开始时，自身【蓄势】层数翻倍。否则，本回合【护罩】抵挡的伤害等量转化为【蓄势】。"),
            // new WaigongEntry("覆体印", "消散所有金系灵气，每消散一点获得【减伤】*1"),

            // new WaigongEntry("钢刀落", "12攻"),
            // new WaigongEntry("金钟罩", "护甲+6"),
            // new WaigongEntry("寒冰术", "灵气+1，敌方护甲-3"),
            // new WaigongEntry("再生术", "再生+2"),
            // new WaigongEntry("硬化术", "格挡+1", 1),
            // new WaigongEntry("吸血镖", "吸血  4攻"),
            // new WaigongEntry("火吐息", "3攻，灵气+1"),
            // new WaigongEntry("迷踪步", "闪避+1", 1),
            // new WaigongEntry("蓄力术", "灵气+2"),
            // new WaigongEntry("土龙击", "3攻，护甲+3",
            //     execute: (seq, caster) =>
            //     {
            //         StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 3);
            //         StageManager.Instance.ArmorProcedure(seq, caster, caster, 3);
            //     }),


            // new WaiGongEntry("打击", JingJie.LianQi, "5攻",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 5);
            //     }),
            // new WaiGongEntry("铁甲", JingJie.LianQi, "护甲+3",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.ArmorProcedure(seq, caster, caster, 3);
            //     }),
            // new WaiGongEntry("铁傀儡威仪", JingJie.LianQi, "护甲+5，2攻",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.ArmorProcedure(seq, caster, caster, 5);
            //         StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 2);
            //     }),
            // new WaiGongEntry("铁甲桩", JingJie.LianQi, "护甲+4，【免伤】1次",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.ArmorProcedure(seq, caster, caster, 4);
            //         StageManager.Instance.BuffProcedure(seq, caster, caster, "免攻");
            //     }),
            // new WaiGongEntry("突围", JingJie.LianQi, "消耗所有护甲，【无敌】1回合",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.ArmorProcedure(seq, caster, caster, -caster.Armor);
            //         StageManager.Instance.BuffProcedure(seq, caster, caster, "无敌");
            //     }),
            // new WaiGongEntry("铁甲入体", JingJie.LianQi, "【格挡】+2",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.BuffProcedure(seq, caster, caster, "格挡", 2);
            //     }),
            // new WaiGongEntry("金傀儡", JingJie.LianQi, "护甲+10，下回合无法攻击",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.ArmorProcedure(seq, caster, caster, 10);
            //         StageManager.Instance.BuffProcedure(seq, caster, caster, "无法攻击", 2);
            //     }),
            // new WaiGongEntry("活体铠甲", JingJie.LianQi, "护甲+2，【无敌】1回合",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.ArmorProcedure(seq, caster, caster, 2);
            //         StageManager.Instance.BuffProcedure(seq, caster, caster, "无敌");
            //     }),
            // new WaiGongEntry("金元素爆发", JingJie.LianQi, "消耗所有护甲，造成1.5倍伤害",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         int armor = caster.Armor;
            //         StageManager.Instance.ArmorProcedure(seq, caster, caster, -armor);
            //         StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), Mathf.FloorToInt(1.5f * armor));
            //     }),
            // new WaiGongEntry("铁甲冲撞", JingJie.LianQi, "造成护甲一半的伤害，敌方【晕眩】1回合",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         int armor = caster.Armor;
            //         StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), Mathf.FloorToInt(0.5f * armor));
            //         StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "晕眩");
            //     }),
            // new WaiGongEntry("不动", JingJie.LianQi, "消耗，获得【不动】",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         waiGong.Consumed = true;
            //         StageManager.Instance.BuffProcedure(seq, caster, caster, "不动");
            //     }),
            // new WaiGongEntry("明", JingJie.LianQi, "消耗，获得【明】",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //     }),
            // new WaiGongEntry("觉有情", JingJie.LianQi, "消耗，如果有【不动】和【明】，则获得【不动明王】。否则，减少10生命",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //     }),
            //
            // new WaiGongEntry("跳回合", JingJie.LianQi, "敌方跳过下一回合",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "跳回合");
            //     }),
            //
            // new WaiGongEntry("跳卡牌", JingJie.LianQi, "敌方跳过下一卡牌",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "跳卡牌");
            //     }),
            //
            // new WaiGongEntry("二动", JingJie.LianQi, "再次行动",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         caster.Swift = true;
            //         StageManager.Instance.ArmorProcedure(seq, caster, caster, 1);
            //     }),
            //
            // new WaiGongEntry("双发", JingJie.LianQi, "下一张牌使用两次",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.BuffProcedure(seq, caster, caster, "双发");
            //     }),
            //
            // new WaiGongEntry("蓝太多", JingJie.LianQi, "消耗两点灵气", 2,
            //     execute: (seq, caster, waiGong) =>
            //     {
            //     }),
            //
            // new WaiGongEntry("吸血", JingJie.LianQi, "4攻，吸血", 0,
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 4, damaged:
            //             d => StageManager.Instance.HealProcedure(seq, caster, caster, d.Value));
            //     }),
            //
            // new WaiGongEntry("守剑式", JingJie.LianQi, "护甲+4，【受伤反击】+4",
            //     execute: (seq, caster, waiGong) =>
            //     {
            //         StageManager.Instance.ArmorProcedure(seq, caster, caster, 4);
            //         StageManager.Instance.BuffProcedure(seq, caster, caster, "受伤反击", 4);
            //     }),
            //

            new WaiGongEntry("聚气术", JingJie.LianQi, "灵气+1",
                execute: (seq, caster, waiGong, recursive) =>
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气")),

            new WuXingChipEntry("金", "周围金+1", WuXing.Jin),
            new WuXingChipEntry("水", "周围水+1", WuXing.Shui),
            new WuXingChipEntry("木", "周围木+1", WuXing.Mu),
            new WuXingChipEntry("火", "周围火+1", WuXing.Huo),
            new WuXingChipEntry("土", "周围土+1", WuXing.Tu),

            new WaiGongEntry("火剑", JingJie.LianQi, "5, 有【火】时攻+4",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), waiGong.GetPower(WuXing.Huo) >= 1 ? 9 : 4);
                }),

            new ChipEntry("拆除", JingJie.LianQi, "拆除",
                canPlug: (tile, runChip) => tile.AcquiredRunChip != null && tile.AcquiredRunChip.Chip._entry.CanUnplug(tile.AcquiredRunChip),
                plug: (tile, runChip) => tile.AcquiredRunChip.Chip._entry.Unplug(tile.AcquiredRunChip),
                canUnplug: acquiredRunChip => false,
                unplug: acquiredRunChip => { }),

            // new XueWeiEntry("穴位1", "穴位1", 0),
            // new XueWeiEntry("穴位2", "穴位2", 1),
            // new XueWeiEntry("穴位3", "穴位3", 2),
            // new XueWeiEntry("穴位4", "穴位4", 3),
            // new XueWeiEntry("穴位5", "穴位5", 4),
            // new XueWeiEntry("穴位6", "穴位6", 5),
            // new XueWeiEntry("穴位7", "穴位7", 6),
            // new XueWeiEntry("穴位8", "穴位8", 7),
            // new XueWeiEntry("穴位9", "穴位9", 8),
            // new XueWeiEntry("穴位10", "穴位10", 9),
            // new XueWeiEntry("穴位11", "穴位11", 10),
            // new XueWeiEntry("穴位12", "穴位12", 11),

            new WaiGongEntry("自动使用", JingJie.LianQi, "自动消耗掉自己", 0,
                startStage: (caster, waiGong) =>
                    waiGong.Consumed = true),

            new WaiGongEntry("给自己点可被驱散", JingJie.LianQi, "", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "可被驱散", 2);
                }),

            new WaiGongEntry("驱散过程", JingJie.LianQi, "", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.DispelProcedure(seq, caster, 1, true);
                    StageManager.Instance.DispelProcedure(seq, caster, 1, false);
                }),

            new WaiGongEntry("给自己治疗同时加护甲Buff", JingJie.LianQi, "", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "治疗同时加护甲", 2);
                }),

            new WaiGongEntry("治疗自己", JingJie.LianQi, "", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.HealProcedure(seq, caster, caster, 5);
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster, 1);
                }),

            new WaiGongEntry("给自己临金Buff", JingJie.LianQi, "", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "临金");
                }),

            new WaiGongEntry("金剑", JingJie.LianQi, "造成(临金)伤害", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), waiGong.GetPower(WuXing.Jin));
                }),

            new WaiGongEntry("给自己下回合开始三动Buff", JingJie.LianQi, "", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "下回合开始三动");
                }),

            new WaiGongEntry("给自己加点护甲", JingJie.LianQi, "", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster, 1);
                }),

            new WaiGongEntry("根据已损失护甲造成伤害", JingJie.LianQi, "", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), caster.LostArmorRecord);
                }),

            new WaiGongEntry("先给自己掉点血", JingJie.LianQi, "", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster, 1);
                }),

            new WaiGongEntry("再把血加回来", JingJie.LianQi, "", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.HealProcedure(seq, caster, caster, 1);
                }),

            new WaiGongEntry("根据已回复生命造成伤害", JingJie.LianQi, "", 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), caster.HealedRecord);
                }),

            /**********************************************************************************************************/
            /*********************************************** 大剑哥 ****************************************************/
            /**********************************************************************************************************/

            /**********************************************************************************************************/
            /*********************************************** 石丸 ******************************************************/
            /**********************************************************************************************************/

            /**********************************************************************************************************/
            /*********************************************** Summer68 *************************************************/
            /**********************************************************************************************************/

            //金
            new WaiGongEntry("蓄力", JingJie.LianQi, "获得蓄力buff，下次攻击时攻击力乘二，受到伤害后移除。",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "蓄力", 1);
                }),

            new WaiGongEntry("挥砍", JingJie.LianQi, "8攻击,每有1【金】+2攻",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 8+2*waiGong.GetPower(WuXing.Jin) );
                }),

            new WaiGongEntry("强袭", JingJie.ZhuJi, "13攻击,3反伤,每有1【金】+3攻,+1反伤",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 13+3*waiGong.GetPower(WuXing.Jin) );
                    StageManager.Instance.AttackProcedure(seq, caster, caster, 3+1*waiGong.GetPower(WuXing.Jin) );
                }),
            new WaiGongEntry("挥斧", JingJie.ZhuJi, "8攻击，8防御,每有1【金】+2攻,+2防御",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 8+2*waiGong.GetPower(WuXing.Jin) );
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster, 8+2*waiGong.GetPower(WuXing.Jin));

                }),

            new WaiGongEntry("磨刀", JingJie.ZhuJi, "获得1层磨刀buff（所有牌的【金】相邻数+1）",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "磨刀", 1);

                }),//磨刀buff的具体实现
            new WaiGongEntry("堕天", JingJie.JinDan, "20攻击，下俩回合无法攻击,每有1【金】+3攻",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 20+3*waiGong.GetPower(WuXing.Jin) );
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "疲劳", 3);

                }),

            new WaiGongEntry("邪灵剑", JingJie.JinDan, "获得时永久减少10命元，获得全局buff嗜血邪剑（当敌人生命值低于10+2*【金】时直接斩杀））",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "邪灵剑", 10+2*waiGong.GetPower(WuXing.Jin));

                }),//全局buff待实现
            new WaiGongEntry("丛云剑", JingJie.JinDan, "获得时获得全局buff护主灵刃（下次遭遇致死伤害时仍会保留1血，触发后移除将丛云剑替换为破碎剑刃）",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "丛云剑", 1);

                }),//全局buff待实现
            new WaiGongEntry("破碎剑刃", JingJie.JinDan, "每相邻1【金】，造成一次5点攻击伤害",
                manaCost: (level, powers) => 3,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    for(int i=0;i<waiGong.GetPower(WuXing.Jin);i++)
                    {
                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 5 );
                    }

                }),
            new WaiGongEntry("封神剑", JingJie.YuanYing, "对方20%最大生命值攻击，给敌人施加2层疲劳",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(),(int)((StageManager.Instance._enemy.Hp-1)*0.2+1) );
                    StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "疲劳", 2);

                }),
            new WaiGongEntry("金刚之躯", JingJie.YuanYing, "消耗，获得buff，攻击额外提高3点攻击，防御额外提高3点防御。每有1【金】相邻，额外提高1点攻击，1点防御",
                manaCost: (level, powers) => 2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "金刚之躯", 3+waiGong.GetPower(WuXing.Jin) );

                }),

            //木
            new WaiGongEntry("叶刃", JingJie.LianQi, "4攻击俩次，每有1【木】攻击+2",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 4+2*waiGong.GetPower(WuXing.Mu) );
                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 4+2*waiGong.GetPower(WuXing.Mu) );

                }),

            new WaiGongEntry("藤甲", JingJie.LianQi, "10防御,每有1【木】+3防御",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster, 10+3*waiGong.GetPower(WuXing.Mu));

                }),
            new WaiGongEntry("万物有灵", JingJie.ZhuJi, "每有1【木】+3防御，+3治疗，+1灵力",
                execute: (seq, caster, waiGong, recursive) =>
                {       StageManager.Instance.HealProcedure(seq, caster, caster, 3*waiGong.GetPower(WuXing.Mu));
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "治疗计数器",3+waiGong.GetPower(WuXing.Mu));
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster, 3*waiGong.GetPower(WuXing.Mu));

                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",waiGong.GetPower(WuXing.Mu));
                }),
            new WaiGongEntry("见血封喉", JingJie.ZhuJi, "给对手施加3中毒，每有1【木】额外施加1中毒",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {


                        StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "中毒",3+waiGong.GetPower(WuXing.Mu));
                }),
            new WaiGongEntry("生生不息", JingJie.ZhuJi, "施加生生不息，每回合结束后治疗3，每有1【木】额外治疗1",
                manaCost: (level, powers) => 2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "生生不息",3+waiGong.GetPower(WuXing.Mu));
                }),
            new WaiGongEntry("蜜糖砒霜", JingJie.ZhuJi, "buff,每次自身治疗是给对方施加1中毒，相邻至少【2】木时施加2中毒",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.BuffProcedure(seq, caster, caster, "蜜糖砒霜",waiGong.GetPower(WuXing.Huo) >= 2 ? 2 : 1);

                }),
            new WaiGongEntry("疯狂生长", JingJie.JinDan, "自身所有有益buff翻倍",
                manaCost: (level, powers) => 4,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    for(int i = 0;i<caster.Buffs.Count;i++)
                    {
                        if(caster.Buffs[i].BuffEntry.Friendly)
                        {
                            caster.Buffs[i].Stack*=2;
                        }

                    }
                }),

            new WaiGongEntry("移花接木", JingJie.JinDan, "获得1移花接木buff,若相邻至少3【木】则额外获得一层移花接木",
                manaCost: (level, powers) => 2,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.BuffProcedure(seq, caster, caster, "移花接木",waiGong.GetPower(WuXing.Mu) >= 3 ? 2 : 1);

                }),

            new WaiGongEntry("寄生", JingJie.YuanYing, "debuff,每次对手获得灵力时偷取1点灵力，1+【木】生命值",
                manaCost: (level, powers) => 2,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "寄生",1+waiGong.GetPower(WuXing.Mu));

                }),

            new WaiGongEntry("森罗万象", JingJie.YuanYing, "消耗，随机造成0-你已恢复的生命值总数之间的伤害",
                manaCost: (level, powers) => 3,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        waiGong.Consumed = true;
                        Buff same=caster.FindBuff("治疗计数器");
                        int atk=0;
                        if(same!=null){
                            atk=RandomManager.Range(0,same.Stack);
                            StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), atk);
                        }



                }),

            //水
            new WaiGongEntry("水刃", JingJie.LianQi, "2灵力，相邻【水】则+5攻",

                execute: (seq, caster, waiGong, recursive) =>
                {

                        if(waiGong.GetPower(WuXing.Shui)>0)
                            StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(),5);

                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",2);

                }),

            new WaiGongEntry("水盾", JingJie.LianQi, "2灵力，相邻【水】则+6防御",

                execute: (seq, caster, waiGong, recursive) =>
                {

                        if(waiGong.GetPower(WuXing.Shui)>0)
                            StageManager.Instance.ArmorGainProcedure(seq, caster, caster,6);

                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",2);

                }),

            new WaiGongEntry("净化", JingJie.ZhuJi, "净化自身,相邻至少2【水】则不消耗灵力",
                manaCost: (level, powers) => powers[WuXing.Shui] >= 2 ? 0 : 2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        caster.Buffs.RemoveAll(bf=>{
                            if(!bf.BuffEntry.Friendly)
                                return true;
                            else
                                return false;
                        });

                }),

            new WaiGongEntry("灵气暴涨", JingJie.ZhuJi, "使你的灵力翻倍,相邻至少2【水】则不消耗灵力",
                manaCost: (level, powers) => powers[WuXing.Shui] >= 2 ? 0 : 2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        Buff mana=caster.FindBuff("灵气");
                        int lq=0;
                        if(mana!= null)
                        {
                            lq=mana.Stack;
                        }
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",lq);

                }),

            new WaiGongEntry("水之守护", JingJie.ZhuJi, "下次受到攻击时获得当时灵力值的防御，相邻至少2【水】则不消耗灵力",
                manaCost: (level, powers) => powers[WuXing.Shui] >= 2 ? 0 : 2,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.BuffProcedure(seq, caster, caster, "水之守护",1);

                }),

            new WaiGongEntry("水之抉择", JingJie.JinDan, "相信水的抉择,相邻至少2【水】则不消耗灵力",
                manaCost: (level, powers) => powers[WuXing.Shui] >= 2 ? 0 : 2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        Buff mana=caster.FindBuff("灵气");
                        int lq=0;
                        double hp=caster.Hp;
                        double mhp=caster.MaxHp;
                        double HpPc=hp/mhp;
                        if(mana!= null)
                        {
                            lq=mana.Stack;
                        }
                        if(caster.Opponent().Hp+caster.Opponent().Armor<=lq)
                        {
                            StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(),lq);
                        }
                        else if((HpPc)>=0.7||caster.Opponent().Armor>0){
                            StageManager.Instance.BuffProcedure(seq, caster, caster, "力量",2);
                        }
                        else if((HpPc)<=0.3){
                            StageManager.Instance.HealProcedure(seq, caster, caster,lq );
                            StageManager.Instance.BuffProcedure(seq, caster, caster, "治疗计数器",lq);
                        }
                        else {
                            StageManager.Instance.ArmorGainProcedure(seq, caster, caster,lq);
                        }

                }),

            new WaiGongEntry("美酒", JingJie.JinDan, "给对手施加混乱3回合,相邻至少2【水】则不消耗灵力",
                manaCost: (level, powers) => powers[WuXing.Shui] >= 2 ? 0 : 2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "混乱",3);

                }),

            new WaiGongEntry("海啸", JingJie.YuanYing, "每消耗一点灵力，造成一次1攻击。相邻至少3【水】则不消耗灵力",
                manaCost: (level, powers) => powers[WuXing.Shui] >= 3 ? 0 : 3,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        Buff mana=caster.FindBuff("灵气");
                        int lq=0;
                        if(mana!= null)
                        {
                            lq=mana.Stack;
                        }
                        int num=lq < 9 ? lq :9;
                        for(int i=0;i<num;i++)
                        {
                            StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(),1);
                        }
                }),

            new WaiGongEntry("滴水石穿", JingJie.JinDan, "每回合开始获得1灵力",
                manaCost: (level, powers) => 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "浩瀚无穷",2);

                }),

            //火
            new WaiGongEntry("火花", JingJie.LianQi, "造成4点不可防御伤害，每有1【火】伤害+2",
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.DamageProcedure(seq, caster, caster.Opponent(), 4+2*waiGong.GetPower(WuXing.Huo), false);

                }),

            new WaiGongEntry("香火", JingJie.LianQi, "献祭5，获得1+【火】灵力，5不可防御伤害",
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.DamageProcedure(seq, caster, caster, 5, false);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "献祭计数器",5);
                        StageManager.Instance.DamageProcedure(seq, caster, caster.Opponent(), 5, false);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",1+waiGong.GetPower(WuXing.Huo));
                }),

            new WaiGongEntry("融化", JingJie.ZhuJi, "移除对方护甲，再造成等量不可防御伤害",
                manaCost: (level, powers) => 2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        int d=caster.Opponent().Armor;
                        StageManager.Instance.ArmorLoseProcedure(seq, caster.Opponent(), d);
                        StageManager.Instance.DamageProcedure(seq, caster, caster.Opponent(), d, false);
                }),
            new WaiGongEntry("熊熊烈火", JingJie.ZhuJi, "献祭10，15+2*【火】不可防御伤害",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.DamageProcedure(seq, caster, caster, 10, false);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "献祭计数器",10);
                        StageManager.Instance.DamageProcedure(seq, caster, caster.Opponent(), 15+2*waiGong.GetPower(WuXing.Huo), false);

                }),
            new WaiGongEntry("燃烧殆尽", JingJie.JinDan, "献祭10,造成你已损失生命值的不可防御伤害",
                manaCost: (level, powers) => 2,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.DamageProcedure(seq, caster, caster, 10, false);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "献祭计数器",10);
                        int d=caster.MaxHp-caster.Hp;
                        StageManager.Instance.DamageProcedure(seq, caster, caster.Opponent(), d, false);

                }),
            new WaiGongEntry("灵魂燃烧", JingJie.JinDan, "献祭15，获得15灵力，你无法再获得灵力",
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.DamageProcedure(seq, caster, caster, 15, false);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "献祭计数器",15);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",15);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵魂燃烧",1);
                }),
            new WaiGongEntry("浴火凤凰", JingJie.YuanYing, "死亡后复活，生命值重置为献祭值，移除所有buff",
                manaCost: (level, powers) => 2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        waiGong.Consumed = true;
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "浴火凤凰",1);
                }),

            new WaiGongEntry("侵略如火", JingJie.YuanYing, "你每回合获得3动，你会在3回合后死亡",
                manaCost: (level, powers) => 3,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        waiGong.Consumed = true;
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "侵略如火",3);
                }),

            //土
            new WaiGongEntry("地动术", JingJie.LianQi, "3+2*【土】防御,再次行动",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        caster.Swift = true;
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,3+2*waiGong.GetPower(WuXing.Tu));
                }),
            new WaiGongEntry("大地灵气", JingJie.LianQi, "5防御，1灵气,相邻【土】则+2灵气",
                manaCost: (level, powers) => 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,5);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",waiGong.GetPower(WuXing.Tu)>0?2:1);
                }),

            new WaiGongEntry("幸运女神", JingJie.ZhuJi, "投掷俩枚骰子，获得骰子之和的攻击和防御值，如果点数相同，则额外+点数点灵力",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        int r1=RandomManager.Range(0,6);
                        int r2=RandomManager.Range(0,6);
                        Debug.Log(r1);
                        if(r1==r2)
                        {
                            if(r1==5){
                                StageManager.Instance.DamageProcedure(seq, caster, caster.Opponent(), 666, false);
                                Debug.Log("666666！");
                            }

                            StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",r1);
                        }
                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(),r1+r2);
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,r1+r2);

                }),
            new WaiGongEntry("地震", JingJie.ZhuJi, "造成对手10+【土】%最大生命值的攻击",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        Buff ddlz=caster.FindBuff("大地领主");
                        int d=(int)((StageManager.Instance._enemy.MaxHp-1)*(0.1+0.01*waiGong.GetPower(WuXing.Tu))+1);
                        if(ddlz!=null)
                        {
                            d*=2;
                        }
                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), d);
                }),
            new WaiGongEntry("撕裂大地", JingJie.JinDan, "你每获得30护甲，使用一次地震。",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        waiGong.Consumed = true;
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "撕裂大地",1);
                }),
            new WaiGongEntry("大地领主", JingJie.JinDan, "地震造成双倍伤害",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        waiGong.Consumed = true;
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "大地领主",1);
                }),

            new WaiGongEntry("坚不可摧", JingJie.JinDan, "20+4*【土】防御",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,20+4*waiGong.GetPower(WuXing.Tu));
                }),

            new WaiGongEntry("重峦叠嶂", JingJie.YuanYing, "每回合结束后获得5+【土】防御",
                manaCost: (level, powers) => 3,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        waiGong.Consumed = true;
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "重峦叠嶂",5+waiGong.GetPower(WuXing.Tu));
                }),

            /**********************************************************************************************************/
            /*********************************************** End ******************************************************/
            /**********************************************************************************************************/

        };
    }
}
