
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinAnchor : MonoBehaviour
{
    // private Tween _handle;
    // public bool IsAnimating => _handle != null && _handle.active;
    //
    // public void BeginDrag(LegacyInteractBehaviour ib, PointerEventData d)
    // {
    //     ib.GetCLView().SetInactive(ib, d);
    //     
    //     gameObject.SetActive(true);
    //
    //     SimpleView.SetAddress(ib.GetSimpleView().GetAddress());
    //     SimpleView.Refresh();
    //
    //     LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
    //     if (pivotBehaviour != null)
    //     {
    //         _mouseOffset = d.position;
    //         AnimateDisplay(pivotBehaviour.GetDisplayTransform(), pivotBehaviour.FollowTransform);
    //     }
    // }
    //
    // private Vector2 _mouseOffset;
    //
    // public void EndDrag(LegacyInteractBehaviour ib, PointerEventData d)
    // {
    //     ib.GetCLView().SetIdle(ib, d);
    //
    //     // InteractBehaviour firstHit = CanvasManager.Instance.FirstRayCastHit(d);
    //     //
    //     // bool dropOnNothing = firstHit == null;
    //     // bool dropOnSelf = firstHit == ib;
    //     // if (dropOnNothing || dropOnSelf)
    //     // {
    //     //     ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
    //     //     if (extraBehaviourPivot != null)
    //     //         extraBehaviourPivot.RectTransformToIdle(SimpleView.GetDisplayTransform());
    //     // }
    //     
    //     LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
    //     if (pivotBehaviour != null)
    //         pivotBehaviour.RectTransformToIdle(SimpleView.GetViewTransform());
    //     
    //     gameObject.SetActive(false);
    // }
    //
    // public void Dropping(LegacyInteractBehaviour ib, PointerEventData d)
    // {
    //     ib.GetCLView().SetIdle(ib, d);
    //     gameObject.SetActive(false);
    // }
    //
    // public void Drag(LegacyInteractBehaviour ib, PointerEventData eventData)
    // {
    //     LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
    //     if (pivotBehaviour != null)
    //     {
    //         Drag(pivotBehaviour.FollowTransform, eventData.position);
    //     }
    // }
    //
    // private void Drag(RectTransform pivot, Vector2 mouse)
    // {
    //     pivot.position = CanvasManager.Instance.UI2World(mouse);
    //     if (IsAnimating)
    //         return;
    //     SimpleView.SetViewTransform(pivot);
    // }
    //
    // public RectTransform GetDisplayTransform()
    //     => SimpleView.GetViewTransform();
    //
    //
    //
    //
    //
    //
    //
    // private void SetDisplay(RectTransform end)
    // {
    //     _handle?.Kill();
    //     SimpleView.SetViewTransform(end);
    // }
    //
    // private void AnimateDisplay(RectTransform start, RectTransform end)
    // {
    //     SetDisplay(start);
    //     AnimateDisplay(end);
    // }
    //
    // private void AnimateDisplay(RectTransform end)
    // {
    //     _handle?.Kill();
    //     FollowAnimation f = new FollowAnimation(SimpleView.GetViewTransform(), end);
    //     _handle = f.GetHandle();
    //     _handle.SetAutoKill().Restart();
    // }
}
