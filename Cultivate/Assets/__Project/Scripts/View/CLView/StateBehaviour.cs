using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CLView))]
public abstract class StateBehaviour : MonoBehaviour
{
    protected Tween _handle;

    public abstract void PointerEnter(CLView v, PointerEventData d);
    public abstract void PointerExit(CLView v, PointerEventData d);
    public abstract void PointerMove(CLView v, PointerEventData d);
}
