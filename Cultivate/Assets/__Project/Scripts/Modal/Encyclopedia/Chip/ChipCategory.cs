using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using System.Text;

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

            /*new WaiGongEntry("聚气术", JingJie.LianQi, "灵气+1",
                execute: (seq, caster, waiGong, recursive) =>
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气")),*/

            new WaiGongEntry("聚气术", JingJie.LianQi, "空过",
            execute: (seq, caster, waiGong, recursive) =>
                {
                    // Buff mana=caster.FindBuff("灵气");
                    // mana.Stack-=1;

                    // 写成下面这样
                    caster.TryConsumeMana();
                }),
            /*
            new WuXingChipEntry("金", "周围金+1", WuXing.Jin),
            new WuXingChipEntry("水", "周围水+1", WuXing.Shui),
            new WuXingChipEntry("木", "周围木+1", WuXing.Mu),
            new WuXingChipEntry("火", "周围火+1", WuXing.Huo),
            new WuXingChipEntry("土", "周围土+1", WuXing.Tu),
            */
            /*new WaiGongEntry("火剑", JingJie.LianQi, "5, 有【火】时攻+4",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), waiGong.GetPower(WuXing.Huo) >= 1 ? 9 : 4);
                }),*/

            new ChipEntry("拆除", JingJie.LianQi, "拆除",
                canPlug: (tile, runChip) => tile.AcquiredRunChip != null && tile.AcquiredRunChip.Chip._entry.CanUnplug(tile.AcquiredRunChip),
                plug: (tile, runChip) => tile.AcquiredRunChip.Chip._entry.Unplug(tile.AcquiredRunChip),
                canUnplug: acquiredRunChip => false,
                unplug: acquiredRunChip => { }),
