
public class TryCommitDetails : EventDetails
{
    public StageEntity Owner;

    public enum StageState
    {
        OnGoing,
        HomeVictory,
        HomeFailure,
    }

    public StageState State;

    public TryCommitDetails(StageEntity owner)
    {
        Owner = owner;
        State = StageState.OnGoing;
    }
}
