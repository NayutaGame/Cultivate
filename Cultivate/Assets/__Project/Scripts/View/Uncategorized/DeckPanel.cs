
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckPanel : Panel
{
    public PropagatePointerEnter DeckOpenZone;
    public PropagatePointerEnter DeckCloseZone;

    public Button SortButton;
    public AnimatedListView FieldView;
    public AnimatedListView HandView;
    public ListView FormationListView;
    public ListView MechListView;

    [SerializeField] public RectTransform _sortButtonTransform;
    [SerializeField] public RectTransform _fieldTransform;
    [SerializeField] public RectTransform _handTransform;
    [SerializeField] public RectTransform _formationListTransform;
    [SerializeField] public RectTransform _mechListTransform;

    [SerializeField] private RectTransform SortButtonShowPivot;
    [SerializeField] private RectTransform FieldShowPivot;
    [SerializeField] private RectTransform HandShowPivot;
    [SerializeField] private RectTransform FormationListShowPivot;
    [SerializeField] private RectTransform MechListShowPivot;

    [SerializeField] private RectTransform SortButtonHidePivot;
    [SerializeField] private RectTransform FieldHidePivot;
    [SerializeField] private RectTransform HandHidePivot;
    [SerializeField] private RectTransform FormationListHidePivot;
    [SerializeField] private RectTransform MechListHidePivot;

    [SerializeField] private RectTransform HandViewPivotTransform;
    [SerializeField] private HorizontalLayoutGroup HandViewLayout;

    public override void Configure()
    {
        base.Configure();

        DeckOpenZone._onPointerEnter = TryShow;
        DeckCloseZone._onPointerEnter = TryHide;
        SetLocked(false);

        FieldView.SetAddress(new Address("Run.Environment.Hero.Slots"));
        FieldView.PointerEnterNeuron.Join(PlayCardHoverSFX);
        FieldView.DropNeuron.Join(TryEquipSkill, TrySwap, TryEquipMech);

        HandView.SetAddress(new Address("Run.Environment.Hand"));
        HandView.PointerEnterNeuron.Join(PlayCardHoverSFX);
        HandView.DropNeuron.Join(TryMerge, TryUnequip);
        HandView.GetComponent<PropagateDrop>()._onDrop = TryUnequip;

        FormationListView.SetAddress(new Address("Run.Environment.Hero.ShowingFormations"));

        MechListView.SetAddress(new Address("Run.Environment.MechBag"));
        MechListView.DropNeuron.Join(TryUnequip);

        SortButton.onClick.RemoveAllListeners();
        SortButton.onClick.AddListener(Sort);
    }

    public override void Refresh()
    {
        base.Refresh();
        FieldView.Refresh();
        HandView.Refresh();
        FormationListView.Refresh();
        MechListView.Refresh();
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
        FormationListView.Sync();
    }

    private void SyncSlot(JingJie jingJie)
    {
        FieldView.Sync();
    }

    #region IInteractable

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");

    private void TryMerge(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        RunSkill lhs = from.GetSimpleView().Get<RunSkill>();
        RunSkill rhs = to.GetSimpleView().Get<RunSkill>();
        bool success = env.MergeProcedure(lhs, rhs);
        if (!success)
            return;

        ExtraBehaviourGhost ghost = from.GetCLView().GetExtraBehaviour<ExtraBehaviourGhost>();
        ghost.FromDrop();
        
        // Merge Animation
        to.OnEndDrag(eventData);
        AudioManager.Play("CardUpgrade");

        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
    }

    private void TryEquipSkill(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        RunSkill toEquip = from.GetSimpleView().Get<RunSkill>();
        SkillSlot slot = to.GetSimpleView().Get<SkillSlot>();
        bool success = env.TryEquipSkill(toEquip, slot);
        if (!success)
            return;

        {
            // Equip Skill Animation
            ExtraBehaviourGhost ghost = from.GetCLView().GetExtraBehaviour<ExtraBehaviourGhost>();
            ExtraBehaviourPivot pivot = to.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            ghost.FromDrop();
            pivot.AnimateState(ghost.GetDisplayTransform(), pivot.IdleTransform);

            AudioManager.Play("CardPlacement");
        }

        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
    }

    private void TryEquipMech(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is MechInteractBehaviour))
            return;
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        Mech toEquip = from.GetSimpleView().Get<Mech>();
        SkillSlot slot = to.GetSimpleView().Get<SkillSlot>();
        bool success = env.TryEquipMech(toEquip, slot);
        if (!success)
            return;

        // Equip Mech Animation
        AudioManager.Play("CardPlacement");

        from.GetSimpleView().Refresh();
        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
    }

    private void TryUnequip(InteractBehaviour from, MonoBehaviour to, PointerEventData eventData)
        => TryUnequip(from, null, eventData);

    private void TryUnequip(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is FieldSlotInteractBehaviour))
            return;
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        SkillSlot slot = from.GetSimpleView().Get<SkillSlot>();
        UnequipResult result = env.TryUnequip(slot, null);
        if (!result.Success)
            return;

        if (result.IsRunSkill)
        {
            // Unequip Skill Animation
            InteractBehaviour newIB = HandView.ActivePool.Last().GetInteractBehaviour();
            ExtraBehaviourGhost ghost = from.GetCLView().GetExtraBehaviour<ExtraBehaviourGhost>();
            ExtraBehaviourPivot pivot = newIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            ghost.FromDrop();
            pivot.AnimateState(ghost.GetDisplayTransform(), pivot.IdleTransform);

            AudioManager.Play("CardPlacement");
        }
        else
        {
            // Unequip Mech Animation
        }

        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
        MechListView.Refresh();
    }

    private void TrySwap(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is FieldSlotInteractBehaviour))
            return;

        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        SkillSlot fromSlot = from.GetSimpleView().Get<SkillSlot>();
        SkillSlot toSlot = to.GetSimpleView().Get<SkillSlot>();
        bool success = env.TrySwap(fromSlot, toSlot);
        if (!success)
            return;

        {
            // Swap Animation
            eventData.pointerDrag = null;
            to.OnEndDrag(eventData);
            from.OnEndDrag(eventData);
            // from.ComplexView.AnimateBehaviour.SetStartAndPivot(to.ComplexView.PivotBehaviour.IdlePivot, from.ComplexView.PivotBehaviour.IdlePivot);

            AudioManager.Play("CardPlacement");
        }

        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
    }

    #endregion

    private Tween _animationHandle;

    private void Sort()
    {
        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();

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

    private void TryShow(PointerEventData eventData)
    {
        SetShowing(true);
    }

    private void TryHide(PointerEventData eventData)
    {
        SetShowing(false);
    }

    private bool _locked;
    public void SetLocked(bool locked)
    {
        if (_locked == locked)
            return;
        _locked = locked;
        DeckCloseZone.gameObject.SetActive(!_locked);
    }

    public override Tween ShowAnimation()
        => DOTween.Sequence()
            .AppendCallback(Sync)
            .AppendCallback(() => DeckOpenZone.gameObject.SetActive(false))
            .AppendCallback(() => DeckCloseZone.gameObject.SetActive(!_locked))
            .Join(_sortButtonTransform.DOAnchorPos(SortButtonShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(_fieldTransform.DOAnchorPos(FieldShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(_handTransform.DOAnchorPos(HandShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(_formationListTransform.DOAnchorPos(FormationListShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad))
            .Join(_mechListTransform.DOAnchorPos(MechListShowPivot.anchoredPosition, 0.15f).SetEase(Ease.OutQuad));

    public override Tween HideAnimation()
        => DOTween.Sequence()
            .AppendCallback(() => DeckOpenZone.gameObject.SetActive(true))
            .AppendCallback(() => DeckCloseZone.gameObject.SetActive(false))
            .Join(_sortButtonTransform.DOAnchorPos(SortButtonHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad))
            .Join(_fieldTransform.DOAnchorPos(FieldHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad))
            .Join(_handTransform.DOAnchorPos(HandHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad))
            .Join(_formationListTransform.DOAnchorPos(FormationListHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad))
            .Join(_mechListTransform.DOAnchorPos(MechListHidePivot.anchoredPosition, 0.15f).SetEase(Ease.InQuad));
}
