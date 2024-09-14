
public class AscensionStepDescriptor : StepDescriptor
{
    public override RunNode Draw(Map map)
    {
        return new RunNode(Encyclopedia.NodeCategory["突破境界"]);
    }

    public AscensionStepDescriptor(int ladder) : base(ladder)
    {
    }
}
