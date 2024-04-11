
public class DirectStepDescriptor : StepDescriptor
{
    private NodeEntry[] _nodes;

    public DirectStepDescriptor(params NodeEntry[] nodes)
    {
        _nodes = nodes;
    }
    
    public override void Draw(Map map)
    {
        map.CurrStepItem._nodes.Clear();
        foreach (var node in _nodes)
            map.CurrStepItem._nodes.Add(new RunNode(node));
    }
}
