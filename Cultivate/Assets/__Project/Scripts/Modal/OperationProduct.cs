using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationProduct : Product
{
    public OperationEntry _entry;

    public OperationProduct(string entryName) : this(Encyclopedia.OperationCategory[entryName]) { }
    public OperationProduct(OperationEntry entry)
    {
        _entry = entry;
    }

    public override bool CanDrop(Tile tile)
    {
        if (!_entry.IsDrag)
            return false;

        var dragProductEntry = _entry as DragOperationEntry;
        return dragProductEntry.CanDrop(this, tile);
    }

    public override void Drop(Tile tile)
    {
        var dragProductEntry = _entry as DragOperationEntry;
        dragProductEntry.Drop(this, tile);
    }

    public override string GetName() => _entry.Name;
    public override int GetCost()
    {
        return _entry.Cost;
        // return _entry.Cost(RunUsedTimes);
    }

    public override bool IsClick() => _entry.IsClick;
    public override bool IsDrag() => _entry.IsDrag;
}
