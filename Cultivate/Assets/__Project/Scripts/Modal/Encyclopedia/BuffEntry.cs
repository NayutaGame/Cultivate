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

    // private ModifierLeaf _modifierLeaf;
    // public ModifierLeaf ModifierLeaf => _modifierLeaf;

    public Action<Buff, StageEntity, int> _gain;
    public Action<Buff, StageEntity> _lose;
    public Action<Buff, StageEntity> _stackChanged;
    public Action<Buff, TurnDetails> _startTurn;
    public Action<Buff, StageEntity> _endTurn;
    public Action<Buff, AttackDetails> _attack;
    public Action<Buff, AttackDetails> _attacked;
    public Action<Buff, DamageDetails> _damage;
    public Action<Buff, DamageDetails> _damaged;
    public Action<Buff, AttackDetails> _killed;
    public Action<Buff, AttackDetails> _kill;
    // public Action<Buff, HealDetails> _heal;
    // public Action<Buff, HealDetails> _healed;
    public Action<Buff, ArmorDetails> _armor;
    public Action<Buff, ArmorDetails> _armored;
    public Action<Buff, DamageDetails> _laststand;
    public Action<Buff, AttackDetails> _evade;
    public Action<Buff, int> _clean;
    public Tuple<int, Func<Buff, BuffDetails, BuffDetails>> _buff;
    public Tuple<int, Func<Buff, BuffDetails, BuffDetails>> _anyBuff;
    public Tuple<int, Func<Buff, BuffDetails, BuffDetails>> _buffed;
    public Tuple<int, Func<Buff, BuffDetails, BuffDetails>> _anyBuffed;

    public BuffEntry(string name, string description, BuffStackRule buffStackRule, bool friendly,
        // ModifierLeaf modifierLeaf = null,
        Action<Buff, StageEntity, int> gain = null,
        Action<Buff, StageEntity> lose = null,
        Action<Buff, StageEntity> stackChanged = null,
        Action<Buff, TurnDetails> startTurn = null,
        Action<Buff, StageEntity> endTurn = null,
        Action<Buff, AttackDetails> attack = null,
        Action<Buff, AttackDetails> attacked = null,
        Action<Buff, DamageDetails> damage = null,
        Action<Buff, DamageDetails> damaged = null,
        Action<Buff, AttackDetails> killed = null,
        Action<Buff, AttackDetails> kill = null,
        // Action<Buff, HealDetails> heal = null,
        // Action<Buff, HealDetails> healed = null,
        Action<Buff, ArmorDetails> armor = null,
        Action<Buff, ArmorDetails> armored = null,
        Action<Buff, DamageDetails> laststand = null,
        Action<Buff, AttackDetails> evade = null,
        Action<Buff, int> clean = null,
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

        // _modifierLeaf = modifierLeaf;

        _gain = gain;
        _lose = lose;
        _stackChanged = stackChanged;
        _startTurn = startTurn;
        _endTurn = endTurn;
        _attack = attack;
        _attacked = attacked;
        _damage = damage;
        _damaged = damaged;
        _killed = killed;
        _kill = kill;
        // _heal = heal;
        // _healed = healed;
        _armor = armor;
        _armored = armored;
        _laststand = laststand;
        _evade = evade;
        _clean = clean;

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
