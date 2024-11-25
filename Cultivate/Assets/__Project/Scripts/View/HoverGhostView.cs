
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

    public void PointerEnter(LegacyInteractBehaviour ib, PointerEventData d)
    {
        // ib.GetCLView().SetHide(ib, d);
        ib.GetCLView().SetVisible(false);
        
        gameObject.SetActive(true);

        _skillView.SetAddress(ib.GetSimpleView().GetAddress());
        _skillView.Refresh();

        LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
        if (pivotBehaviour != null)
        {
            AnimateDisplay(pivotBehaviour.GetDisplayTransform(), pivotBehaviour.HoverTransform);
        }
    }

    public void PointerExit(LegacyInteractBehaviour ib, PointerEventData d)
    {
        // ib.GetCLView().SetShow(ib, d);
        ib.GetCLView().SetVisible(true);
        
        LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
        if (pivotBehaviour != null)
            pivotBehaviour.RectTransformToIdle(_skillView.GetViewTransform());
        
        gameObject.SetActive(false);
    }

    public void BeginDrag(LegacyInteractBehaviour ib, PointerEventData d)
    {
        gameObject.SetActive(false);
    }

    public void DraggingExit(LegacyInteractBehaviour from, LegacyInteractBehaviour to, PointerEventData d)
    {
        to.GetCLView().SetVisible(true);
    }

    public void Dropping(LegacyInteractBehaviour ib, PointerEventData d)
    {
        // ib.GetCLView().SetShow(ib, d);
        // gameObject.SetActive(false);
    }

    public void Drag(LegacyInteractBehaviour ib, PointerEventData eventData)
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
        _skillView.SetViewTransform(pivot);
    }

    public RectTransform GetDisplayTransform()
        => _skillView.GetViewTransform();







    private void SetDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        _skillView.SetViewTransform(end);
    }

    private void AnimateDisplay(RectTransform start, RectTransform end)
    {
        SetDisplay(start);
        AnimateDisplay(end);
    }

    private void AnimateDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(_skillView.GetViewTransform(), end);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }
    
    
    
    
    
    
    
    
    
    
    
    public void ResetJingJie(LegacyInteractBehaviour ib, PointerEventData d)
    {
        ISkill skill = _skillView.Get<ISkill>();
        if (skill == null)
            return;
        _skillView.Refresh();
    }
    
    public void NextJingJie(LegacyInteractBehaviour ib, PointerEventData d)
    {
        ISkill skill = _skillView.Get<ISkill>();
        if (skill == null)
            return;
        _skillView.SetShowingJingJie(skill.NextJingJie(_skillView.GetShowingJingJie()));
    }
}
