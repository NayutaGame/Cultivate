using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ArenaWaiGongInventory: Inventory<RunChip>
{
    public override string GetIndexPathString() => "TryGetArenaChip";

    public ArenaWaiGongInventory()
    {
        Clear();
        Encyclopedia.ChipCategory.Traversal.FilterObj(chip => chip is WaiGongEntry).Map(chip => new RunChip(chip)).Do(Add);
    }
}
