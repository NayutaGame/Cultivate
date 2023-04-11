
using System;
using CLLibrary;

/// <summary>
/// Buff
/// </summary>
public class Buff
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private BuffEntry _buffEntry;
    public BuffEntry BuffEntry => _buffEntry;

    public string GetName() => _buffEntry.Name;

    private int _stack;
    public int Stack
    {
        get => _stack;
        set
        {
            _stack = value;
            // _owner.OnBuffChanged();
            StackChangedEvent?.Invoke();
            if(_stack <= 0)
                _owner.RemoveBuff(this);
        }
    }

    // private ModifierLeaf _modifierLeaf;
    // public ModifierLeaf ModifierLeaf => _modifierLeaf;

    public event Action StackChangedEvent;


    /// <summary>
    /// 是否有益
    /// </summary>
    public bool Friendly => _buffEntry.Friendly;

    /// <summary>
    /// 是否可驱散
    /// </summary>
    public bool Dispellable => _buffEntry.Dispellable;

    public Buff(StageEntity owner, string name, int stack = 1)
        : this(owner, Encyclopedia.BuffCategory[name], stack) { }
    public Buff(StageEntity owner, BuffEntry buffEntry, int stack = 1)
    {
        _owner = owner;
        _buffEntry = buffEntry;
        _stack = stack;
    }

    public void Register()
    {
        // if (_buffEntry.ModifierLeaf != null) CreateModifierLeaf(_buffEntry.ModifierLeaf);
        if (_buffEntry._stackChanged != null) StackChangedEvent += StackChanged;
        if (_buffEntry._startStage != null) _owner.StartStageEvent += StartStage;
        if (_buffEntry._endStage != null) _owner.EndStageEvent += EndStage;
        if (_buffEntry._startTurn != null) _owner.StartTurnEvent += StartTurn;
        if (_buffEntry._endTurn != null) _owner.EndTurnEvent += EndTurn;
        if (_buffEntry._startRound != null) _owner.StartRoundEvent += StartRound;
        if (_buffEntry._endRound != null) _owner.EndRoundEvent += EndRound;
        if (_buffEntry._startStep != null) _owner.StartStepEvent += StartStep;
        if (_buffEntry._endStep != null) _owner.EndStepEvent += EndStep;
        if (_buffEntry._attack != null) _owner.AttackEvent += Attack;
        if (_buffEntry._attacked != null) _owner.AttackedEvent += Attacked;
        if (_buffEntry._damage != null) _owner.DamageEvent += Damage;
        if (_buffEntry._damaged != null) _owner.DamagedEvent += Damaged;
        if (_buffEntry._killed != null) _owner.KilledEvent += Killed;
        if (_buffEntry._kill != null) _owner.KillEvent += Kill;
        if (_buffEntry._heal != null) _owner.HealEvent += Heal;
        if (_buffEntry._healed != null) _owner.HealedEvent += Healed;
        if (_buffEntry._armor != null) _owner.ArmorEvent += _Armor;
        if (_buffEntry._armored != null) _owner.ArmoredEvent += Armored;
        // if (_buffEntry._laststand != null) _owner.LaststandEvent += Laststand;
        // if (_buffEntry._evade != null) _owner.EvadeEvent += Evade;
        // if (_buffEntry._clean != null) _owner.CleanEvent += Clean;

        if (_buffEntry._buff      != null) _owner.Buff.Add            (_buffEntry._buff.Item1,      _Buff);
        if (_buffEntry._buffed    != null) _owner.Buffed.Add          (_buffEntry._buffed.Item1,    Buffed);

        StackChangedEvent?.Invoke();
    }

    public void Unregister()
    {
        // if (_modifierLeaf != null) DestroyModifierLeaf();
        if (_buffEntry._stackChanged != null) StackChangedEvent -= StackChanged;
        if (_buffEntry._startStage != null) _owner.StartStageEvent -= StartStage;
        if (_buffEntry._endStage != null) _owner.EndStageEvent -= EndStage;
        if (_buffEntry._startTurn != null) _owner.StartTurnEvent -= StartTurn;
        if (_buffEntry._endTurn != null) _owner.EndTurnEvent -= EndTurn;
        if (_buffEntry._startRound != null) _owner.StartRoundEvent -= StartRound;
        if (_buffEntry._endRound != null) _owner.EndRoundEvent -= EndRound;
        if (_buffEntry._startStep != null) _owner.StartStepEvent -= StartStep;
        if (_buffEntry._endStep != null) _owner.EndStepEvent -= EndStep;
        if (_buffEntry._attack != null) _owner.AttackEvent -= Attack;
        if (_buffEntry._attacked != null) _owner.AttackedEvent -= Attacked;
        if (_buffEntry._damage != null) _owner.DamageEvent -= Damage;
        if (_buffEntry._damaged != null) _owner.DamagedEvent -= Damaged;
        if (_buffEntry._killed != null) _owner.KilledEvent -= Killed;
        if (_buffEntry._kill != null) _owner.KillEvent -= Kill;
        if (_buffEntry._heal != null) _owner.HealEvent -= Heal;
        if (_buffEntry._healed != null) _owner.HealedEvent -= Healed;
        if (_buffEntry._armor != null) _owner.ArmorEvent -= _Armor;
        if (_buffEntry._armored != null) _owner.ArmoredEvent -= Armored;

        // if (_buffEntry._laststand != null) _owner.LaststandEvent -= Laststand;
        // if (_buffEntry._evade != null) _owner.EvadeEvent -= Evade;
        // if (_buffEntry._clean != null) _owner.CleanEvent -= Clean;

        if (_buffEntry._buff      != null) _owner.Buff.Remove            (_Buff);
        if (_buffEntry._buffed    != null) _owner.Buffed.Remove          (Buffed);
    }

    // public void ForceSetModifierLeaf(string key, float value)
    // {
    //     if (ModifierLeaf == null) CreateModifierLeaf(new ModifierLeaf());
    //     ModifierLeaf.ForceSet(key, value);
    // }
    //
    // public void CreateModifierLeaf(ModifierLeaf modifierLeaf)
    // {
    //     _modifierLeaf = modifierLeaf.Clone();
    //     _modifierLeaf.Changed += _owner.Modifier.SetDirty;
    //     _modifierLeaf.Changed += _owner.OnStatsChanged;
    //     _owner.Modifier.AddLeaf(_modifierLeaf);
    // }
    //
    // public void DestroyModifierLeaf()
    // {
    //     _owner.Modifier.RemoveLeaf(_modifierLeaf);
    //     _modifierLeaf.Changed -= _owner.Modifier.SetDirty;
    //     _modifierLeaf.Changed -= _owner.OnStatsChanged;
    //     _modifierLeaf = null;
    // }

    public void Gain(int gain) => _buffEntry._gain?.Invoke(this, _owner, gain);
    public void Lose() => _buffEntry._lose?.Invoke(this, _owner);
    private void StackChanged() => _buffEntry._stackChanged(this, _owner);

    private void StartStage() => _buffEntry._startStage(this, _owner);
    private void EndStage() => _buffEntry._endStage(this, _owner);
    private void StartTurn(TurnDetails d) => _buffEntry._startTurn(this, d);
    private void EndTurn(TurnDetails d) => _buffEntry._endTurn(this, d);
    private void StartRound() => _buffEntry._startRound(this, _owner);
    private void EndRound() => _buffEntry._endRound(this, _owner);
    private void StartStep(StepDetails d) => _buffEntry._startStep(this, d);
    private void EndStep(StepDetails d) => _buffEntry._endStep(this, d);

    private void Attack(AttackDetails d) => _buffEntry._attack(this, d);
    private void Attacked(AttackDetails d) => _buffEntry._attacked(this, d);
    private void Damage(DamageDetails d) => _buffEntry._damage(this, d);
    private void Damaged(DamageDetails d) => _buffEntry._damaged(this, d);
    private void Killed(AttackDetails d) => _buffEntry._killed(this, d);
    private void Kill(AttackDetails d) => _buffEntry._kill(this, d);
    private void Heal(HealDetails d) => _buffEntry._heal(this, d);
    private void Healed(HealDetails d) => _buffEntry._healed(this, d);
    private void _Armor(ArmorDetails d) => _buffEntry._armor(this, d);
    private void Armored(ArmorDetails d) => _buffEntry._armored(this, d);
    // private void Laststand(DamageDetails d) => _buffEntry._laststand(this, d);
    // private void Evade(AttackDetails d) => _buffEntry._evade(this, d);
    // private void Clean(int stack) => _buffEntry._clean(this, stack);

    private BuffDetails _Buff(BuffDetails d) => _buffEntry._buff.Item2(this, d);
    private BuffDetails AnyBuff(BuffDetails d) => _buffEntry._anyBuff.Item2(this, d);
    private BuffDetails Buffed(BuffDetails d) => _buffEntry._buffed.Item2(this, d);
    private BuffDetails AnyBuffed(BuffDetails d) => _buffEntry._anyBuffed.Item2(this, d);
}
