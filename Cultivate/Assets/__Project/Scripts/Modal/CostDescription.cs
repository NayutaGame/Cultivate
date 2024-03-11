
public struct CostDescription
{
    public enum CostType
    {
        Empty,
        Mana,
        Health,
        Channel,
        Armor,
    }
    
    public enum CostState
    {
        Unwritten,
        Normal,
        Reduced,
        Shortage,
    }

    public CostType Type;
    public CostState State;
    public int Value;

    public CostDescription(CostType type, CostState state, int value)
    {
        Type = type;
        State = state;
        Value = value;
    }

    public int ByType(CostType type)
    {
        if (Type == type)
            return Value;
        return 0;
    }
    
    public static CostDescription Default() => new() { State = CostState.Unwritten };
}
