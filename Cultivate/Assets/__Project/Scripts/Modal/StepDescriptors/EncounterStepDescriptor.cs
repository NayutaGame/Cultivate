
public class EncounterStepDescriptor : StepDescriptor
{
    public override RunNode Draw(Map map)
    {
        map.AdventurePool.TryPopItem(out NodeEntry entry, pred: e => e.CanCreate(map, Ladder));
        return new RunNode(entry, Ladder);
    }

    public EncounterStepDescriptor(int ladder) : base(ladder)
    {
    }
}
