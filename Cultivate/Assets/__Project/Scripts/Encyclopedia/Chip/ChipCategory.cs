using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipCategory : Category<ChipEntry>
{
    public ChipCategory()
    {
        _list = new List<ChipEntry>()
        {
            new XinfaEntry("龙象吞海决", "水系心法"),
            new XinfaEntry("魔焰决", "火系心法"),
            new XinfaEntry("明王决", "金系心法"),
            new XinfaEntry("玄清天衍录", "通用心法"),
            new XinfaEntry("大帝轮华经", "通用心法"),
            new XinfaEntry("自在极意式", "通用心法"),
            new XinfaEntry("逍遥游", "通用心法"),
            new NeigongEntry("水雾决", "内功"),
            new NeigongEntry("冰心决", "内功"),
            new NeigongEntry("飞云劲", "内功"),
            new NeigongEntry("春草决", "内功"),
            new WaigongEntry("真金印", "提升吸收到金系灵气的概率"),
            new WaigongEntry("生水印", "提升吸收到水系灵气的概率"),
            new WaigongEntry("回春印", "提升吸收到木系灵气的概率"),
            new WaigongEntry("聚火印", "提升吸收到火系灵气的概率"),
            new WaigongEntry("玄土印", "提升吸收到土系灵气的概率"),
            new WaigongEntry("紫微印", "吸收一点水系灵气，【生】额外再吸收一点水系灵气"),
            new WaigongEntry("善水印", "吸收一点木系灵气，【生】额外再吸收一点木系灵气"),
            new WaigongEntry("上清印", "吸收一点火系灵气，【生】额外再吸收一点火系灵气"),
            new WaigongEntry("火铃印", "吸收一点土系灵气，【生】额外再吸收一点土系灵气"),
            new WaigongEntry("三山印", "吸收一点金系灵气，【生】额外再吸收一点金系灵气"),
            new WaigongEntry("丹阳印", "获得【焰】*2"),
            new WaigongEntry("灵光印", "在本回合下一次受到伤害时，获得3点金系灵气"),
            new WaigongEntry("炙火印", "在本回合下一次受到技能伤害时，使对手获得【灼烧】*2"),
            new WaigongEntry("璇水印", "在本回合下一次受到技能伤害时，恢复3点生命值。"),
            new WaigongEntry("罡水印", "下回合造成的水系技能伤害+2"),
            new WaigongEntry("春丝印", "下一次造成的木系技能伤害+5"),
            new WaigongEntry("灵体印", "获得【护罩】*6；本回合释放金系技能时，将额外消耗一点金系灵气，并获得【护罩】*1"),
            new WaigongEntry("分金印", "本场战斗中每次触发五行【连击】后，敌方获得【易伤】*1。若上一次释放的是土系技能，则立即触发此效果。"),
            new WaigongEntry("驱寒印", "本场战斗中每吸收一点灵气，移除自身1层【霜冻】。只能使用一次。"),
            new WaigongEntry("业火印", "本场战斗中每次触发五行【连击】后，敌方获得【灼烧】*1。若上一次释放的是木系技能，则立即触发此效果。"),
            new WaigongEntry("灵藤印", "本场战斗中每次触发五行【连击】后，敌方获得【缠绕】*1。若上一次释放的是水系技能，则立即触发此效果。"),
            new WaigongEntry("怒水印", "本场战斗中每次触发五行【连击】后，获得【疗】*1。若上一次释放的是金系技能，则立即触发此效果。"),
            new WaigongEntry("回风印", "直到下回合开始前，每次受到伤害后获得【蓄力】*1"),
            new WaigongEntry("神皇印", "若使用相同的灵气释放，则下回合开始时，自身【蓄势】层数翻倍。否则，本回合【护罩】抵挡的伤害等量转化为【蓄势】。"),
            new WaigongEntry("覆体印", "消散所有金系灵气，每消散一点获得【减伤】*1"),
        };
    }
}
