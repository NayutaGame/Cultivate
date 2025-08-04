
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class SlotView : XView
{
    public static readonly int ANY = -1;
    public static readonly int FREE = 0;
    public static readonly int IDLE = 1;
    public static readonly int HOVER = 2;
    public static readonly int FOLLOW = 3;
    
    [SerializeField] private XView _contentView;
    private ListView _parentListView;
    
    [SerializeField] private bool _useGrabber = true;
    [SerializeField] public Configuration IdleConfiguration = new(localScale: Vector3.one);
    [SerializeField] public Configuration HoverConfiguration = new(localScale: 1.2f * Vector3.one);
    private bool _allowHover;
    private bool _allowDrag;
    
    public XView GetContentView() => _contentView;
    public void SetContentView(XView contentView)
    {
        _contentView = contentView;
        _contentView.SetInteractBehaviour(_interactBehaviour);
    }

    public ListView GetParentListView() => _parentListView;
    public void SetParentListView(ListView parentListView) => _parentListView = parentListView;

    public override Address GetAddress() => _contentView.GetAddress();
    public override T Get<T>() => _contentView.Get<T>();
    public override void SetAddress(Address address) => _contentView.SetAddress(address);
    public override void Refresh() => _contentView.Refresh();

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        if (_contentView != null)
            _contentView.CheckAwake();
    }

    public void SetMoveFromRectToIdle(RectTransform rect)
    {
        GetContentView().GetRect().position = rect.position;
        GetContentView().GetRect().localScale = rect.localScale;
        GetAnimator().SetStateAsync(IDLE);
    }

    public void Align()
    {
        GetContentView().GetRect().position = GetRect().position;
    }
    
    protected override Animator InitAnimator()
    {
        Animator animator = new(4, name);
        if (_useGrabber)
        {
            animator[ANY, IDLE] = EnterIdleUseGrabber;
            animator[ANY, HOVER] = EnterHoverUseGrabber;
            animator[ANY, FOLLOW] = EnterFollowUseGrabber;
        }
        else
        {
            animator[ANY, IDLE] = EnterIdle;
            animator[ANY, HOVER] = EnterHover;
            animator[ANY, FOLLOW] = EnterFollow;
        }
        animator[ANY, FREE] = EnterFree;
        animator[FREE, ANY] = ExitFree;
        return animator;
    }
    
    public bool AllowHover
    {
        get => _allowHover;
        set
        {
            _allowHover = value;
            SetInteractBehaviour(_interactBehaviour);
        }
    }
    

    public bool AllowDrag
    {
        get => _allowDrag;
        set
        {
            _allowDrag = value;
            SetInteractBehaviour(_interactBehaviour);
        }
    }

    public override void SetInteractBehaviour(InteractBehaviour ib)
    {
        if (_interactBehaviour != null)
        {
            _interactBehaviour.PointerEnterNeuron.Remove(PointerEnter);
            _interactBehaviour.PointerExitNeuron.Remove(PointerExit);
            _interactBehaviour.BeginDragNeuron.Remove(BeginDrag);
            _interactBehaviour.EndDragNeuron.Remove(EndDrag);
            _interactBehaviour.DragNeuron.Remove(Drag);
            _interactBehaviour.DragNeuron.Remove(DragUseGrabber);
            _interactBehaviour.DraggingExitNeuron.Remove(DraggingExit);
        }
        base.SetInteractBehaviour(ib);
        if (_interactBehaviour != null)
        {
            if (_allowHover)
            {
                _interactBehaviour.PointerEnterNeuron.Join(PointerEnter);
                _interactBehaviour.PointerExitNeuron.Join(PointerExit);
            }
            
            if (_allowDrag)
            {
                _interactBehaviour.BeginDragNeuron.Join(BeginDrag);
                _interactBehaviour.EndDragNeuron.Join(EndDrag);
                if (_useGrabber)
                    _interactBehaviour.DragNeuron.Join(DragUseGrabber);
                else
                    _interactBehaviour.DragNeuron.Join(Drag);
            }
            else
            {
                _interactBehaviour.DraggingExitNeuron.Join(DraggingExit);
            }
        }
    }

    private Tween EnterIdle()
        => DOTween.Sequence()
            .Append(GoToConfiguration(IdleConfiguration));

    private Tween EnterHover()
        => DOTween.Sequence()
            .Append(GoToConfiguration(HoverConfiguration));

    private Tween EnterFollow()
        => DOTween.Sequence()
            .Append(new FollowAnimation(GetContentView().GetRect(), GetRect()).GetHandle());

    private Tween EnterIdleUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberRelease)
            .Append(GoToConfiguration(IdleConfiguration));

    private Tween EnterHoverUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetHover)
            .Append(GoToConfiguration(HoverConfiguration));

    private Tween EnterFollowUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetDrag)
            .Append(new FollowAnimation(GetContentView().GetRect(), CanvasManager.Instance.GetGrabber().GetRect()).GetHandle());

    private Tween EnterFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(false));

    private Tween ExitFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(true));

    private Tween GoToConfiguration(Configuration configuration)
    {
        return new GoToConfigurationAnimation(GetRect(), GetContentView().GetRect(), configuration).GetHandle();
    }
    
    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(HOVER);
    }
    
    private void PointerExit(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(IDLE);
    }
    
    private void BeginDrag(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(FOLLOW);
    }
    
    public void EndDrag(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(IDLE);
    }
    
    private void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        GetContentView().GetRect().position = CanvasManager.Instance.UI2World(eventData.position);
    }
    
    private void DragUseGrabber(InteractBehaviour ib, PointerEventData eventData)
    {
        CanvasManager.Instance.GetGrabber().SetPosition(eventData);
    }

    private void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        GetAnimator().SetStateAsync(IDLE);
    }

    public void GrabberSetHover()
    {
        CanvasManager.Instance.GetGrabber().SetHover(this);
    }

    public void GrabberSetDrag()
    {
        CanvasManager.Instance.GetGrabber().SetDrag(this);
    }

    public void GrabberRelease()
    {
        CanvasManager.Instance.GetGrabber().Release(this);
    }
    
    private void OnDisable()
    {
        GrabberRelease();
    }
}