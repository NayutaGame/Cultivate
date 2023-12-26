using System;
using System.Collections.Generic;

public class RunEventDict : Dictionary<int, RunEventElement>
{
    public static readonly int START_RUN             = 100;
    public static readonly int END_RUN               = 101;
    public static readonly int WILL_SET_JINGJIE      = 102;
    public static readonly int DID_SET_JINGJIE       = 103;
    public static readonly int WILL_SET_D_MINGYUAN   = 104;
    public static readonly int DID_SET_D_MINGYUAN    = 105;
    public static readonly int WILL_SET_MAX_MINGYUAN = 106;
    public static readonly int DID_SET_MAX_MINGYUAN  = 107;


    public static readonly int RUN_ENVIRONMENT    = 100;

    public void Register(RunEventListener listener, RunEventDescriptor eventDescriptor)
    {
        int eventId = eventDescriptor.EventId;
        if (!ContainsKey(eventId))
            this[eventId] = new();

        this[eventId].Add(eventDescriptor.Order, listener, eventDescriptor);
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
        RunEventElement eventElement = this[eventId];
        foreach (Tuple<int, RunEventListener, RunEventDescriptor> tuple in eventElement.Traversal())
        {
            if (eventDetails.Cancel) return;
            tuple.Item3.Invoke(tuple.Item2, eventDetails);
        }
    }
}
