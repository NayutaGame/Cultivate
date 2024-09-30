
using System;

public class RunClosure
{
    public readonly int EventId;
    public readonly int Order;

    private Action<RunClosureOwner, ClosureDetails> _func;
    public void Invoke(RunClosureOwner listener, ClosureDetails closureDetails) => _func(listener, closureDetails);

    public RunClosure(int eventId, int order, Action<RunClosureOwner, ClosureDetails> func)
    {
        EventId = eventId;
        Order = order;
        _func = func;
    }
}
