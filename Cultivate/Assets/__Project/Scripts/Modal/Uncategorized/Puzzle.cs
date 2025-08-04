
using CLLibrary;

public class Puzzle
{
    private string _description;
    public string Description => _description;
    private string _condition;
    public string Condition => _condition;
    
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
    
        return StageResult.FromConfig(StageConfig.ForPuzzle(_home, _away, _kernel));
    }

    public Puzzle(string description, string condition,
        RunEntity home, RunEntity away, StageKernel kernel)
    {
        ChangedNeuron = new();

        _description = description;
        _condition = condition;
        
        _home = home;
        _home.ChangedNeuron.Add(ChangedNeuron);
        _away = away;
        _away.ChangedNeuron.Add(ChangedNeuron);
        
        _kernel = kernel;
        
        _result = new(Calculate);
        
        ChangedNeuron.Add(_result.SetDirty);
        ChangedNeuron.Invoke();
    }
    
    ~Puzzle()
    {
        _home.ChangedNeuron.Remove(ChangedNeuron);
        _away.ChangedNeuron.Remove(ChangedNeuron);
    }
}
