
using CLLibrary;

public class CostDescription
{
    public CostType Type;
    public CostState State;
    private int _value;

    public int Value
    {
        get => _value;
        set => _value = value.ClampLower(0);
    }
    
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
