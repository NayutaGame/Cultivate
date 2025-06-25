
public class BattleResultSignal : Signal
{
    public bool Win;

    public BattleResultSignal(bool win)
    {
        Win = win;
    }
}
