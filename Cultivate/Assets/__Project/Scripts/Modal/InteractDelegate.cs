using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class InteractDelegate
{
    private int _distinctItems;
    private Func<IInteractable, IInteractable, bool>[] _dragDropTable;
    private Func<IInteractable, bool>[] _rMouseTable;
    private Func<IInteractable, int?> _getID;

    private Func<IInteractable, IInteractable, bool> GetDragDrop(int from, int to)
        => _dragDropTable[to + from * _distinctItems];

    private Func<IInteractable, IInteractable, bool> GetDragDrop(IInteractable from, IInteractable to)
    {
        if (from.GetInteractDelegate() != to.GetInteractDelegate())
            return null;

        int? fromId = _getID(from);
        int? toId = _getID(to);
        if (!fromId.HasValue || !toId.HasValue)
            return null;

        return GetDragDrop(fromId.Value, toId.Value);
    }

    private Func<IInteractable, bool> GetRMouse(int item)
        => _rMouseTable[item];

    private Func<IInteractable, bool> GetRMouse(IInteractable item)
    {
        int? itemId = _getID(item);
        if (!itemId.HasValue)
            return null;

        return GetRMouse(itemId.Value);
    }

    public InteractDelegate(int distinctItems, Func<IInteractable, int?> getID,
        Func<IInteractable, IInteractable, bool>[] dragDropTable,
        Func<IInteractable, bool>[] rMouseTable)
    {
        _distinctItems = distinctItems;
        _getID = getID;
        _dragDropTable = dragDropTable;
        _rMouseTable = rMouseTable;
    }

    public bool CanDrag(IInteractable item)
    {
        int? itemId = _getID(item);
        if (!itemId.HasValue)
            return false;

        for (int i = 0; i < _distinctItems; i++)
        {
            var func = GetDragDrop(itemId.Value, i);
            if (func != null)
                return true;
        }

        return false;
    }

    public bool DragDrop(IInteractable from, IInteractable to)
        => GetDragDrop(from, to)?.Invoke(from, to) ?? false;

    public bool RMouse(IInteractable item)
        => GetRMouse(item)?.Invoke(item) ?? false;
}
