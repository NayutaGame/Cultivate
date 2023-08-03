
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLLibrary;

/// <summary>
/// Formation
/// </summary>
public class Formation : StageEventListener
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private FormationEntry _entry;
    public FormationEntry Entry => _entry;

    public string GetName() => _entry.GetName();

    private Dictionary<string, Func<StageEventDetails, Task>> _eventPropagatorDict;

    public Formation(StageEntity owner, FormationEntry entry)
    {
        _owner = owner;
        _entry = entry;

        _eventPropagatorDict = new();
    }

    public void Register()
    {
        if (_entry._anyFormationAdd != null) _owner.Env.AnyFormationAddEvent += AnyFormationAdd;
        if (_entry._anyFormationAdded != null) _owner.Env.AnyFormationAddedEvent += AnyFormationAdded;
        if (_entry._startStage != null) _owner.StartStageEvent += StartStage;
        if (_entry._endStage != null) _owner.EndStageEvent += EndStage;
        if (_entry._startTurn != null) _owner.StartTurnEvent += StartTurn;
        if (_entry._endTurn != null) _owner.EndTurnEvent += EndTurn;
        if (_entry._buff      != null) _owner.Buff.Add            (_entry._buff.Item1,      _Buff);
        if (_entry._buffed    != null) _owner.Buffed.Add          (_entry._buffed.Item1,    Buffed);

        foreach (string eventId in _entry._eventCaptureDict.Keys)
        {
            _eventPropagatorDict[eventId] = d => _entry._eventCaptureDict[eventId].Invoke(this, d);
            if (_owner.Env._stageEventTriggerDict.ContainsKey(eventId))
            {
                _owner.Env._stageEventTriggerDict[eventId] += _eventPropagatorDict[eventId];
            }
            else
            {
                _owner.Env._stageEventTriggerDict[eventId] = _eventPropagatorDict[eventId];
            }
        }
    }

    public void Unregister()
    {
        if (_entry._anyFormationAdd != null) _owner.Env.AnyFormationAddEvent -= AnyFormationAdd;
        if (_entry._anyFormationAdded != null) _owner.Env.AnyFormationAddedEvent -= AnyFormationAdded;
        if (_entry._startStage != null) _owner.StartStageEvent -= StartStage;
        if (_entry._endStage != null) _owner.EndStageEvent -= EndStage;
        if (_entry._startTurn != null) _owner.StartTurnEvent -= StartTurn;
        if (_entry._endTurn != null) _owner.EndTurnEvent -= EndTurn;
        if (_entry._buff      != null) _owner.Buff.Remove            (_Buff);
        if (_entry._buffed    != null) _owner.Buffed.Remove          (Buffed);

        foreach (string eventId in _entry._eventCaptureDict.Keys)
            _owner.Env._stageEventTriggerDict[eventId] -= _eventPropagatorDict[eventId];
    }

    public async Task Gain()
    {
        if (_entry._gain != null)
            await _entry._gain.Invoke(this, _owner);
    }

    public async Task Lose()
    {
        if (_entry._lose != null)
            await _entry._lose.Invoke(this, _owner);
    }

    private async Task<FormationDetails> AnyFormationAdd   (FormationDetails d) => await _entry._anyFormationAdd   (this, _owner, d);
    private async Task<FormationDetails> AnyFormationAdded (FormationDetails d) => await _entry._anyFormationAdded (this, _owner, d);
    private async Task StartStage                          () =>                   await _entry._startStage        (this, _owner);
    private async Task EndStage                            () =>                   await _entry._endStage          (this, _owner);
    private async Task StartTurn                           (TurnDetails d) =>      await _entry._startTurn         (this, d);
    private async Task EndTurn                             (TurnDetails d) =>      await _entry._endTurn           (this, d);
    private async Task<BuffDetails> _Buff                  (BuffDetails d) =>      await _entry._buff.Item2        (this, d);
    private async Task<BuffDetails> Buffed                 (BuffDetails d) =>      await _entry._buffed.Item2      (this, d);
}
