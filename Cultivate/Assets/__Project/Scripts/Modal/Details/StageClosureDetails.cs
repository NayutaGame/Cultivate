
public class StageClosureDetails : ClosureDetails
{
    public StageEnvironment Env;
    public bool Induced;
    
    public StageClosureDetails(StageEnvironment env, bool induced = false)
    {
        Env = env;
        Induced = induced;
    }
}
