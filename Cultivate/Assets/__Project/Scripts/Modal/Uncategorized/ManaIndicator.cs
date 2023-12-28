
public struct ManaIndicator
{
    public enum ManaCostState
    {
        Unwritten,
        Normal,
        Reduced,
        Shortage,
    }

    public ManaCostState State;
    public int LiteralCost;
    public int ActualCost;

    public ManaIndicator(ManaCostState state, int literalCost, int actualCost)
    {
        State = state;
        LiteralCost = literalCost;
        ActualCost = actualCost;
    }

    public static ManaIndicator Default() => new() { State = ManaCostState.Unwritten };
}
