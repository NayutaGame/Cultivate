
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro.EditorUtilities;

public abstract class PanelDescriptor : Addressable
{
    public Func<Signal, PanelDescriptor> _receiveSignal;
    public PanelDescriptor ReceiveSignal(Signal signal) => _receiveSignal.Invoke(signal);
    public virtual PanelDescriptor DefaultReceiveSignal(Signal signal) => this;

    public Action _enter;
    public void Enter() => _enter.Invoke();
    public virtual void DefaultEnter() { }

    public Action _exit;
    public void Exit() => _exit.Invoke();
    public virtual void DefaultExit() { }

    protected Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();

    private Guide[] _guideDescriptors;
    private int _index;
    public Guide GetGuideDescriptor() => _guideDescriptors != null && _index < _guideDescriptors.Length ? _guideDescriptors[_index] : null;
    public void SetGuideDescriptors(Guide[] guideDescriptors)
    {
        _guideDescriptors = guideDescriptors;
        _index = 0;
    }

    public void MoveNextGuideDescriptor()
        => _index++;

    public PanelDescriptor()
    {
        _receiveSignal = DefaultReceiveSignal;
        _enter = DefaultEnter;
        _exit = DefaultExit;
    }
}
