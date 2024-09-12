
public abstract class StepDescriptor
{
    public static readonly int[] GoldRewardTable = new int[]
    {
        1, /*1,*/ 2,
        2, 2, 4,
        4, 4, 8,
        8, 8, 16,
        16, 16, 16,
    };
    
    public abstract RunNode Draw(Map map);

    public int Ladder;
    public StepDescriptor(int ladder)
    {
        Ladder = ladder;
    }
}
