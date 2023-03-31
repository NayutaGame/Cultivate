using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            new ChipEntry("拆除", JingJie.LianQi, "拆除", null,
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

            new WaiGongEntry("金00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, p) => $"{12 + 3 * j}攻"),
                WuXing.Jin, 2, WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 12 + 3 * waiGong.GetJingJie());
                }),

            new WaiGongEntry("金01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, p) => $"护甲+{6 + 2 * j}"), WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 6 + 2 * waiGong.GetJingJie());
                }),

            new WaiGongEntry("水00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, p) => $"灵气+{1 + j / 2}\n施加{3 + j}减甲"), WuXing.Shui, type: WaiGongEntry.WaiGongType.NONATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 1 + waiGong.GetJingJie() / 2);
                    StageManager.Instance.ArmorLoseProcedure(caster.Opponent(), 3 + waiGong.GetJingJie());
                }),

            new WaiGongEntry("水01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, p) => $"再生+{2 + j}"), WuXing.Shui, type: WaiGongEntry.WaiGongType.NONATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "再生", 2 + waiGong.GetJingJie());
                }),

            new WaiGongEntry("木00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, p) => $"格挡+{1 + j / 2}"), WuXing.Mu, 1, type: WaiGongEntry.WaiGongType.NONATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "格挡", 1 + waiGong.GetJingJie() / 2);
                }),

            new WaiGongEntry("木01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, p) => $"{4 + j}攻 吸血"), WuXing.Mu, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 4 + waiGong.GetJingJie(), lifeSteal: true);
                }),

            new WaiGongEntry("火00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, p) => $"灵气+{1 + j / 2}\n{3 + j}攻"), WuXing.Huo, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 1 + waiGong.GetJingJie() / 2);
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 3 + waiGong.GetJingJie());
                }),

            new WaiGongEntry("火01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, p) => $"消耗{4 - j / 2}生命\n闪避+1"), WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.DamageProcedure(caster, caster, 4 - waiGong.GetJingJie() / 2);
                    StageManager.Instance.BuffProcedure(caster, caster, "闪避");
                }),

            new WaiGongEntry("土00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, p) => $"灵气+{2 + j}"), WuXing.Tu, type: WaiGongEntry.WaiGongType.NONATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 2 + waiGong.GetJingJie());
                }),

            new WaiGongEntry("土01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, p) => $"{3 + j}攻\n护甲+{3 + j}"), WuXing.Tu, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 3 + waiGong.GetJingJie());
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 3 + waiGong.GetJingJie());
                }),

            new WaiGongEntry("金10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, p) => $"{4 + 2 * j}攻 击伤:护甲+{4 + 2 * j}"), WuXing.Jin, 1, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 4 + 2 * waiGong.GetJingJie(),
                        damaged: d => StageManager.Instance.ArmorGainProcedure(caster, caster, 4 + 2 * waiGong.GetJingJie()));
                }),

            new WaiGongEntry("金11", new CLLibrary.Range(1, 5), new ChipDescription((l, j, p) => $"消耗\n自动护甲+{(int)j}"), WuXing.Jin, type: WaiGongEntry.WaiGongType.NONATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "自动护甲", waiGong.GetJingJie());
                }),

            new WaiGongEntry("水10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, p) => $"护甲+{4 + j}\n施加{4 + j}减甲"), WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 4 + waiGong.GetJingJie());
                    StageManager.Instance.ArmorLoseProcedure(caster.Opponent(), 4 + waiGong.GetJingJie());
                }),

            new WaiGongEntry("水11", new CLLibrary.Range(1, 5), new ChipDescription((l, j, p) => $"灵气+{2 + j / 2}\n再生+{2 + j / 2}\n治疗【再生的层数】"), WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 2 + waiGong.GetJingJie() / 2);
                    StageManager.Instance.BuffProcedure(caster, caster, "再生", 2 + waiGong.GetJingJie() / 2);
                    StageManager.Instance.HealProcedure(caster, caster, caster.GetStackOfBuff("再生"));
                }),

            // new WaiGongEntry("木10", JingJie.ZhuJi, "每有3点再生消耗1点，格挡+【被消耗层数】", WuXing.Mu, 1,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int consumed = caster.GetStackOfBuff("再生") / 3;
            //         caster.TryConsumeBuff("再生", consumed);
            //         StageManager.Instance.BuffProcedure(caster, caster, "格挡", consumed);
            //     }),

            new WaiGongEntry("木11", new CLLibrary.Range(1, 5), new ChipDescription((l, j, p) => $"{6 + 2 * j}攻 击伤：格挡+{1 + j / 2}"), WuXing.Mu, 2, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 6 + 2 * waiGong.GetJingJie(),
                        damaged: d => StageManager.Instance.BuffProcedure(caster, caster, "格挡", 1 + waiGong.GetJingJie() / 2));
                }),

            new WaiGongEntry("火10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, p) => $"闪避+1\n满血：闪避+{1 + j / 2}"), WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "闪避");
                    if(caster.Hp == caster.MaxHp)
                        StageManager.Instance.BuffProcedure(caster, caster, "闪避", 1 + waiGong.GetJingJie() / 2);
                }),

            new WaiGongEntry("火11", new CLLibrary.Range(1, 5), new ChipDescription((l, j, p) => $"消耗6生命\n力量+{(1 + j) / 2}"), WuXing.Huo, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.DamageProcedure(caster, caster, 6);
                    StageManager.Instance.BuffProcedure(caster, caster, "力量", (1 + waiGong.GetJingJie()) / 2);
                }),

            new WaiGongEntry("火12", new CLLibrary.Range(1, 5), new ChipDescription((l, j, p) => $"灵气+{1 + j}\n净化{1 + j}"), WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 1 + waiGong.GetJingJie());
                    StageManager.Instance.DispelProcedure(caster, 1 + waiGong.GetJingJie(), false);
                }),

            new WaiGongEntry("土10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, p) => $"{2 + j / 2}攻x{2 + (1 + j) / 2}"), WuXing.Tu, 2, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 2 + waiGong.GetJingJie() / 2, 2 + (1 + waiGong.GetJingJie()) / 2);
                }),

            new WaiGongEntry("土11", new CLLibrary.Range(1, 5), new ChipDescription((l, j, p) => $"{1 + j}攻x2\n不屈+{(int)j}"), WuXing.Tu, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 1 + waiGong.GetJingJie(), 2);
                    StageManager.Instance.BuffProcedure(caster, caster, "不屈", waiGong.GetJingJie());
                }),

            new WaiGongEntry("金20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"{14 + 2 * j}攻 每有1点不屈多{2 + j}攻"), WuXing.Jin, 1, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(),
                        14 + 2 * waiGong.GetJingJie() + (2 + waiGong.GetJingJie()) * caster.GetSumOfStackOfBuffs("不屈", "激活的不屈"));
                }),

            new WaiGongEntry("金21", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"灵气+{1 + j}\n【灵气Stack】护甲"), WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 1 + waiGong.GetJingJie());
                    StageManager.Instance.ArmorGainProcedure(caster, caster, caster.GetStackOfBuff("灵气"));
                }),

            new WaiGongEntry("金22", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"{6 + 2 * j}攻 击伤：对方减少{j / 2}灵气"), WuXing.Jin, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 6 + 2 * waiGong.GetJingJie(),
                        damaged: d => d.Tgt.TryConsumeMana(waiGong.GetJingJie() / 2));
                }),

            new WaiGongEntry("水20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"消耗\n灵气+{3 * j}"), WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 3 * waiGong.GetJingJie());
                }),

            new WaiGongEntry("水21", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"消耗\n本场战斗中，造成伤害：施加【造成伤害，最多{2 + (1 + j) / 2}】减甲"), WuXing.Shui, 4,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "铃鹿御前", 2 + (1 + waiGong.GetJingJie()) / 2);
                }),

            new WaiGongEntry("水22", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"施加{2 + j}内伤"), WuXing.Shui, 1,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "内伤", 2 + waiGong.GetJingJie());
                }),

            new WaiGongEntry("木20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"{8 + 2 * j}攻 如果可以消耗1格挡，吸血"), WuXing.Mu, 2, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 8 + 2 * waiGong.GetJingJie(),
                        lifeSteal: caster.TryConsumeBuff("格挡"));
                }),

            new WaiGongEntry("木21", new CLLibrary.Range(2, 5), "消耗\n攻击一直具有吸血，直到使用一张非攻击牌", WuXing.Mu, manaCost: new ManaCost((l, j, p) => 5 - j),
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "木21");
                }),

            new WaiGongEntry("木22", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"{3 * j}攻\n二动\n初次使用：遭受1跳卡牌"), WuXing.Mu, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 3 * waiGong.GetJingJie());
                    caster.Swift = true;
                    if (waiGong.StageUsedTimes == 0)
                        StageManager.Instance.BuffProcedure(caster, caster, "跳卡牌");
                }),

            new WaiGongEntry("火20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"灵气+2\n每有{10 - j}点格挡，力量+1"), WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 10 - waiGong.GetJingJie());
                    int stack = caster.GetStackOfBuff("格挡") / 8;
                    StageManager.Instance.BuffProcedure(caster, caster, "力量", stack);
                }),

            new WaiGongEntry("火21", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"{9 + 3 * j}攻\n下一次攻击具有穿透"), WuXing.Huo, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 9 + 3 * waiGong.GetJingJie());
                    StageManager.Instance.BuffProcedure(caster, caster, "穿透");
                }),

            new WaiGongEntry("火22", new CLLibrary.Range(2, 5), "消耗\n本场战斗中，每次受到不少于10点伤害的时候，力量+1", WuXing.Huo, manaCost: new ManaCost((l, j, p) => 5 - j), WaiGongEntry.WaiGongType.NONATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "火22");
                }),

            new WaiGongEntry("土20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"消耗\n如果卡牌中没有攻击牌，本场战斗中，Step开始：{2 * j - 1}攻"), WuXing.Tu,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    bool noAttack = caster._waiGongList.FirstObj(wg => wg.GetWaiGongType() == WaiGongEntry.WaiGongType.ATTACK) == null;
                    if (noAttack)
                        StageManager.Instance.BuffProcedure(caster, caster, "天衣无缝", 2 * waiGong.GetJingJie() - 1);
                }),

            new WaiGongEntry("土21", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"消耗\n生命上限变为一半\n不屈+{3 + (j + 1) / 2}"), WuXing.Tu,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    caster.MaxHp /= 2;
                    StageManager.Instance.BuffProcedure(caster, caster, "不屈", 3 + (waiGong.GetJingJie() + 1) / 2);
                }),

            new WaiGongEntry("土22", new CLLibrary.Range(2, 5), new ChipDescription((l, j, p) => $"{30 + 10 * j}攻\n遭受2跳回合"), WuXing.Tu, 2, type: WaiGongEntry.WaiGongType.ATTACK,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 30 + 10 * waiGong.GetJingJie());
                    StageManager.Instance.BuffProcedure(caster, caster, "跳回合", 2);
                }),

            new WaiGongEntry("金通透世界", 5, "消耗，本场战斗中，自己的所有攻击具有穿透", WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "通透世界");
                }),

            new WaiGongEntry("水通透世界", 5, "消耗，本场战斗中，自己的所有攻击具有穿透", WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "通透世界");
                }),

            new WaiGongEntry("木通透世界", 5, "消耗，本场战斗中，自己的所有攻击具有穿透", WuXing.Mu,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "通透世界");
                }),

            new WaiGongEntry("火通透世界", 5, "消耗，本场战斗中，自己的所有攻击具有穿透", WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "通透世界");
                }),

            new WaiGongEntry("土通透世界", 5, "消耗，本场战斗中，自己的所有攻击具有穿透", WuXing.Tu,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "通透世界");
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
