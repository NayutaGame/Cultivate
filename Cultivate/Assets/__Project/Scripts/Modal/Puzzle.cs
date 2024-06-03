
using CLLibrary;

public class Puzzle
{
    private RunEntity _home;
    public RunEntity Home => _home;
    private RunEntity _away;
    public RunEntity Away => _away;
    private StageKernel _kernel;
    
    public Neuron ChangedNeuron;
    private Dirty<StageResult> _result;
    public StageResult GetResult() => _result.Value;
    private StageResult Calculate()
    {
        // EnvironmentUpdateDetails d = new();
        // _eventDict.SendEvent(RunEventDict.WIL_UPDATE, d);
        _home.PlacementProcedure();
        _away.PlacementProcedure();
        
        // _home.FormationProcedure();
        // _away.FormationProcedure();
        // _eventDict.SendEvent(RunEventDict.DID_UPDATE, d);
    
        return StageEnvironment.CalcSimulateResult(StageConfig.ForPuzzle(_home, _away, _kernel));
    }

    public Puzzle(RunEntity home, RunEntity away, StageKernel kernel)
    {
        ChangedNeuron = new();
        
        _home = home;
        _home.EnvironmentChangedNeuron.Add(ChangedNeuron);
        _away = away;
        _away.EnvironmentChangedNeuron.Add(ChangedNeuron);
        
        _kernel = kernel;
        
        _result = new(Calculate);
        
        ChangedNeuron.Add(_result.SetDirty);
        ChangedNeuron.Invoke();
    }
    
    ~Puzzle() // cyclic referencing may cause puzzle never gets disposed
    {
        _home.EnvironmentChangedNeuron.Remove(ChangedNeuron);
        _away.EnvironmentChangedNeuron.Remove(ChangedNeuron);
    }
}
