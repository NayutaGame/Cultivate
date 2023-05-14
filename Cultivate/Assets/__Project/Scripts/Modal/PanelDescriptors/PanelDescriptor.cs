using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelDescriptor : GDictionary
{
    public Action<Signal> _receiveSignal;
    public void ReceiveSignal(Signal signal) => (_receiveSignal ?? DefaultReceiveSignal).Invoke(signal);
    public virtual void DefaultReceiveSignal(Signal signal) { }

    public Action _enter;
    public void Enter() => (_enter ?? DefaultEnter).Invoke();
    public virtual void DefaultEnter() { }

    public Action _exit;
    public void Exit() => (_exit ?? DefaultExit).Invoke();
    public virtual void DefaultExit() { }

    protected Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
}
