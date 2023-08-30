
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
    private Dictionary<int, Func<StageEventDetails, Task>> _eventPropagatorDict;

    public Formation(StageEntity owner, FormationEntry entry)
    {
        _owner = owner;
        _entry = entry;

        _eventDict = new();
        _eventPropagatorDict = new();
    }

    public void Register()
    {
        foreach (int eventId in _entry._eventCaptureDict.Keys)
        {
            StageEventCapture eventCapture = _entry._eventCaptureDict[eventId];
            _eventPropagatorDict[eventId] = d => eventCapture.Invoke(this, d);

            if (eventCapture is StageEnvironmentEventCapture)
            {
                _owner.Env._eventDict.Register(eventId, eventCapture.Order, _eventPropagatorDict[eventId]);
            }
            else if (eventCapture is StageListenerEventCapture)
            {
                _eventDict.Register(eventId, eventCapture.Order, _eventPropagatorDict[eventId]);
            }
        }
    }

    public void Unregister()
    {
        foreach (int eventId in _entry._eventCaptureDict.Keys)
        {
            StageEventCapture eventCapture = _entry._eventCaptureDict[eventId];

            if (eventCapture is StageEnvironmentEventCapture)
            {
                _owner.Env._eventDict.Unregister(eventId, _eventPropagatorDict[eventId]);
            }
            else if (eventCapture is StageListenerEventCapture)
            {
                _eventDict.Unregister(eventId, _eventPropagatorDict[eventId]);
            }
        }
    }
}
