using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StageEventCapture
{
    private string _eventId;
    public string EventId => _eventId;

    private Func<Buff, StageEventDetails, Task> _func;
    public async Task Invoke(Buff buff, StageEventDetails d) => await _func(buff, d);

    public StageEventCapture(string eventId, Func<Buff, StageEventDetails, Task> func)
    {
        _eventId = eventId;
        _func = func;
    }
}
