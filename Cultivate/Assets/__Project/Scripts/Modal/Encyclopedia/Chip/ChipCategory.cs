using System.Collections;
using System.Collections.Generic;
using CLLibrary;
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

            new WaiGongEntry("查询接口", JingJie.LianQi, "",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    int anyStack = caster.GetStackOfBuff("灵气");
                    int mana = caster.GetMana();
                }),

            new WaiGongEntry("聚气术", JingJie.LianQi, "灵气+1",
                execute: (seq, caster, waiGong, recursive) =>
                    StageManager.Instance.BuffProcedure(seq, caster, caster, "灵气")),

            new WuXingChipEntry("金", "周围金+1", WuXing.Jin),
            new WuXingChipEntry("水", "周围水+1", WuXing.Shui),
            new WuXingChipEntry("木", "周围木+1", WuXing.Mu),
            new WuXingChipEntry("火", "周围火+1", WuXing.Huo),
            new WuXingChipEntry("土", "周围土+1", WuXing.Tu),

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
            new XueWeiEntry("穴位12", "穴位12", 11),

            new ChipEntry("拆除", JingJie.LianQi, "拆除",
                canPlug: (tile, runChip) => tile.AcquiredRunChip != null && tile.AcquiredRunChip.CanUnplug(),
                plug: (tile, runChip) => tile.AcquiredRunChip.Unplug(),
                canUnplug: acquiredRunChip => false,
                unplug: acquiredRunChip => { }),




            new WaiGongEntry("水剑", JingJie.LianQi, "3攻, 有【水】时无需灵气",
                manaCost: (level, powers) => powers[WuXing.Shui] >= 1 ? 0 : 1,
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), 7);
                }),
            new WaiGongEntry("火剑", JingJie.LianQi, "5, 有【火】时攻+4",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(seq, caster, caster.Opponent(), waiGong.GetPower(WuXing.Huo) >= 1 ? 9 : 4);
                }),
            new WaiGongEntry("再来一次", JingJie.LianQi, "再使用一次上一张牌",
                execute: (seq, caster, waiGong, recursive) =>
                {
                    if (!recursive) return;
                    waiGong.Prev().Execute(seq, caster, false);
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

            /**********************************************************************************************************/
            /*********************************************** End ******************************************************/
            /**********************************************************************************************************/

        };
    }
}
