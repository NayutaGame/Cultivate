
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractHandler
{
    private int _distinctItems;

    private Func<InteractDelegate, int?> _getId;

    private Action<InteractDelegate, InteractDelegate>[] _dragDropTable;
    private Action<InteractDelegate, InteractDelegate> GetDragDrop(int fromId, int toId)
        => _dragDropTable?[toId + fromId * _distinctItems];
    public void SetDragDrop(int fromId, int toId, Action<InteractDelegate, InteractDelegate> func)
    {
        _dragDropTable ??= new Action<InteractDelegate, InteractDelegate>[_distinctItems * _distinctItems];
        _dragDropTable[toId + fromId * _distinctItems] = func;
    }

    private Action<InteractDelegate, InteractDelegate> GetDragDrop(InteractDelegate fromView, InteractDelegate toView)
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
    public bool CanDrag(InteractDelegate view)
    {
        int? viewId = _getId(view);
        if (!viewId.HasValue)
            return false;

        for (int i = 0; i < _distinctItems; i++)
        {
            Action<InteractDelegate, InteractDelegate> func = GetDragDrop(viewId.Value, i);
            if (func != null)
                return true;
        }

        return false;
    }

    public void DragDrop(InteractDelegate fromView, InteractDelegate toView)
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

    private Action<InteractDelegate, PointerEventData>[] _handleTable;
    private Action<InteractDelegate, PointerEventData> GetHandle(int gestureId, int viewId)
        => _handleTable?[gestureId * _distinctItems + viewId];
    public void SetHandle(int gestureId, int viewId, Action<InteractDelegate, PointerEventData> func)
    {
        _handleTable ??= new Action<InteractDelegate, PointerEventData>[_distinctItems * GESTURE_COUNT];
        _handleTable[gestureId * _distinctItems + viewId] = func;
    }

    private Action<InteractDelegate, PointerEventData> GetHandle(int gestureId, InteractDelegate view)
    {
        int? viewId = _getId(view);
        if (!viewId.HasValue)
            return null;

        return GetHandle(gestureId, viewId.Value);
    }

    public void Handle(int gestureId, InteractDelegate view, PointerEventData eventData)
        => GetHandle(gestureId, view)?.Invoke(view, eventData);

    public InteractHandler(int distinctItems,
        Func<InteractDelegate, int?> getId,
        Action<InteractDelegate, InteractDelegate>[] dragDropTable = null,
        Action<InteractDelegate, PointerEventData>[] handleTable = null)
    {
        _distinctItems = distinctItems;
        _getId = getId;
        _dragDropTable = dragDropTable;
        _handleTable = handleTable;
    }
}
