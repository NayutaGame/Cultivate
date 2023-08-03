using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StageEventCapture
{
    private string _eventId;
    public string EventId => _eventId;

    private Func<StageEventListener, StageEventDetails, Task> _func;
    public async Task Invoke(StageEventListener buff, StageEventDetails d) => await _func(buff, d);

    public StageEventCapture(string eventId, Func<StageEventListener, StageEventDetails, Task> func)
    {
        _eventId = eventId;
        _func = func;
    }
}
