
using System;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class DeckPanel : Panel
{
    [SerializeField] private CharacterIconView CharacterIconView;
    
    public PlayerEntityView PlayerEntity;
    [SerializeField] private RectTransform PlayerEntityTransform;
    [SerializeField] private RectTransform PlayerEntityShowPivot;
    [SerializeField] private RectTransform PlayerEntityHidePivot;

    public ListView HandView;
    [SerializeField] private RectTransform HandTransform;
    [SerializeField] private RectTransform HandShowPivot;
    [SerializeField] private RectTransform HandHidePivot;

    [SerializeField] private RectTransform PlayerEntityOtherHalfTransform;
    [SerializeField] private RectTransform PlayerEntityOtherHalfShowPivot;
    [SerializeField] private RectTransform PlayerEntityOtherHalfHidePivot;
    
    public XButton SortButton;
    [SerializeField] private RectTransform SortButtonTransform;
    [SerializeField] private RectTransform SortButtonShowPivot;
    [SerializeField] private RectTransform SortButtonHidePivot;
    
    [SerializeField] private PropagatePointerEnter OpenZone;
    [SerializeField] private PropagatePointerEnter CloseZone;

    [SerializeField] public RectTransform DropRectTransform;
    [SerializeField] private RectTransform HandViewPivotTransform;
    [SerializeField] private HorizontalLayoutGroup HandViewLayout;

    [SerializeField] private PropagateDrop UnequipZone;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        OpenZone._onPointerEnter = TryShow;
        CloseZone._onPointerEnter = TryHide;
        
        CharacterIconView.SetAddress("Run.Environment.Config.CharacterProfile");
        
        PlayerEntity.SetAddress("Run.Environment.Home");
        PlayerEntity.FormationList.PointerEnterNeuron.Join(InvokeHighlightQualifiers);
        PlayerEntity.FormationList.PointerExitNeuron.Join(InvokeUnhighlightQualifiers);

        HandView.SetAddress("Run.Environment.Hand");
        HandView.DropNeuron.Join(MoveSkill);
        
        HandView.DroppingNeuron.Join(RemoveMergePreresult);
        HandView.EndDragNeuron.Join(RemoveMergePreresult);
        HandView.DraggingEnterNeuron.Join(DraggingEnter);
        HandView.DraggingExitNeuron.Join(DraggingExit);
        HandView.BeginDragNeuron.Join(DragBeginRunSkill);
        HandView.EndDragNeuron.Join(DragEndRunSkill);
        HandView.DroppingNeuron.Join(DragEndRunSkill);
        
        HandView.ItemCountChanged.Join(RefreshLayoutSpacing);
        
        UnequipZone._onDrop = Unequip;

        SortButton._button.onClick.RemoveAllListeners();
        SortButton._button.onClick.AddListener(Sort);
        SortButton._button.onClick.AddListener(AudioManager.PlayButtonPress);
        SortButton._button.onClick.AddListener(AudioManager.PlaySort);

        SortButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
    }

    private void DragBeginRunSkill(InteractBehaviour ib, PointerEventData d)
    {
        RunSkill skill = ib.Get<RunSkill>();
        RunManager.Instance.Environment.DragBeginRunSkill.Invoke(skill);
    }

    private void DragEndRunSkill(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.DragEndRunSkill.Invoke();
    }

    private void DraggingEnter(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;
        RunEnvironment env = RunManager.Instance.Environment;
        RunSkill lhs = from.Get<RunSkill>();
        RunSkill rhs = to.Get<RunSkill>();

        CanvasManager.Instance.MergePreresultView.SetMergeTargetAsync(1, env.GetMergePreresult(lhs, rhs));
    }

    private void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        CanvasManager.Instance.MergePreresultView.SetMergeTargetAsync(0, null);
    }

    private void RefreshLayoutSpacing()
    {
        float L = HandView.GetRect().rect.width;
        float l = 150;
        int n = HandView.GetCount();
        float s;
        if (n < L / l)
            s = 0;
        else
            s = -(n * l - L) / (n - 1);
        HandViewLayout.spacing = s;
    }

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for show, 2 for locked
        Animator animator = new(3, "Deck Panel");
        animator[-1, 2] = LockTween;
        animator[-1, 1] = EnterIdle;
        animator[-1, 0] = EnterHide;
        return animator;
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.ResimulateNeuron.Add(PlayerEntity.OnFieldChange);
        
        CharacterIconView.Refresh();
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.ResimulateNeuron.Remove(PlayerEntity.OnFieldChange);
    }

    // extra views
    // { typeof(BattlePanelDescriptor), 2 },
    // { typeof(PuzzlePanelDescriptor), 3 },
    // { typeof(DialogPanelDescriptor), 4 },
    // { typeof(DiscoverSkillPanelDescriptor), 5 },
    // { typeof(CardPickerPanelDescriptor), 6 },
    // { typeof(ShopPanelDescriptor), 7 },
    // { typeof(BarterPanelDescriptor), 8 },
    // { typeof(GachaPanelDescriptor), 9 },
    // { typeof(ArbitraryCardPickerPanelDescriptor), 10 },
    // { typeof(ImagePanelDescriptor), 11 },
    // { typeof(RunResultPanelDescriptor), 12 },

    #region IInteractable

    private void InvokeHighlightQualifiers(InteractBehaviour ib, PointerEventData d)
    {
        Predicate<RunSkill> pred = ib.Get<RunFormation>().GetContributorPred();
        CanvasManager.Instance.RunCanvas.HighlightQualifiersNeuron.Invoke(pred);
    }

    private void InvokeUnhighlightQualifiers(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.RunCanvas.UnhighlightQualifiersNeuron.Invoke();
    }

    private void RemoveMergePreresult(InteractBehaviour from, PointerEventData d)
    {
        CanvasManager.Instance.MergePreresultView.SetMergeTargetAsync(0, null);
    }

    private void Unequip(InteractBehaviour from, MonoBehaviour to, PointerEventData d)
    {
        IDeckIndex fromIndex = GetDeckIndex(from);
        if (fromIndex == null || fromIndex.Region == SkillRegion.Hand)
            return;
        
        RunManager.Instance.Environment.MoveSkillProcedure(fromIndex, new NextHandDeckIndexDefinition());
    }

    private void MoveSkill(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        IDeckIndex fromIndex = GetDeckIndex(from);
        IDeckIndex toIndex = GetDeckIndex(to);
        if (fromIndex == null || toIndex == null)
            return;

        RunManager.Instance.Environment.MoveSkillProcedure(fromIndex, toIndex);
    }

    private IDeckIndex GetDeckIndex(InteractBehaviour ib)
    {
        object obj = ib.Get<object>();
        if (obj is RunSkill runSkill)
            return runSkill.ToDeckIndex();
        
        if (obj is SkillSlot skillSlot)
            return skillSlot.ToDeckIndex();
        
        if (obj is RequirementSlot requirementSlot)
            return requirementSlot.ToDeckIndex();

        return null;
    }

    #endregion

    public SlotView SkillItemFromDeckIndex(DeckIndex deckIndex)
    {
        if (deckIndex.Region == SkillRegion.Field)
            return PlayerEntity.FieldView.ViewFromIndex(deckIndex.Index);
        else if (deckIndex.Region == SkillRegion.Hand)
            return HandView.ViewFromIndex(deckIndex.Index);

        throw new NotImplementedException();
    }

    public SlotView LatestSkillItem()
        => HandView.LastView();

    private Tween _animationHandle;

    private void Sort()
    {
        _animationHandle?.Kill();
        
        _animationHandle = DOTween.Sequence()
            .AppendCallback(() =>
            {
                HandViewLayout.spacing = -170;
                HandView.RefreshPivotsAsync();
            })
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                HandView.Get<SkillInventory>().SortByComparisonId(0);
                HandView.Refresh();
                RefreshLayoutSpacing();
                HandView.RefreshPivotsAsync();
            });
        
        _animationHandle.SetAutoKill().Restart();
    }

    private void TryShow(PointerEventData eventData) => GetAnimator().SetStateAsync(1);
    private void TryHide(PointerEventData eventData) => GetAnimator().SetStateAsync(0);

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(PlayerEntity.Sync)
            .AppendCallback(() => OpenZone.gameObject.SetActive(false))
            .AppendCallback(() => CloseZone.gameObject.SetActive(true))
            .Join(SortButtonTransform.DOAnchorPos(SortButtonShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(PlayerEntityTransform.DOAnchorPos(PlayerEntityShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(HandTransform.DOAnchorPos(HandShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(PlayerEntityOtherHalfTransform.DOAnchorPos(PlayerEntityOtherHalfShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad));

    private Tween LockTween()
        => DOTween.Sequence()
            .AppendCallback(PlayerEntity.Sync)
            .AppendCallback(() => OpenZone.gameObject.SetActive(false))
            .AppendCallback(() => CloseZone.gameObject.SetActive(false))
            .Join(SortButtonTransform.DOAnchorPos(SortButtonShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(PlayerEntityTransform.DOAnchorPos(PlayerEntityShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(HandTransform.DOAnchorPos(HandShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(PlayerEntityOtherHalfTransform.DOAnchorPos(PlayerEntityOtherHalfShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad));

    public override Tween EnterHide()
        => DOTween.Sequence()
            .AppendCallback(() => OpenZone.gameObject.SetActive(true))
            .AppendCallback(() => CloseZone.gameObject.SetActive(false))
            .Join(SortButtonTransform.DOAnchorPos(SortButtonHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad))
            .Join(PlayerEntityTransform.DOAnchorPos(PlayerEntityHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad))
            .Join(HandTransform.DOAnchorPos(HandHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad))
            .Join(PlayerEntityOtherHalfTransform.DOAnchorPos(PlayerEntityOtherHalfHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad));
}
