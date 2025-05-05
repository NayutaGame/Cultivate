
using System;

public class RunClosure
{
    public readonly int EventId;
    public readonly int Order;

    private Action<RunClosureListener, RunClosure, RunClosureDetails> _func;
    public void Invoke(RunClosureListener listener, RunClosureDetails closureDetails) => _func(listener, this, closureDetails);

    public RunClosure(int eventId, int order, Action<RunClosureListener, RunClosure, RunClosureDetails> func)
    {
        EventId = eventId;
        Order = order;
        _func = func;
    }
}
