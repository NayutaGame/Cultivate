using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromFieldToBarterDetails : RunClosureDetails
{
    public SkillSlot FromSlot;
    public DeckIndex FromIndex;

    public FromFieldToBarterDetails(SkillSlot slot, DeckIndex deckIndex)
    {
        FromSlot = slot;
        FromIndex = deckIndex;
    }
}