
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractHandler
{
    private int _distinctItems;

    private Func<InteractBehaviour, int?> _getId;

    private Action<InteractBehaviour, InteractBehaviour, PointerEventData>[] _dragDropTable;
    private Action<InteractBehaviour, InteractBehaviour, PointerEventData> GetDragDrop(int fromId, int toId)
        => _dragDropTable?[toId + fromId * _distinctItems];
    public void SetDragDrop(int fromId, int toId, Action<InteractBehaviour, InteractBehaviour, PointerEventData> func)
    {
        _dragDropTable ??= new Action<InteractBehaviour, InteractBehaviour, PointerEventData>[_distinctItems * _distinctItems];
        _dragDropTable[toId + fromId * _distinctItems] = func;
    }

    private Action<InteractBehaviour, InteractBehaviour, PointerEventData> GetDragDrop(InteractBehaviour fromView, InteractBehaviour toView)
    {
        InteractHandler fromHandler = fromView?.GetHandler();
        InteractHandler toHandler = toView?.GetHandler();
        if (fromHandler == null || toHandler == null || fromHandler != toHandler)
            return null;

        int? fromId = _getId(fromView);
        int? toId = _getId(toView);
        if (!fromId.HasValue || !toId.HasValue)
            return null;

        return GetDragDrop(fromId.Value, toId.Value);
    }
    public bool CanDrag(InteractBehaviour view)
    {
        int? viewId = _getId(view);
        if (!viewId.HasValue)
            return false;

        for (int i = 0; i < _distinctItems; i++)
        {
            Action<InteractBehaviour, InteractBehaviour, PointerEventData> func = GetDragDrop(viewId.Value, i);
            if (func != null)
                return true;
        }

        return false;
    }

    public void DragDrop(InteractBehaviour fromView, InteractBehaviour toView, PointerEventData eventData)
        => GetDragDrop(fromView, toView)?.Invoke(fromView, toView, eventData);



    public static readonly int POINTER_ENTER = 0;
    public static readonly int POINTER_EXIT = 1;
    public static readonly int POINTER_MOVE = 2;
    public static readonly int POINTER_LEFT_CLICK = 3;
    public static readonly int POINTER_RIGHT_CLICK = 4;
    public static readonly int BEGIN_DRAG = 5;
    public static readonly int END_DRAG = 6;
    public static readonly int DRAG = 7;
    public static readonly int GESTURE_COUNT = 8;

    private Action<InteractBehaviour, PointerEventData>[] _handleTable;
    private Action<InteractBehaviour, PointerEventData> GetHandle(int gestureId, int viewId)
        => _handleTable?[gestureId * _distinctItems + viewId];
    public void SetHandle(int gestureId, int viewId, Action<InteractBehaviour, PointerEventData> func)
    {
        _handleTable ??= new Action<InteractBehaviour, PointerEventData>[_distinctItems * GESTURE_COUNT];
        _handleTable[gestureId * _distinctItems + viewId] = func;
    }

    private Action<InteractBehaviour, PointerEventData> GetHandle(int gestureId, InteractBehaviour view)
    {
        int? viewId = _getId(view);
        if (!viewId.HasValue)
            return null;

        return GetHandle(gestureId, viewId.Value);
    }

    public void Handle(int gestureId, InteractBehaviour view, PointerEventData eventData)
        => GetHandle(gestureId, view)?.Invoke(view, eventData);

    public InteractHandler(int distinctItems,
        Func<InteractBehaviour, int?> getId,
        Action<InteractBehaviour, InteractBehaviour, PointerEventData>[] dragDropTable = null,
        Action<InteractBehaviour, PointerEventData>[] handleTable = null)
    {
        _distinctItems = distinctItems;
        _getId = getId;
        _dragDropTable = dragDropTable;
        _handleTable = handleTable;
    }
}
