using UnityEngine;
using UnityEngine.EventSystems;

public class Grabber : MonoBehaviour
{
    private RectTransform _rect;
    public RectTransform GetRect() => _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _state = GrabState.Empty;
    }

    private enum GrabState
    {
        Empty,
        Hover,
        Drag,
    }

    private GrabState _state;
    private SlotView _slotView;

    private SlotView SlotView
    {
        set
        {
            if (_slotView != null)
            {
                IListView parent = _slotView.GetParentListView();
                if (parent != null)
                {
                    parent.RecoverSlotView(_slotView);
                }
                else
                {
                    _slotView.GetContentView().GetRect().SetParent(_slotView.GetRect(), true);
                }
            }

            _slotView = value;
            if (_slotView != null)
            {
                _slotView.GetContentView().GetRect().SetParent(GetRect(), true);
            }
        }
    }

    public void SetHover(SlotView slotView)
    {
        if (_state == GrabState.Drag)
            return;
        
        _state = GrabState.Hover;
        SlotView = slotView;
    }

    public void SetDrag(SlotView slotView)
    {
        if (_state == GrabState.Drag)
            return;

        if (_state == GrabState.Hover)
        {
            if (_slotView != slotView)
                _slotView.GetAnimator().SetStateAsync(1);
        }
        
        _state = GrabState.Drag;
        SlotView = slotView;
    }

    public void Release(SlotView slotView)
    {
        if (slotView != _slotView)
            return;
        
        _state = GrabState.Empty;
        SlotView = null;
    }

    public void SetPosition(PointerEventData eventData)
    {
        Vector3 position = CameraManager.UI2World(eventData.position);
        GetRect().position = position;
    }
}