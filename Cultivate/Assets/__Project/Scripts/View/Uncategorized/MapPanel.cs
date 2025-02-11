
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MapPanel : Panel
{
    [SerializeField] private ListView RoomListView;
    
    [SerializeField] private RectTransform BodyTransform;
    [SerializeField] private RectTransform BodyShowPivot;
    [SerializeField] private RectTransform BodyHidePivot;
    
    [SerializeField] private PropagatePointerEnter OpenZone;
    [SerializeField] private PropagatePointerEnter CloseZone;

    public override void Configure()
    {
        base.Configure();
        
        RoomListView.SetAddress(new Address("Run.Environment.Map.CurrLevel.Rooms"));

        OpenZone._onPointerEnter = TryShow;
        CloseZone._onPointerEnter = TryHide;
    }

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for show, 2 for locked
        Animator animator = new(3, "Map Panel");
        animator[-1, 2] = LockTween;
        animator[-1, 1] = ShowTween;
        animator[-1, 0] = HideTween;
        return animator;
    }

    public override void Refresh()
    {
        base.Refresh();
        RoomListView.Sync();
        RoomListView.Refresh();
    }

    private void OnEnable()
    {
        Sync();
    }

    private void OnDisable()
    {
        // if (RunManager.Instance != null && RunManager.Instance.Environment != null)
        //     RunManager.Instance.Environment.MapJingJieChangedEvent -= SyncSlot;
    }

    private void Sync()
    {
        RoomListView.Refresh();
    }

    // private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
    //     => AudioManager.Play("CardHover");

    private Tween _animationHandle;

    private void TryShow(PointerEventData eventData) => Animator.SetStateAsync(1);
    private void TryHide(PointerEventData eventData) => Animator.SetStateAsync(0);

    public override Tween ShowTween()
        => DOTween.Sequence()
            .AppendCallback(Sync)
            .AppendCallback(() => OpenZone.gameObject.SetActive(false))
            .AppendCallback(() => CloseZone.gameObject.SetActive(true))
            .Join(BodyTransform.DOAnchorPos(BodyShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad));

    private Tween LockTween()
        => DOTween.Sequence()
            .AppendCallback(Sync)
            .AppendCallback(() => OpenZone.gameObject.SetActive(false))
            .AppendCallback(() => CloseZone.gameObject.SetActive(false))
            .Join(BodyTransform.DOAnchorPos(BodyShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad));

    public override Tween HideTween()
        => DOTween.Sequence()
            .AppendCallback(() => OpenZone.gameObject.SetActive(true))
            .AppendCallback(() => CloseZone.gameObject.SetActive(false))
            .Join(BodyTransform.DOAnchorPos(BodyHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad));
}
