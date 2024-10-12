
using System;
using Cysharp.Threading.Tasks;

public class StageClosure
{
    public readonly int EventId;
    public readonly int Order;

    private Func<StageClosureOwner, ClosureDetails, UniTask> _func;
    public async UniTask Invoke(StageClosureOwner owner, ClosureDetails closureDetails) => await _func(owner, closureDetails);

    public StageClosure(int eventId, int order, Func<StageClosureOwner, ClosureDetails, UniTask> func)
    {
        EventId = eventId;
        Order = order;
        _func = func;
    }
}
