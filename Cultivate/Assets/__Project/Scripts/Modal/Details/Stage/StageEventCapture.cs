using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StageEventCapture
{
    private string _eventId;
    public string EventId => _eventId;

    private int _order;
    public int Order => _order;

    private Func<StageEventListener, StageEventDetails, Task> _func;
    public async Task Invoke(StageEventListener listener, StageEventDetails stageEventDetails) => await _func(listener, stageEventDetails);

    public StageEventCapture(string eventId, int order, Func<StageEventListener, StageEventDetails, Task> func)
    {
        _eventId = eventId;
        _order = order;
        _func = func;
    }
}
