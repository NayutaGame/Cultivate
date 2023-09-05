using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
