
public class SuccessStepDescriptor : StepDescriptor
{
    public override void Draw(Map map)
    {
        map.CurrStepItem._nodes.Clear();
        map.CurrStepItem._nodes.Add(new RunNode(Encyclopedia.NodeCategory["胜利"]));
    }
}
