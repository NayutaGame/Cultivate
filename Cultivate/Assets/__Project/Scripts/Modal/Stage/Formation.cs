
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

    public StageEventDict _eventDict;

    public Formation(StageEntity owner, FormationEntry entry)
    {
        _owner = owner;
        _entry = entry;

        _eventDict = new();
    }

    public void Register()
    {
        foreach (int eventId in _entry._eventDescriptorDict.Keys)
        {
            StageEventDescriptor eventDescriptor = _entry._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == StageEventDict.STAGE_ENVIRONMENT)
                _owner.Env.EventDict.Register(this, eventDescriptor);
            else if (senderId == StageEventDict.STAGE_ENTITY)
                ;
            else if (senderId == StageEventDict.STAGE_BUFF)
                ;
            else if (senderId == StageEventDict.STAGE_FORMATION)
                _eventDict.Register(this, eventDescriptor);
        }
    }

    public void Unregister()
    {
        foreach (int eventId in _entry._eventDescriptorDict.Keys)
        {
            StageEventDescriptor eventDescriptor = _entry._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == StageEventDict.STAGE_ENVIRONMENT)
                _owner.Env.EventDict.Unregister(this, eventDescriptor);
            else if (senderId == StageEventDict.STAGE_ENTITY)
                ;
            else if (senderId == StageEventDict.STAGE_BUFF)
                ;
            else if (senderId == StageEventDict.STAGE_FORMATION)
                _eventDict.Unregister(this, eventDescriptor);
        }
    }
}
