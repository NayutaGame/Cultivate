
using System;
using System.Threading.Tasks;

public class CLEventDescriptor
{
    private int _listenerId;
    public int ListenerId => _listenerId;

    private int _eventId;
    public int EventId => _eventId;

    private int _order;
    public int Order => _order;

    private Func<CLEventListener, EventDetails, Task> _func;
    public async Task Invoke(CLEventListener listener, EventDetails eventDetails) => await _func(listener, eventDetails);

    public CLEventDescriptor(int listenerId, int eventId, int order, Func<CLEventListener, EventDetails, Task> func)
    {
        _listenerId = listenerId;
        _eventId = eventId;
        _order = order;
        _func = func;
    }
}
