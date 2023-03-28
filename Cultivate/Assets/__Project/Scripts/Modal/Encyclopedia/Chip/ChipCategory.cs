using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using System.Text;
using Unity.VisualScripting;

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


            new WaiGongEntry("聚气术", JingJie.LianQi, "灵气+1",
                execute: (caster, waiGong, recursive) =>
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气")),

            // new WuXingChipEntry("金", "周围金+1", WuXing.Jin),
            // new WuXingChipEntry("水", "周围水+1", WuXing.Shui),
            // new WuXingChipEntry("木", "周围木+1", WuXing.Mu),
            // new WuXingChipEntry("火", "周围火+1", WuXing.Huo),
            // new WuXingChipEntry("土", "周围土+1", WuXing.Tu),

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

            new WaiGongEntry("金00", JingJie.LianQi, "12攻", 2, WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 12);
                }),

            new WaiGongEntry("金01", JingJie.LianQi, "护甲+6",
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 6);
                }),

            new WaiGongEntry("水00", JingJie.LianQi, "灵气+1，施加3减甲", type: WaiGongEntry.WaiGongType.NONATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气");
                    StageManager.Instance.ArmorLoseProcedure(caster.Opponent(), 3);
                }),

            new TargetSelfBuffWaiGongEntry("水01", JingJie.LianQi, "再生+2", 0, WaiGongEntry.WaiGongType.NONATTACK, "再生", 2),

            new TargetSelfBuffWaiGongEntry("木00", JingJie.LianQi, "格挡+1", 1, WaiGongEntry.WaiGongType.NONATTACK, "格挡"),

            new WaiGongEntry("木01", JingJie.LianQi, "4攻 吸血", type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 4, lifeSteal: true);
                }),

            new WaiGongEntry("火00", JingJie.LianQi, "灵气+1，3攻", type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气");
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 3);
                }),

            new WaiGongEntry("火01", JingJie.LianQi, "消耗4生命，闪避+1",
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.DamageProcedure(caster, caster, 4);
                    StageManager.Instance.BuffProcedure(caster, caster, "闪避");
                }),

            new TargetSelfBuffWaiGongEntry("土00", JingJie.LianQi, "灵气+2", 0, WaiGongEntry.WaiGongType.NONATTACK, "灵气", 2),

            new WaiGongEntry("土01", JingJie.LianQi, "3攻，护甲+3", type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 3);
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 3);
                }),

            new WaiGongEntry("金10", JingJie.ZhuJi, "6攻，击伤:护甲+6", 1, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 6,
                        damaged: d => StageManager.Instance.ArmorGainProcedure(caster, caster, 6));
                }),

            new TargetSelfBuffWaiGongEntry("金11", JingJie.ZhuJi, "消耗，自动护甲+1", 0, WaiGongEntry.WaiGongType.NONATTACK, "自动护甲", 1),

            new WaiGongEntry("水10", JingJie.ZhuJi, "护甲+5，施加5减甲",
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 5);
                    StageManager.Instance.ArmorLoseProcedure(caster.Opponent(), 5);
                }),

            new WaiGongEntry("水11", JingJie.ZhuJi, "灵气+2，再生+2，治疗【再生的层数】",
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 2);
                    StageManager.Instance.BuffProcedure(caster, caster, "再生", 2);
                    StageManager.Instance.HealProcedure(caster, caster, caster.GetStackOfBuff("再生"));
                }),

            // new WaiGongEntry("木10", JingJie.ZhuJi, "每有3点再生消耗1点，格挡+【被消耗层数】", 1,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int consumed = caster.GetStackOfBuff("再生") / 3;
            //         caster.TryConsumeBuff("再生", consumed);
            //         StageManager.Instance.BuffProcedure(caster, caster, "格挡", consumed);
            //     }),

            new WaiGongEntry("木11", JingJie.ZhuJi, "8攻 击伤：格挡+1", 2, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 8,
                        damaged: d => StageManager.Instance.BuffProcedure(caster, caster, "格挡"));
                }),

            new WaiGongEntry("火10", JingJie.ZhuJi, "闪避+1，满血：闪避+1",
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "闪避");
                    if(caster.Hp == caster.MaxHp)
                        StageManager.Instance.BuffProcedure(caster, caster, "闪避");
                }),

            new WaiGongEntry("火11", JingJie.ZhuJi, "消耗6生命，力量+1", type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.DamageProcedure(caster, caster, 4);
                    StageManager.Instance.BuffProcedure(caster, caster, "力量");
                }),

            new WaiGongEntry("火12", JingJie.ZhuJi, "灵气+2，净化2",
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 2);
                    StageManager.Instance.DispelProcedure(caster, 2, false);
                }),

            new WaiGongEntry("土10", JingJie.ZhuJi, "2攻x3", 2, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 2, 3);
                }),

            new WaiGongEntry("土11", JingJie.ZhuJi, "2攻x2，不屈+1", type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 2, 2);
                    StageManager.Instance.BuffProcedure(caster, caster, "不屈");
                }),

            new WaiGongEntry("金20", JingJie.JinDan, "18攻 每有1点不屈多4攻", 1, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 18 + caster.GetSumOfStackOfBuffs("不屈", "激活的不屈"));
                }),

            new WaiGongEntry("金21", JingJie.JinDan, "灵气+3，【灵气Stack】护甲",
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气");
                    StageManager.Instance.ArmorGainProcedure(caster, caster, caster.GetStackOfBuff("灵气"));
                }),

            new WaiGongEntry("金22", JingJie.JinDan, "10攻 击伤：对方减少1灵气", type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 10,
                        damaged: d => d.Tgt.TryConsumeMana());
                }),

            new WaiGongEntry("水20", JingJie.JinDan, "消耗，灵气+6",
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 6);
                }),

            new WaiGongEntry("水21", JingJie.JinDan, "消耗，本场战斗中，造成伤害：施加【造成伤害，最多3】减甲", 4,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "铃鹿御前", 3);
                }),

            new WaiGongEntry("水22", JingJie.JinDan, "施加4内伤", 1,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "内伤", 4);
                }),

            new WaiGongEntry("木20", JingJie.JinDan, "12攻 如果可以消耗1格挡，吸血", 2, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 12,
                        lifeSteal: caster.TryConsumeBuff("格挡"));
                }),

            new WaiGongEntry("木21", JingJie.JinDan, "消耗，攻击一直具有吸血，直到使用一张非攻击牌",
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "木21");
                }),

            new WaiGongEntry("木22", JingJie.JinDan, "6攻，二动，初次使用：遭受1跳卡牌", type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 6);
                    caster.Swift = true;
                    if (waiGong.StageUsedTimes == 0)
                        StageManager.Instance.BuffProcedure(caster, caster, "跳卡牌");
                }),

            new WaiGongEntry("火20", JingJie.JinDan, "灵气+2，每有8点格挡，力量+1",
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 2);
                    int stack = caster.GetStackOfBuff("格挡") / 8;
                    StageManager.Instance.BuffProcedure(caster, caster, "力量", stack);
                }),

            new WaiGongEntry("火21", JingJie.JinDan, "14攻，下一次攻击具有穿透", type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 14);
                    StageManager.Instance.BuffProcedure(caster, caster, "穿透");
                }),

            new WaiGongEntry("火22", JingJie.JinDan, "消耗，本场战斗中，每次受到不少于10点伤害的时候，力量+1", 0, WaiGongEntry.WaiGongType.NONATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "火22");
                }),

            new WaiGongEntry("土20", JingJie.JinDan, "消耗，如果卡牌中没有攻击牌，本场战斗中，Step开始：3攻",
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    bool noAttack = caster._waiGongList.FirstObj(wg => wg.GetWaiGongType() == WaiGongEntry.WaiGongType.ATTACK) == null;
                    if (noAttack)
                        StageManager.Instance.BuffProcedure(caster, caster, "土20");
                }),

            new WaiGongEntry("土21", JingJie.JinDan, "消耗，生命上限变为一半，不屈+4",
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    caster.MaxHp /= 2;
                    StageManager.Instance.BuffProcedure(caster, caster, "不屈", 4);
                }),

            new WaiGongEntry("土22", JingJie.JinDan, "消耗2灵气，50攻，遭受2跳回合", 2, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 50);
                    StageManager.Instance.BuffProcedure(caster, caster, "跳回合", 2);
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
