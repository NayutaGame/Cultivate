using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CLEventDict : Dictionary<int, CLEvent<StageEventDetails>>
{
    public static readonly int START_STAGE        = 0;
    public static readonly int END_STAGE          = 1;
    public static readonly int START_ROUND        = 2;
    public static readonly int END_ROUND          = 3;
    public static readonly int START_TURN         = 4;
    public static readonly int END_TURN           = 5;
    public static readonly int START_STEP         = 6;
    public static readonly int END_STEP           = 7;
    public static readonly int FORMATION_WILL_ADD = 8;
    public static readonly int FORMATION_DID_ADD  = 9;
    public static readonly int WILL_ATTACK        = 10;
    public static readonly int DID_ATTACK         = 11;
    public static readonly int DID_EVADE          = 12;
    public static readonly int WILL_DAMAGE        = 13;
    public static readonly int DID_DAMAGE         = 14;
    public static readonly int WILL_LOSE_HEALTH   = 15;
    public static readonly int DID_LOSE_HEALTH    = 16;
    public static readonly int WILL_HEAL          = 17;
    public static readonly int DID_HEAL           = 18;
    public static readonly int WILL_BUFF          = 19;
    public static readonly int DID_BUFF           = 20;
    public static readonly int ARMOR_WILL_GAIN    = 21;
    public static readonly int ARMOR_DID_GAIN     = 22;
    public static readonly int ARMOR_WILL_LOSE    = 23;
    public static readonly int ARMOR_DID_LOSE     = 24;
    public static readonly int WILL_DISPEL        = 25;
    public static readonly int DID_DISPEL         = 26;
    public static readonly int WILL_EXHAUST       = 27;
    public static readonly int DID_EXHAUST        = 28;
    public static readonly int GAIN_BUFF          = 29;
    public static readonly int LOSE_BUFF          = 30;
    public static readonly int STACK_WILL_CHANGE  = 31;
    public static readonly int STACK_DID_CHANGE   = 32;
    public static readonly int GAIN_FORMATION     = 33;
    public static readonly int LOSE_FORMATION     = 34;
    public static readonly int WILL_SWIFT         = 35;
    public static readonly int DID_SWIFT          = 36;
    public static readonly int WILL_EXECUTE       = 37;
    public static readonly int DID_EXECUTE        = 38;
    public static readonly int WILL_CHANNEL       = 39;
    public static readonly int DID_CHANNEL        = 40;
    public static readonly int COUNT              = 41;

    public void AddCallback(int eventId, int order, Func<StageEventDetails, Task> callback)
    {
        if (!ContainsKey(eventId))
            this[eventId] = new();

        this[eventId].Add(order, callback);
    }

    public void RemoveCallback(int eventId, Func<StageEventDetails, Task> callback)
    {
        this[eventId].Remove(callback);
    }

    public async Task FireEvent(int eventId, StageEventDetails stageEventDetails)
    {
        if (!ContainsKey(eventId))
            return;
        CLEvent<StageEventDetails> clEvent = this[eventId];
        foreach (Func<StageEventDetails, Task> func in clEvent.Traversal())
        {
            if (stageEventDetails.Cancel) return;
            await func(stageEventDetails);
        }
    }
}
