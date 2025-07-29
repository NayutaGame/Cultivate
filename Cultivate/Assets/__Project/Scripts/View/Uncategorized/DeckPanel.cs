
using System;
using System.Collections.Generic;
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
        PlayerEntity.FormationList.PointerEnterNeuron.Join(HighlightContributors);
        PlayerEntity.FormationList.PointerExitNeuron.Join(UnhighlightContributors);

        HandView.SetAddress("Run.Environment.Hand");
        HandView.DropNeuron.Join(Merge, Unequip);
        
        HandView.DroppingNeuron.Join(RemoveMergePreresult);
        HandView.EndDragNeuron.Join(RemoveMergePreresult);
        HandView.DraggingEnterNeuron.Join(DraggingEnter);
        HandView.DraggingExitNeuron.Join(DraggingExit);
        
        HandView.ItemCountChanged.Join(RefreshLayoutSpacing);
        
        UnequipZone._onDrop = Unequip;

        SortButton._button.onClick.RemoveAllListeners();
        SortButton._button.onClick.AddListener(Sort);
        SortButton._button.onClick.AddListener(AudioManager.PlayButtonPress);
        SortButton._button.onClick.AddListener(AudioManager.PlaySort);

        SortButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
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
        HorizontalLayoutGroup horizontalLayoutGroup = HandView.GetLayoutGroup() as HorizontalLayoutGroup;
        float L = HandView.GetRect().rect.width;
        float l = 150;
        int n = HandView.GetCount();
        float s;
        if (n < L / l)
            s = 0;
        else
            s = -(n * l - L) / (n - 1);
        horizontalLayoutGroup.spacing = s;
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
        RunManager.Instance.Environment.FieldChangedNeuron.Add(PlayerEntity.OnFieldChange);
        CharacterIconView.Refresh();
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.FieldChangedNeuron.Remove(PlayerEntity.OnFieldChange);
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

    private void HighlightContributors(InteractBehaviour ib, PointerEventData d)
    {
        Predicate<ISkill> pred = ib.Get<RunFormation>().GetContributorPred();
        PlayerEntity.FieldView.TraversalActive().Do(HighlightSlot);
        HandView.TraversalActive().Do(HighlightSkill);
        
        void HighlightSkill(SlotView view)
        {
            RunSkill runSkill = view.Get<RunSkill>();
            if (runSkill == null || !pred(runSkill))
                return;
            view.GetContentView().GetBehaviour<HighlightBehaviour>().SetHighlight(true);
        }
        
        void HighlightSlot(SlotView view)
        {
            SkillSlot skillSlot = view.Get<SkillSlot>();
            if (skillSlot == null || skillSlot.Skill == null || !pred(skillSlot.Skill))
                return;
            view.GetContentView().GetBehaviour<HighlightBehaviour>().SetHighlight(true);
        }
    }

    private void UnhighlightContributors(InteractBehaviour ib, PointerEventData d)
    {
        PlayerEntity.FieldView.TraversalActive().Do(UnhighlightSlot);
        HandView.TraversalActive().Do(UnhighlightSkill);
        
        void UnhighlightSkill(SlotView view)
        {
            RunSkill runSkill = view.Get<RunSkill>();
            if (runSkill == null)
                return;
            view.GetContentView().GetBehaviour<HighlightBehaviour>().SetHighlight(false);
        }
        
        void UnhighlightSlot(SlotView view)
        {
            SkillSlot skillSlot = view.Get<SkillSlot>();
            if (skillSlot == null)
                return;
            view.GetContentView().GetBehaviour<HighlightBehaviour>().SetHighlight(false);
        }
    }

    private void RemoveMergePreresult(InteractBehaviour from, PointerEventData d)
    {
        CanvasManager.Instance.MergePreresultView.SetMergeTargetAsync(0, null);
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

        UnequipDetails unequipDetails = UnequipDetails.FromSlot(skillSlot);
        RunManager.Instance.Environment.UnequipProcedure(unequipDetails);
    }

    private void Merge(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;

        MergeDetails mergeDetails = new(from.Get<RunSkill>(), to.Get<RunSkill>());
        RunManager.Instance.Environment.MergeProcedure(mergeDetails);
    }

    #endregion

    public SlotView SkillItemFromDeckIndex(DeckIndex deckIndex)
    {
        if (deckIndex.InField)
            return PlayerEntity.FieldView.ViewFromIndex(deckIndex.Index);

        return HandView.ViewFromIndex(deckIndex.Index);
    }

    public SlotView LatestSkillItem()
        => HandView.LastView();

    private Tween _animationHandle;

    private void Sort()
    {
        CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearSelections();
        
        _animationHandle?.Kill();
        
        _animationHandle = DOTween.Sequence()
            .AppendCallback(() =>
            {
                (HandView.GetLayoutGroup() as HorizontalLayoutGroup).spacing = -170;
                // HandViewPivotTransform.SetSizeWithCurrentAnchors(0, 0);
                HandView.RefreshPivotsAsync();
            })
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                HandView.Get<SkillInventory>().SortByComparisonId(0);
                HandView.Refresh();
                RefreshLayoutSpacing();
                // HandViewPivotTransform.SetSizeWithCurrentAnchors(0, 1134);
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
