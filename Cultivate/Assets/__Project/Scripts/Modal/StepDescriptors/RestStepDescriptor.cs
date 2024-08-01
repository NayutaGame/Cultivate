
public class RestStepDescriptor : StepDescriptor
{
    public override void Draw(Map map)
    {
        map.CurrStepItem._nodes.Clear();
        map.CurrStepItem._nodes.Add(new RunNode("休息", Ladder));
    }

    public RestStepDescriptor(int ladder) : base(ladder)
    {
    }
}
