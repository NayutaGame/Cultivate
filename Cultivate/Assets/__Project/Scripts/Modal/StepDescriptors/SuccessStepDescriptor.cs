
public class SuccessStepDescriptor : StepDescriptor
{
    public override void Draw(Map map)
    {
        map.CurrStepItem._nodes.Clear();
        map.CurrStepItem._nodes.Add(new RunNode(Encyclopedia.NodeCategory["胜利"], Ladder));
    }

    public SuccessStepDescriptor(int ladder) : base(ladder)
    {
    }
}
