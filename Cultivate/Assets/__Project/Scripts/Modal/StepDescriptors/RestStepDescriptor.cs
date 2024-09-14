
public class RestStepDescriptor : StepDescriptor
{
    public override RunNode Draw(Map map)
    {
        return new RunNode("休息");
    }

    public RestStepDescriptor(int ladder) : base(ladder)
    {
    }
}
