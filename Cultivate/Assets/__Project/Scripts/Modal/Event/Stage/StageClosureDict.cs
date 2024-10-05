
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLLibrary;

public class StageClosureDict : Dictionary<int, StageClosureList>
{
    public static readonly int WIL_STAGE                = 0;
    public static readonly int DID_STAGE                = 1;
    public static readonly int WIL_TURN                 = 2;
    public static readonly int DID_TURN                 = 3;
    public static readonly int WIL_ACTION               = 4;
    public static readonly int DID_ACTION               = 5;
    public static readonly int WIL_EXECUTE              = 6;
    public static readonly int DID_EXECUTE              = 7;
    public static readonly int WIL_CAST                 = 8;
    public static readonly int DID_CAST                 = 9;
    public static readonly int WIL_START_STAGE_CAST     = 10;
    public static readonly int DID_START_STAGE_CAST     = 11;
    public static readonly int WIL_STEP                 = 12;
    public static readonly int DID_STEP                 = 13;
    public static readonly int WIL_ROUND                = 14;
    public static readonly int DID_ROUND                = 15;
    public static readonly int WIL_GAIN_FORMATION       = 16;
    public static readonly int DID_GAIN_FORMATION       = 17;
    public static readonly int WIL_GAIN_BUFF            = 18;
    public static readonly int DID_GAIN_BUFF            = 19;
    public static readonly int WIL_LOSE_BUFF            = 20;
    public static readonly int DID_LOSE_BUFF            = 21;
    public static readonly int WIL_FULL_ATTACK          = 22;
    public static readonly int DID_FULL_ATTACK          = 23;
    public static readonly int WIL_ATTACK               = 24;
    public static readonly int DID_ATTACK               = 25;
    public static readonly int WIL_INDIRECT             = 26;
    public static readonly int DID_INDIRECT             = 27;
    public static readonly int DID_EVADE                = 28;
    public static readonly int WIL_DAMAGE               = 29;
    public static readonly int DID_DAMAGE               = 30;
    public static readonly int UNDAMAGED                = 31;
    public static readonly int WIL_LOSE_HEALTH          = 32;
    public static readonly int DID_LOSE_HEALTH          = 33;
    public static readonly int WIL_HEAL                 = 34;
    public static readonly int DID_HEAL                 = 35;
    public static readonly int WIL_GAIN_ARMOR           = 36;
    public static readonly int DID_GAIN_ARMOR           = 37;
    public static readonly int WIL_LOSE_ARMOR           = 38;
    public static readonly int DID_LOSE_ARMOR           = 39;
    public static readonly int WIL_EXHAUST              = 40;
    public static readonly int DID_EXHAUST              = 41;
    public static readonly int WIL_CHANNEL              = 42;
    public static readonly int DID_CHANNEL              = 43;
    public static readonly int WIL_CHANNEL_COST         = 44;
    public static readonly int DID_CHANNEL_COST         = 45;
    public static readonly int WIL_MANA_COST            = 46;
    public static readonly int DID_MANA_COST            = 47;
    public static readonly int WIL_ARMOR_COST           = 48;
    public static readonly int DID_ARMOR_COST           = 49;
    public static readonly int WIL_HEALTH_COST          = 50;
    public static readonly int DID_HEALTH_COST          = 51;
    public static readonly int WIL_MANA_SHORTAGE        = 52;
    public static readonly int DID_MANA_SHORTAGE        = 53;
    public static readonly int WIL_ARMOR_SHORTAGE       = 54;
    public static readonly int DID_ARMOR_SHORTAGE       = 55;
    public static readonly int WIL_CYCLE                = 56;
    public static readonly int DID_CYCLE                = 57;
    public static readonly int WIL_DISPEL               = 58;
    public static readonly int DID_DISPEL               = 59;
    public static readonly int WIL_COMMIT               = 60;
    public static readonly int DID_COMMIT               = 61;

    public void Register(StageClosureOwner owner, StageClosure[] closures)
    {
        closures.Do(e => Register(owner, e));
    }

    public void Register(StageClosureOwner owner, StageClosure closure)
    {
        int eventId = closure.EventId;
        if (!ContainsKey(eventId))
            this[eventId] = new();

        this[eventId].Add(owner, closure);
    }

    public void Unregister(StageClosureOwner owner, StageClosure[] closures)
    {
        closures.Do(e => Unregister(owner, e));
    }

    public void Unregister(StageClosureOwner owner, StageClosure closure)
    {
        int eventId = closure.EventId;
        this[eventId].Remove(owner);
    }

    public async Task SendEvent(int eventId, ClosureDetails closureDetails)
    {
        if (!ContainsKey(eventId))
            return;
        StageClosureList closureList = this[eventId];
        foreach (Tuple<StageClosureOwner, StageClosure> tuple in closureList.Traversal())
        {
            if (closureDetails.Cancel) return;
            await tuple.Item2.Invoke(tuple.Item1, closureDetails);
        }
    }
}
