
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
        foreach (string eventId in _entry._eventCaptureDict.Keys)
        {
            StageEventCapture eventCapture = _entry._eventCaptureDict[eventId];
            _eventPropagatorDict[eventId] = d => eventCapture.Invoke(this, d);

            if (!_owner.Env._stageEventFuncQueueDict.ContainsKey(eventId))
                _owner.Env._stageEventFuncQueueDict[eventId] = new();

            _owner.Env._stageEventFuncQueueDict[eventId].Add(eventCapture.Order, _eventPropagatorDict[eventId]);
        }
    }

    public void Unregister()
    {
        foreach (string eventId in _entry._eventCaptureDict.Keys)
            _owner.Env._stageEventFuncQueueDict[eventId].Remove(_eventPropagatorDict[eventId]);
    }

    public async Task Gain()
    {
        if (_entry._gain != null) await _entry._gain.Invoke(this, _owner);
    }

    public async Task Lose()
    {
        if (_entry._lose != null) await _entry._lose.Invoke(this, _owner);
    }
}
