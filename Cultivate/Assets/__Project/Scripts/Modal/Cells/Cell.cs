
using System;
using System.Collections.Generic;

public abstract class Cell : Addressable
{
    public Func<Signal, Cell> _receiveSignal;
    public Cell ReceiveSignal(Signal signal) => _receiveSignal.Invoke(signal);
    public virtual Cell DefaultReceiveSignal(Signal signal) => this;

    public Action<Cell> _enter;
    public void Enter() => _enter.Invoke(this);
    public virtual void DefaultEnter(Cell cell) { }
    public Cell SetEnter(Action<Cell> enter)
    {
        _enter = enter;
        return this;
    }

    public Action<Cell> _exit;
    public void Exit() => _exit.Invoke(this);
    public virtual void DefaultExit(Cell cell) { }
    public Cell SetExit(Action<Cell> exit)
    {
        _exit = exit;
        return this;
    }

    public abstract object Get(string s);

    private Guide[] _guideDescriptors;
    private int _index;
    public Guide GetGuideDescriptor() => _guideDescriptors != null && _index < _guideDescriptors.Length ? _guideDescriptors[_index] : null;
    public void SetGuideDescriptors(Guide[] guideDescriptors)
    {
        _guideDescriptors = guideDescriptors;
        ResetGuideIndex();
    }

    public void SetGuideToFinish()
    {
        if (_guideDescriptors != null)
            _index = _guideDescriptors.Length;
    }

    public void ResetGuideIndex()
        => _index = 0;

    public void MoveNextGuideDescriptor()
        => _index++;

    public Cell()
    {
        _receiveSignal = DefaultReceiveSignal;
        _enter = DefaultEnter;
        _exit = DefaultExit;
    }
}
