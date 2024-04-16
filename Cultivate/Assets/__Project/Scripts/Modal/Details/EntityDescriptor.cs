
public class EntityDescriptor
{
    private int Ladder;

    public EntityDescriptor(int ladder)
    {
        Ladder = ladder;
    }

    public bool CanDraw(RunEntity entity)
    {
        return Ladder == entity.GetLadder();
    }
}
