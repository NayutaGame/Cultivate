
using Cysharp.Threading.Tasks;
using CLLibrary;

public class Buff : StageClosureListener, IEmphasizable
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private BuffEntry _entry;
    public BuffEntry GetEntry() => _entry;

    private Neuron _emphasisNeuron;
    public Neuron GetEmphasisNeuron()
        => _emphasisNeuron;

    public string GetName() => _entry.GetName();
    public string GetCascadeAnnotated() => _entry.GetCascadeAnnotated();
    public string GetTrivia() => _entry.GetTrivia();

    private int _stack;
    public int Stack => _stack;
    public void SetStack(int value)
        => _stack = value;

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
        => await _owner.Env.GainBuffProcedure(new(_owner, _owner, GetEntry(), stack, true, true));

    public async UniTask LoseStackProcedure(int stack = 1)
        => await _owner.Env.LoseBuffProcedure(new(_owner, _owner, GetEntry(), stack, true, true));
}
