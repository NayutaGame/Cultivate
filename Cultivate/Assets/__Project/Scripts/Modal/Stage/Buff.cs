
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using CLLibrary;

public class Buff : StageClosureListener, IEmphasizable, AnnotatableBuff
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private BuffEntry _entry;
    public BuffEntry GetEntry() => _entry;

    private Neuron _emphasisNeuron;
    public Neuron GetEmphasisNeuron()
        => _emphasisNeuron;

    public string GetName() => _entry.GetName();
    public Description GetDescription() => _entry.GetDescription();

    public string GetTrivia() => _entry.GetTrivia();

    private int _stack;
    public int Stack => _stack;
    public void SetStack(int value)
        => _stack = value;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public Buff(StageEntity owner, BuffEntry entry)
    {
        _owner = owner;
        _entry = entry;
        _stack = 0;

        _emphasisNeuron = new();
    }

    public void Emphasize()
        => _emphasisNeuron.Invoke();

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

    public async UniTask GainStackProcedure(int stack)
        => await _owner.Env.GainBuffProcedure(new(_owner.Env, _owner, _owner, GetEntry(), stack, true, null, null, null, true));

    public async UniTask LoseStackProcedure(int stack = 1)
        => await _owner.Env.LoseBuffProcedure(new(_owner.Env, _owner, _owner, GetEntry(), stack, true, true));

    public bool CanShowAnnotation()
        => true;
}
