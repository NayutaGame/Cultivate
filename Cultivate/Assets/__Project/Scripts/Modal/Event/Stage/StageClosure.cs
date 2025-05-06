
using System;
using Cysharp.Threading.Tasks;

public class StageClosure
{
    public readonly int EventId;
    public readonly int Order;

    private Func<StageClosureListener, StageClosure, StageClosureDetails, UniTask> _func;
    public async UniTask Invoke(StageClosureListener listener, StageClosureDetails closureDetails)
    {
        if (closureDetails is NestedStageClosureDetails nestedDetails)
            await InvokeWithNested(listener, nestedDetails);
        else
            await _func(listener, this, closureDetails);
    }

    private async UniTask InvokeWithNested(StageClosureListener listener, NestedStageClosureDetails nestedDetails)
    {
        if (_checkListener)
            if (nestedDetails.Listener != null && nestedDetails.Listener != listener)
                return;
        await _func(listener, this, nestedDetails);

        // TODO: after everything is working, this null propagation should be removed
        if (nestedDetails.CastResult != null && Key != null)
            nestedDetails.CastResult.Append(Key, true);
    }

    public string Key;
    private Description _description;
    public Description Description => _description.Clone();

    private bool _checkListener;

    public StageClosure(
        int eventId,
        int order,
        Func<StageClosureListener, StageClosure, StageClosureDetails, UniTask> func,
        string key = null,
        string description = null,
        bool checkListener = false)
    {
        EventId = eventId;
        Order = order;
        _func = func;
        Key = key;
        _description = description;
        _checkListener = checkListener;
    }
}
