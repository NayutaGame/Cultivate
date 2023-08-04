
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

    public CLEventDict _eventDict;
    private Dictionary<string, Func<StageEventDetails, Task>> _eventPropagatorDict;

    public Formation(StageEntity owner, FormationEntry entry)
    {
        _owner = owner;
        _entry = entry;

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
