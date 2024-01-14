
using System;

public class RunEventDescriptor
{
    private int _listenerId;
    public int ListenerId => _listenerId;

    private int _eventId;
    public int EventId => _eventId;

    private int _order;
    public int Order => _order;

    private Action<RunEventListener, EventDetails> _func;
    public void Invoke(RunEventListener listener, EventDetails eventDetails) => _func(listener, eventDetails);

    public RunEventDescriptor(int listenerId, int eventId, int order, Action<RunEventListener, EventDetails> func)
    {
        _listenerId = listenerId;
        _eventId = eventId;
        _order = order;
        _func = func;
    }
}
