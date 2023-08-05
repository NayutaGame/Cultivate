using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StageEnvironmentEventCapture : StageEventCapture
{
    public StageEnvironmentEventCapture(int eventId, int order, Func<StageEventListener, StageEventDetails, Task> func) : base(eventId, order, func)
    {
    }
}
