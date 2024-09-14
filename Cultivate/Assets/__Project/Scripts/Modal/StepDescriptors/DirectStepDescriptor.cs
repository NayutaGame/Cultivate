
public class DirectStepDescriptor : StepDescriptor
{
    private NodeEntry _nodeEntry;

    public DirectStepDescriptor(int ladder, NodeEntry nodeEntry) : base(ladder)
    {
        _nodeEntry = nodeEntry;
    }
    
    public override RunNode Draw(Map map)
    {
        return new RunNode(_nodeEntry);
    }
}
