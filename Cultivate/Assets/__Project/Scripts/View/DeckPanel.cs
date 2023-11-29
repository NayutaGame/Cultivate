
using System;
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

        ConfigureInteractDelegate();

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
    private void ConfigureInteractDelegate()
    {
        _interactHandler = new(5,
            getId: d =>
            {
                if (d is HandSkillDelegate)
                    return 0;
                if (d is AnimatedListDelegate)
                    return 1;
                if (d is FieldSlotDelegate)
                    return 2;
                if (d is MechDelegate)
                    return 3;
                if (d is RunFormationIconDelegate)
                    return 4;
                return null;
            },
            dragDropTable: new Action<InteractDelegate, InteractDelegate>[]
            {
                /*                  SkillView,  HandView,       SlotView,        Mech,       FormationView */
                /* SkillView     */ TryMerge,   null,           TryEquipSkill,   null,       null,
                /* HandView      */ null,       null,           null,            null,       null,
                /* SlotView      */ TryUnequip, TryUnequip,     TrySwap,         TryUnequip, null,
                /* Mech          */ null,       null,           TryEquipMech,    null,       null,
                /* FormationView */ null,       null,           null,            null,       null,
            });

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 0, (v, d) => ((HandSkillDelegate)v).HoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 0, (v, d) => ((HandSkillDelegate)v).UnhoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 0, (v, d) => ((HandSkillDelegate)v).PointerMove(d));
        _interactHandler.SetHandle(InteractHandler.BEGIN_DRAG, 0, (v, d) => ((HandSkillDelegate)v).BeginDrag(d));
        _interactHandler.SetHandle(InteractHandler.END_DRAG, 0, (v, d) => ((HandSkillDelegate)v).EndDrag(d));
        _interactHandler.SetHandle(InteractHandler.DRAG, 0, (v, d) => ((HandSkillDelegate)v).Drag(d));

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 2, (v, d) => ((FieldSlotDelegate)v).HoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 2, (v, d) => ((FieldSlotDelegate)v).UnhoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 2, (v, d) => ((FieldSlotDelegate)v).PointerMove(d));
        _interactHandler.SetHandle(InteractHandler.BEGIN_DRAG, 2, (v, d) => ((FieldSlotDelegate)v).BeginDrag(d));
        _interactHandler.SetHandle(InteractHandler.END_DRAG, 2, (v, d) => ((FieldSlotDelegate)v).EndDrag(d));
        _interactHandler.SetHandle(InteractHandler.DRAG, 2, (v, d) => ((FieldSlotDelegate)v).Drag(d));

        _interactHandler.SetHandle(InteractHandler.BEGIN_DRAG, 3, (v, d) => ((MechDelegate)v).BeginDrag(d));
        _interactHandler.SetHandle(InteractHandler.END_DRAG, 3, (v, d) => ((MechDelegate)v).EndDrag(d));
        _interactHandler.SetHandle(InteractHandler.DRAG, 3, (v, d) => ((MechDelegate)v).Drag(d));

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 4, (v, d) => ((RunFormationIconDelegate)v).PointerEnter(v, d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 4, (v, d) => ((RunFormationIconDelegate)v).PointerExit(v, d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 4, (v, d) => ((RunFormationIconDelegate)v).PointerMove(v, d));

        FieldView.SetHandler(_interactHandler);
        HandView.SetHandler(_interactHandler);
        FormationListView.SetHandler(_interactHandler);
        MechListView.SetHandler(_interactHandler);
    }

    private void TryMerge(InteractDelegate from, InteractDelegate to)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        RunSkill lhs = from.AddressDelegate.Get<RunSkill>();
        RunSkill rhs = to.AddressDelegate.Get<RunSkill>();
        bool success = env.TryMerge(lhs, rhs);
        if (!success)
            return;

        AudioManager.Play("CardUpgrade");
        CanvasManager.Instance.RunCanvas.NodeLayer.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
    }

    private void TryEquipSkill(InteractDelegate fromDelegate, InteractDelegate toDelegate)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        RunSkill toEquip = fromDelegate.AddressDelegate.Get<RunSkill>();
        SkillSlot slot = toDelegate.AddressDelegate.Get<SkillSlot>();
        bool success = env.TryEquipSkill(toEquip, slot);
        if (!success)
            return;

        AudioManager.Play("CardPlacement");
        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.NodeLayer.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
    }

    private void TryEquipMech(InteractDelegate fromDelegate, InteractDelegate toDelegate)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        Mech toEquip = fromDelegate.AddressDelegate.Get<Mech>();
        SkillSlot slot = toDelegate.AddressDelegate.Get<SkillSlot>();
        bool success = env.TryEquipMech(toEquip, slot);
        if (!success)
            return;

        AudioManager.Play("CardPlacement");
        fromDelegate.AddressDelegate.Refresh();
        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.NodeLayer.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
    }

    private void TryUnequip(InteractDelegate fromDelegate, InteractDelegate toDelegate)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        SkillSlot slot = fromDelegate.AddressDelegate.Get<SkillSlot>();
        bool success = env.TryUnequip(slot, null);
        if (!success)
            return;

        AudioManager.Play("CardPlacement");
        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.NodeLayer.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
        MechListView.Refresh();
    }

    private void TrySwap(InteractDelegate fromDelegate, InteractDelegate toDelegate)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        SkillSlot fromSlot = fromDelegate.AddressDelegate.Get<SkillSlot>();
        SkillSlot toSlot = toDelegate.AddressDelegate.Get<SkillSlot>();
        bool success = env.TrySwap(fromSlot, toSlot);
        if (!success)
            return;

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
