
public class SetLevelDetails : EventDetails
{
    public int FromLevel;
    public int ToLevel;

    public SetLevelDetails(int fromLevel, int toLevel)
    {
        FromLevel = fromLevel;
        ToLevel = toLevel;
    }
}
