
public class SetDGoldDetails : EventDetails
{
    public int Value;
    public bool Consume;

    public SetDGoldDetails(int value, bool consume)
    {
        Value = value;
        Consume = consume;
    }
}
