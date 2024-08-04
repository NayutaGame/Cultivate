
using System;
using System.Linq;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckPanel : Panel
{
    public PlayerEntityView PlayerEntity;
    [SerializeField] private RectTransform PlayerEntityTransform;
    [SerializeField] private RectTransform PlayerEntityShowPivot;
    [SerializeField] private RectTransform PlayerEntityHidePivot;

    public AnimatedListView HandView;
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
    
    [SerializeField] private PropagatePointerEnter DeckOpenZone;
    [SerializeField] private PropagatePointerEnter DeckCloseZone;

    [SerializeField] public RectTransform DropRectTransform;
    [SerializeField] private RectTransform HandViewPivotTransform;
    [SerializeField] private HorizontalLayoutGroup HandViewLayout;

    public override void Configure()
    {
        base.Configure();

        DeckOpenZone._onPointerEnter = TryShow;
        DeckCloseZone._onPointerEnter = TryHide;
        
        PlayerEntity.SetAddress(new Address("Run.Environment.Home"));
        PlayerEntity.FormationList.PointerEnterNeuron.Join(HighlightContributors);
        PlayerEntity.FormationList.PointerExitNeuron.Join(UnhighlightContributors);

        HandView.SetAddress(new Address("Run.Environment.Hand"));
        HandView.PointerEnterNeuron.Join(PlayCardHoverSFX);
        HandView.DropNeuron.Join(Merge, Unequip);
        
        HandView.DraggingEnterNeuron.Join(DraggingEnter);
        HandView.DraggingExitNeuron.Join(DraggingExit);
        
        HandView.GetComponent<PropagateDrop>()._onDrop = Unequip;

        SortButton.onClick.RemoveAllListeners();
        SortButton.onClick.AddListener(Sort);
    }

    private void DraggingEnter(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;
        RunEnvironment env = RunManager.Instance.Environment;
        RunSkill lhs = from.GetSimpleView().Get<RunSkill>();
        RunSkill rhs = to.GetSimpleView().Get<RunSkill>();

        CanvasManager.Instance.MergePreresultView.SetMergePreresult(env.GetMergePreresult(lhs, rhs));
    }

    private void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        CanvasManager.Instance.MergePreresultView.SetMergePreresult(null);
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
        PlayerEntity.Refresh();
        HandView.Refresh();
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
        HandView.Sync();
        PlayerEntity.Sync();
    }

    #region IInteractable

    private void HighlightContributors(InteractBehaviour ib, PointerEventData d)
    {
        Predicate<ISkillModel> pred = ib.GetCLView().Get<IFormationModel>().GetContributorPred();
        PlayerEntity.SkillList.TraversalActive().Do(HighlightSlot);
        HandView.TraversalActive().Do(HighlightSkill);

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
        
        void HighlightSkill(ItemBehaviour itemBehaviour)
        {
            ISkillModel runSkill = itemBehaviour.GetSimpleView().Get<ISkillModel>();
            if (runSkill != null && pred(runSkill))
                itemBehaviour.GetSimpleView().GetComponent<SkillCardView>().SetHighlight(true);
        }
        
        void HighlightSlot(ItemBehaviour itemBehaviour)
        {
            SkillSlot skillSlot = itemBehaviour.GetSimpleView().Get<SkillSlot>();
            if (skillSlot != null && skillSlot.Skill != null && pred(skillSlot.Skill))
                itemBehaviour.GetSimpleView().GetComponent<SlotCardView>().SkillCardView.SetHighlight(true);
        }
    }

    private void UnhighlightContributors(InteractBehaviour ib, PointerEventData d)
    {
        PlayerEntity.SkillList.TraversalActive().Do(UnhighlightSlot);
        HandView.TraversalActive().Do(UnhighlightSkill);
        
        void UnhighlightSkill(ItemBehaviour itemBehaviour)
        {
            ISkillModel runSkill = itemBehaviour.GetSimpleView().Get<ISkillModel>();
            if (runSkill != null)
                itemBehaviour.GetSimpleView().GetComponent<SkillCardView>().SetHighlight(false);
        }
        
        void UnhighlightSlot(ItemBehaviour itemBehaviour)
        {
            SkillSlot skillSlot = itemBehaviour.GetSimpleView().Get<SkillSlot>();
            if (skillSlot != null)
                itemBehaviour.GetSimpleView().GetComponent<SlotCardView>().SkillCardView.SetHighlight(false);
        }
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
        => AudioManager.Play("CardHover");

    private void Merge(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;
        RunEnvironment env = RunManager.Instance.Environment;
        RunSkill lhs = from.GetSimpleView().Get<RunSkill>();
        RunSkill rhs = to.GetSimpleView().Get<RunSkill>();
        bool success = env.MergeProcedure(lhs, rhs);
        if (!success)
        {
            // merge fail staging
            CanvasManager.Instance.MergePreresultView.SetMergePreresult(null);
            return;
        }
        
        CanvasManager.Instance.MergePreresultView.SetMergePreresult(null);
        env.ReceiveSignalProcedure(new FieldChangedSignal());
        
        MergeStaging(from, to, d);

        CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void MergeStaging(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        // From: 本体被移除
        
        // Ghost
        ExtraBehaviourGhost ghost = from.GetCLView().GetExtraBehaviour<ExtraBehaviourGhost>();
        ghost.Hide();
        
        // To: Ghost Display -> ToIdle + Ping Animation
        ExtraBehaviourPivot extraBehaviourPivot = to.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        if (extraBehaviourPivot != null)
            extraBehaviourPivot.SetPathAnimated(ghost.GetDisplayTransform(), extraBehaviourPivot.IdleTransform);

        AudioManager.Play("CardUpgrade");
    }

    private void Unequip(InteractBehaviour from, MonoBehaviour to, PointerEventData d)
        => Unequip(from, null, d);

    private void Unequip(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is FieldSlotInteractBehaviour))
            return;
        RunEnvironment env = RunManager.Instance.Environment;
        SkillSlot slot = from.GetSimpleView().Get<SkillSlot>();
        UnequipResult result = env.UnequipProcedure(slot, null);
        if (!result.Success)
            return;
        
        env.ReceiveSignalProcedure(new FieldChangedSignal());

        UnequipStaging(from);

        PlayerEntity.Refresh();
        CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void UnequipStaging(InteractBehaviour from)
    {
        // From: No Animation
        
        // Ghost
        ExtraBehaviourGhost ghost = from.GetCLView().GetExtraBehaviour<ExtraBehaviourGhost>();
        ghost.Hide();
        
        // New IB: Ghost Display -> To Idle
        InteractBehaviour newIB = HandView.ActivePool.Last().GetInteractBehaviour();
        ExtraBehaviourPivot extraBehaviourPivot = newIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        if (extraBehaviourPivot != null)
            extraBehaviourPivot.SetPathAnimated(ghost.GetDisplayTransform(), extraBehaviourPivot.IdleTransform);

        AudioManager.Play("CardPlacement");
    }

    #endregion

    public RectTransform Find(Address address)
    {
        ItemBehaviour itemBehaviour =
            HandView.ActivePool.Find(item => item.GetSimpleView().GetAddress().Equals(address)) ??
            PlayerEntity.SkillList.ActivePool.Find(item => item.GetSimpleView().GetAddress().Equals(address));
        if (itemBehaviour == null)
            return null;
        return itemBehaviour.GetDisplayTransform();
    }

    private Tween _animationHandle;

    private void Sort()
    {
        CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();

        _animationHandle?.Kill();

        _animationHandle = DOTween.Sequence()
            .AppendCallback(() =>
            {
                HandViewPivotTransform.SetSizeWithCurrentAnchors(0, 0);
                HandView.RefreshPivots();
            })
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                HandView.Get<SkillInventory>().SortByComparisonId(0);
                CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
                HandViewPivotTransform.SetSizeWithCurrentAnchors(0, 1134);
                HandView.RefreshPivots();
            });

        _animationHandle.SetAutoKill().Restart();
    }

    private void TryShow(PointerEventData eventData) => SetStateAsync(1);
    private void TryHide(PointerEventData eventData) => SetStateAsync(0);

    public override Tween ShowTween()
        => DOTween.Sequence()
            .AppendCallback(Sync)
            .AppendCallback(() => DeckOpenZone.gameObject.SetActive(false))
            .AppendCallback(() => DeckCloseZone.gameObject.SetActive(true))
            .Join(SortButtonTransform.DOAnchorPos(SortButtonShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(PlayerEntityTransform.DOAnchorPos(PlayerEntityShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(HandTransform.DOAnchorPos(HandShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(PlayerEntityOtherHalfTransform.DOAnchorPos(PlayerEntityOtherHalfShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad));

    private Tween LockTween()
        => DOTween.Sequence()
            .AppendCallback(Sync)
            .AppendCallback(() => DeckOpenZone.gameObject.SetActive(false))
            .AppendCallback(() => DeckCloseZone.gameObject.SetActive(false))
            .Join(SortButtonTransform.DOAnchorPos(SortButtonShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(PlayerEntityTransform.DOAnchorPos(PlayerEntityShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(HandTransform.DOAnchorPos(HandShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(PlayerEntityOtherHalfTransform.DOAnchorPos(PlayerEntityOtherHalfShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad));

    public override Tween HideTween()
        => DOTween.Sequence()
            .AppendCallback(() => DeckOpenZone.gameObject.SetActive(true))
            .AppendCallback(() => DeckCloseZone.gameObject.SetActive(false))
            .Join(SortButtonTransform.DOAnchorPos(SortButtonHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad))
            .Join(PlayerEntityTransform.DOAnchorPos(PlayerEntityHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad))
            .Join(HandTransform.DOAnchorPos(HandHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad))
            .Join(PlayerEntityOtherHalfTransform.DOAnchorPos(PlayerEntityOtherHalfHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad));
}
