
public class RestStepDescriptor : StepDescriptor
{
    public override void Draw(Map map)
    {
        map.CurrStepItem._nodes.Clear();
        map.CurrStepItem._nodes.Add(new RunNode("休息"));
        map.CurrStepItem._nodes.Add(new RunNode("商店"));
    }
}
