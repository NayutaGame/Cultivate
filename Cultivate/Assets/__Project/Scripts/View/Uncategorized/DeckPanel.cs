
using System;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    
    public Button SortButton;
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
        // PlayerEntity.FormationList.PointerEnterNeuron.Join(HighlightContributors);
        // PlayerEntity.FormationList.PointerExitNeuron.Join(UnhighlightContributors);

        HandView.SetAddress("Run.Environment.Hand");
        HandView.PointerEnterNeuron.Join(PlayCardHoverSFX);
        HandView.DropNeuron.Join(Merge, Unequip);
        // HandView.DroppingNeuron.Join(RemoveMergePreresult);
        // HandView.EndDragNeuron.Join(RemoveMergePreresult);
        //
        // HandView.DraggingEnterNeuron.Join(DraggingEnter);
        // HandView.DraggingExitNeuron.Join(DraggingExit);
        
        UnequipZone._onDrop = Unequip;

        SortButton.onClick.RemoveAllListeners();
        // SortButton.onClick.AddListener(Sort);
    }

    private void DraggingEnter(LegacyInteractBehaviour from, LegacyInteractBehaviour to, PointerEventData d)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;
        RunEnvironment env = RunManager.Instance.Environment;
        RunSkill lhs = from.GetSimpleView().Get<RunSkill>();
        RunSkill rhs = to.GetSimpleView().Get<RunSkill>();

        CanvasManager.Instance.MergePreresultView.SetMergePreresultAsync(1, env.GetMergePreresult(lhs, rhs));
    }

    private void DraggingExit(LegacyInteractBehaviour from, LegacyInteractBehaviour to, PointerEventData d)
    {
        CanvasManager.Instance.MergePreresultView.SetMergePreresultAsync(0, null);
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

    public override void Refresh()
    {
        base.Refresh();
        CharacterIconView.Refresh();
        PlayerEntity.Refresh();
        // LegacyHandView.Refresh();
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
        // LegacyHandView.Sync();
        PlayerEntity.Sync();
    }

    #region IInteractable

    private void HighlightContributors(LegacyInteractBehaviour ib, PointerEventData d)
    {
        Predicate<ISkill> pred = ib.GetCLView().Get<IFormationModel>().GetContributorPred();
        // PlayerEntity.SkillList.TraversalActive().Do(HighlightSlot);
        // HandView.TraversalActive().Do(HighlightSkill);

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
        
        void HighlightSkill(LegacyItemBehaviour itemBehaviour)
        {
            ISkill runSkill = itemBehaviour.GetSimpleView().Get<ISkill>();
            if (runSkill != null && pred(runSkill))
                itemBehaviour.GetSimpleView().GetComponent<SkillCardView>().SetHighlight(true);
        }
        
        void HighlightSlot(LegacyItemBehaviour itemBehaviour)
        {
            // SkillSlot skillSlot = itemBehaviour.GetSimpleView().Get<SkillSlot>();
            // if (skillSlot != null && skillSlot.Skill != null && pred(skillSlot.Skill))
            //     itemBehaviour.GetSimpleView().GetComponent<SlotCardView>().SkillCardView.SetHighlight(true);
        }
    }

    private void UnhighlightContributors(LegacyInteractBehaviour ib, PointerEventData d)
    {
        // PlayerEntity.SkillList.TraversalActive().Do(UnhighlightSlot);
        // HandView.TraversalActive().Do(UnhighlightSkill);
        
        void UnhighlightSkill(LegacyItemBehaviour itemBehaviour)
        {
            ISkill runSkill = itemBehaviour.GetSimpleView().Get<ISkill>();
            if (runSkill != null)
                itemBehaviour.GetSimpleView().GetComponent<SkillCardView>().SetHighlight(false);
        }
        
        void UnhighlightSlot(LegacyItemBehaviour itemBehaviour)
        {
            // SkillSlot skillSlot = itemBehaviour.GetSimpleView().Get<SkillSlot>();
            // if (skillSlot != null)
            //     itemBehaviour.GetSimpleView().GetComponent<SlotCardView>().SkillCardView.SetHighlight(false);
        }
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
        => AudioManager.Play("CardHover");

    private void RemoveMergePreresult(LegacyInteractBehaviour from, PointerEventData d)
    {
        CanvasManager.Instance.MergePreresultView.SetMergePreresultAsync(0, null);
    }

    private void Unequip(InteractBehaviour from, MonoBehaviour to, PointerEventData d)
        => Unequip(from, null, d);

    private void Unequip(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is FieldSlotInteractBehaviour))
            return;

        SkillSlot skillSlot = from.Get<SkillSlot>();
        if (skillSlot.Skill == null)
            return;
        
        UnequipDetails unequipDetails = new(skillSlot);
        CanvasManager.Instance.RunCanvas.UnequipEvent.Invoke(unequipDetails);
    }

    private void Merge(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;

        MergeDetails mergeDetails = new(from.Get<RunSkill>(), to.Get<RunSkill>());
        CanvasManager.Instance.RunCanvas.MergeEvent.Invoke(mergeDetails);
    }

    #endregion

    public XView SkillItemFromDeckIndex(DeckIndex deckIndex)
    {
        if (deckIndex.InField)
            return PlayerEntity.FieldView.ViewFromIndex(deckIndex.Index);

        return HandView.ViewFromIndex(deckIndex.Index);
    }

    public XView LatestSkillItem()
        => HandView.LastView();

    public LegacyItemBehaviour LegacySkillItemFromDeckIndex(DeckIndex deckIndex)
    {
        return null;
    }

    public LegacyItemBehaviour LegacyLatestSkillItem()
    {
        return null;
    }

    private Tween _animationHandle;

    private void Sort()
    {
        // CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
        //
        // _animationHandle?.Kill();
        //
        // _animationHandle = DOTween.Sequence()
        //     .AppendCallback(() =>
        //     {
        //         HandViewPivotTransform.SetSizeWithCurrentAnchors(0, 0);
        //         HandView.RefreshPivots();
        //     })
        //     .AppendInterval(0.2f)
        //     .AppendCallback(() =>
        //     {
        //         HandView.Get<SkillInventory>().SortByComparisonId(0);
        //         CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
        //         HandViewPivotTransform.SetSizeWithCurrentAnchors(0, 1134);
        //         HandView.RefreshPivots();
        //     });
        //
        // _animationHandle.SetAutoKill().Restart();
    }

    private void TryShow(PointerEventData eventData) => GetAnimator().SetStateAsync(1);
    private void TryHide(PointerEventData eventData) => GetAnimator().SetStateAsync(0);

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(Sync)
            .AppendCallback(() => OpenZone.gameObject.SetActive(false))
            .AppendCallback(() => CloseZone.gameObject.SetActive(true))
            .Join(SortButtonTransform.DOAnchorPos(SortButtonShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(PlayerEntityTransform.DOAnchorPos(PlayerEntityShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(HandTransform.DOAnchorPos(HandShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(PlayerEntityOtherHalfTransform.DOAnchorPos(PlayerEntityOtherHalfShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad));

    private Tween LockTween()
        => DOTween.Sequence()
            .AppendCallback(Sync)
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
