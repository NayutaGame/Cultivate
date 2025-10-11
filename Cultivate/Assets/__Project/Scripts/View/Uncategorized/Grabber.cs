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
    private SlotView _view;

    public void SetHover(SlotView view)
    {
        if (_state == GrabState.Drag)
            return;
        
        if (_state == GrabState.Hover)
            Release(_view);
        
        _state = GrabState.Hover;
        _view = view;
        _view.GetContentView().GetRect().SetParent(GetRect());
    }

    public void SetDrag(SlotView view)
    {
        if (_state == GrabState.Drag)
            return;

        if (_state == GrabState.Hover)
        {
            if (_view != view)
                _view.GetAnimator().SetStateAsync(1);
            Release(_view);
        }
        
        _state = GrabState.Drag;
        _view = view;
        _view.GetContentView().GetRect().SetParent(GetRect());
    }

    public void Release(SlotView view)
    {
        Debug.Log("Release");
        _state = GrabState.Empty;
        if (_view == null || view != _view)
            return;
        
        ListView parent = _view.GetParentListView();
        if (parent != null)
        {
            parent.RecoverDelegatingView(_view);
        }
        else
        {
            _view.GetContentView().GetRect().SetParent(_view.GetRect());
        }
        
        _view = null;
    }

    public void SetPosition(PointerEventData eventData)
    {
        Vector3 position = CameraManager.UI2World(eventData.position);
        GetRect().position = position;
    }
}