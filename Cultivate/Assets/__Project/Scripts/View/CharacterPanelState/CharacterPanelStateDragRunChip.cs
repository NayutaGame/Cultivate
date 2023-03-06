using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelStateDragRunChip : CharacterPanelState
{
    private RunChipView _item;
    public RunChipView Item => _item;

    public CharacterPanelStateDragRunChip(RunChipView item)
    {
        _item = item;
    }
}
