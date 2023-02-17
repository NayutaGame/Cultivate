
using System;
using CLLibrary;

public class Buff
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private BuffEntry _buffEntry;
    public BuffEntry BuffEntry => _buffEntry;

    private int _stack;
    public int Stack
    {
        get => _stack;
        set
        {
            _stack = value;
            // _owner.OnBuffChanged();
            StackChangedEvent?.Invoke();
            if(_stack <= 0) _owner.RemoveBuff(this);
        }
    }

    // private ModifierLeaf _modifierLeaf;
    // public ModifierLeaf ModifierLeaf => _modifierLeaf;

    public event Action StackChangedEvent;

    public bool Friendly => _buffEntry.Friendly;

    public Buff(StageEntity owner, string name, int stack = 1)
        : this(owner, Encyclopedia.BuffCategory.Find(name), stack) { }
    public Buff(StageEntity owner, BuffEntry buffEntry, int stack = 1)
    {
        _owner = owner;
        _buffEntry = buffEntry;
        _stack = stack;
    }

    public void Register()
    {
        // if (_buffEntry.ModifierLeaf != null) CreateModifierLeaf(_buffEntry.ModifierLeaf);
        if (_buffEntry._startTurn != null) _owner.StartTurnEvent += StartTurn;
        if (_buffEntry._endTurn != null) _owner.EndTurnEvent += EndTurn;
        if (_buffEntry._attack != null) _owner.AttackEvent += Attack;
        if (_buffEntry._attacked != null) _owner.AttackedEvent += Attacked;
        if (_buffEntry._damage != null) _owner.DamageEvent += Damage;
        if (_buffEntry._damaged != null) _owner.DamagedEvent += Damaged;
        if (_buffEntry._killed != null) _owner.KilledEvent += Killed;
        if (_buffEntry._kill != null) _owner.KillEvent += Kill;
        // if (_buffEntry._heal != null) _owner.HealEvent += Heal;
        // if (_buffEntry._healed != null) _owner.HealedEvent += Healed;
        // if (_buffEntry._laststand != null) _owner.LaststandEvent += Laststand;
        // if (_buffEntry._evade != null) _owner.EvadeEvent += Evade;
        // if (_buffEntry._clean != null) _owner.CleanEvent += Clean;
        if (_buffEntry._stackChanged != null) StackChangedEvent += StackChanged;

        if (_buffEntry._buff      != null) _owner.Buff.Add            (_buffEntry._buff.Item1,      _Buff);
        if (_buffEntry._buffed    != null) _owner.Buffed.Add          (_buffEntry._buffed.Item1,    Buffed);

        StackChangedEvent?.Invoke();
    }

    public void Unregister()
    {
        // if (_modifierLeaf != null) DestroyModifierLeaf();
        if (_buffEntry._startTurn != null) _owner.StartTurnEvent -= StartTurn;
        if (_buffEntry._endTurn != null) _owner.EndTurnEvent -= EndTurn;
        if (_buffEntry._attack != null) _owner.AttackEvent -= Attack;
        if (_buffEntry._attacked != null) _owner.AttackedEvent -= Attacked;
        if (_buffEntry._damage != null) _owner.DamageEvent -= Damage;
        if (_buffEntry._damaged != null) _owner.DamagedEvent -= Damaged;
        if (_buffEntry._killed != null) _owner.KilledEvent -= Killed;
        if (_buffEntry._kill != null) _owner.KillEvent -= Kill;
        // if (_buffEntry._heal != null) _owner.HealEvent -= Heal;
        // if (_buffEntry._healed != null) _owner.HealedEvent -= Healed;
        // if (_buffEntry._laststand != null) _owner.LaststandEvent -= Laststand;
        // if (_buffEntry._evade != null) _owner.EvadeEvent -= Evade;
        // if (_buffEntry._clean != null) _owner.CleanEvent -= Clean;
        if (_buffEntry._stackChanged != null) StackChangedEvent -= StackChanged;

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

    public void Gain(int stack)
    {
        _buffEntry._gain?.Invoke(this, _owner, stack);
    }

    public void Lose()
    {
        _buffEntry._lose?.Invoke(this, _owner);
    }

    public void StackChanged()
    {
        _buffEntry._stackChanged(this, _owner);
    }

    public void StartTurn()
    {
        _buffEntry._startTurn(this, _owner);
    }

    public void EndTurn()
    {
        _buffEntry._endTurn(this, _owner);
    }

    public void Attack(AttackDetails d) => _buffEntry._attack(this, d);
    public void Attacked(AttackDetails d) => _buffEntry._attacked(this, d);
    public void Damage(DamageDetails d) => _buffEntry._damage(this, d);
    public void Damaged(DamageDetails d) => _buffEntry._damaged(this, d);
    public void Killed(AttackDetails d) => _buffEntry._killed(this, d);
    public void Kill(AttackDetails d) => _buffEntry._kill(this, d);
    // public void Heal(HealDetails d) => _buffEntry._heal(this, d);
    // public void Healed(HealDetails d) => _buffEntry._healed(this, d);
    // public void Laststand(DamageDetails d) => _buffEntry._laststand(this, d);
    // public void Evade(AttackDetails d) => _buffEntry._evade(this, d);
    // public void Clean(int stack) => _buffEntry._clean(this, stack);

    public BuffDetails _Buff(BuffDetails d) => _buffEntry._buff.Item2(this, d);
    public BuffDetails AnyBuff(BuffDetails d) => _buffEntry._anyBuff.Item2(this, d);
    public BuffDetails Buffed(BuffDetails d) => _buffEntry._buffed.Item2(this, d);
    public BuffDetails AnyBuffed(BuffDetails d) => _buffEntry._anyBuffed.Item2(this, d);
}
