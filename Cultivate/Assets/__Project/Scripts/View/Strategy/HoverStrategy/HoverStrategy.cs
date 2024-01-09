
using DG.Tweening;
using UnityEngine.EventSystems;

public abstract class HoverStrategy
{
    protected Tween _handle;

    protected virtual void SetStateToIdle() { }

    public void AnimateStateToIdle(AddressBehaviour ab, PointerEventData eventData)
        => AnimateStateToIdle();

    protected virtual void AnimateStateToIdle() { }

    public void AnimateStateToHover(AddressBehaviour ab, PointerEventData eventData)
        => AnimateStateToHover();

    protected virtual void AnimateStateToHover() { }
}
