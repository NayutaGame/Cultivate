using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDetails
{
    public StageEntity Owner;
    public int SlotIndex;

    public TurnDetails(StageEntity owner, int slotIndex)
    {
        Owner = owner;
        SlotIndex = slotIndex;
    }
}
