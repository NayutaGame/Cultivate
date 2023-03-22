using System;
using System.Collections;
using System.Collections.Generic;
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

    // private ModifierLeaf _modifierLeaf;
    // public ModifierLeaf ModifierLeaf => _modifierLeaf;

    public Action<Buff, StageEntity, int> _gain;
    public Action<Buff, StageEntity> _lose;
    public Action<Buff, StageEntity> _stackChanged;
    public Action<Buff, StageEntity> _startStage;
    public Action<Buff, StageEntity> _endStage;
    public Action<Buff, StageEntity> _startTurn;
    public Action<Buff, StageEntity> _endTurn;
    public Action<Buff, StageEntity> _startRound;
    public Action<Buff, StageEntity> _endRound;
    public Action<Buff, StageEntity> _startStep;
    public Action<Buff, EndStepDetails> _endStep;
    public Action<Buff, AttackDetails> _attack;
    public Action<Buff, AttackDetails> _attacked;
    public Action<Buff, DamageDetails> _damage;
    public Action<Buff, DamageDetails> _damaged;
    public Action<Buff, AttackDetails> _killed;
    public Action<Buff, AttackDetails> _kill;
    public Action<Buff, HealDetails> _heal;
    public Action<Buff, HealDetails> _healed;
    public Action<Buff, ArmorDetails> _armor;
    public Action<Buff, ArmorDetails> _armored;
    // public Action<Buff, DamageDetails> _laststand;
    // public Action<Buff, AttackDetails> _evade;
    // public Action<StageReport, Buff, int> _clean;
    public Tuple<int, Func<Buff, BuffDetails, BuffDetails>> _buff;
    public Tuple<int, Func<Buff, BuffDetails, BuffDetails>> _anyBuff;
    public Tuple<int, Func<Buff, BuffDetails, BuffDetails>> _buffed;
    public Tuple<int, Func<Buff, BuffDetails, BuffDetails>> _anyBuffed;

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
    /// <param name="armor">给予护甲时的额外行为</param>
    /// <param name="armored">接受护甲时的额外行为</param>
    /// <param name="buff">受到Buff时的额外行为，结算之前</param>
    /// <param name="buffed">受到Buff时的额外行为，结算之后</param>
    public BuffEntry(string name, string description, BuffStackRule buffStackRule, bool friendly, bool dispellable,
        // ModifierLeaf modifierLeaf = null,
        Action<Buff, StageEntity, int> gain = null,
        Action<Buff, StageEntity> lose = null,
        Action<Buff, StageEntity> stackChanged = null,
        Action<Buff, StageEntity> startStage = null,
        Action<Buff, StageEntity> endStage = null,
        Action<Buff, StageEntity> startTurn = null,
        Action<Buff, StageEntity> endTurn = null,
        Action<Buff, StageEntity> startRound = null,
        Action<Buff, StageEntity> endRound = null,
        Action<Buff, StageEntity> startStep = null,
        Action<Buff, EndStepDetails> endStep = null,
        Action<Buff, AttackDetails> attack = null,
        Action<Buff, AttackDetails> attacked = null,
        Action<Buff, DamageDetails> damage = null,
        Action<Buff, DamageDetails> damaged = null,
        Action<Buff, AttackDetails> killed = null,
        Action<Buff, AttackDetails> kill = null,
        Action<Buff, HealDetails> heal = null,
        Action<Buff, HealDetails> healed = null,
        Action<Buff, ArmorDetails> armor = null,
        Action<Buff, ArmorDetails> armored = null,
        // Action<Buff, DamageDetails> laststand = null,
        // Action<Buff, AttackDetails> evade = null,
        // Action<StageReport, Buff, int> clean = null,
        Tuple<int, Func<Buff, BuffDetails, BuffDetails>> buff = null,
        Tuple<int, Func<Buff, BuffDetails, BuffDetails>> anyBuff = null,
        Tuple<int, Func<Buff, BuffDetails, BuffDetails>> buffed = null,
        Tuple<int, Func<Buff, BuffDetails, BuffDetails>> anyBuffed = null
    ) : base(name)
    {
        _description = description;
        // _sprite = Resources.Load<Sprite>($"Sprites/Buff/{Name}");
        _buffStackRule = buffStackRule;
        _friendly = friendly;
        _dispellable = dispellable;

        // _modifierLeaf = modifierLeaf;

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
        _armor = armor;
        _armored = armored;
        // _laststand = laststand;
        // _evade = evade;
        // _clean = clean;

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
}
