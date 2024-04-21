using System;
using System.Collections.Generic;

public class RunEventDict : Dictionary<int, RunEventElementList>
{
    public static readonly int START_RUN             = 100;
    public static readonly int END_RUN               = 101;
    public static readonly int WIL_UPDATE            = 102;
    public static readonly int DID_UPDATE            = 103;
    public static readonly int WIL_SET_JINGJIE       = 104;
    public static readonly int DID_SET_JINGJIE       = 105;
    public static readonly int WILL_SET_D_MINGYUAN   = 106;
    public static readonly int DID_SET_D_MINGYUAN    = 107;
    public static readonly int WILL_SET_MAX_MINGYUAN = 108;
    public static readonly int DID_SET_MAX_MINGYUAN  = 109;
    public static readonly int WILL_PLACEMENT        = 110;
    public static readonly int DID_PLACEMENT         = 111;
    public static readonly int WILL_FORMATION        = 112;
    public static readonly int DID_FORMATION         = 113;
    public static readonly int WILL_DISCOVER_SKILL   = 114;
    public static readonly int DID_DISCOVER_SKILL    = 115;
    public static readonly int WILL_DEPLETE          = 116;
    public static readonly int DID_DEPLETE           = 117;

    public static readonly int RUN_ENVIRONMENT       = 100;

    public void Register(RunEventListener listener, RunEventDescriptor eventDescriptor)
    {
        int eventId = eventDescriptor.EventId;
        if (!ContainsKey(eventId))
            this[eventId] = new();

        this[eventId].Add(listener, eventDescriptor);
    }

    public void Unregister(RunEventListener listener, RunEventDescriptor eventDescriptor)
    {
        int eventId = eventDescriptor.EventId;
        this[eventId].Remove(listener);
    }

    public void SendEvent(int eventId, EventDetails eventDetails)
    {
        if (!ContainsKey(eventId))
            return;
        RunEventElementList eventElementList = this[eventId];
        foreach (Tuple<RunEventListener, RunEventDescriptor> tuple in eventElementList.Traversal())
        {
            if (eventDetails.Cancel) return;
            tuple.Item2.Invoke(tuple.Item1, eventDetails);
        }
    }
}
