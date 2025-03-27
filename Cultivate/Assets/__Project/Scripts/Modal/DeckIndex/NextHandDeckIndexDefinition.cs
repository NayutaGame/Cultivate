
public struct NextHandDeckIndexDefinition : IDeckIndex
{
    public bool InField => false;
    public int Index => RunManager.Instance.Environment.Hand.Count();

    public override string ToString()
    {
        return "手牌下一个位置";
    }

    public DeckIndex Reify()
        => new(InField, Index);
}