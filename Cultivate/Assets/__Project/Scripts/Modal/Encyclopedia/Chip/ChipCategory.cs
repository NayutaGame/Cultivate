using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            /*
            new WaiGongEntry("自动使用", JingJie.LianQi, "自动消耗掉自己", 0,
                startStage: (caster, waiGong) =>
                    waiGong.Consumed = true),

            new WaiGongEntry("给自己点可被驱散", JingJie.LianQi, "", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "可被驱散", 2);
                }),

            new WaiGongEntry("驱散过程", JingJie.LianQi, "", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.DispelProcedure(caster, 1, true);
                    StageManager.Instance.DispelProcedure(caster, 1, false);
                }),

            new WaiGongEntry("给自己治疗同时加护甲Buff", JingJie.LianQi, "", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "治疗同时加护甲", 2);
                }),

            new WaiGongEntry("治疗自己", JingJie.LianQi, "", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.HealProcedure(caster, caster, 5);
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 1);
                }),

            new WaiGongEntry("给自己临金Buff", JingJie.LianQi, "", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "临金");
                }),

            new WaiGongEntry("金剑", JingJie.LianQi, "造成(临金)伤害", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), waiGong.GetPower(WuXing.Jin));
                }),

            new WaiGongEntry("给自己下回合开始三动Buff", JingJie.LianQi, "", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster, "下回合开始三动");
                }),

            new WaiGongEntry("给自己加点护甲", JingJie.LianQi, "", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.ArmorGainProcedure(caster, caster, 1);
                }),

            new WaiGongEntry("根据已损失护甲造成伤害", JingJie.LianQi, "", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), caster.LostArmorRecord);
                }),

            new WaiGongEntry("先给自己掉点血", JingJie.LianQi, "", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster, 1);
                }),

            new WaiGongEntry("再把血加回来", JingJie.LianQi, "", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.HealProcedure(caster, caster, 1);
                }),

            new WaiGongEntry("根据已回复生命造成伤害", JingJie.LianQi, "", 0,
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), caster.HealedRecord);
                }),
            */
            /**********************************************************************************************************/
            /*********************************************** 大剑哥 ****************************************************/
            /**********************************************************************************************************/

            /**********************************************************************************************************/
            /*********************************************** 石丸 ******************************************************/
            /**********************************************************************************************************/

            new WaiGongEntry("三昧真火",JingJie.LianQi,"造成12点伤害。\r\n\r\n[2火：伤害+4]\r\n[2木：生成1灰烬]",
                execute:(caster, waiGong, recursive) =>
                {
                    if(waiGong.GetPower(WuXing.Huo)>=2)
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 16 );
                    else
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 12 );
                    if(waiGong.GetPower(WuXing.Mu)>=2)
                        StageManager.Instance.BuffProcedure(caster, caster, "灰烬", 1);
                }),

            new WaiGongEntry("沸血", JingJie.ZhuJi, "获得4|强化|。\r\n\r\n[2火：+2|强化|]\r\n[4火：+4|强化|]",
                execute: (caster, waiGong, recursive) =>
                {
                    if(waiGong.GetPower(WuXing.Huo)>=4)
                        StageManager.Instance.BuffProcedure(caster, caster, "强化", 10);
                    else if(waiGong.GetPower(WuXing.Huo)>=2)
                            StageManager.Instance.BuffProcedure(caster, caster, "强化", 6);
                    else
                        StageManager.Instance.BuffProcedure(caster, caster, "强化", 4);
                }),

            new WaiGongEntry("茫茫焦土", JingJie.ZhuJi, "生成4灰烬，消耗场上所有木桩，每个被消耗的木桩生成2灰烬。\r\n\r\n[2木：灰烬+2]\r\n[3火：获得[毁灭]",
                execute: (caster, waiGong, recursive) =>
                {
                    int stack = (caster.GetStackOfBuff("木桩") + caster.Opponent().GetStackOfBuff("木桩"));
                    caster.RemoveBuffs("木桩");
                    caster.Opponent().RemoveBuffs("木桩");
                    StageManager.Instance.BuffProcedure(caster, caster, "灰烬", 2*stack);

                    if(waiGong.GetPower(WuXing.Mu)>=2)
                        StageManager.Instance.BuffProcedure(caster, caster, "灰烬", 6);
                    else
                        StageManager.Instance.BuffProcedure(caster, caster, "灰烬", 4);

                    if(waiGong.GetPower(WuXing.Huo)>=3)
                        StageManager.Instance.BuffProcedure(caster, caster, "毁灭", 1);
                }),

            new WaiGongEntry("浴火锻体", JingJie.YuanYing, "消耗场上所有灰烬，获得消耗数的|力量|。\r\n\r\n[1水：获得3|强化|]\r\n[5火：改为不消耗灰烬]",
                execute: (caster, waiGong, recursive) =>
                {
                    int stack = (caster.GetStackOfBuff("灰烬") + caster.Opponent().GetStackOfBuff("灰烬"));

                    StageManager.Instance.BuffProcedure(caster, caster, "力量", stack);

                    if(waiGong.GetPower(WuXing.Huo)<5)
                    {
                        caster.RemoveBuffs("灰烬");
                        caster.Opponent().RemoveBuffs("灰烬");
                    }

                    if(waiGong.GetPower(WuXing.Shui)>=1)
                        StageManager.Instance.BuffProcedure(caster, caster, "强化", 3);
                }),

            new WaiGongEntry("太初之火",JingJie.HuaShen,"消耗场上所有Buff和造物，造成消耗数×5点伤害。\r\n\r\n[3火：5点改为7点]\r\n[3木：消耗数+2]",
                execute:(caster, waiGong, recursive) =>
                {
                    List<Buff> casterBuffs = caster.Buffs.FilterObj(b => b.Dispellable).ToList();
                    List<Buff> opponentBuffs = caster.Opponent().Buffs.FilterObj(b => b.Dispellable).ToList();
                    int sumStack = 0;
                    casterBuffs.Do(b => sumStack += b.Stack);
                    opponentBuffs.Do(b => sumStack += b.Stack);
                    casterBuffs.Do(b => caster.RemoveBuff(b));
                    opponentBuffs.Do(b => caster.Opponent().RemoveBuff(b));

                    if(waiGong.GetPower(WuXing.Mu)>=3)
                        sumStack += 2;
                    else if(waiGong.GetPower(WuXing.Huo)>=3)
                            StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 7 * sumStack );
                    else
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 5 * sumStack );
                }),

            new WaiGongEntry("巨木撞击",JingJie.LianQi,"造成10点伤害，生成1木桩。\r\n\r\n[2木：伤害+3]\r\n[3水：木桩+1]",
                execute:(caster, waiGong, recursive) =>
                {
                    if(waiGong.GetPower(WuXing.Mu)>=2)
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 13 );
                    else
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 10 );
                    if(waiGong.GetPower(WuXing.Shui)>=3)
                        StageManager.Instance.BuffProcedure(caster, caster, "木桩", 2);
                    else
                        StageManager.Instance.BuffProcedure(caster, caster, "木桩", 1);
                }),

            new WaiGongEntry("荆棘之狱",JingJie.ZhuJi,"造成6点伤害，敌方受到3点持续伤害3回合。\r\n\r\n[2木：持续伤害+2回合]\r\n[5木：敌方停止行动1回合]",
                execute:(caster, waiGong, recursive) =>
                {
                    StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 6 );
                    if(waiGong.GetPower(WuXing.Mu)>=2)
                        StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "持续伤害", 5);
                    else
                        StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "持续伤害", 3);
                    if(waiGong.GetPower(WuXing.Mu)>=5)
                        StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "跳回合", 1);
                }),

            new WaiGongEntry("机关巨兽", JingJie.YuanYing, "消耗场上所有木桩，造成消耗数×8点伤害。\r\n\r\n[4木：改为×12点伤害]\r\n[1金：追击<炼化>]",
                execute: (caster, waiGong, recursive) =>
                {
                    int stack = (caster.GetStackOfBuff("木桩") + caster.Opponent().GetStackOfBuff("木桩"));

                    if(waiGong.GetPower(WuXing.Mu)>=4)
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 12*stack );
                    else
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 8*stack );

                    if(waiGong.GetPower(WuXing.Jin)>=1)
                    {
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 8 );
                        StageManager.Instance.BuffProcedure(caster, caster, "利刃", 2);
                    }
                }),

            new WaiGongEntry("太始之木",JingJie.HuaShen,"消耗自身所有Debuff，恢复消耗数×5生命，生成消耗数木桩。\r\n\r\n[2水：改为×10生命]\r\n[4木：之后使木桩数翻倍]",
                execute:(caster, waiGong, recursive) =>
                {
                    int stack = caster.GetSumOfStackOfBuffs("停止","跳过","持续伤害");
                    caster.RemoveBuffs("停止","跳过","持续伤害");//待确认，优先级问题

                    if(waiGong.GetPower(WuXing.Shui)>=2)
                        StageManager.Instance.HealProcedure(caster, caster, 10*stack);
                    else
                        StageManager.Instance.HealProcedure(caster, caster, 5*stack);

                    if(waiGong.GetPower(WuXing.Mu)>=4)
                        StageManager.Instance.BuffProcedure(caster, caster, "木桩", stack);
                }),

            new WaiGongEntry("聚水成涡", JingJie.LianQi, "生成2水涡。\r\n\r\n[2水：+1水涡]\r\n[4水：+1水涡]",
                execute: (caster, waiGong, recursive) =>
                {
                    if(waiGong.GetPower(WuXing.Shui)>=4)
                        StageManager.Instance.BuffProcedure(caster, caster, "水涡", 4);
                    else if(waiGong.GetPower(WuXing.Shui)>=2)
                        StageManager.Instance.BuffProcedure(caster, caster, "水涡", 3);
                    else
                        StageManager.Instance.BuffProcedure(caster, caster, "水涡", 2);
                }),

            new WaiGongEntry("黑水阵", JingJie.ZhuJi, "获得水涡数×8护甲，之后消耗1水涡。\r\n\r\n[2金：获得【固化】3回合]\r\n[4水：同时造成水涡数×8攻]",
                execute: (caster, waiGong, recursive) =>
                {
                    int stack = caster.GetStackOfBuff("水涡");

                    StageManager.Instance.ArmorGainProcedure(caster, caster,8*stack );

                    if(waiGong.GetPower(WuXing.Shui)>=4)
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 8*stack );

                    caster.TryConsumeBuff("水涡");//当水涡层数为0时待确认

                    if(waiGong.GetPower(WuXing.Jin)>=2)
                        StageManager.Instance.BuffProcedure(caster, caster, "固化", 3);//固化未做
                }),

            new WaiGongEntry("涤孽诡术", JingJie.JinDan, "掠夺对方的所有Buff和造物。\r\n\r\n[2水：5攻，5恢复]\r\n[5水：每掠夺1层buff或造物，造成5攻]",
                execute: (caster, waiGong, recursive) =>
                {
                    List<Buff> opponentBuffs = caster.Opponent().Buffs.FilterObj(b => b.Dispellable).ToList();
                    int sumStack = 0;
                    opponentBuffs.Do(b => sumStack += b.Stack);
                    opponentBuffs.Do(b => caster.Opponent().RemoveBuff(b));
                    opponentBuffs.Do(b => StageManager.Instance.BuffProcedure(caster, caster, b.BuffEntry, b.Stack));

                    if(waiGong.GetPower(WuXing.Shui)>=5)
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 5 * sumStack);

                    if (waiGong.GetPower(WuXing.Shui) >= 2)
                    {
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 5);
                        StageManager.Instance.HealProcedure(caster, caster, 5 );
                    }
                }),

            new WaiGongEntry("水牢", JingJie.YuanYing, "对方【停止】1回合，消耗1水涡。\r\n\r\n[3水：【停止】改为【跳过】]\r\n[5水：改为2回合]",
                execute: (caster, waiGong, recursive) =>//文本待优化
                {
                    if(waiGong.GetPower(WuXing.Shui)>=5)
                        StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "跳卡牌", 2);
                    else if(waiGong.GetPower(WuXing.Shui)>=3)
                        StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "跳卡牌", 1);
                    else
                        StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "跳回和", 1);
                }),

            new WaiGongEntry("太易之水", JingJie.HuaShen, "消耗场上所有造物，获得消耗数【强化】。\r\n\r\n[5水：之后生成4水涡]\r\n[4金：提前【追击】<黑水阵>、<反震>]",
                execute: (caster, waiGong, recursive) =>
                {
                    int stack = (caster.GetSumOfStackOfBuffs("水涡","灰烬","木桩","砂尘","利刃") + caster.Opponent().GetSumOfStackOfBuffs("水涡","灰烬","木桩","砂尘","利刃"));
                    int Whirlpool = caster.GetStackOfBuff("水涡");

                    if(waiGong.GetPower(WuXing.Jin)>=4)
                    {
                        StageManager.Instance.ArmorGainProcedure(caster, caster,8*Whirlpool );
                        caster.TryConsumeBuff("水涡");
                        StageManager.Instance.BuffProcedure(caster, caster, "反震", 1);
                    }

                    caster.RemoveBuffs("水涡","灰烬","木桩","砂尘","利刃");
                    caster.Opponent().RemoveBuffs("水涡","灰烬","木桩","砂尘","利刃");

                    StageManager.Instance.BuffProcedure(caster, caster, "强化", stack);

                    if(waiGong.GetPower(WuXing.Shui)>=5)
                        StageManager.Instance.BuffProcedure(caster, caster, "水涡", 4);
                }),

            new WaiGongEntry("炼化", JingJie.LianQi, "造成8点伤害，生成2利刃。\r\n\r\n[2金：改为5点×2伤害]\r\n[2土：利刃+1]",
                execute: (caster, waiGong, recursive) =>
                {
                    if(waiGong.GetPower(WuXing.Jin)>=2)
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 5 ,2 );
                    if(waiGong.GetPower(WuXing.Tu)>=2)
                        StageManager.Instance.BuffProcedure(caster, caster, "利刃", 3);
                    else
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 8 );
                        StageManager.Instance.BuffProcedure(caster, caster, "利刃", 2);
                }),

            new WaiGongEntry("冶金术", JingJie.ZhuJi, "消耗我方灰烬，获得消耗数×2护甲，生成消耗数利刃。\r\n\r\n[2金：2护甲改为3护甲]\r\n[5金：生成1【铁卫】]",
                execute: (caster, waiGong, recursive) =>
                {
                    int stack = caster.GetStackOfBuff("灰烬");

                    if(waiGong.GetPower(WuXing.Jin)>=2)
                        StageManager.Instance.ArmorGainProcedure(caster, caster,3*stack );
                    else
                        StageManager.Instance.ArmorGainProcedure(caster, caster,2*stack );

                    StageManager.Instance.BuffProcedure(caster, caster, "利刃", stack);

                    caster.TryConsumeBuff("灰烬");

                    if(waiGong.GetPower(WuXing.Jin)>=5)
                        StageManager.Instance.BuffProcedure(caster, caster, "铁卫", 1);
                }),

            new WaiGongEntry("利刃决", JingJie.JinDan, "造成6点×利刃数伤害。\r\n\r\n[4金：6点改为8点]\r\n[3土：提前生成2利刃]",
                execute: (caster, waiGong, recursive) =>
                {
                    int stack = caster.GetStackOfBuff("利刃");
                    if(waiGong.GetPower(WuXing.Tu)>=3)
                        StageManager.Instance.BuffProcedure(caster, caster, "利刃", 2 );
                    if(waiGong.GetPower(WuXing.Jin)>=4)
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 8*stack );
                    else
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 6*stack );
                }),

            new WaiGongEntry("反震", JingJie.YuanYing, "下一次被攻击后，造成我方受到该攻击前护甲数伤害。\r\n\r\n[3金：获得1【坚韧】]\r\n[6金：改为下2次被攻击，【坚韧】+1]",
                execute: (caster, waiGong, recursive) =>
                {
                    if(waiGong.GetPower(WuXing.Jin)>=6)
                    {
                        StageManager.Instance.BuffProcedure(caster, caster, "反震", 2);
                        StageManager.Instance.BuffProcedure(caster, caster, "坚韧", 2);
                    }
                    else if(waiGong.GetPower(WuXing.Jin)>=3)
                    {
                        StageManager.Instance.BuffProcedure(caster, caster, "反震", 1);
                        StageManager.Instance.BuffProcedure(caster, caster, "坚韧", 1);
                    }
                    else
                        StageManager.Instance.BuffProcedure(caster, caster, "反震", 1);
                }),

            new WaiGongEntry("太素之金", JingJie.HuaShen, "将我方所有造物转变为利刃，【追击】<利刃决>。\r\n\r\n[3金：生成1【铁卫】]\r\n[5金：改为提前生成1【铁卫】]",
                execute: (caster, waiGong, recursive) =>
                {
                    int stack = caster.GetSumOfStackOfBuffs("水涡","灰烬","木桩","砂尘","利刃");

                    caster.RemoveBuffs("水涡","灰烬","木桩","砂尘","利刃");
                    StageManager.Instance.BuffProcedure(caster, caster, "利刃", stack);

                    if(waiGong.GetPower(WuXing.Jin)>=5)
                    {
                        StageManager.Instance.BuffProcedure(caster, caster, "铁卫", 1);
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 6*stack );//现阶段只打出基础利刃决
                    }
                    else
                    {
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 6*stack );//现阶段只打出基础利刃决
                        StageManager.Instance.BuffProcedure(caster, caster, "铁卫", 1);
                    }
                }),

            new WaiGongEntry("土砂之术", JingJie.LianQi, "生成2砂尘，转变我方灰烬为砂尘。\r\n\r\n[2火：砂尘+1]\r\n[2土：我方改为全场]",
                execute: (caster, waiGong, recursive) =>
                {
                    if(waiGong.GetPower(WuXing.Huo)>=2)
                        StageManager.Instance.BuffProcedure(caster, caster, "砂尘", 3 );
                    else
                        StageManager.Instance.BuffProcedure(caster, caster, "砂尘", 2 );

                    while(caster.TryConsumeBuff("灰烬", 1))
                            StageManager.Instance.BuffProcedure(caster, caster, "砂尘", 1);
                    if(waiGong.GetPower(WuXing.Tu)>=2)
                        while(caster.Opponent().TryConsumeBuff("灰烬", 1))
                            StageManager.Instance.BuffProcedure(caster, caster, "砂尘", 1);
                }),

            new WaiGongEntry("聚砂成墙", JingJie.ZhuJi, "消耗砂尘，生成【土墙】，每3砂尘生成1【土墙】。\r\n\r\n[2土：额外生成1土墙]\r\n[4土：每3砂尘改为每2砂尘]",
                execute: (caster, waiGong, recursive) =>
                {
                    if(waiGong.GetPower(WuXing.Tu)>=4)
                        while(caster.TryConsumeBuff("砂尘", 2))
                            StageManager.Instance.BuffProcedure(caster, caster, "土墙", 1);
                    else
                        while(caster.TryConsumeBuff("砂尘", 3))
                            StageManager.Instance.BuffProcedure(caster, caster, "土墙", 1);

                    if(waiGong.GetPower(WuXing.Tu)>=2)
                        StageManager.Instance.BuffProcedure(caster, caster, "土墙", 1);
                }),

            new WaiGongEntry("承天载物", JingJie.JinDan, "对方【跳过】1回合。\r\n\r\n[3土：造成18点伤害]\r\n[3火：生成3砂尘]",
                execute: (caster, waiGong, recursive) =>
                {
                    StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "跳过", 1);

                    if(waiGong.GetPower(WuXing.Huo)>=3)
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 18 );

                    if(waiGong.GetPower(WuXing.Huo)>=3)
                        StageManager.Instance.BuffProcedure(caster, caster, "砂尘", 3);
                }),

            new WaiGongEntry("太极之土", JingJie.HuaShen, "消耗全场10砂尘（优先敌方），对方【跳过】3回合，对方失去所有气。\r\n\r\n[3火：提前转变双方灰烬为砂尘]\r\n[4土：造成30点伤害]",
                execute: (caster, waiGong, recursive) =>//失去气未做
                {
                    if(waiGong.GetPower(WuXing.Huo)>=3)
                    {
                        while(caster.TryConsumeBuff("灰烬", 1))
                            StageManager.Instance.BuffProcedure(caster, caster, "砂尘", 1);
                        while(caster.Opponent().TryConsumeBuff("灰烬", 1))
                            StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "砂尘", 1);
                    }

                    int Sand = caster.GetStackOfBuff("砂尘");
                    int OSand = caster.Opponent().GetStackOfBuff("砂尘");

                    if(OSand>=10)
                    {
                        caster.Opponent().TryConsumeBuff("砂尘",10);
                        StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "跳过", 3);
                    }
                    else if(OSand<10 && (OSand + Sand)>=10)
                    {
                        caster.Opponent().TryConsumeBuff("砂尘",OSand);
                        caster.TryConsumeBuff("砂尘",(Sand - OSand));
                        StageManager.Instance.BuffProcedure(caster, caster.Opponent(), "跳过", 3);
                    }
                    else

                    if(waiGong.GetPower(WuXing.Tu)>=4)
                        StageManager.Instance.AttackProcedure(caster, caster.Opponent(), 30 );
                }),

            /**********************************************************************************************************/
            /*********************************************** Summer68 *************************************************/
            /**********************************************************************************************************/
            /**********************************************************************************************************/
            /*********************************************** End ******************************************************/
            /**********************************************************************************************************/

        };
    }
}
