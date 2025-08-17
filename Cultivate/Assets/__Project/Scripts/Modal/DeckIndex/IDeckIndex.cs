
public interface IDeckIndex
{
    SkillRegion Region { get; }
    int Index { get; }
    string ToString();
    DeckIndex Reify();
}