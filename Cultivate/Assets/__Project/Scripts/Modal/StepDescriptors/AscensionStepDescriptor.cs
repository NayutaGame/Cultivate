
public class AscensionStepDescriptor : StepDescriptor
{
    public override void Draw(Map map)
    {
        map.CurrStepItem._nodes.Clear();
        map.CurrStepItem._nodes.Add(new RunNode(Encyclopedia.NodeCategory["突破境界"]));
    }
}
