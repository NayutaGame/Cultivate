using System;
using System.Collections.Generic;

public class RunClosureDict : Dictionary<int, RunClosureList>
{
    public static readonly int START_RUN             = 100;
    public static readonly int END_RUN               = 101;
    public static readonly int WIL_JINGJIE_CHANGE    = 102;
    public static readonly int DID_JINGJIE_CHANGE    = 103;
    public static readonly int WIL_SET_D_MINGYUAN    = 104;
    public static readonly int DID_SET_D_MINGYUAN    = 105;
    public static readonly int WIL_SET_D_GOLD        = 106;
    public static readonly int DID_SET_D_GOLD        = 107;
    public static readonly int WIL_SET_DDHEALTH      = 108;
    public static readonly int DID_SET_DDHEALTH      = 109;
    public static readonly int WIL_SET_MAX_MINGYUAN  = 110;
    public static readonly int DID_SET_MAX_MINGYUAN  = 111;
    public static readonly int WIL_PLACEMENT         = 112;
    public static readonly int DID_PLACEMENT         = 113;
    public static readonly int WIL_FORMATION         = 114;
    public static readonly int DID_FORMATION         = 115;
    public static readonly int WIL_DISCOVER_SKILL    = 116;
    public static readonly int DID_DISCOVER_SKILL    = 117;
    public static readonly int WIL_DEPLETE           = 118;
    public static readonly int DID_DEPLETE           = 119;
    public static readonly int WIL_MERGE             = 120;
    public static readonly int DID_MERGE             = 121;

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
