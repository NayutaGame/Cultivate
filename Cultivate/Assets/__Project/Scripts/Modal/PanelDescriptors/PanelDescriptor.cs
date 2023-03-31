using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelDescriptor
{
    public Action<Signal> _receiveSignal;

    public virtual void ReceiveSignal(Signal signal)
    {
        _receiveSignal(signal);
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }
}
