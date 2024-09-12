
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapPanel : Panel
{
    // public PlayerEntityView PlayerEntity;
    [SerializeField] private RectTransform BodyTransform;
    [SerializeField] private RectTransform BodyShowPivot;
    [SerializeField] private RectTransform BodyHidePivot;
    
    [SerializeField] private PropagatePointerEnter OpenZone;
    [SerializeField] private PropagatePointerEnter CloseZone;

    public override void Configure()
    {
        base.Configure();

        OpenZone._onPointerEnter = TryShow;
        CloseZone._onPointerEnter = TryHide;
    }

    protected override void InitStateMachine()
    {
        SM = new(3);
        // 0 for hide, 1 for show, 2 for locked
        SM[-1, 2] = LockTween;
        SM[-1, 1] = ShowTween;
        SM[-1, 0] = HideTween;
    }

    public override void Refresh()
    {
        base.Refresh();
    }

    private void OnEnable()
    {
        // if (RunManager.Instance != null && RunManager.Instance.Environment != null)
        //     RunManager.Instance.Environment.MapJingJieChangedEvent += SyncSlot;
        Sync();
    }

    private void OnDisable()
    {
        // if (RunManager.Instance != null && RunManager.Instance.Environment != null)
        //     RunManager.Instance.Environment.MapJingJieChangedEvent -= SyncSlot;
    }

    private void Sync()
    {
    }

    // private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
    //     => AudioManager.Play("CardHover");

    private Tween _animationHandle;

    private void TryShow(PointerEventData eventData) => SetStateAsync(1);
    private void TryHide(PointerEventData eventData) => SetStateAsync(0);

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
