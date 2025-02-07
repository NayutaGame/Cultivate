
public class StageCommitDetails : ClosureDetails
{
    public StageEnvironment Env;
    public int Turn;
    public int Whosturn;
    public bool Forced;
    
    public StageEntity Owner;
    
    public int Flag;

    public StageCommitDetails(StageEnvironment env, int turn, int whosTurn, bool forced)
    {
        Env = env;
        Turn = turn;
        Whosturn = whosTurn;
        Forced = forced;

        Owner = env.Entities[whosTurn];
        
        Flag = 0;
    }
}
