
using System;
using System.Threading.Tasks;

public abstract class StageEventCapture
{
    private int _eventId;
    public int EventId => _eventId;

    private int _order;
    public int Order => _order;

    private Func<StageEventListener, StageEventDetails, Task> _func;
    public async Task Invoke(StageEventListener listener, StageEventDetails stageEventDetails) => await _func(listener, stageEventDetails);

    public StageEventCapture(int eventId, int order, Func<StageEventListener, StageEventDetails, Task> func)
    {
        _eventId = eventId;
        _order = order;
        _func = func;
    }
}
