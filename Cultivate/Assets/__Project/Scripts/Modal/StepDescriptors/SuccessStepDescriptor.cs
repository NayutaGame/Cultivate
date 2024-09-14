
public class SuccessStepDescriptor : StepDescriptor
{
    public override RunNode Draw(Map map)
    {
        return new RunNode(Encyclopedia.NodeCategory["胜利"]);
    }

    public SuccessStepDescriptor(int ladder) : base(ladder)
    {
    }
}
