
public class DrawEntityDetails
{
    private int Ladder;

    public DrawEntityDetails(int ladder)
    {
        Ladder = ladder;
    }

    public bool CanDraw(RunEntity entity)
    {
        return Ladder == entity.GetLadder();
    }
}
