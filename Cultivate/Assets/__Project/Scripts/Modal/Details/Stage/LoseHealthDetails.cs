
public class LoseHealthDetails : StageClosureDetails
{
    public StageEntity Owner;
    public int Value;
    public bool CausedByAttack;

    public LoseHealthDetails(StageEntity owner, int value, bool causedByAttack, bool induced)
    {
        Owner = owner;
        Value = value;
        CausedByAttack = causedByAttack;
        Induced = induced;
    }
}
