using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractDelegate
{
    private int _distinctItems;

    private Func<IInteractable, int?> _getId;

    private Func<IInteractable, IInteractable, bool>[] _dragDropTable;
    private Func<IInteractable, IInteractable, bool> GetDragDrop(int fromId, int toId)
        => _dragDropTable?[toId + fromId * _distinctItems];
    private Func<IInteractable, IInteractable, bool> GetDragDrop(IInteractable fromView, IInteractable toView)
    {
        if (fromView.GetDelegate() != toView.GetDelegate())
            return null;

        int? fromId = _getId(fromView);
        int? toId = _getId(toView);
        if (!fromId.HasValue || !toId.HasValue)
            return null;

        return GetDragDrop(fromId.Value, toId.Value);
    }
    public bool CanDrag(IInteractable view)
    {
        int? viewId = _getId(view);
        if (!viewId.HasValue)
            return false;

        for (int i = 0; i < _distinctItems; i++)
        {
            var func = GetDragDrop(viewId.Value, i);
            if (func != null)
                return true;
        }

        return false;
    }

    public bool DragDrop(IInteractable fromView, IInteractable toView)
        => GetDragDrop(fromView, toView)?.Invoke(fromView, toView) ?? false;



    private Func<IInteractable, bool>[] _rMouseTable;
    private Func<IInteractable, bool> GetRMouse(int viewId)
        => _rMouseTable?[viewId];
    private Func<IInteractable, bool> GetRMouse(IInteractable view)
    {
        int? viewId = _getId(view);
        if (!viewId.HasValue)
            return null;

        return GetRMouse(viewId.Value);
    }
    public bool RMouse(IInteractable view)
        => GetRMouse(view)?.Invoke(view) ?? false;



    private Func<IInteractable, bool>[] _lMouseTable;
    private Func<IInteractable, bool> GetLMouse(int viewId)
        => _lMouseTable?[viewId];
    private Func<IInteractable, bool> GetLMouse(IInteractable view)
    {
        int? viewId = _getId(view);
        if (!viewId.HasValue)
            return null;

        return GetLMouse(viewId.Value);
    }
    public bool LMouse(IInteractable view)
        => GetLMouse(view)?.Invoke(view) ?? false;



    public static readonly int POINTER_ENTER = 0;
    public static readonly int POINTER_EXIT = 1;
    public static readonly int POINTER_MOVE = 2;

    private Func<IInteractable, PointerEventData, bool>[] _handleTable;
    private Func<IInteractable, PointerEventData, bool> GetHandle(int gestureId, int viewId)
        => _handleTable?[gestureId * _distinctItems + viewId];
    private Func<IInteractable, PointerEventData, bool> GetHandle(int gestureId, IInteractable view)
    {
        int? viewId = _getId(view);
        if (!viewId.HasValue)
            return null;

        return GetHandle(gestureId, viewId.Value);
    }

    public bool Handle(int gestureId, IInteractable view, PointerEventData eventData)
    {
        Func<IInteractable, PointerEventData, bool> handle = GetHandle(gestureId, view);
        return handle?.Invoke(view, eventData) ?? false;
    }

    public InteractDelegate(int distinctItems,
        Func<IInteractable, int?> getId,
        Func<IInteractable, IInteractable, bool>[] dragDropTable = null,
        Func<IInteractable, PointerEventData, bool>[] handleTable = null,
        Func<IInteractable, bool>[] lMouseTable = null,
        Func<IInteractable, bool>[] rMouseTable = null)
    {
        _distinctItems = distinctItems;
        _getId = getId;
        _dragDropTable = dragDropTable;
        _handleTable = handleTable;
        _lMouseTable = lMouseTable;
        _rMouseTable = rMouseTable;
    }
}
