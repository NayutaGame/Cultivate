using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class PanelDescriptor : GDictionary
{
    public Func<Signal, PanelDescriptor> _receiveSignal;
    public PanelDescriptor ReceiveSignal(Signal signal) => (_receiveSignal ?? DefaultReceiveSignal).Invoke(signal);
    public virtual PanelDescriptor DefaultReceiveSignal(Signal signal) => this;

    public Action _enter;
    public void Enter() => (_enter ?? DefaultEnter).Invoke();
    public virtual void DefaultEnter() { }

    public Action _exit;
    public void Exit() => (_exit ?? DefaultExit).Invoke();
    public virtual void DefaultExit() { }

    protected Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
}
