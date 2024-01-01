
using System;
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
        FieldView.PointerEnterNeuron.Set((ib, d) => ((FieldSlotInteractBehaviour)ib).HoverAnimation(d));
        FieldView.PointerExitNeuron.Set((ib, d) => ((FieldSlotInteractBehaviour)ib).UnhoverAnimation(d));
        FieldView.PointerMoveNeuron.Set((ib, d) => ((FieldSlotInteractBehaviour)ib).PointerMove(d));
        FieldView.BeginDragNeuron.Set((ib, d) => ((FieldSlotInteractBehaviour)ib).BeginDrag(d));
        FieldView.EndDragNeuron.Set((ib, d) => ((FieldSlotInteractBehaviour)ib).EndDrag(d));
        FieldView.DragNeuron.Set((ib, d) => ((FieldSlotInteractBehaviour)ib).Drag(d));

        HandView.SetAddress(new Address("Run.Environment.Hand"));
        HandView.PointerEnterNeuron.Set((ib, d) => ((HandSkillInteractBehaviour)ib).HoverAnimation(d));
        HandView.PointerExitNeuron.Set((ib, d) => ((HandSkillInteractBehaviour)ib).UnhoverAnimation(d));
        HandView.PointerMoveNeuron.Set((ib, d) => ((HandSkillInteractBehaviour)ib).PointerMove(d));
        HandView.BeginDragNeuron.Set((ib, d) => ((HandSkillInteractBehaviour)ib).BeginDrag(d));
        HandView.EndDragNeuron.Set((ib, d) => ((HandSkillInteractBehaviour)ib).EndDrag(d));
        HandView.DragNeuron.Set((ib, d) => ((HandSkillInteractBehaviour)ib).Drag(d));

        FormationListView.SetAddress(new Address("Run.Environment.Hero.ActivatedSubFormations"));
        FormationListView.PointerEnterNeuron.Set(PointerEnterFormation);
        FormationListView.PointerExitNeuron.Set(PointerExitFormation);
        FormationListView.PointerMoveNeuron.Set(PointerMoveFormation);

        MechListView.SetAddress(new Address("Run.Environment.MechBag"));
        MechListView.BeginDragNeuron.Set((ib, d) => ((MechInteractBehaviour)ib).BeginDrag(d));
        MechListView.EndDragNeuron.Set((ib, d) => ((MechInteractBehaviour)ib).EndDrag(d));
        MechListView.DragNeuron.Set((ib, d) => ((MechInteractBehaviour)ib).Drag(d));

        HandView.DropNeuron.Set(TryMerge, TryUnequip);

        HandView.GetComponent<DropInteractBehaviour>().DropNeuron.Set(TryUnequip);

        FieldView.DropNeuron.Set(TryEquipSkill, TrySwap, TryEquipMech);

        MechListView.DropNeuron.Set(TryUnequip);

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
        HandView.Sync();
    }

    private void OnDisable()
    {
        // if (RunManager.Instance != null && RunManager.Instance.Environment != null)
        //     RunManager.Instance.Environment.MapJingJieChangedEvent -= SyncSlot;
    }

    private void SyncSlot(JingJie jingJie)
    {
        FieldView.Sync();
    }

    #region IInteractable

    private void PointerEnterFormation(InteractBehaviour ib, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.SetAddressFromIB(ib, eventData);
    }

    private void PointerExitFormation(InteractBehaviour ib, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.SetAddressToNull(ib, eventData);
    }

    private void PointerMoveFormation(InteractBehaviour ib, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.UpdateMousePos(eventData.position);
    }

    private void TryMerge(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        RunSkill lhs = from.ComplexView.AddressBehaviour.Get<RunSkill>();
        RunSkill rhs = to.ComplexView.AddressBehaviour.Get<RunSkill>();
        bool success = env.TryMerge(lhs, rhs);
        if (!success)
            return;

        // Merge Animation
        to.OnEndDrag(eventData);
        AudioManager.Play("CardUpgrade");

        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
    }

    private void TryEquipSkill(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        RunSkill toEquip = from.ComplexView.AddressBehaviour.Get<RunSkill>();
        SkillSlot slot = to.ComplexView.AddressBehaviour.Get<SkillSlot>();
        bool success = env.TryEquipSkill(toEquip, slot);
        if (!success)
            return;

        // Equip Skill Animation
        eventData.pointerDrag = null;
        to.OnEndDrag(eventData);
        from.OnEndDrag(eventData);
        from.ComplexView.AnimateBehaviour.SetStartAndPivot(to.ComplexView.PivotBehaviour.IdlePivot, from.ComplexView.PivotBehaviour.IdlePivot);
        AudioManager.Play("CardPlacement");

        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
    }

    private void TryEquipMech(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is MechInteractBehaviour))
            return;
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        Mech toEquip = from.ComplexView.AddressBehaviour.Get<Mech>();
        SkillSlot slot = to.ComplexView.AddressBehaviour.Get<SkillSlot>();
        bool success = env.TryEquipMech(toEquip, slot);
        if (!success)
            return;

        // Equip Mech Animation
        AudioManager.Play("CardPlacement");

        from.ComplexView.AddressBehaviour.Refresh();
        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
    }

    private void TryUnequip(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is FieldSlotInteractBehaviour))
            return;
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        SkillSlot slot = from.ComplexView.AddressBehaviour.Get<SkillSlot>();
        UnequipResult result = env.TryUnequip(slot, null);
        if (!result.Success)
            return;

        // Unequip Animation
        if (result.IsRunSkill)
        {
            AudioManager.Play("CardPlacement");
            InteractBehaviour newIB = (HandView.ActivePool.Last() as AnimatedItemView).InteractBehaviour;
            eventData.pointerDrag = null;
            newIB.OnEndDrag(eventData);
        }
        else
        {

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
        SkillSlot fromSlot = from.ComplexView.AddressBehaviour.Get<SkillSlot>();
        SkillSlot toSlot = to.ComplexView.AddressBehaviour.Get<SkillSlot>();
        bool success = env.TrySwap(fromSlot, toSlot);
        if (!success)
            return;

        // Swap Animation
        eventData.pointerDrag = null;
        to.OnEndDrag(eventData);
        from.OnEndDrag(eventData);
        from.ComplexView.AnimateBehaviour.SetStartAndPivot(to.ComplexView.PivotBehaviour.IdlePivot, from.ComplexView.PivotBehaviour.IdlePivot);
        AudioManager.Play("CardPlacement");

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
            .AppendCallback(() => HandView.Sync())
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
