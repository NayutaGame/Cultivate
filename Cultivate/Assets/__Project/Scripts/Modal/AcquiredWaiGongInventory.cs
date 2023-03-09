using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquiredWaiGongInventory: Inventory<AcquiredRunChip>
{
    public override string GetIndexPathString() => "TryGetAcquired";
}
