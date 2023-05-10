using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DragDropDelegate
{
    private int _distinctItems;
    private Func<IDragDrop, IDragDrop, bool>[] _dragDropTable;
    private Func<IDragDrop, int?> _getID;

    public Func<IDragDrop, IDragDrop, bool> this[int from, int to]
        => _dragDropTable[to + from * _distinctItems];

    public Func<IDragDrop, IDragDrop, bool> this[IDragDrop from, IDragDrop to]
    {
        get
        {
            if (from.GetDragDropDelegate() != to.GetDragDropDelegate())
                return null;

            int? fromId = _getID(from);
            int? toId = _getID(to);
            if (!fromId.HasValue || !toId.HasValue)
                return null;

            return this[fromId.Value, toId.Value];
        }
    }


    public DragDropDelegate(int distinctItems, Func<IDragDrop, IDragDrop, bool>[] dragDropTable, Func<IDragDrop, int?> getID)
    {
        _distinctItems = distinctItems;
        _dragDropTable = dragDropTable;
        _getID = getID;
    }

    public bool CanDrag(IDragDrop item)
    {
        int? itemId = _getID(item);
        if (!itemId.HasValue)
            return false;

        for (int i = 0; i < _distinctItems; i++)
        {
            var func = this[itemId.Value, i];
            if (func != null)
                return true;
        }

        return false;
    }

    public bool DragDrop(IDragDrop from, IDragDrop to)
    {
        return this[from, to]?.Invoke(from, to) ?? false;
    }
}
