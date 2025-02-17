
public class RunCommitDetails : ClosureDetails
{
    public RunEnvironment RunEnvironment { get; }

    public RunCommitDetails(RunEnvironment runEnvironment)
    {
        RunEnvironment = runEnvironment;
    }
}
