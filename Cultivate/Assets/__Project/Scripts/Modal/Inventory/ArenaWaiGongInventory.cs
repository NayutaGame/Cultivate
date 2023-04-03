using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ArenaWaiGongInventory: Inventory<RunChip>
{
    public ArenaWaiGongInventory()
    {
        Clear();
        Encyclopedia.ChipCategory.Traversal.FilterObj(chip => chip is WaiGongEntry).Map(chip => new RunChip(chip, chip.JingJieRange.Start)).Do(Add);
    }
}
