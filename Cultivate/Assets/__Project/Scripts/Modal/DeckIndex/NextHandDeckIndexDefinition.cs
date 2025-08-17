
public struct NextHandDeckIndexDefinition : IDeckIndex
{
    public SkillRegion Region => SkillRegion.Hand;
    public int Index => RunManager.Instance.Environment.Hand.Count();

    public override string ToString()
    {
        return "手牌下一个位置";
    }

    public DeckIndex Reify()
        => new(Region, Index);
}