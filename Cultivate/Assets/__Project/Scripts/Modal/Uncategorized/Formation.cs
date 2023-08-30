
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLLibrary;

/// <summary>
/// Formation
/// </summary>
public class Formation : CLEventListener
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private FormationEntry _entry;
    public FormationEntry Entry => _entry;

    public string GetName() => _entry.GetName();

    public CLEventDict _eventDict;

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
            CLEventDescriptor eventDescriptor = _entry._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.SenderId;

            if (senderId == CLEventDict.STAGE_ENVIRONMENT)
                _owner.Env._eventDict.Register(this, eventDescriptor);
            else if (senderId == CLEventDict.STAGE_ENTITY)
                ;
            else if (senderId == CLEventDict.STAGE_BUFF)
                ;
            else if (senderId == CLEventDict.STAGE_FORMATION)
                _eventDict.Register(this, eventDescriptor);
        }
    }

    public void Unregister()
    {
        foreach (int eventId in _entry._eventDescriptorDict.Keys)
        {
            CLEventDescriptor eventDescriptor = _entry._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.SenderId;

            if (senderId == CLEventDict.STAGE_ENVIRONMENT)
                _owner.Env._eventDict.Unregister(this, eventDescriptor);
            else if (senderId == CLEventDict.STAGE_ENTITY)
                ;
            else if (senderId == CLEventDict.STAGE_BUFF)
                ;
            else if (senderId == CLEventDict.STAGE_FORMATION)
                _eventDict.Unregister(this, eventDescriptor);
        }
    }
}
