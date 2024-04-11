
public class AdventureStepDescriptor : StepDescriptor
{
    public override void Draw(Map map)
    {
        map.CurrStepItem._nodes.Clear();
        map.AdventurePool.TryPopItem(out NodeEntry entry, pred: e => e.CanCreate(map));
        map.CurrStepItem._nodes.Add(new RunNode(entry));
    }
}
