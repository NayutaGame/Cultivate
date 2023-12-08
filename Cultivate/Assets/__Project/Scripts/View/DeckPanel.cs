
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

    public override void Configure()
    {
        base.Configure();

        DeckOpenZone._onPointerEnter = TryShow;
        DeckCloseZone._onPointerEnter = TryHide;
        SetLocked(false);

        FieldView.SetAddress(new Address("Run.Environment.Hero.Slots"));
        HandView.SetAddress(new Address("Run.Environment.Hand"));
        FormationListView.SetAddress(new Address("Run.Environment.Hero.ActivatedSubFormations"));
        MechListView.SetAddress(new Address("Run.Environment.MechBag"));

        DefineInteractHandler();

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
        if (RunManager.Instance != null && RunManager.Instance.Environment != null)
            RunManager.Instance.Environment.MapJingJieChangedEvent += SyncSlot;
        HandView.Sync();
    }

    private void OnDisable()
    {
        if (RunManager.Instance != null && RunManager.Instance.Environment != null)
            RunManager.Instance.Environment.MapJingJieChangedEvent -= SyncSlot;
    }

    private void SyncSlot(JingJie jingJie)
    {
        FieldView.Sync();
    }

    #region IInteractable

    private InteractHandler _interactHandler;
    public InteractHandler GetDelegate() => _interactHandler;
    private void DefineInteractHandler()
    {
        _interactHandler = new(5,
            getId: d =>
            {
                if (d is HandSkillInteractBehaviour)
                    return 0;
                if (d is DropInteractBehaviour)
                    return 1;
                if (d is FieldSlotInteractBehaviour)
                    return 2;
                if (d is MechInteractBehaviour)
                    return 3;
                if (d is RunFormationIconInteractBehaviour)
                    return 4;
                return null;
            },
            dragDropTable: new Action<InteractBehaviour, InteractBehaviour, PointerEventData>[]
            {
                /*                  SkillView,  HandView,       SlotView,        Mech,       FormationView */
                /* SkillView     */ TryMerge,   null,           TryEquipSkill,   null,       null,
                /* HandView      */ null,       null,           null,            null,       null,
                /* SlotView      */ TryUnequip, TryUnequip,     TrySwap,         TryUnequip, null,
                /* Mech          */ null,       null,           TryEquipMech,    null,       null,
                /* FormationView */ null,       null,           null,            null,       null,
            });

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 0, (v, d) => ((HandSkillInteractBehaviour)v).HoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 0, (v, d) => ((HandSkillInteractBehaviour)v).UnhoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 0, (v, d) => ((HandSkillInteractBehaviour)v).PointerMove(d));
        _interactHandler.SetHandle(InteractHandler.BEGIN_DRAG, 0, (v, d) => ((HandSkillInteractBehaviour)v).BeginDrag(d));
        _interactHandler.SetHandle(InteractHandler.END_DRAG, 0, (v, d) => ((HandSkillInteractBehaviour)v).EndDrag(d));
        _interactHandler.SetHandle(InteractHandler.DRAG, 0, (v, d) => ((HandSkillInteractBehaviour)v).Drag(d));

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 2, (v, d) => ((FieldSlotInteractBehaviour)v).HoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 2, (v, d) => ((FieldSlotInteractBehaviour)v).UnhoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 2, (v, d) => ((FieldSlotInteractBehaviour)v).PointerMove(d));
        _interactHandler.SetHandle(InteractHandler.BEGIN_DRAG, 2, (v, d) => ((FieldSlotInteractBehaviour)v).BeginDrag(d));
        _interactHandler.SetHandle(InteractHandler.END_DRAG, 2, (v, d) => ((FieldSlotInteractBehaviour)v).EndDrag(d));
        _interactHandler.SetHandle(InteractHandler.DRAG, 2, (v, d) => ((FieldSlotInteractBehaviour)v).Drag(d));

        _interactHandler.SetHandle(InteractHandler.BEGIN_DRAG, 3, (v, d) => ((MechInteractBehaviour)v).BeginDrag(d));
        _interactHandler.SetHandle(InteractHandler.END_DRAG, 3, (v, d) => ((MechInteractBehaviour)v).EndDrag(d));
        _interactHandler.SetHandle(InteractHandler.DRAG, 3, (v, d) => ((MechInteractBehaviour)v).Drag(d));

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 4, (v, d) => ((RunFormationIconInteractBehaviour)v).PointerEnter(v, d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 4, (v, d) => ((RunFormationIconInteractBehaviour)v).PointerExit(v, d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 4, (v, d) => ((RunFormationIconInteractBehaviour)v).PointerMove(v, d));

        FieldView.SetHandler(_interactHandler);
        HandView.SetHandler(_interactHandler);
        HandView.GetComponent<DropInteractBehaviour>().SetHandler(_interactHandler);
        FormationListView.SetHandler(_interactHandler);
        MechListView.SetHandler(_interactHandler);
    }

    private void TryMerge(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        RunSkill lhs = from.AddressBehaviour.Get<RunSkill>();
        RunSkill rhs = to.AddressBehaviour.Get<RunSkill>();
        bool success = env.TryMerge(lhs, rhs);
        if (!success)
            return;

        // Merge Animation
        to.OnEndDrag(eventData);
        AudioManager.Play("CardUpgrade");

        CanvasManager.Instance.RunCanvas.NodeLayer.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
    }

    private void TryEquipSkill(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        RunSkill toEquip = from.AddressBehaviour.Get<RunSkill>();
        SkillSlot slot = to.AddressBehaviour.Get<SkillSlot>();
        bool success = env.TryEquipSkill(toEquip, slot);
        if (!success)
            return;

        // Equip Skill Animation
        eventData.pointerDrag = null;
        to.OnEndDrag(eventData);
        from.OnEndDrag(eventData);
        from.SetStartAndPivot(to.PivotBehaviour.IdlePivot, from.PivotBehaviour.IdlePivot);
        AudioManager.Play("CardPlacement");

        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.NodeLayer.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
    }

    private void TryEquipMech(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        Mech toEquip = from.AddressBehaviour.Get<Mech>();
        SkillSlot slot = to.AddressBehaviour.Get<SkillSlot>();
        bool success = env.TryEquipMech(toEquip, slot);
        if (!success)
            return;

        // Equip Mech Animation
        AudioManager.Play("CardPlacement");

        from.AddressBehaviour.Refresh();
        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.NodeLayer.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
    }

    private void TryUnequip(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        SkillSlot slot = from.AddressBehaviour.Get<SkillSlot>();
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
        CanvasManager.Instance.RunCanvas.NodeLayer.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
        MechListView.Refresh();
    }

    private void TrySwap(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        SkillSlot fromSlot = from.AddressBehaviour.Get<SkillSlot>();
        SkillSlot toSlot = to.AddressBehaviour.Get<SkillSlot>();
        bool success = env.TrySwap(fromSlot, toSlot);
        if (!success)
            return;

        // Swap Animation
        eventData.pointerDrag = null;
        to.OnEndDrag(eventData);
        from.OnEndDrag(eventData);
        from.SetStartAndPivot(to.PivotBehaviour.IdlePivot, from.PivotBehaviour.IdlePivot);
        AudioManager.Play("CardPlacement");

        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.NodeLayer.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
    }

    #endregion

    private void Sort()
    {
        CanvasManager.Instance.RunCanvas.NodeLayer.CardPickerPanel.ClearAllSelections();
        HandView.Get<SkillInventory>().SortByComparisonId(0);
        CanvasManager.Instance.RunCanvas.Refresh();
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
