using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using CLLibrary;
using DG.Tweening;

public class ChipCategory : Category<ChipEntry>
{
    public ChipCategory()
    {
        List = new List<ChipEntry>()
        {
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

            new WaiGongEntry("金00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"{12 + 6 * dj}攻"), WuXing.Jin, new ManaCost((l, j, dj, p) => 2 + dj), WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 12 + 6 * waiGong.Dj);
                }),

            new WaiGongEntry("金01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"护甲+{6 + 2 * dj}"), WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 6 + 2 * waiGong.Dj);
                }),

            new WaiGongEntry("金02", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"{3 + dj}攻\n自己或对方有护甲：多{6 + 2 * dj}攻"), WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    bool anyHasArmor = caster.Armor > 0 || caster.Opponent().Armor > 0;
                    int value = 3 + waiGong.Dj + (anyHasArmor ? (6 + 2 * waiGong.Dj) : 0);
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), value);
                }),

            // new WaiGongEntry("金02", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"灵气+{1 + dj}\n护甲+{3 + dj}"), WuXing.Jin,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         StageManager.Instance.ArmorGainProcedure(caster, caster, 6 + 2 * waiGong.Dj);
            //     }),

            new WaiGongEntry("水00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"灵气+{1 + dj}\n施加{3 + dj}减甲"), WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 1 + waiGong.Dj);
                    StageManager.Instance.ArmorLoseProcedure(caster.Opponent(), 3 + waiGong.Dj);
                }),

            new WaiGongEntry("水01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"灵气+{2 + dj}"), WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 2 + waiGong.Dj);
                }),

            new WaiGongEntry("木00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"格挡+{1 + dj}"), WuXing.Mu, new ManaCost((l, j, dj, p) => 1 + dj),
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "格挡", 1 + waiGong.Dj);
                }),

            new WaiGongEntry("木01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"{4 + 2 * dj}攻 吸血"), WuXing.Mu, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 4 + 2 * waiGong.Dj, lifeSteal: true);
                }),

            new WaiGongEntry("火00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"灵气+{1 + dj}\n{3 + dj}攻"), WuXing.Huo, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 1 + waiGong.Dj);
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 3 + waiGong.Dj);
                }),

            new WaiGongEntry("火02", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"消耗{5 + 5 * dj}生命\n力量+{1 + dj}"), WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.DamageProcedure(caster, caster, 5 + 5 * waiGong.Dj);
                    StageManager.Instance.BuffProcedure(caster, caster, "力量", 1 + waiGong.Dj);
                }),

            new WaiGongEntry("火01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"{6 + 2 * dj}攻 穿透"), WuXing.Huo, manaCost: new ManaCost((l, j, dj, p) => 2),
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 6 + 2 * waiGong.Dj, pierce: true);
                }),

            new WaiGongEntry("土00", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"3攻x{2 + dj}"), WuXing.Tu,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 3, 2 + waiGong.Dj);
                }),

            new WaiGongEntry("土01", new CLLibrary.Range(0, 5), new ChipDescription((l, j, dj, p) => $"{3 + dj}攻\n护甲+{3 + dj}"), WuXing.Tu, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 3 + waiGong.Dj);
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 3 + waiGong.Dj);
                }),

            new WaiGongEntry("金10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"{8 + 2 * dj}攻\n击伤：护甲+{8 + 2 * dj}"), WuXing.Jin, 1, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    int value = 8 + 2 * waiGong.Dj;
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), value,
                        damaged: d => StageManager.Instance.ArmorGainProcedure(caster, caster, value));
                }),

            new WaiGongEntry("金11", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"消耗\n自动护甲+{1 + dj}"), WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "自动护甲", 1 + waiGong.Dj);
                }),

            new WaiGongEntry("起", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"水势+{2 + dj}\n初次：翻倍"), WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    int d = waiGong.IsFirstTime ? 2 : 1;
                    StageManager.Instance.BuffProcedure(caster, caster, "水势", (2 + waiGong.Dj) * d);
                }),

            new WaiGongEntry("水10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"护甲+{5 + dj}\n施加{5 + dj}减甲"), WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 5 + waiGong.Dj);
                    StageManager.Instance.ArmorLoseProcedure(caster.Opponent(), 5 + waiGong.Dj);
                }),

            new WaiGongEntry("水11", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"奇偶：施加{10 + 2 * dj}减甲/护甲+{10 + 2 * dj}\n消耗1水势：翻倍"), WuXing.Shui, manaCost: 1,
                execute: (caster, waiGong, recursive) =>
                {
                    int d = caster.TryConsumeBuff("水势") ? 2 : 1;
                    int value = (10 + 2 * waiGong.Dj) * d;
                    if (waiGong.IsOdd)
                    {
                        StageManager.Instance.ArmorLoseProcedure(caster.Opponent(), value);
                    }
                    else
                    {
                        StageManager.Instance.ArmorGainProcedure(caster, caster, value);
                    }
                }),

            // new WaiGongEntry("竹剑", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"{5 + 2 * dj}攻x2"), WuXing.Shui, manaCost: 1, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 5 + 2 * waiGong.Dj, 2);
            //     }),

            new WaiGongEntry("吐纳", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"灵气+{2 + dj}\n生命上限+{6 + 3 * dj}"), WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 2 + waiGong.Dj);
                    caster.MaxHp += 6 + 3 * waiGong.Dj;
                }),

            new WaiGongEntry("潜龙在渊", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"{8 + 2 * (j - 1)}攻 击伤：+{1 + (j - 1)}格挡"), WuXing.Mu, manaCost: 2, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 8 + 2 * (waiGong.GetJingJie() - 1),
                        damaged: d => StageManager.Instance.BuffProcedure(caster, caster, "格挡", 1 + (waiGong.GetJingJie() - 1)));
                }),

            new WaiGongEntry("木10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"消耗{5 + 5 * dj}生命上限\n灵气+{3 + 2 * dj}"), WuXing.Mu,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.DamageProcedure(caster, caster, 5 + 5 * waiGong.Dj);
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 3 + 2 * waiGong.Dj);
                }),

            new WaiGongEntry("火10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"消耗{8 + 8 * dj}生命\n闪避+{1 + dj}\n初次：闪避+1"), WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.DamageProcedure(caster, caster, 8 + 8 * waiGong.Dj);
                    int firstTime = waiGong.IsFirstTime ? 1 : 0;
                    StageManager.Instance.BuffProcedure(caster, caster, "闪避", 1 + waiGong.Dj + firstTime);
                }),

            new WaiGongEntry("火12", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"力量+1\n{3 + 3 * dj}攻\n消耗1格挡：翻倍"), WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    int d = caster.TryConsumeBuff("格挡") ? 2 : 1;
                    StageManager.Instance.BuffProcedure(caster, caster, "力量", d);
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), (3 + 3 * waiGong.Dj) * d);
                }),

            new WaiGongEntry("土10", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"4攻x{3 + dj}"), WuXing.Tu, 1, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 4, 3 + waiGong.Dj);
                }),

            // new WaiGongEntry("土11", new CLLibrary.Range(1, 5), new ChipDescription((l, j, dj, p) => $"{1 + j}攻x2\n不屈+{(int)j}"), WuXing.Tu, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 1 + waiGong.GetJingJie(), 2);
            //         StageManager.Instance.BuffProcedure(caster, caster, "不屈", waiGong.GetJingJie());
            //     }),

            new WaiGongEntry("金20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{10 + 4 * dj}攻\n每1不屈，多{4 + dj}攻"), WuXing.Jin, 1, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(),
                        10 + 4 * waiGong.Dj + (4 + waiGong.Dj) * caster.GetSumOfStackOfBuffs("不屈", "激活的不屈"));
                }),

            new WaiGongEntry("金21", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+{3 + (j - 2)}\n每1灵气，护甲+1"), WuXing.Jin, 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 3 + (waiGong.GetJingJie() - 2));
                    StageManager.Instance.ArmorGainProcedure(caster, caster, caster.GetMana());
                }),

            new WaiGongEntry("高速剑阵", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+4\n消耗{10 - 5 * (j - 2)}护甲：二动"), WuXing.Jin, 0, type: WaiGongType.JianZhen,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 4);
                    int value = 10 - 5 * (waiGong.GetJingJie() - 2);
                    if (caster.Armor >= value)
                    {
                        StageManager.Instance.ArmorLoseProcedure(caster, value);
                        caster.Swift = true;
                    }
                }),

            new WaiGongEntry("磐石剑阵", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"护甲+{20 + 6 * (j - 2)}\n遭受1跳回合\n架势：无需跳回合"), WuXing.Jin, 0, type: WaiGongType.JianZhen,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 20 + 6 * (waiGong.GetJingJie() - 2));
                    if (!waiGong.JiaShi)
                        StageManager.Instance.BuffProcedure(caster, caster, "跳回合");
                }),

           new WaiGongEntry("利剑", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{10 + 2 * (j - 2)}攻\n击伤：对方减少{1 + (j - 2)}灵气"), WuXing.Jin, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 10 + 2 * (waiGong.GetJingJie() - 2),
                        damaged: d => d.Tgt.TryConsumeMana(1 + (waiGong.GetJingJie() - 2)));
                }),

           new WaiGongEntry("少阳", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗\n自己获得护甲时，额外获得{3 + 2 * (j - 2)}护甲"), WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "少阳");
                }),

           new WaiGongEntry("拔刀", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"下张牌激活架势\n下回合护甲+{10 + 4 * dj}"), WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "延迟护甲", 10 + 4 * waiGong.Dj);
                }),

            new WaiGongEntry("水20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗\n灵气+{3 * j}"), WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 3 * waiGong.GetJingJie());
                }),

            new WaiGongEntry("水21", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗\n本场战斗中，造成伤害：施加{3 + dj}减甲，不高于伤害值"), WuXing.Shui, 4,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "铃鹿御前", 3 + waiGong.Dj);
                }),

            new WaiGongEntry("自动灵气", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗\n每回合灵气+{1 + (j - 2) / 2}"), WuXing.Shui, 0,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "自动灵气", 1 + (waiGong.GetJingJie() - 2) / 2);
                }),

            new WaiGongEntry("承", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+{3 + dj}\n每2灵气，水势+1"), WuXing.Shui, 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 3 + waiGong.Dj);
                    int value = caster.GetStackOfBuff("灵气") / 2;
                    StageManager.Instance.BuffProcedure(caster, caster, "水势", value);
                }),

            new WaiGongEntry("木20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{12 + 2 * dj}攻\n消耗1格挡：吸血"), WuXing.Mu, 1, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 12 + 2 * waiGong.Dj,
                        lifeSteal: caster.TryConsumeBuff("格挡"));
                }),

            new WaiGongEntry("木21", new CLLibrary.Range(2, 5), "消耗\n永久吸血，直到使用非攻击牌", WuXing.Mu, manaCost: new ManaCost((l, j, dj, p) => 3 - dj),
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "永久吸血");
                }),

            new WaiGongEntry("木22", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{3 * j}攻\n二动\n初次：遭受1跳卡牌"), WuXing.Mu, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 3 * waiGong.GetJingJie());
                    caster.Swift = true;
                    if (waiGong.IsFirstTime)
                        StageManager.Instance.BuffProcedure(caster, caster, "跳卡牌");
                }),

            new WaiGongEntry("木23", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"每{6 - dj}水势，消耗1点，格挡+1"), WuXing.Mu, 1,
                execute: (caster, waiGong, recursive) =>
                {
                    int value = caster.GetStackOfBuff("水势") / (6 - waiGong.Dj);
                    caster.TryConsumeBuff("水势", value);
                    StageManager.Instance.BuffProcedure(caster, caster, "格挡", value);
                }),

            new WaiGongEntry("见龙在田", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{12 + 3 * dj}攻\n每1水势，多1攻"), WuXing.Mu, 1, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 12 + 3 * waiGong.Dj + caster.GetStackOfBuff("水势"));
                }),

            new WaiGongEntry("木24", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+{2 + dj}\n满血：翻倍"), WuXing.Mu,
                execute: (caster, waiGong, recursive) =>
                {
                    int d = caster.IsFullHp ? 2 : 1;
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", (2 + waiGong.Dj) * d);
                }),

            new WaiGongEntry("火20", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"灵气+2\n每有{6 - dj}点格挡，力量+1"), WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", 2);
                    int value = caster.GetStackOfBuff("格挡") / (6 - waiGong.Dj);
                    StageManager.Instance.BuffProcedure(caster, caster, "力量", value);
                }),

            new WaiGongEntry("火22", new CLLibrary.Range(2, 5), "消耗\n本场战斗中，每次受到不少于10点伤害的时候，力量+1", WuXing.Huo, manaCost: new ManaCost((l, j, dj, p) => 3 - dj),
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "火22");
                }),

            new WaiGongEntry("朝孔雀", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"力量+1\n{8 + 2 * (j - 2)}攻\n消耗1格挡：翻倍"), WuXing.Huo, manaCost: 1, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    int d = caster.TryConsumeBuff("格挡") ? 2 : 1;
                    StageManager.Instance.BuffProcedure(caster, caster, "力量", d);
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), d * (8 + 2 * (waiGong.GetJingJie() - 2)));
                }),

            new WaiGongEntry("昼虎", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{15 + 3 * (j - 2)}攻 穿透"), WuXing.Huo, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 15 + 3 * (waiGong.GetJingJie() - 2), pierce: true);
                }),

            new WaiGongEntry("势如火", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"下{1 + (j - 2)}次攻击具有穿透"), WuXing.Huo, manaCost: 1,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "穿透", 1 + (waiGong.GetJingJie() - 2));
                }),

            new WaiGongEntry("天衣无缝", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗\n若无攻击牌，每回合：{8 + 2 * (j - 2)}攻"), WuXing.Tu,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    bool noAttack = caster._waiGongList.FirstObj(wg => wg.GetWaiGongType().Contains(WaiGongType.Attack)) == null;
                    if (noAttack)
                        StageManager.Instance.BuffProcedure(caster, caster, "天衣无缝", 8 + 2 * (waiGong.GetJingJie() - 2));
                }),

            // new WaiGongEntry("土21", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"消耗\n生命上限变为一半\n不屈+{3 + (j + 1) / 2}"), WuXing.Tu,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         waiGong.Consumed = true;
            //         caster.MaxHp /= 2;
            //         StageManager.Instance.BuffProcedure(caster, caster, "不屈", 3 + (waiGong.GetJingJie() + 1) / 2);
            //     }),
            //
            // new WaiGongEntry("土22", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"{30 + 10 * j}攻\n遭受2跳回合"), WuXing.Tu, 2, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 30 + 10 * waiGong.GetJingJie());
            //         StageManager.Instance.BuffProcedure(caster, caster, "跳回合", 2);
            //     }),
            //
            // new WaiGongEntry("贪狼", new CLLibrary.Range(2, 5), new ChipDescription((l, j, dj, p) => $"5攻x{3 + dj}\n不屈+2"), WuXing.Tu, 2, type: WaiGongType.Attack,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 5, 3 + waiGong.Dj);
            //         StageManager.Instance.BuffProcedure(caster, caster, "不屈", 2);
            //     }),

            new WaiGongEntry("木剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{16 + 4 * dj}攻\n架势：暴击"), WuXing.Jin, 0, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 16 + 4 * waiGong.Dj, crit: waiGong.JiaShi);
                }),

            new WaiGongEntry("金31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"1攻 每失去过{8 - j}点护甲，多1攻"), WuXing.Jin, 0, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 1 + (8 - caster.LostArmorRecord / waiGong.GetJingJie()), crit: true);
                }),

            new WaiGongEntry("金32", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"护甲翻倍"), WuXing.Jin, new ManaCost((l, j, dj, p) => 12 - 2 * j),
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(caster, caster, caster.Armor);
                }),

            new WaiGongEntry("软剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{4 + (j - 3) * 4}攻\n击伤：二动"), WuXing.Jin, 1, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 4 + (waiGong.GetJingJie() - 3) * 4,
                        damaged: d => caster.Swift = true);
                }),

            new WaiGongEntry("重剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{22 + 8 * dj}攻\n击伤：护甲+击伤值\n遭受2跳回合\n架势：无需跳回合"), WuXing.Jin, 2, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 22 + 8 * waiGong.Dj,
                        damaged: d => StageManager.Instance.ArmorGainProcedure(caster, caster, d.Value));
                    if (!waiGong.JiaShi)
                        StageManager.Instance.BuffProcedure(caster, caster, "跳回合", 2);
                }),

            new WaiGongEntry("金刚剑阵", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"1攻 每有一点护甲多1攻"), WuXing.Jin, new ManaCost((l, j, dj, p) => 3 - 2 * (j - 3)), type: WaiGongType.Attack | WaiGongType.JianZhen,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 1 + Mathf.Max(0, caster.Armor));
                }),

            new WaiGongEntry("菊剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"敌方减甲不少于5：二动\n{4 + 4 * dj}攻"), WuXing.Shui, 0, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    if (caster.Opponent().Armor <= -5)
                        caster.Swift = true;
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), -2 + 4 * waiGong.GetJingJie());
                }),

            new WaiGongEntry("兰剑", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"施加{4 + 4 * (j - 3)}减甲\n二动"), WuXing.Shui, 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorLoseProcedure(caster.Opponent(), 4 + 4 * (waiGong.GetJingJie() - 3));
                    caster.Swift = true;
                }),

            new WaiGongEntry("水31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗一半护甲，施加等量破甲"), WuXing.Shui, new ManaCost((l, j, dj, p) => 6 - j),
                execute: (caster, waiGong, recursive) =>
                {
                    int value = caster.Armor / 2;
                    StageManager.Instance.ArmorLoseProcedure(caster, value);
                    StageManager.Instance.ArmorLoseProcedure(caster.Opponent(), value);
                }),

            new WaiGongEntry("隐如云", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"下{1 + dj}次造成伤害转减甲"), WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "隐如云", 1 + waiGong.Dj);
                }),

            new WaiGongEntry("转", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"奇偶：每1水势，施加1减甲/生命及上限+1"), WuXing.Shui, new ManaCost((l, j, dj, p) => 4 - 2 * dj),
                execute: (caster, waiGong, recursive) =>
                {
                    int value = caster.GetStackOfBuff("水势");
                    if (waiGong.IsOdd)
                    {
                        StageManager.Instance.ArmorLoseProcedure(caster.Opponent(), value);
                    }
                    else
                    {
                        caster.MaxHp += value;
                        StageManager.Instance.HealProcedure(caster, caster, value);
                    }
                }),

            new WaiGongEntry("气吞山河", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"将灵气补至本局最大值+{1 + dj}"), WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    int space = caster.HighestManaRecord - caster.GetMana() + 1 + waiGong.Dj;
                    StageManager.Instance.BuffProcedure(caster, caster, "灵气", space);
                }),

            new WaiGongEntry("木30", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{10 + (j - 3) * 4}攻\n每造成3点伤害，格挡+1"), WuXing.Mu, 4, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 10 + (waiGong.GetJingJie() - 3) * 4,
                        damaged: d => StageManager.Instance.BuffProcedure(caster, caster, "格挡", d.Value / 3));
                }),

            new WaiGongEntry("木31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗\n自动格挡+{(int)j}"), WuXing.Mu, 4,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "自动格挡", waiGong.GetJingJie());
                }),

            new WaiGongEntry("飞龙在天", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"{18 + 3 * dj}攻\n击伤：施加{2 + dj}缠绕"), WuXing.Mu, 4, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 18 + 3 * waiGong.Dj,
                        damaged: d => StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "缠绕", 2 + waiGong.Dj));
                }),

            new WaiGongEntry("徐如林", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"下{1 + (j - 3)}次耗灵气免费"), WuXing.Mu,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "免费");
                }),

            new WaiGongEntry("火30", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗10生命\n{18 + 4 * dj}攻\n每1闪避，多{2 + dj}攻"), WuXing.Huo, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    int value = (2 + waiGong.Dj) * caster.GetStackOfBuff("闪避");
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 18 + 4 * waiGong.Dj + value);
                }),

            new WaiGongEntry("火31", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"每{4 - dj}格挡，消耗1点，闪避+1"), WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    int value = caster.GetStackOfBuff("格挡") / (4 - waiGong.Dj);
                    caster.TryConsumeBuff("格挡", value);
                    StageManager.Instance.BuffProcedure(caster, caster, "闪避", value);
                }),

            new WaiGongEntry("火32", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗\n每轮：闪避补至{3 + dj}"), WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "自动闪避", 3 + waiGong.Dj);
                }),

            new WaiGongEntry("夕象", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"消耗{20 - 10 * dj}生命\n三动"), WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.DamageProcedure(caster, caster, 20 - 10 * waiGong.Dj);
                    caster.UltraSwift = true;
                }),

            new WaiGongEntry("文曲", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"下{1 + dj}次攻击时，次数+1"), WuXing.Tu,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "连击", 1 + waiGong.Dj);
                }),

            // new WaiGongEntry("廉贞", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"对敌方造成伤害时，至少为{4 + 3 * dj}"), WuXing.Tu,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         throw new NotImplementedException();
            //     }),
            //
            // new WaiGongEntry("土31", new CLLibrary.Range(3, 5), new ChipDescription((l, j,, dj p) => $"消耗所有生命转护甲"), WuXing.Tu, new ManaCost((l, j,, dj p) => 4 - 2 * (j - 3)),
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         int value = caster.Hp;
            //         StageManager.Instance.AttackProcedure(caster, caster, value);
            //         StageManager.Instance.ArmorGainProcedure(caster, caster, value);
            //     }),
            //
            // new WaiGongEntry("稳如山", new CLLibrary.Range(3, 5), new ChipDescription((l, j, dj, p) => $"下{1 + (j - 3)}张牌后驱散"), WuXing.Tu,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         throw new NotImplementedException();
            //     }),

            new WaiGongEntry("金40", new CLLibrary.Range(4, 5), "消耗\n护甲+50", WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 50);
                }),

            new WaiGongEntry("金41", new CLLibrary.Range(4, 5), "消耗\n累计获得200护甲：永久暴击", WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    throw new NotImplementedException();
                    // waiGong.Consumed = true;
                    // StageManager.Instance.BuffProcedure(caster, caster, "待激活的永久暴击");
                }),

            new WaiGongEntry("收刀", new CLLibrary.Range(4, 5), "二动\n上张牌激活架势", WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    caster.Swift = true;
                }),

            new WaiGongEntry("无极剑阵", new CLLibrary.Range(4 , 5), "消耗\n使用之前3张剑阵牌", WuXing.Jin, 6, WaiGongType.JianZhen,
                execute: (caster, waiGong, recursive) =>
                {
                    if (recursive == false)
                        return;

                    waiGong.Consumed = true;
                    waiGong.Prevs(false)
                        .FilterObj(wg => wg.GetWaiGongType().Contains(WaiGongType.JianZhen) && wg.GetName() != "无极剑阵")
                        .FirstN(3)
                        .Reverse()
                        .Do(wg => wg.Execute(caster, false));
                }),

            // new WaiGongEntry("金42", new CLLibrary.Range(4, 5), new ChipDescription((l, j, dj, p) => $"持续4次，护甲减少时，加回来"), WuXing.Jin, 0, type: WaiGongEntry.WaiGongType.NONATTACK,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //     }),

            new WaiGongEntry("梅剑", new CLLibrary.Range(4, 5), "5攻*2\n敌方有减甲：暴击", WuXing.Shui, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 5, crit: caster.Opponent().Armor < 0);
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 5, crit: caster.Opponent().Armor < 0);
                }),

            new WaiGongEntry("水42", new CLLibrary.Range(4, 5), "消耗\n施加2跳回合", WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "跳回合", 2);
                }),

            new WaiGongEntry("少阴", new CLLibrary.Range(4, 5), "消耗\n敌方获得减甲时，+3\n消耗少阳：额外层数", WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    int value = caster.GetStackOfBuff("少阳") + 3;
                    caster.TryRemoveBuff("少阳");
                    StageManager.Instance.BuffProcedure(caster, caster, "少阴", value);
                }),

            new WaiGongEntry("玄武吐息法", new CLLibrary.Range(4, 5), "消耗\n水势不少于30激活\n本局游戏中：治疗可以穿上限，无法二动", WuXing.Shui,
                execute: (caster, waiGong, recursive) =>
                {
                    throw new NotImplementedException();
                    // waiGong.Consumed = true;
                    // StageManager.Instance.BuffProcedure(caster, caster, "玄武吐息法");
                }),

            new WaiGongEntry("合", new CLLibrary.Range(4, 5), new ChipDescription((l, j, dj, p) => $"消耗所有水势\n奇偶：每消耗1，给与3减甲/生命及上限+3"), WuXing.Shui, 6,
                execute: (caster, waiGong, recursive) =>
                {
                    int value = caster.GetStackOfBuff("水势") * 3;
                    caster.TryRemoveBuff("水势");
                    if (waiGong.IsOdd)
                    {
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), value);
                    }
                    else
                    {
                        caster.MaxHp += value;
                        StageManager.Instance.HealProcedure(caster, caster, value);
                    }
                }),

            new WaiGongEntry("亢龙有悔", new CLLibrary.Range(4, 5), "消耗24生命上限\n24攻 吸血", WuXing.Mu, 1, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    caster.MaxHp -= 24;
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 24, lifeSteal: true);
                }),

            new WaiGongEntry("木41", new CLLibrary.Range(4, 5), "消耗\n本场战斗中，治疗被代替，每有10点，格挡+1\n格挡效果翻倍", WuXing.Mu,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "强化格挡");
                }),

            new WaiGongEntry("木42", new CLLibrary.Range(4, 5), "消耗\n本场战斗中，被治疗时，如果实际治疗>=20，二动", WuXing.Mu,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "治疗转二动");
                }),

            new WaiGongEntry("火40", new CLLibrary.Range(4, 5), "10攻*3 穿透", WuXing.Huo, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 10, 3, pierce: true);
                }),

            new WaiGongEntry("涅槃", new CLLibrary.Range(4, 5), "消耗\n累计获得20闪避激活\n每轮：生命回满", WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "待激活的涅槃");
                }),

            new WaiGongEntry("夜凯", new CLLibrary.Range(4, 5), "每回合：消耗10生命，力量+1", WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "夜凯");
                }),

            new WaiGongEntry("惊如雷", new CLLibrary.Range(4, 5), "下一张牌使用两次", WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "双发");
                }),

            new WaiGongEntry("天女散花", new CLLibrary.Range(4, 5), "1攻 消耗所有闪避，每消耗1点，多攻击1次", WuXing.Tu, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    int value = caster.GetStackOfBuff("闪避");
                    caster.TryConsumeBuff("闪避", value);
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 1, value);
                }),

            // new WaiGongEntry("强化不屈", new CLLibrary.Range(4, 5), "消耗\n累计12不屈激活\n对手回合中，自己不屈激活时，施加1跳回合", WuXing.Tu,
            //     execute: (caster, waiGong, recursive) =>
            //     {
            //         throw new NotImplementedException();
            //         // if (!caster.TryConsumeBuff("不屈", 8))
            //         //     return;
            //         // waiGong.Consumed = true;
            //         // StageManager.Instance.BuffProcedure(caster, caster, "强化不屈");
            //     }),

            new WaiGongEntry("破军", new CLLibrary.Range(4, 5), "5攻x5", WuXing.Tu, 3,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 5, 5);
                }),

            new WaiGongEntry("庚金：万千辉", 5, "无效化敌人下一次攻击，并且反击", WuXing.Jin, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "看破");
                }),

            new WaiGongEntry("天人合一", 5, "消耗\n激活所有架势", WuXing.Jin,
                execute: (caster, waiGong, recursive) =>
                {
                    throw new NotImplementedException();
                }),

            // 莲花
            new WaiGongEntry("凶水：三步", 5, "10攻 击伤：对方剩余生命每有2点，施加1减甲", WuXing.Shui, type: WaiGongType.Attack,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster, 10,
                        damaged: d =>
                        {
                            int value = d.Tgt.Hp / 2;
                            StageManager.Instance.ArmorLoseProcedure(d.Tgt, value);
                        });
                }),

            new WaiGongEntry("缠枝：周天结", 5, "消耗6格挡\n消耗\n本场战斗中，灵气消耗后加回", WuXing.Mu,
                execute: (caster, waiGong, recursive) =>
                {
                    // waiGong.Consumed = true;
                    // StageManager.Instance.BuffProcedure(caster, caster, "通透世界");
                }),

            new WaiGongEntry("烬焰：须菩提", 5, "下一张牌使用之后消耗，第六次使用时消耗", WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    // waiGong.Consumed = true;
                    // StageManager.Instance.BuffProcedure(caster, caster, "通透世界");
                }),

            new WaiGongEntry("轰炎：焚天", 5, "消耗，本场战斗中，自己的所有攻击具有穿透", WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                    waiGong.Consumed = true;
                    StageManager.Instance.BuffProcedure(caster, caster, "通透世界");
                }),

            new WaiGongEntry("狂火：钟声", 5, "消耗，永久三动，三回合后死亡", WuXing.Huo,
                execute: (caster, waiGong, recursive) =>
                {
                }),

            new WaiGongEntry("霸王鼎：离别", 5, "消耗，永久不屈（待实现）", WuXing.Tu,
                execute: (caster, waiGong, recursive) =>
                {
                    // waiGong.Consumed = true;
                    // StageManager.Instance.BuffProcedure(caster, caster, "通透世界");
                }),
        };
    }
}
