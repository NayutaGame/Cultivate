
using System;

public class PuzzlePanelDescriptor : PanelDescriptor
{
    private string _description;
    private Puzzle _puzzle;
    
    // public StageResult GetResult() => RunManager.Instance.Environment.SimulateResult;

    public PuzzlePanelDescriptor(Puzzle puzzle, string description)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Home",                     () => _puzzle.Home },
            { "Away",                     () => _puzzle.Away },
            { "Result",                   () => _puzzle.GetResult() },
        };

        _puzzle = puzzle;
        _description = description;
    }

    // public override void DefaultEnter()
    // {
    //     base.DefaultEnter();
    //     SetEnemy(RunEntity.FromTemplate(_template));
    // }
    //
    // public override void DefaultExit()
    // {
    //     base.DefaultExit();
    //     // make sure puzzle is disposed
    // }
    
    private Func<PuzzleResultSignal, PanelDescriptor> _operation;
    public PuzzlePanelDescriptor SetOperation(Func<PuzzleResultSignal, PanelDescriptor> operation)
    {
        _operation = operation;
        return this;
    }
    
    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is PuzzleResultSignal puzzleResultSignal)
        {
            return _operation(puzzleResultSignal);
        }
    
        return this;
    }
    
    // public void Combat()
    // {
    //     RunManager.Instance.Environment.Combat();
    // }
}
