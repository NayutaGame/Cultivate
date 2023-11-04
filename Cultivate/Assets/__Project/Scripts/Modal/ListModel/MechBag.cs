
public sealed class MechBag : ListModel<Mech>
{
    public MechBag()
    {
        foreach (MechType t in MechType.Traversal)
        {
            Add(new(t));
        }
    }

    public int GetCount(MechType mechType)
        => this[mechType._index].Count;

    public void AddMech(MechType mechType, int count = 1)
    {
        this[mechType._index].Count += count;
    }

    public bool TryConsumeMech(MechType mechType, int count = 1)
    {
        if (!CanConsumeMech(mechType, count))
            return false;

        this[mechType._index].Count -= count;
        return true;
    }

    public bool CanConsumeMech(MechType mechType, int count = 1)
    {
        return this[mechType._index].Count >= count;
    }
}
