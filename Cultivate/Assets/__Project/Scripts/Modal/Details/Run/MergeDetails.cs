
public class MergeDetails : ClosureDetails
{
    public RunSkill Lhs;
    public RunSkill Rhs;

    public DeckIndex FromDeckIndex;
    public DeckIndex ToDeckIndex;
    
    public MergeDetails(RunSkill lhs, RunSkill rhs)
    {
        Lhs = lhs;
        Rhs = rhs;

        FromDeckIndex = lhs.ToDeckIndex();
        ToDeckIndex = rhs.ToDeckIndex();
    }
}
