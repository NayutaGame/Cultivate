
public class DispelDetails : ClosureDetails
{
    public StageEntity Owner;
    public int Stack;

    public DispelDetails(StageEntity owner, int stack, bool induced)
    {
        Owner = owner;
        Stack = stack;
        Induced = induced;
    }

    public DispelDetails Clone() => new(Owner, Stack, Induced);
}
