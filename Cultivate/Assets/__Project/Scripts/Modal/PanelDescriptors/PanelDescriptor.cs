
using System;
using System.Collections.Generic;
using TMPro.EditorUtilities;

public abstract class PanelDescriptor : Addressable
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
    public object Get(string s) => _accessors[s]();

    private GuideDescriptor _guideDescriptor;
    public GuideDescriptor GetGuideDescriptor() => _guideDescriptor;
    public void SetGuideDescriptor(GuideDescriptor guideDescriptor)
        => _guideDescriptor = guideDescriptor;
}
