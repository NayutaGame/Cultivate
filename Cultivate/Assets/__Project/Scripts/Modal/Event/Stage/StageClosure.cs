
using System;
using Cysharp.Threading.Tasks;

public class StageClosure
{
    public readonly int EventId;
    public readonly int Order;

    private Func<StageClosureListener, StageClosureDetails, UniTask> _func;
    public async UniTask Invoke(StageClosureListener listener, StageClosureDetails closureDetails) => await _func(listener, closureDetails);

    public string Key;
    private Description _description;
    public Description Description => _description.Clone();

    public StageClosure(
        int eventId,
        int order,
        Func<StageClosureListener, StageClosureDetails, UniTask> func,
        string key = null,
        string description = null)
    {
        EventId = eventId;
        Order = order;
        _func = func;
        Key = key;
        _description = description;
    }
}
