
using System;
using Cysharp.Threading.Tasks;

public class StageClosure
{
    public readonly int EventId;
    public readonly int Order;

    private Func<StageClosureListener, ClosureDetails, UniTask> _func;
    public async UniTask Invoke(StageClosureListener listener, ClosureDetails closureDetails) => await _func(listener, closureDetails);

    public StageClosure(int eventId, int order, Func<StageClosureListener, ClosureDetails, UniTask> func)
    {
        EventId = eventId;
        Order = order;
        _func = func;
    }
}
