
public class StepItem
{
    public enum StepState
    {
        Past,
        Curr,
        Future,
    }

    private StepState _state;
    public StepState State => _state;

    private StepDescriptor _stepDescriptor;
    public StepDescriptor StepDescriptor => _stepDescriptor;
    
    private RunNode _node;
    public RunNode Node => _node;

    public bool HasNode()
        => _node != null;

    public StepItem(StepDescriptor stepDescriptor)
    {
        _stepDescriptor = stepDescriptor;
        _state = StepState.Future;
    }

    public void DrawNode(Map map)
    {
        _node = _stepDescriptor.Draw(map);
    }

    public void CreatePanel(Map map)
    {
        _node.Entry.Create(map, _stepDescriptor.Ladder);
    }
}
