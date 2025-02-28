
public class RunCommitDetails : RunClosureDetails
{
    public RunEnvironment RunEnvironment { get; }

    public RunCommitDetails(RunEnvironment runEnvironment)
    {
        RunEnvironment = runEnvironment;
    }
}
