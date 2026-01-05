
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapPanel : Panel
{
    [SerializeField] private RectTransform BodyTransform;
    [SerializeField] private RectTransform BodyShowPivot;
    [SerializeField] private RectTransform BodyHidePivot;

    [SerializeField] private TMP_Text StepText;
    [SerializeField] private CLButton LastRoomButton;
    
    [SerializeField] private PropagatePointerEnter OpenZone;
    [SerializeField] private PropagatePointerEnter CloseZone;

    [SerializeField] public ListView LocationList;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        Address address = new Address("Run.Environment.Map");
        LocationList.SetAddress(new("Run.Environment.Map.Locations"));
        LocationList.NeuronBundle.LeftClickNeuron.Join(CreateMenu);
        
        LastRoomButton.LeftClickNeuron.Join(EnterLastRoom);

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
        RunManager.Instance.Environment.Map.RoomChangedNeuron.Add(RoomChanged);
        RunManager.Instance.Environment.Map.LevelChangedNeuron.Add(LevelChanged);
        LocationList.Sync();
        StepText.text = RunManager.Instance.Environment.Map.GetStepText();
        LastRoomButton.SetStateToActiveIf(RunManager.Instance.Environment.Map.IsLastSelecting());
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.Map.RoomChangedNeuron.Remove(RoomChanged);
        RunManager.Instance.Environment.Map.LevelChangedNeuron.Remove(LevelChanged);
    }

    private void RoomChanged(RoomChangedDetails d)
    {
        LocationList.Refresh();
        StepText.text = RunManager.Instance.Environment.Map.GetStepText();
        LastRoomButton.SetStateToActiveIf(RunManager.Instance.Environment.Map.IsLastSelecting());
    }

    private void LevelChanged()
    {
        LocationList.Refresh();
    }

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
        Location location = ib.Get<Location>();
        if (location.State != LocationState.Available)
            return;
        MenuDetails menuDetails = RunManager.Instance.Environment.Map.GetMenuDetailsFromLocation(location);
        CanvasManager.Instance.MenuManager.CreateMenu(menuDetails);
    }

    private void EnterLastRoom(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.Map.ReceiveSignalProcedure(new SelectLastRoomSignal());
    }
}
