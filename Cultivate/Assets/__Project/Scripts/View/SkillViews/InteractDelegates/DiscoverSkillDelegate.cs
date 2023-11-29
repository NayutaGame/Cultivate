
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscoverSkillDelegate : InteractDelegate,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler
{
    [SerializeField] private RectTransform ContentTransform;

    [SerializeField] private RectTransform IdlePivot;
    [SerializeField] private RectTransform HoverPivot;

    private Tween _animationHandle;

    private void OnEnable()
    {
        ContentTransform.anchoredPosition = IdlePivot.anchoredPosition;
        ContentTransform.localScale = IdlePivot.localScale;
    }

    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        AudioManager.Play("CardHover");

        PlayFollowAnimation(ContentTransform, HoverPivot);

        CanvasManager.Instance.SkillAnnotation.SetAddress(GetComponent<IAddress>().GetAddress());
        CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        PlayFollowAnimation(ContentTransform, IdlePivot);

        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
        CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    public void PointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.SkillAnnotation.UpdateMousePos(eventData.position);
    }
}
