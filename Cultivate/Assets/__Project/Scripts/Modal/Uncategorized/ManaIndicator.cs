
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
    public int Cost;

    public ManaIndicator(ManaCostState state, int literalCost, int cost)
    {
        State = state;
        LiteralCost = literalCost;
        Cost = cost;
    }

    public static ManaIndicator Default() => new() { State = ManaCostState.Unwritten };
}
