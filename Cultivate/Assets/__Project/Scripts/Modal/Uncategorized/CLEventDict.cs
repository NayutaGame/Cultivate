using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CLEventDict : Dictionary<string, CLEvent<StageEventDetails>>
{
    public void AddCallback(string eventId, int order, Func<StageEventDetails, Task> callback)
    {
        if (!ContainsKey(eventId))
            this[eventId] = new();

        this[eventId].Add(order, callback);
    }

    public void RemoveCallback(string eventId, Func<StageEventDetails, Task> callback)
    {
        this[eventId].Remove(callback);
    }

    public async Task FireEvent(string eventId, StageEventDetails stageEventDetails)
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
