
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
            if (nestedDetails.Listener == null || nestedDetails.Listener != listener)
                return;

        // TODO: after everything is working, this null propagation should be removed
        if (nestedDetails.CastResult != null && Key != null)
            nestedDetails.CastResult.Append(Key, true);
        
        await _func(listener, this, nestedDetails);
    }

    public string Key;
    private string _rawDescription;
    private Description _description;
    public Description Description
    {
        get
        {
            if (_description != null)
                return _description.Clone();

            _description = new Description(_rawDescription);
            return _description.Clone();
        }
    }

    private bool _checkListener;

    public StageClosure(
        int eventId,
        int order,
        Func<StageClosureListener, StageClosure, StageClosureDetails, UniTask> func,
        string key = null,
        string rawDescription = null,
        bool checkListener = false)
    {
        EventId = eventId;
        Order = order;
        _func = func;
        Key = key;
        _rawDescription = rawDescription;
        _checkListener = checkListener;
    }
}
