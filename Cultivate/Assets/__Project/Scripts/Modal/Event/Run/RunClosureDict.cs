
using System;
using System.Collections.Generic;
using CLLibrary;

public class RunClosureDict : Dictionary<int, RunClosureRow>
{
    public static readonly int START_RUN             = 100;
    public static readonly int END_RUN               = 101;
    public static readonly int WIL_JINGJIE_CHANGE    = 102;
    public static readonly int DID_JINGJIE_CHANGE    = 103;
    public static readonly int WIL_SET_D_MINGYUAN    = 104;
    public static readonly int DID_SET_D_MINGYUAN    = 105;
    public static readonly int WIL_SET_D_GOLD        = 106;
    public static readonly int DID_SET_D_GOLD        = 107;
    public static readonly int WIL_SET_HEALTH        = 108;
    public static readonly int DID_SET_HEALTH        = 109;
    public static readonly int WIL_SET_MAX_MINGYUAN  = 110;
    public static readonly int DID_SET_MAX_MINGYUAN  = 111;
    public static readonly int WIL_PLACEMENT         = 112;
    public static readonly int DID_PLACEMENT         = 113;
    public static readonly int WIL_FORMATION         = 114;
    public static readonly int DID_FORMATION         = 115;
    public static readonly int WIL_SECOND_PLACEMENT  = 116;
    public static readonly int DID_SECOND_PLACEMENT  = 117;
    public static readonly int WIL_DISCOVER_SKILL    = 118;
    public static readonly int DID_DISCOVER_SKILL    = 119;
    public static readonly int WIL_DEPLETE           = 120;
    public static readonly int DID_DEPLETE           = 121;
    public static readonly int WIL_MERGE             = 122;
    public static readonly int DID_MERGE             = 123;
    public static readonly int DID_COMMIT_RUN        = 124;
    public static readonly int WIL_CHANGE_CELL       = 125;
    
    public void Register(RunClosureListener listener, RunClosure[] closures)
    {
        closures.Do(e => Register(listener, e));
    }

    public void Register(RunClosureListener listener, RunClosure closure)
    {
        int eventId = closure.EventId;
        if (!ContainsKey(eventId))
            this[eventId] = new();

        this[eventId].Add(listener, closure);
    }

    public void Unregister(RunClosureListener listener, RunClosure[] closures)
    {
        closures.Do(e => Unregister(listener, e));
    }

    public void Unregister(RunClosureListener listener, RunClosure closure)
    {
        int eventId = closure.EventId;
        this[eventId].Remove(listener);
    }

    public void SendEvent(int eventId, RunClosureDetails closureDetails)
    {
        if (!ContainsKey(eventId))
            return;
        RunClosureRow closureRow = this[eventId];
        foreach (Tuple<RunClosureListener, RunClosure> tuple in closureRow.Traversal())
        {
            if (closureDetails.Cancel) return;
            tuple.Item2.Invoke(tuple.Item1, closureDetails);
        }
    }
}
