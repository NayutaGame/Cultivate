
public class RunResult : Result
{
    public enum RunResultState
    {
        Unfinished,
        Defeat,
        Victory,
    }

    public RunResultState State;

    public RunResult()
    {
        State = RunResultState.Unfinished;
    }
}
