
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverGhostView : MonoBehaviour
{
    [SerializeField] private SkillCardView _skillView;

    public void Awake()
    {
        _skillView.AwakeFunction();
    }

    private void OnDisable()
    {
        _animationHandle?.Kill();
    }

    private Tween _animationHandle;
    public bool IsAnimating => _animationHandle != null && _animationHandle.active;

    public void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        // ib.GetCLView().SetHide(ib, d);
        ib.GetCLView().SetVisible(false);
        
        gameObject.SetActive(true);

        _skillView.SetAddress(ib.GetSimpleView().GetAddress());
        _skillView.Refresh();

        XBehaviourPivot xBehaviourPivot = ib.GetCLView().GetBehaviour<XBehaviourPivot>();
        if (xBehaviourPivot != null)
        {
            AnimateDisplay(xBehaviourPivot.GetDisplayTransform(), xBehaviourPivot.HoverTransform);
        }
    }

    public void PointerExit(InteractBehaviour ib, PointerEventData d)
    {
        // ib.GetCLView().SetShow(ib, d);
        ib.GetCLView().SetVisible(true);
        
        XBehaviourPivot xBehaviourPivot = ib.GetCLView().GetBehaviour<XBehaviourPivot>();
        if (xBehaviourPivot != null)
            xBehaviourPivot.RectTransformToIdle(_skillView.GetDisplayTransform());
        
        gameObject.SetActive(false);
    }

    public void BeginDrag(InteractBehaviour ib, PointerEventData d)
    {
        gameObject.SetActive(false);
    }

    public void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        to.GetCLView().SetVisible(true);
    }

    public void Dropping(InteractBehaviour ib, PointerEventData d)
    {
        // ib.GetCLView().SetShow(ib, d);
        // gameObject.SetActive(false);
    }

    public void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        // ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        // if (extraBehaviourPivot != null)
        // {
        //     Drag(extraBehaviourPivot.FollowTransform, eventData.position);
        // }
    }

    private void Drag(RectTransform pivot, Vector2 mouse)
    {
        pivot.position = CanvasManager.Instance.UI2World(mouse);
        if (IsAnimating)
            return;
        _skillView.SetDisplayTransform(pivot);
    }

    public RectTransform GetDisplayTransform()
        => _skillView.GetDisplayTransform();







    private void SetDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        _skillView.SetDisplayTransform(end);
    }

    private void AnimateDisplay(RectTransform start, RectTransform end)
    {
        SetDisplay(start);
        AnimateDisplay(end);
    }

    private void AnimateDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(_skillView.GetDisplayTransform(), end);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }
    
    
    
    
    
    
    
    
    
    
    
    public void ResetJingJie(InteractBehaviour ib, PointerEventData d)
    {
        ISkill skill = _skillView.Get<ISkill>();
        if (skill == null)
            return;
        _skillView.Refresh();
    }
    
    public void NextJingJie(InteractBehaviour ib, PointerEventData d)
    {
        ISkill skill = _skillView.Get<ISkill>();
        if (skill == null)
            return;
        _skillView.SetShowingJingJie(skill.NextJingJie(_skillView.GetShowingJingJie()));
    }
}
