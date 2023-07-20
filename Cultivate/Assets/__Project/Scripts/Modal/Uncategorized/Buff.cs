
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

    private BuffEntry _entry;
    public BuffEntry Entry => _entry;

    public string GetName() => _entry.Name;

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
    public bool Friendly => _entry.Friendly;

    /// <summary>
    /// 是否可驱散
    /// </summary>
    public bool Dispellable => _entry.Dispellable;

    public Buff(StageEntity owner, BuffEntry entry, int stack = 1)
    {
        _owner = owner;
        _entry = entry;
        _stack = stack;
    }

    public void Register()
    {
        if (_entry._stackChanged != null) StackChangedEvent += StackChanged;
        if (_entry._startStage != null) _owner.StartStageEvent += StartStage;
        if (_entry._endStage != null) _owner.EndStageEvent += EndStage;
        if (_entry._startTurn != null) _owner.StartTurnEvent += StartTurn;
        if (_entry._endTurn != null) _owner.EndTurnEvent += EndTurn;
        if (_entry._startRound != null) _owner.StartRoundEvent += StartRound;
        if (_entry._endRound != null) _owner.EndRoundEvent += EndRound;
        if (_entry._startStep != null) _owner.StartStepEvent += StartStep;
        if (_entry._endStep != null) _owner.EndStepEvent += EndStep;
        if (_entry._attack != null) _owner.AttackEvent += Attack;
        if (_entry._attacked != null) _owner.AttackedEvent += Attacked;
        if (_entry._damage != null) _owner.DamageEvent += Damage;
        if (_entry._damaged != null) _owner.DamagedEvent += Damaged;
        if (_entry._heal != null) _owner.HealEvent += Heal;
        if (_entry._healed != null) _owner.HealedEvent += Healed;
        if (_entry._armorGain != null) _owner.ArmorGainEvent += ArmorGain;
        if (_entry._armorGained != null) _owner.ArmorGainedEvent += ArmorGained;
        if (_entry._armorLose != null) _owner.ArmorLoseEvent += ArmorLose;
        if (_entry._armorLost != null) _owner.ArmorLostEvent += ArmorLost;
        if (_entry._evaded != null) _owner.EvadedEvent += Evaded;
        if (_entry._buff      != null) _owner.Buff.Add            (_entry._buff.Item1,      _Buff);
        if (_entry._buffed    != null) _owner.Buffed.Add          (_entry._buffed.Item1,    Buffed);
        if (_entry._consumed != null) _owner.ConsumedEvent += Consumed;

        StackChangedEvent?.Invoke();
    }

    public void Unregister()
    {
        if (_entry._stackChanged != null) StackChangedEvent -= StackChanged;
        if (_entry._startStage != null) _owner.StartStageEvent -= StartStage;
        if (_entry._endStage != null) _owner.EndStageEvent -= EndStage;
        if (_entry._startTurn != null) _owner.StartTurnEvent -= StartTurn;
        if (_entry._endTurn != null) _owner.EndTurnEvent -= EndTurn;
        if (_entry._startRound != null) _owner.StartRoundEvent -= StartRound;
        if (_entry._endRound != null) _owner.EndRoundEvent -= EndRound;
        if (_entry._startStep != null) _owner.StartStepEvent -= StartStep;
        if (_entry._endStep != null) _owner.EndStepEvent -= EndStep;
        if (_entry._attack != null) _owner.AttackEvent -= Attack;
        if (_entry._attacked != null) _owner.AttackedEvent -= Attacked;
        if (_entry._damage != null) _owner.DamageEvent -= Damage;
        if (_entry._damaged != null) _owner.DamagedEvent -= Damaged;
        if (_entry._killed != null) _owner.KilledEvent -= Killed;
        if (_entry._kill != null) _owner.KillEvent -= Kill;
        if (_entry._heal != null) _owner.HealEvent -= Heal;
        if (_entry._healed != null) _owner.HealedEvent -= Healed;
        if (_entry._armorGain != null) _owner.ArmorGainEvent -= ArmorGain;
        if (_entry._armorGained != null) _owner.ArmorGainedEvent -= ArmorGained;
        if (_entry._armorLose != null) _owner.ArmorLoseEvent -= ArmorLose;
        if (_entry._armorLost != null) _owner.ArmorLostEvent -= ArmorLost;
        if (_entry._evaded != null) _owner.EvadedEvent -= Evaded;
        if (_entry._buff      != null) _owner.Buff.Remove            (_Buff);
        if (_entry._buffed    != null) _owner.Buffed.Remove          (Buffed);
        if (_entry._consumed != null) _owner.ConsumedEvent -= Consumed;
    }

    public async Task Gain(int gain)
    {
        if (_entry._gain != null) await _entry._gain.Invoke(this, _owner, gain);
    }

    public async Task Lose()
    {
        if (_entry._lose != null) await _entry._lose.Invoke(this, _owner);
    }

    private void StackChanged()
    {
        if (_entry._stackChanged != null) _entry._stackChanged(this, _owner);
    }

    private async Task StartStage          () =>                   await _entry._startStage   (this, _owner);
    private async Task EndStage            () =>                   await _entry._endStage     (this, _owner);
    private async Task StartTurn           (TurnDetails d) =>      await _entry._startTurn    (this, d);
    private async Task EndTurn             (TurnDetails d) =>      await _entry._endTurn      (this, d);
    private async Task StartRound          () =>                   await _entry._startRound   (this, _owner);
    private async Task EndRound            () =>                   await _entry._endRound     (this, _owner);
    private async Task StartStep           (StepDetails d) =>      await _entry._startStep    (this, d);
    private async Task EndStep             (StepDetails d) =>      await _entry._endStep      (this, d);
    private async Task Attack              (AttackDetails d) =>    await _entry._attack       (this, d);
    private async Task Attacked            (AttackDetails d) =>    await _entry._attacked     (this, d);
    private async Task Damage              (DamageDetails d) =>    await _entry._damage       (this, d);
    private async Task Damaged             (DamageDetails d) =>    await _entry._damaged      (this, d);
    private async Task Killed              () =>                   await _entry._killed       (this);
    private async Task Kill                () =>                   await _entry._kill         (this);
    private async Task Heal                (HealDetails d) =>      await _entry._heal         (this, d);
    private async Task Healed              (HealDetails d) =>      await _entry._healed       (this, d);
    private async Task ArmorGain           (ArmorGainDetails d) => await _entry._armorGain    (this, d);
    private async Task ArmorGained         (ArmorGainDetails d) => await _entry._armorGained  (this, d);
    private async Task ArmorLose           (ArmorLoseDetails d) => await _entry._armorLose    (this, d);
    private async Task ArmorLost           (ArmorLoseDetails d) => await _entry._armorLost    (this, d);
    private async Task Evaded              (EvadeDetails d) =>     await _entry._evaded       (this, d);
    private async Task<BuffDetails> _Buff  (BuffDetails d) =>      await _entry._buff.Item2   (this, d);
    private async Task<BuffDetails> Buffed (BuffDetails d) =>      await _entry._buffed.Item2 (this, d);
    private async Task Consumed            (ConsumeDetails d) =>   await _entry._consumed     (this, d);
}
