using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StageListenerEventCapture : StageEventCapture
{
    public StageListenerEventCapture(string eventId, int order, Func<StageEventListener, StageEventDetails, Task> func) : base(eventId, order, func)
    {
    }
}
