
public class BattleResultSignal : Signal
{
    public BattleResultState State;

    public BattleResultSignal(BattleResultState state)
    {
        State = state;
    }

    public enum BattleResultState
    {
        Win,
        Lose,
    }
}
