using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
    public static readonly int WIL_ROUND                = 8;
    public static readonly int DID_ROUND                = 9;
    public static readonly int START_STEP               = 10;
    public static readonly int END_STEP                 = 11;
    public static readonly int FORMATION_WILL_ADD       = 12;
    public static readonly int FORMATION_DID_ADD        = 13;
    public static readonly int WILL_ATTACK              = 14;
    public static readonly int DID_ATTACK               = 15;
    public static readonly int WILL_INDIRECT            = 16;
    public static readonly int DID_INDIRECT             = 17;
    public static readonly int DID_EVADE                = 18;
    public static readonly int WILL_DAMAGE              = 19;
    public static readonly int DID_DAMAGE               = 20;
    public static readonly int WILL_LOSE_HEALTH         = 21;
    public static readonly int DID_LOSE_HEALTH          = 22;
    public static readonly int WILL_HEAL                = 23;
    public static readonly int DID_HEAL                 = 24;
    public static readonly int BUFF_WILL_GAIN           = 25;
    public static readonly int BUFF_DID_GAIN            = 26;
    public static readonly int ARMOR_WILL_GAIN          = 27;
    public static readonly int ARMOR_DID_GAIN           = 28;
    public static readonly int ARMOR_WILL_LOSE          = 29;
    public static readonly int ARMOR_DID_LOSE           = 30;
    public static readonly int BUFF_WILL_LOSE           = 31;
    public static readonly int BUFF_DID_LOSE            = 32;
    public static readonly int WILL_EXHAUST             = 33;
    public static readonly int DID_EXHAUST              = 34;
    public static readonly int BUFF_APPEAR              = 35;
    public static readonly int BUFF_DISAPPEAR           = 36;
    public static readonly int STACK_WILL_CHANGE        = 37;
    public static readonly int STACK_DID_CHANGE         = 38;
    public static readonly int GAIN_FORMATION           = 39;
    public static readonly int LOSE_FORMATION           = 40;
    public static readonly int WILL_EXECUTE             = 41;
    public static readonly int DID_EXECUTE              = 42;
    public static readonly int WILL_CHANNEL             = 43;
    public static readonly int DID_CHANNEL              = 44;
    public static readonly int WILL_CHANNEL_COST        = 45;
    public static readonly int DID_CHANNEL_COST         = 46;
    public static readonly int WILL_MANA_COST           = 47;
    public static readonly int DID_MANA_COST            = 48;
    public static readonly int WILL_ARMOR_COST          = 49;
    public static readonly int DID_ARMOR_COST           = 50;
    public static readonly int WILL_HEALTH_COST         = 51;
    public static readonly int DID_HEALTH_COST          = 52;
    public static readonly int WILL_MANA_SHORTAGE       = 53;
    public static readonly int DID_MANA_SHORTAGE        = 54;
    public static readonly int WILL_ARMOR_SHORTAGE      = 55;
    public static readonly int DID_ARMOR_SHORTAGE       = 56;
    public static readonly int WILL_CYCLE               = 57;
    public static readonly int DID_CYCLE                = 58;
    public static readonly int WILL_DISPEL              = 59;
    public static readonly int DID_DISPEL               = 60;

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
