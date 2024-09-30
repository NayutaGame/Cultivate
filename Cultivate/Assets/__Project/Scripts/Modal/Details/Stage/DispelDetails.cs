
public class DispelDetails : ClosureDetails
{
    public StageEntity Owner;
    public int Stack;

    public DispelDetails(StageEntity owner, int stack)
    {
        Owner = owner;
        Stack = stack;
    }

    public DispelDetails Clone() => new(Owner, Stack);
}
