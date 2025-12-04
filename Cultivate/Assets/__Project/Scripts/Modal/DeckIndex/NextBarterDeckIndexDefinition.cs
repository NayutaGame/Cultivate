
using UnityEngine.Assertions;

public struct NextBarterDeckIndexDefinition : IDeckIndex
{
    public SkillRegion Region => SkillRegion.Barter;
    public int Index
    {
        get
        {
            BarterCell barterCell = RunManager.Instance.Environment.Cell.AsCell() as BarterCell;
            Assert.IsTrue(barterCell != null);
            return barterCell.LeftBucketItems.Count();
        }
    }

    public override string ToString()
    {
        return "Barter Left Bucket下一个位置";
    }

    public DeckIndex Reify()
        => new(Region, Index);
}