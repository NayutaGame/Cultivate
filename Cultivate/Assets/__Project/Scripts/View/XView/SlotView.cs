
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class SlotView : XView
{
    [SerializeField] private XView _contentView;
    public XView GetContentView() => _contentView;
    public void SetContentView(XView contentView)
    {
        _contentView = contentView;
        _contentView.SetInteractBehaviour(_interactBehaviour);
    }

    private ListView _parentListView;
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
        GetAnimator().SetStateAsync(1);
    }

    public void Align()
    {
        GetContentView().GetRect().position = GetRect().position;
    }
    
    
    public static readonly int ANY = -1;
    public static readonly int FREE = 0;
    public static readonly int IDLE = 1;
    public static readonly int HOVER = 2;
    public static readonly int FOLLOW = 3;
    
    
    protected override Animator InitAnimator()
    {
        Animator animator = new(4, name);
        animator[ANY, IDLE] = EnterIdle;
        animator[ANY, HOVER] = EnterHover;
        animator[ANY, FOLLOW] = EnterFollow;
        animator[ANY, FREE] = EnterFree;
        animator[FREE, ANY] = ExitFree;
        return animator;
    }

    private bool _allowHover;
    
    public bool AllowHover
    {
        get => _allowHover;
        set
        {
            _allowHover = value;
            SetInteractBehaviour(_interactBehaviour);
        }
    }
    
    private bool _allowDrag;

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
            ib.PointerEnterNeuron.Remove(PointerEnter);
            ib.PointerExitNeuron.Remove(PointerExit);
            ib.BeginDragNeuron.Remove(BeginDrag);
            ib.EndDragNeuron.Remove(EndDrag);
            ib.DragNeuron.Remove(Drag);
            ib.DraggingExitNeuron.Remove(DraggingExit);
        }
        base.SetInteractBehaviour(ib);
        if (_interactBehaviour != null)
        {
            if (_allowHover)
            {
                ib.PointerEnterNeuron.Join(PointerEnter);
                ib.PointerExitNeuron.Join(PointerExit);
            }
            
            if (_allowDrag)
            {
                ib.BeginDragNeuron.Join(BeginDrag);
                ib.EndDragNeuron.Join(EndDrag);
                ib.DragNeuron.Join(Drag);
            }
            else
            {
                ib.DraggingExitNeuron.Join(DraggingExit);
            }
        }
    }

    [SerializeField] public Configuration IdleConfiguration = new(localScale: Vector3.one);
    [SerializeField] public Configuration HoverConfiguration = new(localScale: 1.2f * Vector3.one);

    public Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(GrabberRelease)
            .Append(GoToConfiguration(IdleConfiguration));

    private Tween EnterHover()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetHover)
            .Append(GoToConfiguration(HoverConfiguration));

    private Tween EnterFollow()
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