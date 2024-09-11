
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class StageEventDict : Dictionary<int, StageEventElementList>
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
    public static readonly int WIL_ATTACK               = 20;
    public static readonly int DID_ATTACK               = 21;
    public static readonly int WIL_INDIRECT             = 22;
    public static readonly int DID_INDIRECT             = 23;
    public static readonly int DID_EVADE                = 24;
    public static readonly int WIL_DAMAGE               = 25;
    public static readonly int DID_DAMAGE               = 26;
    public static readonly int WIL_LOSE_HEALTH          = 27;
    public static readonly int DID_LOSE_HEALTH          = 28;
    public static readonly int WIL_HEAL                 = 29;
    public static readonly int DID_HEAL                 = 30;
    public static readonly int WIL_GAIN_BUFF            = 31;
    public static readonly int DID_GAIN_BUFF            = 32;
    public static readonly int WIL_LOSE_BUFF            = 33;
    public static readonly int DID_LOSE_BUFF            = 34;
    public static readonly int WIL_GAIN_ARMOR           = 35;
    public static readonly int DID_GAIN_ARMOR           = 36;
    public static readonly int WIL_LOSE_ARMOR           = 37;
    public static readonly int DID_LOSE_ARMOR           = 38;
    public static readonly int WIL_EXHAUST              = 39;
    public static readonly int DID_EXHAUST              = 40;
    public static readonly int BUFF_APPEAR              = 41;
    public static readonly int BUFF_DISAPPEAR           = 42;
    public static readonly int WIL_CHANGE_STACK         = 43;
    public static readonly int DID_CHANGE_STACK         = 44;
    public static readonly int GAIN_FORMATION           = 45;
    public static readonly int LOSE_FORMATION           = 46;
    public static readonly int WIL_CHANNEL              = 47;
    public static readonly int DID_CHANNEL              = 48;
    public static readonly int WIL_CHANNEL_COST         = 49;
    public static readonly int DID_CHANNEL_COST         = 50;
    public static readonly int WIL_MANA_COST            = 51;
    public static readonly int DID_MANA_COST            = 52;
    public static readonly int WIL_ARMOR_COST           = 53;
    public static readonly int DID_ARMOR_COST           = 54;
    public static readonly int WIL_HEALTH_COST          = 55;
    public static readonly int DID_HEALTH_COST          = 56;
    public static readonly int WIL_MANA_SHORTAGE        = 57;
    public static readonly int DID_MANA_SHORTAGE        = 58;
    public static readonly int WIL_ARMOR_SHORTAGE       = 59;
    public static readonly int DID_ARMOR_SHORTAGE       = 60;
    public static readonly int WIL_CYCLE                = 61;
    public static readonly int DID_CYCLE                = 62;
    public static readonly int WIL_DISPEL               = 63;
    public static readonly int DID_DISPEL               = 64;
    public static readonly int WIL_COMMIT               = 65;
    public static readonly int DID_COMMIT               = 66;

    public static readonly int STAGE_ENVIRONMENT        = 0;
    public static readonly int STAGE_ENTITY             = 1;
    public static readonly int STAGE_BUFF               = 2;
    public static readonly int STAGE_FORMATION          = 3;

    public void Register(StageEventListener listener, StageEventDescriptor eventDescriptor)
    {
        int eventId = eventDescriptor.EventId;
        if (!ContainsKey(eventId))
            this[eventId] = new();

        this[eventId].Add(listener, eventDescriptor);
    }

    public void Unregister(StageEventListener listener, StageEventDescriptor eventDescriptor)
    {
        int eventId = eventDescriptor.EventId;
        this[eventId].Remove(listener);
    }

    public async Task SendEvent(int eventId, EventDetails eventDetails)
    {
        if (!ContainsKey(eventId))
            return;
        StageEventElementList eventElementList = this[eventId];
        foreach (Tuple<StageEventListener, StageEventDescriptor> tuple in eventElementList.Traversal())
        {
            if (eventDetails.Cancel) return;
            await tuple.Item2.Invoke(tuple.Item1, eventDetails);
        }
    }
}
