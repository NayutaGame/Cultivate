
using System;
using System.Threading.Tasks;
using CLLibrary;

/// <summary>
/// Buff
/// </summary>
public class Buff : StageClosureOwner
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private BuffEntry _entry;
    public BuffEntry GetEntry() => _entry;

    public Neuron StackChangedNeuron;
    public Neuron PingNeuron;

    public string GetName() => _entry.GetName();
    public string GetExplanation() => _entry.GetExplanation();
    public string GetTrivia() => _entry.GetTrivia();

    private int _stack;
    public int Stack => _stack;
    public async Task SetStack(int stack)
    {
        await _owner.Env.ClosureDict.SendEvent(StageClosureDict.WIL_CHANGE_STACK, new BuffStackChangeDetails(_stack, stack));
        _stack = stack;
        await _owner.Env.ClosureDict.SendEvent(StageClosureDict.DID_CHANGE_STACK, new BuffStackChangeDetails(_stack, stack));

        if(_stack <= 0)
            await _owner.RemoveBuff(this);
        else
            StackChangedNeuron.Invoke();
            
    }

    public async Task SetDStack(int dStack)
        => await SetStack(_stack + dStack);

    /// <summary>
    /// 是否有益
    /// </summary>
    public bool Friendly => _entry.Friendly;

    /// <summary>
    /// 是否可驱散
    /// </summary>
    public bool Dispellable => _entry.Dispellable;

    public Buff(StageEntity owner, BuffEntry entry)
    {
        _owner = owner;
        _entry = entry;
        _stack = 0;

        PingNeuron = new();
        StackChangedNeuron = new();
    }

    public async void PlayPingAnimation()
    {
        PingNeuron.Invoke();
    }

    public void Register()
    {
        foreach (StageClosure closure in _entry.Closures)
            _owner.Env.ClosureDict.Register(this, closure);
    }

    public void Unregister()
    {
        foreach (StageClosure closure in _entry.Closures)
            _owner.Env.ClosureDict.Unregister(this, closure);
    }
}
