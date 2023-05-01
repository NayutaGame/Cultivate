using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BuffEntry : Entry
{
    private string _description;
    public string Description;

    // private Sprite _sprite;
    // public Sprite Sprite => _sprite;

    private BuffStackRule _buffStackRule;
    public BuffStackRule BuffStackRule => _buffStackRule;

    private bool _friendly;
    public bool Friendly => _friendly;

    private bool _dispellable;
    public bool Dispellable => _dispellable;

    public Func<Buff, StageEntity, int, Task> _gain;
    public Func<Buff, StageEntity, Task> _lose;
    public Func<Buff, StageEntity, Task> _stackChanged;
    public Func<Buff, StageEntity, Task> _startStage;
    public Func<Buff, StageEntity, Task> _endStage;
    public Func<Buff, TurnDetails, Task> _startTurn;
    public Func<Buff, TurnDetails, Task> _endTurn;
    public Func<Buff, StageEntity, Task> _startRound;
    public Func<Buff, StageEntity, Task> _endRound;
    public Func<Buff, StepDetails, Task> _startStep;
    public Func<Buff, StepDetails, Task> _endStep;
    public Func<Buff, AttackDetails, Task> _attack;
    public Func<Buff, AttackDetails, Task> _attacked;
    public Func<Buff, DamageDetails, Task> _damage;
    public Func<Buff, DamageDetails, Task> _damaged;
    public Func<Buff, Task> _killed;
    public Func<Buff, Task> _kill;
    public Func<Buff, HealDetails, Task> _heal;
    public Func<Buff, HealDetails, Task> _healed;
    public Func<Buff, ArmorGainDetails, Task> _armorGain;
    public Func<Buff, ArmorGainDetails, Task> _armorGained;
    public Func<Buff, ArmorLoseDetails, Task> _armorLose;
    public Func<Buff, ArmorLoseDetails, Task> _armorLost;
    public Tuple<int, Func<Buff, BuffDetails, Task<BuffDetails>>> _buff;
    public Tuple<int, Func<Buff, BuffDetails, Task<BuffDetails>>> _anyBuff;
    public Tuple<int, Func<Buff, BuffDetails, Task<BuffDetails>>> _buffed;
    public Tuple<int, Func<Buff, BuffDetails, Task<BuffDetails>>> _anyBuffed;

    /// <summary>
    /// 定义一个Buff
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="description">描述</param>
    /// <param name="buffStackRule">堆叠规则</param>
    /// <param name="friendly">是否有益</param>
    /// <param name="dispellable">是否可驱散</param>
    /// <param name="gain">获得时的额外行为</param>
    /// <param name="stackChanged">层数改变时的额外行为</param>
    /// <param name="startStage">Stage开始时的额外行为</param>
    /// <param name="endStage">Stage结束时的额外行为</param>
    /// <param name="startTurn">Turn开始时的额外行为</param>
    /// <param name="endTurn">Turn结束时的额外行为</param>
    /// <param name="startRound">Round开始时的额外行为</param>
    /// <param name="endRound">Round结束时的额外行为</param>
    /// <param name="startStep">Step开始是的额外行为</param>
    /// <param name="endStep">Step结束时的额外行为</param>
    /// <param name="attack">攻击时的额外行为</param>
    /// <param name="attacked">受攻击时的额外行为</param>
    /// <param name="damage">伤害时的额外行为</param>
    /// <param name="damaged">受伤害时的额外行为</param>
    /// <param name="killed">被击杀时的额外行为</param>
    /// <param name="kill">击杀时的额外行为</param>
    /// <param name="heal">治疗时的额外行为</param>
    /// <param name="healed">被治疗时的额外行为</param>
    /// <param name="armorGain">给予护甲时的额外行为</param>
    /// <param name="armorGained">接受护甲时的额外行为</param>
    /// <param name="armorLose">使失去护甲时的额外行为</param>
    /// <param name="armorLost">失去护甲时的额外行为</param>
    /// <param name="buff">受到Buff时的额外行为，结算之前</param>
    /// <param name="buffed">受到Buff时的额外行为，结算之后</param>
    public BuffEntry(string name, string description, BuffStackRule buffStackRule, bool friendly, bool dispellable,
        Func<Buff, StageEntity, int, Task> gain = null,
        Func<Buff, StageEntity, Task> lose = null,
        Func<Buff, StageEntity, Task> stackChanged = null,
        Func<Buff, StageEntity, Task> startStage = null,
        Func<Buff, StageEntity, Task> endStage = null,
        Func<Buff, TurnDetails, Task> startTurn = null,
        Func<Buff, TurnDetails, Task> endTurn = null,
        Func<Buff, StageEntity, Task> startRound = null,
        Func<Buff, StageEntity, Task> endRound = null,
        Func<Buff, StepDetails, Task> startStep = null,
        Func<Buff, StepDetails, Task> endStep = null,
        Func<Buff, AttackDetails, Task> attack = null,
        Func<Buff, AttackDetails, Task> attacked = null,
        Func<Buff, DamageDetails, Task> damage = null,
        Func<Buff, DamageDetails, Task> damaged = null,
        Func<Buff, Task> killed = null,
        Func<Buff, Task> kill = null,
        Func<Buff, HealDetails, Task> heal = null,
        Func<Buff, HealDetails, Task> healed = null,
        Func<Buff, ArmorGainDetails, Task> armorGain = null,
        Func<Buff, ArmorGainDetails, Task> armorGained = null,
        Func<Buff, ArmorLoseDetails, Task> armorLose = null,
        Func<Buff, ArmorLoseDetails, Task> armorLost = null,
        Tuple<int, Func<Buff, BuffDetails, Task<BuffDetails>>> buff = null,
        Tuple<int, Func<Buff, BuffDetails, Task<BuffDetails>>> anyBuff = null,
        Tuple<int, Func<Buff, BuffDetails, Task<BuffDetails>>> buffed = null,
        Tuple<int, Func<Buff, BuffDetails, Task<BuffDetails>>> anyBuffed = null
    ) : base(name)
    {
        _description = description;
        // _sprite = Resources.Load<Sprite>($"Sprites/Buff/{Name}");
        _buffStackRule = buffStackRule;
        _friendly = friendly;
        _dispellable = dispellable;

        _gain = gain;
        _lose = lose;
        _stackChanged = stackChanged;

        _startStage = startStage;
        _endStage = endStage;
        _startTurn = startTurn;
        _endTurn = endTurn;
        _startRound = startRound;
        _endRound = endRound;
        _startStep = startStep;
        _endStep = endStep;

        _attack = attack;
        _attacked = attacked;
        _damage = damage;
        _damaged = damaged;
        _killed = killed;
        _kill = kill;
        _heal = heal;
        _healed = healed;
        _armorGain = armorGain;
        _armorGained = armorGained;
        _armorLose = armorLose;
        _armorLost = armorLost;

        _buff = buff;
        _anyBuff = anyBuff;
        _buffed = buffed;
        _anyBuffed = anyBuffed;
    }

    // public void ConfigureNote(StringBuilder sb)
    // {
    //     sb.Append($"<style=\"NoteName\">{Name}</style>\n");
    //     sb.Append($"<style=\"NoteSeparator\">__________</style>\n");
    //     sb.Append($"<style=\"NoteDescription\">{Description}</style>");
    // }

    public static implicit operator BuffEntry(string name) => Encyclopedia.BuffCategory[name];
}
