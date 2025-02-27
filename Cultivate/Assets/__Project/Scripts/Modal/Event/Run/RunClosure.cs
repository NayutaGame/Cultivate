
using System;

public class RunClosure
{
    public readonly int EventId;
    public readonly int Order;

    private Action<RunClosureListener, ClosureDetails> _func;
    public void Invoke(RunClosureListener listener, ClosureDetails closureDetails) => _func(listener, closureDetails);

    public RunClosure(int eventId, int order, Action<RunClosureListener, ClosureDetails> func)
    {
        EventId = eventId;
        Order = order;
        _func = func;
    }
}
