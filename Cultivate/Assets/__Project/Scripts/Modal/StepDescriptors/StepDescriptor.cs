
public abstract class StepDescriptor
{
    public abstract void Draw(Map map);

    public int Ladder;
    public StepDescriptor(int ladder)
    {
        Ladder = ladder;
    }
}
