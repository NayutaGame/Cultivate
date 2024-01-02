
using DG.Tweening;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    public AddressBehaviour AddressBehaviour;

    private Tween _animationHandle;
    public bool IsAnimating => _animationHandle != null && _animationHandle.active;

    public void BeginDrag(Address address, RectTransform start, RectTransform pivot)
    {
        AddressBehaviour.SetAddress(address);
        AddressBehaviour.Refresh();
        gameObject.SetActive(true);

        SetStartAndPivot(start, pivot);
    }

    public void EndDrag()
    {
        gameObject.SetActive(false);
    }

    public void Drag(RectTransform pivot, Vector2 mouse)
    {
        pivot.position = mouse;
        if (IsAnimating)
            return;
        AddressBehaviour.RectTransform.position = pivot.position;
        AddressBehaviour.RectTransform.localScale = pivot.localScale;
    }

    private void SetPivot(RectTransform pivot)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(AddressBehaviour.RectTransform, pivot);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }

    private void SetPivotWithoutAnimation(RectTransform pivot)
    {
        _animationHandle?.Kill();
        AddressBehaviour.RectTransform.position = pivot.position;
        AddressBehaviour.RectTransform.localScale = pivot.localScale;
    }

    private void SetStartAndPivot(RectTransform start, RectTransform pivot)
    {
        SetPivotWithoutAnimation(start);
        SetPivot(pivot);
    }
}
