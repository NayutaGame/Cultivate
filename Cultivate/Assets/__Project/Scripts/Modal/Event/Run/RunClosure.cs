
using System;

public class RunClosure
{
    public readonly int EventId;
    public readonly int Order;

    private Action<RunClosureListener, RunClosureDetails> _func;
    public void Invoke(RunClosureListener listener, RunClosureDetails closureDetails) => _func(listener, closureDetails);

    public RunClosure(int eventId, int order, Action<RunClosureListener, RunClosureDetails> func)
    {
        EventId = eventId;
        Order = order;
        _func = func;
    }
}
