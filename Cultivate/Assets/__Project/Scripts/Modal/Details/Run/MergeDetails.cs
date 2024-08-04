
public class MergeDetails : EventDetails
{
    public RunSkill Lhs;
    public RunSkill Rhs;
    
    public MergeDetails(RunSkill lhs, RunSkill rhs)
    {
        Lhs = lhs;
        Rhs = rhs;
    }
}
