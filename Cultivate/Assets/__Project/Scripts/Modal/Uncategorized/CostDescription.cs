
public class CostDescription
{
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

    public static CostDescription Empty
        => new(CostType.Empty, CostState.Normal, 0);

    public CostDescription Clone()
        => new(Type, State, Value);
}
