
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLLibrary;

public class StageClosureDict : Dictionary<int, StageClosureList>
{
    public static readonly int WIL_STAGE                = 0;
    public static readonly int DID_STAGE                = 1;
    public static readonly int WIL_BODY                 = 2;
    public static readonly int DID_BODY                 = 3;
    public static readonly int WIL_TURN                 = 4;
    public static readonly int DID_TURN                 = 5;
    public static readonly int WIL_ACTION               = 6;
    public static readonly int DID_ACTION               = 7;
    public static readonly int WIL_EXECUTE              = 8;
    public static readonly int DID_EXECUTE              = 9;
    public static readonly int WIL_CAST                 = 10;
    public static readonly int DID_CAST                 = 11;
    public static readonly int WIL_START_STAGE_CAST     = 12;
    public static readonly int DID_START_STAGE_CAST     = 13;
    public static readonly int WIL_STEP                 = 14;
    public static readonly int DID_STEP                 = 15;
    public static readonly int WIL_ROUND                = 16;
    public static readonly int DID_ROUND                = 17;
    public static readonly int WIL_ADD_FORMATION        = 18;
    public static readonly int DID_ADD_FORMATION        = 19;
    public static readonly int WIL_FULL_ATTACK          = 20;
    public static readonly int DID_FULL_ATTACK          = 21;
    public static readonly int WIL_ATTACK               = 22;
    public static readonly int DID_ATTACK               = 23;
    public static readonly int WIL_INDIRECT             = 24;
    public static readonly int DID_INDIRECT             = 25;
    public static readonly int DID_EVADE                = 26;
    public static readonly int WIL_DAMAGE               = 27;
    public static readonly int DID_DAMAGE               = 28;
    public static readonly int UNDAMAGED                = 29;
    public static readonly int WIL_LOSE_HEALTH          = 30;
    public static readonly int DID_LOSE_HEALTH          = 31;
    public static readonly int WIL_HEAL                 = 32;
    public static readonly int DID_HEAL                 = 33;
    public static readonly int WIL_GAIN_BUFF            = 34;
    public static readonly int DID_GAIN_BUFF            = 35;
    public static readonly int WIL_LOSE_BUFF            = 36;
    public static readonly int DID_LOSE_BUFF            = 37;
    public static readonly int WIL_GAIN_ARMOR           = 38;
    public static readonly int DID_GAIN_ARMOR           = 39;
    public static readonly int WIL_LOSE_ARMOR           = 40;
    public static readonly int DID_LOSE_ARMOR           = 41;
    public static readonly int WIL_EXHAUST              = 42;
    public static readonly int DID_EXHAUST              = 43;
    public static readonly int GAIN_FORMATION           = 44;
    public static readonly int LOSE_FORMATION           = 45;
    public static readonly int WIL_CHANNEL              = 46;
    public static readonly int DID_CHANNEL              = 47;
    public static readonly int WIL_CHANNEL_COST         = 48;
    public static readonly int DID_CHANNEL_COST         = 49;
    public static readonly int WIL_MANA_COST            = 50;
    public static readonly int DID_MANA_COST            = 51;
    public static readonly int WIL_ARMOR_COST           = 52;
    public static readonly int DID_ARMOR_COST           = 53;
    public static readonly int WIL_HEALTH_COST          = 54;
    public static readonly int DID_HEALTH_COST          = 55;
    public static readonly int WIL_MANA_SHORTAGE        = 56;
    public static readonly int DID_MANA_SHORTAGE        = 57;
    public static readonly int WIL_ARMOR_SHORTAGE       = 58;
    public static readonly int DID_ARMOR_SHORTAGE       = 59;
    public static readonly int WIL_CYCLE                = 60;
    public static readonly int DID_CYCLE                = 61;
    public static readonly int WIL_DISPEL               = 62;
    public static readonly int DID_DISPEL               = 63;
    public static readonly int WIL_COMMIT               = 64;
    public static readonly int DID_COMMIT               = 65;

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