/*
             new XueWeiEntry("穴位1", "穴位1", 0),
             new XueWeiEntry("穴位2", "穴位2", 1),
            new XueWeiEntry("穴位3", "穴位3", 2),
            new XueWeiEntry("穴位4", "穴位4", 3),
            new XueWeiEntry("穴位5", "穴位5", 4),
            new XueWeiEntry("穴位6", "穴位6", 5),
            new XueWeiEntry("穴位7", "穴位7", 6),
            new XueWeiEntry("穴位8", "穴位8", 7),
            new XueWeiEntry("穴位9", "穴位9", 8),
            new XueWeiEntry("穴位10", "穴位10", 9),
            new XueWeiEntry("穴位11", "穴位11", 10),
            new XueWeiEntry("穴位12", "穴位12", 11),*/
            /*
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
            */
            /**********************************************************************************************************/
            /*********************************************** 大剑哥 ****************************************************/
            /**********************************************************************************************************/

            /**********************************************************************************************************/
            /*********************************************** 石丸 ******************************************************/
            /**********************************************************************************************************/

            /**********************************************************************************************************/
            /*********************************************** Summer68 *************************************************/
            /**********************************************************************************************************/

           /* //金
            new WaiGongEntry("蓄力", JingJie.LianQi, "获得蓄力buff，下次攻击时攻击力乘二，受到伤害后移除。",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "蓄力", 1);
                }),

            new WaiGongEntry("挥砍", JingJie.LianQi, "7攻击,每有1【金】+2攻",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 7+2*waiGong.GetPower(WuXing.Jin) );
                }),
            new WaiGongEntry("大力出奇迹", JingJie.LianQi, "8+2*【金】攻击,对手有护甲时为12+3*【金】",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 7+2*waiGong.GetPower(WuXing.Jin) );
                }),
            new WaiGongEntry("硬直", JingJie.ZhuJi, "给对手施加2疲劳",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "疲劳", 2);
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
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "临金", 1);

                }),//磨刀buff的具体实现
            new WaiGongEntry("堕天", JingJie.JinDan, "20攻击，下俩回合无法攻击,每有1【金】+3攻",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 20+3*waiGong.GetPower(WuXing.Jin) );
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "疲劳", 3);

                }),

            new WaiGongEntry("邪灵剑", JingJie.JinDan, "获得时永久减少10命元，获得全局buff嗜血邪剑（当敌人生命值低于10+2*【金】时直接斩杀））",
                startStage: (caster, waiGong) =>
                {
                    waiGong.Consumed = true;
                    //StageManager.Instance.BuffProcedure(seq, caster, caster, "邪灵剑", 10+2*waiGong.GetPower(WuXing.Jin));
                }),


            new WaiGongEntry("丛云剑", JingJie.JinDan, "获得时获得全局buff护主灵刃（下次遭遇致死伤害时仍会保留1血，触发后移除将丛云剑替换为破碎剑刃）",
                startStage: (caster, waiGong) =>
                {
                    waiGong.Consumed = true;
                    //StageManager.Instance.BuffProcedure(seq, caster, caster, "丛云剑", 1);

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
            new WaiGongEntry("封神剑", JingJie.JinDan, "对方20%最大生命值攻击，给敌人施加2层疲劳",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(),(int)((StageManager.Instance._enemy.Hp-1)*0.2+1) );
                    StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "疲劳", 2);

                }),
            new WaiGongEntry("金刚之躯", JingJie.JinDan, "消耗，获得buff，攻击额外提高3点攻击，防御额外提高3点防御。每有1【金】相邻，额外提高1点攻击，1点防御",
                manaCost: (level, powers) => 2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "金刚之躯", 3+waiGong.GetPower(WuXing.Jin) );

                }),

            //木
            new WaiGongEntry("叶刃", JingJie.LianQi, "5攻击俩次，每有1【木】攻击+2",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 4+2*waiGong.GetPower(WuXing.Mu) );
                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 4+2*waiGong.GetPower(WuXing.Mu) );

                }),

            new WaiGongEntry("藤甲", JingJie.LianQi, "8防御,每有1【木】+3防御",
                manaCost: (level, powers) => 0,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster, 9+3*waiGong.GetPower(WuXing.Mu));

                }),
            new WaiGongEntry("疗伤", JingJie.LianQi, "5治疗,每有1【木】+2治疗",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.HealProcedure(seq, caster, caster, 5+2*waiGong.GetPower(WuXing.Mu));
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "治疗计数器",5+2*waiGong.GetPower(WuXing.Mu));

                }),
            new WaiGongEntry("万物有灵", JingJie.ZhuJi, "每有1【木】+3防御，+2治疗，+1灵力",
                execute: (seq, caster, waiGong, recursive) =>
                {       StageManager.Instance.HealProcedure(seq, caster, caster, 2*waiGong.GetPower(WuXing.Mu));
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "治疗计数器",2*waiGong.GetPower(WuXing.Mu));
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

            new WaiGongEntry("寄生", JingJie.JinDan, "debuff,每次对手获得灵力时偷取1点灵力，1+【木】生命值",
                manaCost: (level, powers) => 2,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "寄生",1+waiGong.GetPower(WuXing.Mu));

                }),

            new WaiGongEntry("森罗万象", JingJie.JinDan, "消耗，随机造成0-你已恢复的生命值总数之间的伤害",
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
            new WaiGongEntry("水刃", JingJie.LianQi, "2灵力，相邻【水】则+4攻",

                execute: (seq, caster, waiGong, recursive) =>
                {

                        if(waiGong.GetPower(WuXing.Shui)>0)
                            StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(),4);

                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",2);

                }),

            new WaiGongEntry("水盾", JingJie.LianQi, "2灵力，相邻【水】则+5防御",

                execute: (seq, caster, waiGong, recursive) =>
                {

                        if(waiGong.GetPower(WuXing.Shui)>0)
                            StageManager.Instance.ArmorGainProcedure(seq, caster, caster,5);

                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",2);

                }),

            new WaiGongEntry("激流", JingJie.LianQi, "造成12+4*【水】伤害",2,

                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(),12+4*waiGong.GetPower(WuXing.Shui));

                }),

            new WaiGongEntry("净化", JingJie.ZhuJi, "净化自身,每相邻1【水】+1灵力",
                manaCost: (level, powers) =>  0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        caster.Buffs.RemoveAll(bf=>{
                            if(!bf.BuffEntry.Friendly)
                                return true;
                            else
                                return false;
                        });
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",waiGong.GetPower(WuXing.Shui));

                }),

            new WaiGongEntry("灵气暴涨", JingJie.ZhuJi, "使你的灵力翻倍,获得10+2*【水】防御",
                manaCost: (level, powers) => 1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        Buff mana=caster.FindBuff("灵气");
                        int lq=0;
                        if(mana!= null)
                        {
                            lq=mana.Stack;
                        }
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",lq);
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,10+3*waiGong.GetPower(WuXing.Shui));

                }),

            new WaiGongEntry("水之守护", JingJie.ZhuJi, "下次受到攻击时获得当时灵力值*2的防御.",
                manaCost: (level, powers) =>  1,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.BuffProcedure(seq, caster, caster, "水之守护",1);

                }),

            new WaiGongEntry("水之抉择", JingJie.ZhuJi, "相信水的抉择",
                manaCost: (level, powers) =>  0,
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
                            StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",4);
                        }
                        else if((HpPc)<=0.3){
                            StageManager.Instance.HealProcedure(seq, caster, caster,lq );
                            StageManager.Instance.BuffProcedure(seq, caster, caster, "治疗计数器",lq);
                        }
                        else {
                            StageManager.Instance.ArmorGainProcedure(seq, caster, caster,lq);
                        }

                }),

            new WaiGongEntry("美酒", JingJie.ZhuJi, "给对手施加混乱3回合,相邻【水】则不消耗灵力",
                manaCost: (level, powers) => powers[WuXing.Shui] >= 1 ? 0 : 1,
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
            new WaiGongEntry("火花", JingJie.LianQi, "造成4点不可防御伤害，每有1【火】伤害+1",
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.DamageProcedure(seq, caster, caster.Opponent(), 4+waiGong.GetPower(WuXing.Huo), false);

                }),

            new WaiGongEntry("香火", JingJie.LianQi, "献祭5，获得1灵力，8+2*【火】不可防御伤害",
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.DamageProcedure(seq, caster, caster, 5, false);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "献祭计数器",5);
                        StageManager.Instance.DamageProcedure(seq, caster, caster.Opponent(), 8+2*waiGong.GetPower(WuXing.Huo), false);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",1);
                }),
            new WaiGongEntry("烈火冲", JingJie.LianQi, "献祭5，9+2*【火】不可防御伤害",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        caster.Swift = true;
                        StageManager.Instance.DamageProcedure(seq, caster, caster, 5, false);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "献祭计数器",5);
                        StageManager.Instance.DamageProcedure(seq, caster, caster.Opponent(), 7+2*waiGong.GetPower(WuXing.Huo), false);
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
            new WaiGongEntry("燃烧殆尽", JingJie.JinDan, "献祭10,造成你已损失生命值的攻击",
                manaCost: (level, powers) => 2,
                execute: (seq, caster, waiGong, recursive) =>
                {

                        StageManager.Instance.DamageProcedure(seq, caster, caster, 10, false);
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "献祭计数器",10);
                        int d=caster.MaxHp-caster.Hp;
                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), d);

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
            new WaiGongEntry("大地灵气", JingJie.LianQi, "5+【土】防御，5+【土】攻击,",
                manaCost: (level, powers) => 0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,5+waiGong.GetPower(WuXing.Tu));
                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(),5+waiGong.GetPower(WuXing.Tu));
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

            //怪物卡牌
            */



           new WaiGongEntry("放第一", JingJie.FanXu, "",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    if (waiGong.StageUsedTimes == 0)
                    {
                        StageWaiGong waiGongPos7 = StageManager.Instance._hero._waiGongList[6];
                        waiGongPos7.Consumed = true;

                        // 消耗性效果
                    }
                    else
                    {
                        StageWaiGong waiGongPos7 = StageManager.Instance._hero._waiGongList[6];
                        waiGongPos7.Execute(seq, caster);
                    }
                }),

           new WaiGongEntry("放第七", JingJie.FanXu, "",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    // "一般效果"
                }),

           new WaiGongEntry("访问其他卡", JingJie.FanXu, "",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    if (!recursive)
                        return;

                    // 当前卡的序号
                    int currPos = waiGong.SlotIndex;

                    // 访问第0张
                    StageWaiGong waiGongPos0 = StageManager.Instance._hero._waiGongList[0];

                    waiGong.Next().Execute(seq, caster, false);
                }),

           new WaiGongEntry("基础加灵力", JingJie.FanXu, "每回合开始获得1灵力",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        waiGong.Consumed = true;
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "基础加灵力",1);

                }),

            //基础攻击类
            new WaiGongEntry("佯攻", JingJie.FanXu, "4攻",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 4);
                }),

            new WaiGongEntry("轻击", JingJie.FanXu, "7攻", 1 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 7);
                }),

            new WaiGongEntry("重击", JingJie.FanXu, "11攻", 2 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 11);
                }),

            new WaiGongEntry("超重击", JingJie.FanXu, "16攻", 3 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 16);
                }),

            //基础防御
            new WaiGongEntry("单手防御", JingJie.FanXu, "5防御",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,5);
                }),

            new WaiGongEntry("双手防御", JingJie.FanXu, "10防御",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,10);
                }),

            new WaiGongEntry("全力防御", JingJie.FanXu, "15防御",2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,15);
                }),


            //基础强化类
            new WaiGongEntry("聚气", JingJie.FanXu, "本回合额外获得1灵力",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",1);

                }),




            //游玩过程中可以获得的卡，相较于基础卡更强力
            //攻击类
            new WaiGongEntry("醉拳", JingJie.LianQi, "随机1~13攻，",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    int d=RandomManager.Range(1,13);
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), d);
                }),
            new WaiGongEntry("手里剑", JingJie.LianQi, "3攻*3", 1 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 3);
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 3);
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 3);
                }),

            new WaiGongEntry("迅捷之刃", JingJie.LianQi, "9攻，携带此牌后你可以多带一张牌", 2 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 9);

                }),

            new WaiGongEntry("穷途末路攻", JingJie.LianQi, "如果你上回合用完了你的灵力，那么12攻",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    int d=caster.GetMana();
                    Buff add=caster.FindBuff("基础加灵力");
                    int d1=0;
                    if(add!=null)
                        d1=add.Stack;
                    if(d==d1)
                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 12);

                }),

            new WaiGongEntry("铁锤", JingJie.LianQi, "8攻，如果对手有护甲，追加6攻", 1 ,
                execute: (seq, caster, waiGong, recursive) =>
                {

                    int d=caster.Opponent().Armor;
                    if(d>0)
                    {
                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 8);
                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 6);
                    }
                    else
                        StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 8);
                }),

            new WaiGongEntry("苦痛之刃", JingJie.LianQi, "23攻，自身失去3点生命值", 2 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.DamageProcedure(seq, caster, caster, 2, false);
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 19);

                }),

            new WaiGongEntry("泰山压顶", JingJie.LianQi, "9攻，对手本回合无法攻击。", 3 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 21);
                    StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "疲劳",1);
                }),
            new WaiGongEntry("凌迟", JingJie.LianQi, "1攻，本轮战斗内每次打出使伤害翻倍",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "凌迟",1);
                    Buff n=caster.FindBuff("凌迟");
                    int d =1;
                    for(int i=1;i<n.Stack;i++)
                        d*=2;
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), d);

                }),
            new WaiGongEntry("灵气之剑", JingJie.LianQi, "造成你当前灵力值*6的伤害", 1 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    int d=caster.GetMana();
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), d*6);

                }),


            new WaiGongEntry("冲锋", JingJie.LianQi, "13攻，只能放在卡组第一张", 1 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 13);

                }),

            new WaiGongEntry("泰坦", JingJie.LianQi, "15攻，15防御", 3,
                execute: (seq, caster, waiGong, recursive) =>
                {

                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 15);
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster,15);

                }),

            new WaiGongEntry("诅咒之刃", JingJie.LianQi, "携带后,每回合开始自动对敌方造成4点伤害", 0,
                startStage: (caster, waiGong) =>
                {
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "诅咒之刃",1);
                }
                ),
            new WaiGongEntry("虚弱之刃", JingJie.LianQi, "7攻，给对手2虚弱", 1 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 5);
                    StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "虚弱", 2);
                }),

            new WaiGongEntry("痛击", JingJie.LianQi, "5攻，给对手2易伤", 1 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 5);
                    StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "易伤", 2);
                }),

            new WaiGongEntry("通灵剑", JingJie.LianQi, "8攻，恢复1灵力，提升1灵力上限。", 1 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 9);
                    //提升灵力上限
                }),
            new WaiGongEntry("终结", JingJie.LianQi, "5攻*6，只能放在最后一张", 3,
                execute: (seq, caster, waiGong, recursive) =>
                {

                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 5);
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 5);
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 5);
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 5);
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 5);
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 5);
                }),

            new WaiGongEntry("灵力体系大招", JingJie.LianQi, "只能放在最后一张，会重复随机打出你的卡牌，且最小灵力花费为1，直到灵力不够为止。", 1 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    //待实现
                }),
            //防御类
            new WaiGongEntry("预知危险", JingJie.JinDan, "下次被攻击时提供8点格挡",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "下次被攻击减伤8",1);

                }),
            new WaiGongEntry("持盾", JingJie.JinDan, "获得7防御，下回合再获得7防御",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster,7);
                    Buff mana=caster.FindBuff("下回合获得五防御");
                    if(mana!= null)
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "下回合获得五防御",1);
                    else
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "下回合获得五防御",2);

                }),

            new WaiGongEntry("穷途末路守", JingJie.LianQi, "如果你上回合用完了你的灵力，那么14防御",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    int d=caster.GetMana();
                    Buff add=caster.FindBuff("基础加灵力");
                    int d1=0;
                    if(add!=null)
                        d1=add.Stack;
                    if(d==d1)
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster, 14);

                }),
            new WaiGongEntry("迅捷之盾", JingJie.LianQi, "13防，携带此牌后你可以多带一张牌", 2 ,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster, 13);

                }),

            new WaiGongEntry("应急装置", JingJie.LianQi, "15防御，下回合减少1灵力",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster.Opponent(), 15);
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "下回合减一灵力",2);

                }),

            new WaiGongEntry("通灵盾", JingJie.LianQi, "13防御，获得1点灵力上限",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster.Opponent(), 13);
                    //获得1灵力上限

                }),

            new WaiGongEntry("后撤步", JingJie.LianQi, "11防御，如果本回合没受到伤害，那么回合结束后恢复1灵力",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster.Opponent(), 11);
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "没受伤回一灵力",2);

                }),
            new WaiGongEntry("加强版持盾", JingJie.JinDan, "获得15防御，下回合再获得15防御",2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster,15);
                    Buff mana=caster.FindBuff("下回合获得十二防御");
                    if(mana!= null)
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "下回合获得十二防御",1);
                    else
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "下回合获得十二防御",2);
                }),

            new WaiGongEntry("武装力量", JingJie.LianQi, "9防御，获得2力量",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster, 9);
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "力量",2);
                    //获得1灵力上限

                }),
            new WaiGongEntry("武装敏捷", JingJie.LianQi, "9防御，获得2敏捷",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster, 9);
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "敏捷",2);
                    //获得1灵力上限

                }),
            new WaiGongEntry("诅咒之盾", JingJie.LianQi, "携带后,每回合开始自动获得5护甲", 0,
                startStage: (caster, waiGong) =>
                {
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "诅咒之盾",5);
                }
                ),

            new WaiGongEntry("卖个破绽", JingJie.LianQi, "6防御，本回合受到攻击后给对方施加2易伤",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster.Opponent(), 6);
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "卖个破绽",1);
                    //获得1灵力上限

                }),
            //强化投资类
            new WaiGongEntry("灵气收集器", JingJie.JinDan, "携带后每回合开始额外获得1灵力，但在开局时获得2回合虚弱,2回合易伤",0,
                startStage: (caster, waiGong) =>
                {
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "基础加灵力",1);
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "虚弱",3);
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "易伤",3);
                }),

            new WaiGongEntry("增强力量核心", JingJie.JinDan, "额外获得3力量增强",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "力量",3);

                }),

            new WaiGongEntry("增强防御核心", JingJie.JinDan, "额外获得3防御增强",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "敏捷",3);

                }),


            new WaiGongEntry("穷途末路加灵气", JingJie.JinDan, "如果你上回合用完了你的灵力，那么加2灵力",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    int d=caster.GetMana();
                    Buff add=caster.FindBuff("基础加灵力");
                    int d1=0;
                    if(add!=null)
                        d1=add.Stack;
                    if(d==d1)
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气",2);

                }),

            new WaiGongEntry("蓄力", JingJie.JinDan, "下次攻击造成2倍伤害，受到伤害后移除",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.BuffProcedure(seq, caster, caster, "蓄力",1);

                }),

            new WaiGongEntry("我已成神", JingJie.JinDan, "携带后在回合开始时不再能获得灵力，获得3灵力，获得5力量，5敏捷",0,
                startStage: (caster, waiGong) =>
                {

                    Buff Smana=caster.FindBuff("基础加灵力");
                    Smana.Stack=0;
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "敏捷",5);
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "力量",5);
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "灵气",3);
                }),

            new WaiGongEntry("收刀", JingJie.LianQi, "携带后你开局额外获得2灵力，2力量，只能放在最后一张", 0 ,
                startStage: (caster, waiGong) =>
                {
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "灵气",2);
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "力量",2);
                }
                ),
            new WaiGongEntry("吸血鬼", JingJie.LianQi, "携带此卡开局获得吸血（恢复伤害一半的血量），但无法再获得护甲", 0 ,
                startStage: (caster, waiGong) =>
                {
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "吸血鬼",1);
                }
                ),

            new WaiGongEntry("任务：过量伤害", JingJie.JinDan, "携带后把怪物的血量打到低于-18即可移除此卡并获得任务奖励，抽三卡",0),

            new WaiGongEntry("任务：完美格挡", JingJie.JinDan, "携带后在一轮内受到5次以上攻击且不受到伤害，则移除此卡并获得武装力量和武装敏捷",0),


            //怪物专用卡牌


            new WaiGongEntry("怪兽一介绍", JingJie.FanXu, "你好我是你的新手教官，请你把基础加灵力放在第一格，把我的血量设置为16。每人三张卡",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "基础加灵力",1);
                }),

            new WaiGongEntry("怪兽一攻击", JingJie.FanXu, "我会对你造成6攻击，请注意防御",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 6);
                }),

            new WaiGongEntry("怪兽一防御", JingJie.FanXu, "5防御，我会给自己加上5点防御，在下回合你攻击我的时候减少我受到的伤害，到我的下个回合开始防御会消失",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,5);
                }),

            new WaiGongEntry("怪兽一休息", JingJie.FanXu, "有点累了，你快点把我打死吧",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,5);
                }),

            new WaiGongEntry("怪兽二奖励", JingJie.FanXu, "在化神期抽3张卡，并选一张加入到你的卡组中。",0,
                execute: (seq, caster, waiGong, recursive) =>
                {

                }),
            new WaiGongEntry("怪兽二介绍", JingJie.FanXu, "你每个回合都会获得1灵力，灵力花费更高的卡往往也会更强力。这次你可以用所有的基础卡和我战斗，希望你能承受住我的重♂击.血量26",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "基础加灵力",1);
                }),

            new WaiGongEntry("怪兽二防御", JingJie.FanXu, "5防御",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                        StageManager.Instance.ArmorGainProcedure(seq, caster, caster,5);
                }),

            new WaiGongEntry("怪兽二重击", JingJie.FanXu, "13攻击!!!，尝尝我的厉♂害！！",3,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 13);
                }),

            new WaiGongEntry("怪兽二奖励", JingJie.FanXu, "在化神期抽3张卡，并选一张加入到你的卡组中。",0,
                execute: (seq, caster, waiGong, recursive) =>
                {

                }),

            new WaiGongEntry("怪兽三介绍", JingJie.FanXu, "难缠的对手，每一轮他都会变得更强，请注意速战速决,血量为35",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "基础加灵力",1);
                }),
            new WaiGongEntry("怪兽三攻击强化", JingJie.FanXu, "3攻击",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 3);
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "力量",1);
                }),
            new WaiGongEntry("怪兽三强化", JingJie.FanXu, "加2力量",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "力量",2);
                }),
            new WaiGongEntry("怪兽三奖励", JingJie.FanXu, "在化神期抽3张卡，并选一张加入到你的卡组中。并且你升级了！！！之后你可以每轮打4张牌",0,
                execute: (seq, caster, waiGong, recursive) =>
                {

                }),

            new WaiGongEntry("怪兽四介绍", JingJie.FanXu, "会给你施加攻击力减半的debuff。血量33",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "基础加灵力",1);
                }),
            new WaiGongEntry("怪兽四攻击", JingJie.FanXu, "8攻击",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 8);
                }),
            new WaiGongEntry("怪兽四重击", JingJie.FanXu, "14攻击",2,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 14);
                }),
            new WaiGongEntry("虚弱", JingJie.FanXu, "施加虚弱二回合，攻击力减半",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "虚弱", 2);
                }),

             new WaiGongEntry("怪兽四奖励", JingJie.FanXu, "在化神期抽3张卡，并选一张加入到你的卡组中。",0,
                execute: (seq, caster, waiGong, recursive) =>
                {

                }),

            new WaiGongEntry("怪兽五（精英）", JingJie.FanXu, "开局会免疫前三次攻击，血量55。战胜后抽一卡，升级。",0,
                startStage: (caster, waiGong) =>
                {
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "免疫攻击",3);
                },
                execute: (seq, caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "基础加灵力",1);
                }),

            new WaiGongEntry("怪兽五防御强化", JingJie.FanXu, "防御9，获得2力量",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(seq, caster, caster.Opponent(), 9);
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "力量",2);
                }),

            new WaiGongEntry("易伤", JingJie.FanXu, "给玩家施加2易伤",1,
                execute: (seq, caster, waiGong, recursive) =>
                {

                    StageManager.Instance.BuffProcedure(seq, caster, caster.Opponent(), "易伤",2);
                }),
            new WaiGongEntry("怪兽五多段攻击", JingJie.FanXu, "4攻击*3",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 4);
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 4);
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 4);
                }),
            new WaiGongEntry("怪兽五攻击", JingJie.FanXu, "12攻击",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 12);
                }),

            new WaiGongEntry("怪兽六介绍", JingJie.FanXu, "在血量低于15点以下时会恢复所有生命值，48生命值",0,
                startStage: (caster, waiGong) =>
                {
                    StageManager.Instance.BuffProcedure(new StringBuilder(), caster, caster, "低于十五回满",1);
                },
                execute: (seq, caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "基础加灵力",1);
                }),

            new WaiGongEntry("怪兽六攻击", JingJie.FanXu, "11攻击",1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 11);
                }),

            new WaiGongEntry("怪兽七介绍", JingJie.FanXu, "皮糙肉厚的石巨人，血量120！！！力大无穷！！！",0,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "基础加灵力",1);
                }),

            new WaiGongEntry("怪兽七攻击", JingJie.FanXu, "35攻击",3,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 35);
                }),

            new WaiGongEntry("自我再生", JingJie.FanXu, "20治疗",3,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.HealProcedure(seq, caster, caster, 20);
                }),
            /**********************************************************************************************************/
            /*********************************************** End ******************************************************/
            /**********************************************************************************************************/

        };
    }
}
