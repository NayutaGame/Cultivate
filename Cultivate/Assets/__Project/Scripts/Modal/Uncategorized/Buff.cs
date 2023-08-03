
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CLLibrary;

/// <summary>
/// Buff
/// </summary>
public class Buff : StageEventListener
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

    private Dictionary<string, Func<StageEventDetails, Task>> _eventPropagatorDict;

    public Buff(StageEntity owner, BuffEntry entry, int stack = 1)
    {
        _owner = owner;
        _entry = entry;
        _stack = stack;

        _eventPropagatorDict = new();
    }

    public void Register()
    {
        if (_entry._stackChanged != null) StackChangedEvent += StackChanged;
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

        StackChangedEvent?.Invoke();
    }

    public void Unregister()
    {
        if (_entry._stackChanged != null) StackChangedEvent -= StackChanged;
        if (_entry._buff      != null) _owner.Buff.Remove            (_Buff);
        if (_entry._buffed    != null) _owner.Buffed.Remove          (Buffed);

        foreach (string eventId in _entry._eventCaptureDict.Keys)
            _owner.Env._stageEventTriggerDict[eventId] -= _eventPropagatorDict[eventId];
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

    private async Task<BuffDetails> _Buff  (BuffDetails d) =>      await _entry._buff.Item2    (this, d);
    private async Task<BuffDetails> Buffed (BuffDetails d) =>      await _entry._buffed.Item2  (this, d);
}
