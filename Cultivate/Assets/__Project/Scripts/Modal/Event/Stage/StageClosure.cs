
using System;
using System.Threading.Tasks;

public class StageClosure
{
    public readonly int EventId;
    public readonly int Order;

    private Func<StageClosureOwner, ClosureDetails, Task> _func;
    public async Task Invoke(StageClosureOwner owner, ClosureDetails closureDetails) => await _func(owner, closureDetails);

    public StageClosure(int eventId, int order, Func<StageClosureOwner, ClosureDetails, Task> func)
    {
        EventId = eventId;
        Order = order;
        _func = func;
    }
}
