
public class SuccessStepDescriptor : StepDescriptor
{
    public override RunNode Draw(Map map)
    {
        return new RunNode(Encyclopedia.NodeCategory["胜利"], Ladder);
    }

    public SuccessStepDescriptor(int ladder) : base(ladder)
    {
    }
}
