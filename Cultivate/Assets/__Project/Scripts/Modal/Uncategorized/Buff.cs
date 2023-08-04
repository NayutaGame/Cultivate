
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    public int Stack => _stack;
    public async Task SetStack(int stack)
    {
        await _eventDict.FireEvent("StackWillChange", new BuffStackChangeDetails(_stack, stack));
        _stack = stack;
        await _eventDict.FireEvent("StackDidChange", new BuffStackChangeDetails(_stack, stack));

        if(_stack <= 0)
            await _owner.RemoveBuff(this);
    }

    public async Task SetDStack(int dStack)
        => await SetStack(_stack + dStack);

    /// <summary>
    /// 是否有益
    /// </summary>
    public bool Friendly => _entry.Friendly;

    /// <summary>
    /// 是否可驱散
    /// </summary>
    public bool Dispellable => _entry.Dispellable;

    public CLEventDict _eventDict;
    private Dictionary<string, Func<StageEventDetails, Task>> _eventPropagatorDict;

    public Buff(StageEntity owner, BuffEntry entry)
    {
        _owner = owner;
        _entry = entry;
        _stack = 0;

        _eventDict = new();
        _eventPropagatorDict = new();
    }

    public void Register()
    {
        foreach (string eventId in _entry._eventCaptureDict.Keys)
        {
            StageEventCapture eventCapture = _entry._eventCaptureDict[eventId];
            _eventPropagatorDict[eventId] = d => eventCapture.Invoke(this, d);

            if (eventCapture is StageEnvironmentEventCapture)
            {
                _owner.Env._eventDict.AddCallback(eventId, eventCapture.Order, _eventPropagatorDict[eventId]);
            }
            else if (eventCapture is StageListenerEventCapture)
            {
                _eventDict.AddCallback(eventId, eventCapture.Order, _eventPropagatorDict[eventId]);
            }
        }
    }

    public void Unregister()
    {
        foreach (string eventId in _entry._eventCaptureDict.Keys)
        {
            StageEventCapture eventCapture = _entry._eventCaptureDict[eventId];

            if (eventCapture is StageEnvironmentEventCapture)
            {
                _owner.Env._eventDict.RemoveCallback(eventId, _eventPropagatorDict[eventId]);
            }
            else if (eventCapture is StageListenerEventCapture)
            {
                _eventDict.RemoveCallback(eventId, _eventPropagatorDict[eventId]);
            }
        }
    }
}
