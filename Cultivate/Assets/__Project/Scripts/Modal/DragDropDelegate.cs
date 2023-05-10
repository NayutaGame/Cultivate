using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DragDropDelegate
{
    private int _distinctItems;
    private Func<IDragDrop, IDragDrop, bool>[] _dragDropTable;
    private Func<IDragDrop, int> _getID;

    public Func<IDragDrop, IDragDrop, bool> this[int from, int to]
        => _dragDropTable[to + from * _distinctItems];

    public Func<IDragDrop, IDragDrop, bool> this[IDragDrop from, IDragDrop to]
    {
        get
        {
            if (from.GetDragDropDelegate() != to.GetDragDropDelegate())
                return null;

            return this[GetID(from), GetID(to)];
        }
    }


    public DragDropDelegate(int distinctItems, Func<IDragDrop, IDragDrop, bool>[] dragDropTable, Func<IDragDrop, int> getID)
    {
        _distinctItems = distinctItems;
        _dragDropTable = dragDropTable;
        _getID = getID;
    }

    private int GetID(IDragDrop item)
        => _getID(item);

    public bool CanDrag(IDragDrop item)
    {
        for (int i = 0; i < _distinctItems; i++)
        {
            var func = this[GetID(item), i];
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
