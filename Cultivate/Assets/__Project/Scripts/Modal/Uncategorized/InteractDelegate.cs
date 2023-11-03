
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractDelegate
{
    private int _distinctItems;

    private Func<IInteractable, int?> _getId;

    private Action<IInteractable, IInteractable>[] _dragDropTable;
    private Action<IInteractable, IInteractable> GetDragDrop(int fromId, int toId)
        => _dragDropTable?[toId + fromId * _distinctItems];
    public void SetDragDrop(int fromId, int toId, Action<IInteractable, IInteractable> func)
    {
        _dragDropTable ??= new Action<IInteractable, IInteractable>[_distinctItems * _distinctItems];
        _dragDropTable[toId + fromId * _distinctItems] = func;
    }

    private Action<IInteractable, IInteractable> GetDragDrop(IInteractable fromView, IInteractable toView)
    {
        InteractDelegate fromDelegate = fromView?.GetDelegate();
        InteractDelegate toDelegate = toView?.GetDelegate();
        if (fromDelegate == null || toDelegate == null || fromDelegate != toDelegate)
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
            Action<IInteractable, IInteractable> func = GetDragDrop(viewId.Value, i);
            if (func != null)
                return true;
        }

        return false;
    }

    public void DragDrop(IInteractable fromView, IInteractable toView)
        => GetDragDrop(fromView, toView)?.Invoke(fromView, toView);



    public static readonly int POINTER_ENTER = 0;
    public static readonly int POINTER_EXIT = 1;
    public static readonly int POINTER_MOVE = 2;
    public static readonly int POINTER_LEFT_CLICK = 3;
    public static readonly int POINTER_RIGHT_CLICK = 4;
    public static readonly int BEGIN_DRAG = 5;
    public static readonly int END_DRAG = 6;
    public static readonly int DRAG = 7;
    public static readonly int GESTURE_COUNT = 8;

    private Action<IInteractable, PointerEventData>[] _handleTable;
    private Action<IInteractable, PointerEventData> GetHandle(int gestureId, int viewId)
        => _handleTable?[gestureId * _distinctItems + viewId];
    public void SetHandle(int gestureId, int viewId, Action<IInteractable, PointerEventData> func)
    {
        _handleTable ??= new Action<IInteractable, PointerEventData>[_distinctItems * GESTURE_COUNT];
        _handleTable[gestureId * _distinctItems + viewId] = func;
    }

    private Action<IInteractable, PointerEventData> GetHandle(int gestureId, IInteractable view)
    {
        int? viewId = _getId(view);
        if (!viewId.HasValue)
            return null;

        return GetHandle(gestureId, viewId.Value);
    }

    public void Handle(int gestureId, IInteractable view, PointerEventData eventData)
        => GetHandle(gestureId, view)?.Invoke(view, eventData);

    public InteractDelegate(int distinctItems,
        Func<IInteractable, int?> getId,
        Action<IInteractable, IInteractable>[] dragDropTable = null,
        Action<IInteractable, PointerEventData>[] handleTable = null)
    {
        _distinctItems = distinctItems;
        _getId = getId;
        _dragDropTable = dragDropTable;
        _handleTable = handleTable;
    }
}
