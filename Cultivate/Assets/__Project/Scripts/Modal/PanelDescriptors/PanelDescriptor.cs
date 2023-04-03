using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelDescriptor
{
    public Action<Signal> _receiveSignal;

    public void ReceiveSignal(Signal signal)
    {
        if (_receiveSignal != null)
        {
            _receiveSignal(signal);
        }
        else
        {
            DefaultReceiveSignal(signal);
        }
    }

    protected virtual void DefaultReceiveSignal(Signal signal)
    {

    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }
}
