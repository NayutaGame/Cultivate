using System;
using System.Collections.Generic;

public class RunClosureDict : Dictionary<int, RunClosureList>
{
    public static readonly int START_RUN             = 100;
    public static readonly int END_RUN               = 101;
    public static readonly int WIL_UPDATE            = 102;
    public static readonly int DID_UPDATE            = 103;
    public static readonly int WIL_SET_JINGJIE       = 104;
    public static readonly int DID_SET_JINGJIE       = 105;
    public static readonly int WIL_SET_D_MINGYUAN    = 106;
    public static readonly int DID_SET_D_MINGYUAN    = 107;
    public static readonly int WILL_SET_D_GOLD       = 108;
    public static readonly int DID_SET_D_GOLD        = 109;
    public static readonly int WILL_SET_DDHEALTH     = 110;
    public static readonly int DID_SET_DDHEALTH      = 111;
    public static readonly int WILL_SET_MAX_MINGYUAN = 112;
    public static readonly int DID_SET_MAX_MINGYUAN  = 113;
    public static readonly int WILL_PLACEMENT        = 114;
    public static readonly int DID_PLACEMENT         = 115;
    public static readonly int WIL_FORMATION         = 116;
    public static readonly int DID_FORMATION         = 117;
    public static readonly int WILL_DISCOVER_SKILL   = 118;
    public static readonly int DID_DISCOVER_SKILL    = 119;
    public static readonly int WILL_DEPLETE          = 120;
    public static readonly int DID_DEPLETE           = 121;
    public static readonly int WIL_MERGE             = 122;
    public static readonly int DID_MERGE             = 123;

    public void Register(RunClosureOwner listener, RunClosure closure)
    {
        int eventId = closure.EventId;
        if (!ContainsKey(eventId))
            this[eventId] = new();

        this[eventId].Add(listener, closure);
    }

    public void Unregister(RunClosureOwner listener, RunClosure closure)
    {
        int eventId = closure.EventId;
        this[eventId].Remove(listener);
    }

    public void SendEvent(int eventId, ClosureDetails closureDetails)
    {
        if (!ContainsKey(eventId))
            return;
        RunClosureList closureList = this[eventId];
        foreach (Tuple<RunClosureOwner, RunClosure> tuple in closureList.Traversal())
        {
            if (closureDetails.Cancel) return;
            tuple.Item2.Invoke(tuple.Item1, closureDetails);
        }
    }
}
