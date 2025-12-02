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

    public void SetHover(SlotView slotView)
    {
        if (_state == GrabState.Drag)
            return;
        
        if (_state == GrabState.Hover)
            Release(_slotView);
        
        _state = GrabState.Hover;
        _slotView = slotView;
        _slotView.GetContentView().GetRect().SetParent(GetRect());
    }

    public void SetDrag(SlotView slotView)
    {
        if (_state == GrabState.Drag)
            return;

        if (_state == GrabState.Hover)
        {
            if (_slotView != slotView)
                _slotView.GetAnimator().SetStateAsync(1);
            Release(_slotView);
        }
        
        _state = GrabState.Drag;
        _slotView = slotView;
        _slotView.GetContentView().GetRect().SetParent(GetRect());
    }

    public void Release(SlotView slotView)
    {
        _state = GrabState.Empty;
        if (_slotView == null || slotView != _slotView)
            return;
        
        ListView parent = _slotView.GetParentListView();
        if (parent != null)
        {
            parent.RecoverSlotView(_slotView);
        }
        else
        {
            _slotView.GetContentView().GetRect().SetParent(_slotView.GetRect());
        }
        
        _slotView = null;
    }

    public void SetPosition(PointerEventData eventData)
    {
        Vector3 position = CameraManager.UI2World(eventData.position);
        GetRect().position = position;
    }
}