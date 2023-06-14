
using System;
using System.Threading.Tasks;
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
        if (_buffEntry._heal != null) _owner.HealEvent += Heal;
        if (_buffEntry._healed != null) _owner.HealedEvent += Healed;
        if (_buffEntry._armorGain != null) _owner.ArmorGainEvent += ArmorGain;
        if (_buffEntry._armorGained != null) _owner.ArmorGainedEvent += ArmorGained;
        if (_buffEntry._armorLose != null) _owner.ArmorLoseEvent += ArmorLose;
        if (_buffEntry._armorLost != null) _owner.ArmorLostEvent += ArmorLost;
        if (_buffEntry._evaded != null) _owner.EvadedEvent += Evaded;
        if (_buffEntry._buff      != null) _owner.Buff.Add            (_buffEntry._buff.Item1,      _Buff);
        if (_buffEntry._buffed    != null) _owner.Buffed.Add          (_buffEntry._buffed.Item1,    Buffed);
        if (_buffEntry._consumed != null) _owner.ConsumedEvent += Consumed;

        StackChangedEvent?.Invoke();
    }

    public void Unregister()
    {
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
        if (_buffEntry._armorGain != null) _owner.ArmorGainEvent -= ArmorGain;
        if (_buffEntry._armorGained != null) _owner.ArmorGainedEvent -= ArmorGained;
        if (_buffEntry._armorLose != null) _owner.ArmorLoseEvent -= ArmorLose;
        if (_buffEntry._armorLost != null) _owner.ArmorLostEvent -= ArmorLost;
        if (_buffEntry._evaded != null) _owner.EvadedEvent -= Evaded;
        if (_buffEntry._buff      != null) _owner.Buff.Remove            (_Buff);
        if (_buffEntry._buffed    != null) _owner.Buffed.Remove          (Buffed);
        if (_buffEntry._consumed != null) _owner.ConsumedEvent -= Consumed;
    }

    public void Gain(int gain) => _buffEntry._gain?.Invoke(this, _owner, gain);
    public void Lose() => _buffEntry._lose?.Invoke(this, _owner);
    private void StackChanged() => _buffEntry._stackChanged(this, _owner);

    private async Task StartStage          () =>                   await _buffEntry._startStage   (this, _owner);
    private async Task EndStage            () =>                   await _buffEntry._endStage     (this, _owner);
    private async Task StartTurn           (TurnDetails d) =>      await _buffEntry._startTurn    (this, d);
    private async Task EndTurn             (TurnDetails d) =>      await _buffEntry._endTurn      (this, d);
    private async Task StartRound          () =>                   await _buffEntry._startRound   (this, _owner);
    private async Task EndRound            () =>                   await _buffEntry._endRound     (this, _owner);
    private async Task StartStep           (StepDetails d) =>      await _buffEntry._startStep    (this, d);
    private async Task EndStep             (StepDetails d) =>      await _buffEntry._endStep      (this, d);
    private async Task Attack              (AttackDetails d) =>    await _buffEntry._attack       (this, d);
    private async Task Attacked            (AttackDetails d) =>    await _buffEntry._attacked     (this, d);
    private async Task Damage              (DamageDetails d) =>    await _buffEntry._damage       (this, d);
    private async Task Damaged             (DamageDetails d) =>    await _buffEntry._damaged      (this, d);
    private async Task Killed              () =>                   await _buffEntry._killed       (this);
    private async Task Kill                () =>                   await _buffEntry._kill         (this);
    private async Task Heal                (HealDetails d) =>      await _buffEntry._heal         (this, d);
    private async Task Healed              (HealDetails d) =>      await _buffEntry._healed       (this, d);
    private async Task ArmorGain           (ArmorGainDetails d) => await _buffEntry._armorGain    (this, d);
    private async Task ArmorGained         (ArmorGainDetails d) => await _buffEntry._armorGained  (this, d);
    private async Task ArmorLose           (ArmorLoseDetails d) => await _buffEntry._armorLose    (this, d);
    private async Task ArmorLost           (ArmorLoseDetails d) => await _buffEntry._armorLost    (this, d);
    private async Task Evaded              (EvadeDetails d) =>     await _buffEntry._evaded       (this, d);
    private async Task<BuffDetails> _Buff  (BuffDetails d) =>      await _buffEntry._buff.Item2   (this, d);
    private async Task<BuffDetails> Buffed (BuffDetails d) =>      await _buffEntry._buffed.Item2 (this, d);
    private async Task Consumed            (ConsumeDetails d) =>   await _buffEntry._consumed     (this, d);
}
