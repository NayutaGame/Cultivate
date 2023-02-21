using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDetails
{
    public StageEntity Actor;
    public bool Swift;
    public bool SwiftUsed;
    public bool Encore;
    public bool EncoreUsed;
    public bool SkipTurn;
    public bool SkipChip;

    public TurnDetails(StageEntity actor, bool swift = false, bool swiftUsed = false, bool encore = false, bool encoreUsed = false, bool skipTurn = false, bool skipChip = false)
    {
        Actor = actor;
        Swift = swift;
        SwiftUsed = swiftUsed;
        Encore = encore;
        EncoreUsed = encoreUsed;
        SkipTurn = skipTurn;
        SkipChip = skipChip;
    }
}
