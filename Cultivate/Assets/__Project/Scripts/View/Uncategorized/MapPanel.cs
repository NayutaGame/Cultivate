
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapPanel : Panel
{
    [SerializeField] private ListView RoomListView;
    
    [SerializeField] private RectTransform BodyTransform;
    [SerializeField] private RectTransform BodyShowPivot;
    [SerializeField] private RectTransform BodyHidePivot;
    
    [SerializeField] private PropagatePointerEnter OpenZone;
    [SerializeField] private PropagatePointerEnter CloseZone;

    [SerializeField] private ListView MapNodeListView;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        Address address = new Address("Run.Environment.Map");
        RoomListView.SetAddress(address.Append(".CurrLevel.Rooms"));
        MapNodeListView.SetAddress(new("Run.Environment.MapNodes"));
        
        // MapNodeListView.LeftClickNeuron.Join(SelectedMapNode);
        MapNodeListView.LeftClickNeuron.Join(CreateMenu);

        OpenZone._onPointerEnter = TryShow;
        CloseZone._onPointerEnter = TryHide;
    }

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for idle, 2 for locked
        Animator animator = new(3, "Map Panel");
        animator[-1, 2] = LockTween;
        animator[-1, 1] = EnterIdle;
        animator[-1, 0] = EnterHide;
        return animator;
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.RoomChangedNeuron.Add(RoomChanged);
        RunManager.Instance.Environment.LevelChangedNeuron.Add(LevelChanged);
        MapNodeListView.Sync();
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.RoomChangedNeuron.Remove(RoomChanged);
        RunManager.Instance.Environment.LevelChangedNeuron.Remove(LevelChanged);
    }

    private void RoomChanged(RoomChangedDetails d)
    {
        RoomListView.Refresh();
    }

    private void LevelChanged()
    {
        RoomListView.Sync();
        RoomListView.Refresh();
    }

    // private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
    //     => AudioManager.Play("CardHover");

    private void TryShow(PointerEventData eventData) => GetAnimator().SetStateAsync(1);
    private void TryHide(PointerEventData eventData) => GetAnimator().SetStateAsync(0);

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => OpenZone.gameObject.SetActive(false))
            .AppendCallback(() => CloseZone.gameObject.SetActive(true))
            .Join(BodyTransform.DOAnchorPos(BodyShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad));

    private Tween LockTween()
        => DOTween.Sequence()
            .AppendCallback(() => OpenZone.gameObject.SetActive(false))
            .AppendCallback(() => CloseZone.gameObject.SetActive(false))
            .Join(BodyTransform.DOAnchorPos(BodyShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad));

    public override Tween EnterHide()
        => DOTween.Sequence()
            .AppendCallback(() => OpenZone.gameObject.SetActive(true))
            .AppendCallback(() => CloseZone.gameObject.SetActive(false))
            .Join(BodyTransform.DOAnchorPos(BodyHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad));

    private void CreateMenu(InteractBehaviour ib, PointerEventData d)
    {
        MapNode mapNode = ib.Get<MapNode>();
        MenuDetails menuDetails = RunManager.Instance.Environment.GetMenuDetailsFromMapNode(mapNode);
        CanvasManager.Instance.MenuManager.CreateMenu(menuDetails);
    }
}
