
public class StackChangeDetails<SM, S>
{
    private S _fromState;
    private S _toState;

    public StackChangeDetails(S fromState, S toState)
    {
        _fromState = fromState;
        _toState = toState;
    }
}
